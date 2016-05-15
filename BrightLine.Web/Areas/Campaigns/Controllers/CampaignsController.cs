using AttributeRouting;
using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using BrightLine.Web.Controllers;
using BrightLine.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Campaigns.Controllers
{
	[RoutePrefix("campaigns")]
	[RouteArea("campaigns")]
	public class CampaignsController : BaseController
	{
		private ICampaignService Campaigns { get;set;}

		public CampaignsController()
		{
			Campaigns = IoC.Resolve<ICampaignService>();
		}

		[System.Web.Mvc.HttpGet]
		[RestoreModelState]
		public ActionResult Create()
		{
			if (!Campaigns.IsUserAllowedToCreate())
				return Redirect("Index");

			var cvm = new CampaignViewModel();

			GetLookups();

			return View("Edit", cvm);
		}

		[System.Web.Mvc.HttpGet]
		public ActionResult Edit(int id)
		{
			var campaign = Campaigns.Get(id);
			if (!Campaigns.IsAccessible(campaign))
				return ReturnInaccessible(id);

			var cvm = (ViewData["Campaign"] as CampaignViewModel) ?? CampaignViewModel.FromCampaign(campaign);

			GetLookups();

			return View("Edit", cvm);
		}

		
		
		[System.Web.Mvc.HttpPost]
		public ActionResult Save(CampaignViewModel model)
		{
			ResourceViewModel resource = null;

			if (model == null)
				return RedirectToAction("Create").Info("Could not complete request, please try again.");

			try
			{
				if (ModelState.IsValid)
				{
					var file = Request.Files[0];
					if (FileHelper.IsFilePresent(file))
						resource = RegisterResource();

					if (model.Product.Id == 0)  // create the product from the new-product element string
						SaveCampaignProduct(model);

					var campaign = Campaigns.Get(model.Id);
					if (campaign == null)
						campaign = new Campaign();
					campaign = model.ToCampaign(campaign);
					campaign = Campaigns.Upsert(campaign);

					if (resource != null)
					{
						var thumbnail = CreateThumbnail(resource, campaign.Id);
						if (thumbnail != null)
						{
							campaign.Thumbnail = thumbnail;
						}
					}

					Campaigns.Update(campaign);

					// Redirect to the summary page directly.
					// return RedirectToAction("Edit", new { id = campaign.Id }).Success("Campaign saved.");
					return Redirect(String.Format("/campaigns/{0}/summary", campaign.Id));
				}

				GetLookups();

				return View("Edit", model);
			}
			catch (ValidationException vex)
			{
				ModelState.AddModelError("ValidationException", vex.Message);
			}
			catch (ImageSizeException isex)
			{
				ModelState.AddModelError("ImageSizeException", isex.Message);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
			}

			ViewData["Campaign"] = model;
			var ar = model.IsNewEntity ? RedirectToAction("Create") : RedirectToAction("Edit", new { id = model.Id });
			return ar.Error("Could not save campaign.");
		}

		

		/// <summary>
		/// Show high-level campaign details and allows user to administer basic aspects of the Campaign
		/// </summary>
		public ActionResult Details(int id)
		{
			var featureProperties = new[]
                   {
                        "Features", "Features.FeatureType", "Features.FeatureType.FeatureTypeGroup", "Features.FeatureCategory"
                   };
			var campaign = Campaigns.Get(id, featureProperties);
			if (!Campaigns.IsAccessible(campaign))
			{
				IoC.Log.Error(string.Format("User {0} attempted to access i"));
				return RedirectToAction("Campaigns");
			}

			return View("Details", campaign);
		}

		/// <summary>
		/// Show a history of all publishes for a specific Campaign
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Developer)]
		public ActionResult History(int id)
		{
			try
			{
				var cmsPublishService = IoC.Resolve<IPublishService>();

				var viewModel = cmsPublishService.GetViewModel(id);
				return View("../CmsPublish/List", viewModel);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				return RedirectToAction("Campaigns");
			}
		
		}

		public ActionResult Publish(int id, string env)
		{
			try
			{
				var cmsPublishService = IoC.Resolve<IPublishService>();

				var boolMessage = cmsPublishService.Publish(id, env.Trim().ToUpper());
				if (!boolMessage.Success)
				{
					IoC.Log.Error(boolMessage.Message);
				}
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
			}

			return RedirectToAction("History", new { id = id });
			
		}

		#region Private methods

		private void SaveCampaignProduct(CampaignViewModel model)
		{
			var productsRepo = IoC.Resolve<IRepository<Product>>();

			var product = new Product()
			{
				Name = Request["new-product"] //HACK: there has got to be a better way to get the new product name//model.Product.Name
			};
			if (string.IsNullOrWhiteSpace(product.Name))
				throw new ArgumentException("Product name cannot be empty.");
			if (productsRepo.Where(p => p.Name == product.Name).Any())
				throw new ArgumentException("Product name must be unique.");

			productsRepo.Insert(product);
			productsRepo.Save();
			model.Product.Id = product.Id;
		}

		private void GetLookups()
		{
			var productsRepo = IoC.Resolve<IRepository<Product>>();

			var agenciesRepo = IoC.Resolve<IAgencyService>();

			ViewBag.Products = productsRepo.GetAll().
						OrderBy(p => p.Name).
						ToLookups().
						InsertLookups(0, EntityLookup.Select);


			ViewBag.Agencies = agenciesRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);
		}

		private Resource CreateThumbnail(ResourceViewModel resourceViewModel, int campaignId)
		{
			var resourcesRepo = IoC.Resolve<IResourceService>();

			Resource resource = null;
			var auditEvents = IoC.Resolve<IAuditEventService>();

			var resourceId = resourceViewModel.id;
			var file = Request.Files[0];

			if (file.ContentLength > 150 * 1024) // 150k
				throw new ImageSizeException((150 * 1024), "Thumbnail size cannot exceed 150k");

			//var uploadedFilename = file.FileName.Trim('\"');
			var uploadedFilename = file.FileName;
			var extension = (Path.GetExtension(uploadedFilename) ?? "").Replace(".", "");

			int? extensionId = null;
			if (Lookups.FileTypes.HashByName.ContainsKey(extension))
				extensionId = Lookups.FileTypes.HashByName[extension];

			var imageContentTypes = FileHelper.ImageContentTypes;
			if (imageContentTypes.Contains(file.ContentType))
			{
				byte[] contents;
				using (var reader = new BinaryReader(file.InputStream))
				{
					file.InputStream.Position = 0;
					contents = reader.ReadBytes(file.ContentLength);
				}

				resourcesRepo.Upload(resourceId, campaignId, contents);

				auditEvents.Audit("UploadCampaignThumbnail", "Campaign", "CampaignController.CreateThumbnail: " + uploadedFilename);

				resource = resourcesRepo.Get(resourceId);
			}

			return resource;
		}

		private ResourceViewModel RegisterResource()
		{
			var resourceHelper = IoC.Resolve<IResourceHelper>();
			var resourcesRepo = IoC.Resolve<IResourceService>();

			var file = System.Web.HttpContext.Current.Request.Files[0];

			var md5Hash = resourceHelper.GenerateMd5HashForFile(file.InputStream);

			int width, height = 0;
			using (var img = Image.FromStream(file.InputStream, false, false))
			{
				width = img.Width;
				height = img.Height;
			}

			var resourceViewModel = new ResourceViewModel
			{
				name = file.FileName,
				filename = file.FileName,
				height = height,
				size = file.ContentLength,
				resourceType = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage],
			};

			var resourceRegistered = resourcesRepo.Register(resourceViewModel);

			return resourceRegistered;
		}


		#endregion
	}
}