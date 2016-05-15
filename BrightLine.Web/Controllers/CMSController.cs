using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Core;
using BrightLine.Utility;
using BrightLine.Web.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;
using BrightLine.Common.Utility;
using BrightLine.Common.Services;


namespace BrightLine.Web.Controllers
{
	public class CMSController : BaseController
	{
		private FileItem _fileItem;
		private IRepository<CampaignContentSchema> CampaignContentSchemas { get;set;}
		private ICampaignService Campaigns { get;set;}

		public CMSController()
		{
			CampaignContentSchemas = IoC.Resolve<IRepository<CampaignContentSchema>>();
			Campaigns = IoC.Resolve<ICampaignService>();
		}

		
		/// <summary>
		/// Lists all Campaigns with at least one ContentSchema
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			var model = Campaigns.Where(
				o => o.CampaignContentSchemas.Any(s => !s.IsDeleted) && !o.IsDeleted
			).OrderBy(o => o.Name).ToList();

			return View(model);
		}


		/// <summary>
		/// List all Schemas for a given Campaign
		/// </summary>
		/// <param name="id">Id of Campaign</param>
		/// <returns></returns>
		public ActionResult Schemas(int id)
		{
			var model = Campaigns.Get(id);
			if (model == null || model.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			return View(model);
		}


		/// <summary>
		/// Edit a Campaigns content
		/// </summary>
		/// <param name="id">Id of Schema</param>
		/// <returns></returns>
		public ActionResult Edit(int id)
		{
			var contentSchemaId = id;
			var model = CampaignContentSchemas.Get(contentSchemaId);
			if (model == null || model.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = model.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			PopulateViewData(campaign, model);
			this.ViewBag.formAction = "Edit";
			return View("Form", model);
		}


		/// <summary>
		/// POST for editing Campaign's content
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(FormCollection collection, string action)
		{
			var schemaId = Convert.ToInt32(collection["ContentSchemaId"]);


			var model = CampaignContentSchemas.Get(schemaId);
			if (model == null || model.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = model.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			TryUpdateModel<CampaignContentSchema>(model);

			var svc = GetCmsService();
			return Save(collection, action, "Edit", model, (importer) => svc.UpdateSpreadSheet(campaign, importer, schemaId, model.Tag, false));
		}


		/// <summary>
		/// Show all ModelInstanceItems for a given Campaign
		/// </summary>
		/// <returns></returns>
		public ActionResult Instances(int id, string modelType = null)
		{
			var schemaId = id;
			var schema = CampaignContentSchemas.Get(schemaId);
			if (schema == null || schema.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = schema.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var svc = GetCmsService();
			var instances = svc.GetCampaignInstances(schemaId, modelType);
			PopulateViewData(campaign, schema);
			ViewData["modelType"] = modelType;
			return View(instances);
		}


		/// <summary>
		/// Show all ModelInstanceItems for a given Campaign
		/// </summary>
		/// <returns></returns>
		public ActionResult Models(int id)
		{
			var svc = GetCmsService();
			var schemaId = id;
			var schema = CampaignContentSchemas.Get(schemaId);

			if (schema == null || schema.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = schema.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var models = svc.GetCampaignModels(schemaId);
			PopulateViewData(campaign, schema);
			return View(models);
		}


		/// <summary>
		/// Publish items to CMS API
		/// </summary>
		/// <returns></returns>
		public ActionResult Publish(int id, string env)
		{
			if (string.IsNullOrEmpty(env))
				return RedirectToAction("Schemas", new { id }).Error("Publish Environment must be specified.");
			
			var publishTarget = env.ToLower().Trim();
			var cmsPublishTargets = new[] { "dev", "uat", "pro" };
			
			if (!cmsPublishTargets.Contains(publishTarget))
				return RedirectToAction("Schemas", new { id }).Error("Publish Environment is invalid.");

			var svc = GetCmsService();
			var schemaId = id;

			var schema = CampaignContentSchemas.Get(schemaId);
			if (schema == null || schema.IsDeleted || schema.Campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = schema.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			try
			{
				svc.Publish(campaign, schema, env);
			}
			catch (Exception ex)
			{
				string errorMessage;

				if (ex is ViewValidationException)
				{
					errorMessage = ex.Message;
				}
				else
				{
					IoC.Log.Error(ex);
					errorMessage = "Publish failed - an unexpected error occurred.";
				}

				return RedirectToAction("Schemas", new { id = schema.Campaign.Id }).Error(errorMessage);
			}
			return RedirectToAction("Schemas", new { id = schema.Campaign.Id }).Success("Publish successful");
		}

		#region Downloads

		/// <summary>
		/// Download sample JSON schema.
		/// </summary>
		/// <returns></returns>
		public ActionResult Schema(int id, string type = null)
		{
			return GetSchemaContents(id, "_schema.json", (item) => item.DataSchema, type);
		}


		/// <summary>
		/// Download all items in raw JSON format
		/// </summary>
		/// <returns></returns>
		public ActionResult CollectionRaw(int id, string type = null)
		{
			return GetSchemaContents(id, "_data_raw.json", (item) => item.DataModelsRaw, type);
		}


		/// <summary>
		/// Download all items in published JSON format
		/// </summary>
		/// <returns></returns>
		public ActionResult CollectionPublished(int id, string type = null)
		{
			return GetSchemaContents(id, "_data_pub.json", (item) => item.DataModelsPublished, type);
		}


		/// <summary>
		/// Download single item in raw JSON format
		/// </summary>
		/// <returns></returns>
		public ActionResult InstanceRaw(int id, string type = null)
		{
			return GetInstanceContents(id, "_raw.json", (item) => item.InstanceData, type);
		}


		/// <summary>
		/// Download single item in published JSON format
		/// </summary>
		/// <returns></returns>
		public ActionResult InstancePublished(int id, string type = null)
		{
			return GetInstanceContents(id, "_pub.json", (item) => item.InstanceDataJson, type);
		}

		#endregion


		#region Private Methods

		/// <summary>
		/// Template method for both creating/editing import files
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		private ActionResult Save(FormCollection collection, string action, string formAction, CampaignContentSchema viewModel, Action<AppImporter> importCallback)
		{
			ActionResult result = null;

			try
			{
				this.ViewBag.formAction = formAction;

				// Track whether or not to just validate or validate and then save
				bool isValidingOnly = !string.IsNullOrEmpty(action) && action.Trim().ToLower() == "validate";

				// Ensure file + fields are supplied.
				if (!EnsureInputs(viewModel))
					return View("Form", viewModel);

				// Validate first.
				// NOTE: keep reference to importer, because after validation, the importer already has:
				// 1. schema of the app
				// 2. all models and lookups
				// 3. all model instances.
				var svc = new CmsService();
				var importer = new AppImporter();
				var importResult = svc.ValidateSpreadSheet(importer, viewModel.Campaign.CmsKey, _fileItem.Contents);
				if (!importResult.Success)
				{
					foreach (var msg in importResult.Item)
					{
						ModelState.AddModelError(string.Empty, msg);
					}
					result = View("Form", viewModel);
					return result;
				}

				// Validation passed. (Only validating ?)
				if (isValidingOnly)
				{
					return View("Form", viewModel).Success("Validation successful");
				}

				// Either create or update
				importCallback(importer);

				// Now go back to index back.
				var name = viewModel.Campaign.CmsKey;
				result = RedirectToAction("Schemas", new { id = viewModel.Campaign.Id }).Success(name + " data successfully loaded from spreadsheet.");
			}
			catch (CmsValidationException cve)
			{
				if (cve.Errors != null)
				{
					foreach (var error in cve.Errors)
					{
						this.ModelState.AddModelError(string.Empty, error);
					}
					result = View("Form", viewModel);
				}
				Log.Error(cve);
			}
			catch (Exception ex)
			{
				this.ModelState.AddModelError("Name", "Error while saving: " + ex.Message);
				Log.Error(ex);
				result = View("Form", viewModel);
			}
			return result;
		}

		private CampaignContentModelInstance GetInstanceItem(int id)
		{
			var svc = IoC.Resolve<ICrudService<CampaignContentModelInstance>>();
			var item = svc.Get(id);
			return item;
		}


		/// <summary>
		/// Action to index/list all items.
		/// </summary>
		/// <returns></returns>
		private ActionResult GetSchemaContents(int schemaId, string suffix, Func<CampaignContentSchema, string> dataFetcher, string type = "file")
		{
			if (string.IsNullOrEmpty(type))
				type = "file";

			var svc = GetCmsService();

			// Accessible ?
			var schema = CampaignContentSchemas.Get(schemaId);
			if (schema == null || schema.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = schema.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			string fileName = "campaign_" + campaign.Name + suffix;
			var content = dataFetcher(schema);
			if (type == "json")
				return JsonContentRaw(content, true);

			var bytes = System.Text.Encoding.Unicode.GetBytes(content);
			return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}


		/// <summary>
		/// Action to index/list all items.
		/// </summary>
		/// <returns></returns>
		private ActionResult GetInstanceContents(int id, string suffix, Func<CampaignContentModelInstance, string> dataFetcher, string type = "file")
		{
			if (string.IsNullOrEmpty(type))
				type = "file";

			var svc = GetCmsService();
			var item = GetInstanceItem(id);

			// Accesible ?
			var schema = CampaignContentSchemas.Get(item.Schema.Id);

			if (schema == null || schema.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			var campaign = schema.Campaign;

			if (campaign == null || campaign.IsDeleted)
				return RedirectToAction("Index", "Campaigns", new { });

			string fileName = "campaign_" + campaign.Id + "_" + item.ModelName + "_" + item.Key + suffix;
			var content = dataFetcher(item);
			if (type == "json")
				return JsonContentRaw(content, true);

			var bytes = System.Text.Encoding.Unicode.GetBytes(content);
			return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}


		private bool EnsureInputs(CampaignContentSchema schema)
		{
			var isValid = true;
			try
			{
				EnsureFile();
				CmsRules.EnsureCorrectFileName(schema.Campaign.CmsKey, _fileItem.FullNameRaw);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				isValid = false;
			}
			return isValid;
		}


		/// <summary>
		/// Ensures file was supplied ( populates model state with errors ).
		/// </summary>
		/// <returns></returns>
		private void EnsureFile()
		{
			var fileResult = FileItemMapper.GetFile(this.Request);
			if (fileResult.Success)
			{
				_fileItem = fileResult.Item;
				return;
			}

			throw new ArgumentException("File must be supplied");
		}

		private CmsService GetCmsService()
		{
			return new CmsService();
		}


		private void PopulateViewData(Campaign campaign, CampaignContentSchema schema = null)
		{
			ViewData["campaignId"] = campaign.Id;
			ViewData["campaignName"] = GetCampaignName(campaign);

			if (schema != null)
			{
				ViewData["schemaId"] = schema.Id;
				ViewData["tagName"] = schema.Tag;
			}
		}


		/// <summary>
		/// Gets the campaign name to display
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		private object GetCampaignName(Campaign campaign)
		{
			if (!string.IsNullOrEmpty(campaign.ShortDisplay))
				return campaign.ShortDisplay;
			return campaign.Name;
		}

		#endregion
	}
}
