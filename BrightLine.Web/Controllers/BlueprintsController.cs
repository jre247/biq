using BrightLine.Common.Framework;
using BrightLine.Web.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using BrightLine.Common.ViewModels.Blueprints;
using System.Linq;
using BrightLine.Common.Models;
using Octokit;
using BrightLine.Common.Services;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Blueprints;
using BrightLine.Common.Resources;
using System.Collections.Generic;

namespace BrightLine.Web.Controllers
{
    public class BlueprintsController : BaseController
	{
		private IBlueprintService Blueprints { get;set;}

		public BlueprintsController() 
		{ 
			Blueprints = IoC.Resolve<IBlueprintService>();
		}

		public ActionResult Index()
		{
			var blueprints = Blueprints.GetAll().ToList().Select(c => new BlueprintViewModel(c)).ToList();

			return View(blueprints);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Edit(int id)
		{
			try
			{
				ViewBag.FormSubmitAction = CommonResources.GenericSaveText;
				ViewBag.PageTitleAction = CommonResources.GenericEditText;

				var blueprint = Blueprints.Get(id);

				if (blueprint == null)
				{
					IoC.Log.Warn(string.Format(CommonResources.NonExistentEntity, id));
					return RedirectToAction("Index");
				}

				var vm = Blueprints.GetViewModel(blueprint);
				return View(vm);

			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Blueprint to edit." });
			}
		}

		[System.Web.Http.HttpGet]
		public ActionResult Create()
		{
			try
			{
				ViewBag.FormSubmitAction = CommonResources.GenericCreateText;
				ViewBag.PageTitleAction = CommonResources.GenericCreateText;

				var vm = Blueprints.GetViewModel();
				return View("Edit", vm);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Blueprint to edit." });
			}
		}


		public void CascadeDeleteBlueprint(string name)
		{
			try
			{
				var blueprintHelper = IoC.Resolve<IBlueprintHelper>();

				blueprintHelper.CascadeDeleteBlueprint(name);			
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error cascade deleting Blueprint." });
			}
		}


		[System.Web.Http.HttpPost]
		public async Task<ActionResult> Save(BlueprintViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{				
					await Blueprints.Save(model, Request);

					FlashMessage.Success(BlueprintConstants.BLUEPRINT_SAVED, true);					
				}
				else
				{
					return SetupViewModelOnErrors(model);
				}

				var blueprint = Blueprints.Where(b => b.ManifestName == model.ManifestName).Single();
				return RedirectToAction("Edit", new { id = blueprint.Id });
			}
			catch (ModelImageNotFoundException ex)
			{
				Logger.Warn(ex);
				ModelState.AddModelError(string.Empty, BlueprintConstants.Errors.PREVIEW_IMAGE_REQUIRED);
				return SetupViewModelOnErrors(model);
			}
			catch (ModelNotFoundException ex)
			{
				Logger.Warn(ex);
				ModelState.AddModelError(string.Empty, BlueprintConstants.Errors.BLUEPRINT_DOES_NOT_EXIST);
				return SetupViewModelOnErrors(model);
			}
			catch (NotFoundException ex)
			{
				Logger.Warn(ex);
				ModelState.AddModelError(string.Empty, BlueprintConstants.Errors.REPOSITORY_DOES_NOT_EXIST);
				return SetupViewModelOnErrors(model);
			}
			catch (BlueprintImportException ex)
			{
				Logger.Error(ex);
				ModelState.AddModelError(string.Empty, BlueprintConstants.Errors.UNEXPECTED_ERROR);
				return SetupViewModelOnErrors(model);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				ModelState.AddModelError(string.Empty, BlueprintConstants.Errors.UNEXPECTED_ERROR);
				CascadeDeleteBlueprint(model.ManifestName);
				return SetupViewModelOnErrors(model);
			}
		}

		#region Private Methods

		/// <summary>
		/// If there is a user input error then the blueprint lookups and view bag properties will need to be set again
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private ActionResult SetupViewModelOnErrors(BlueprintViewModel model)
		{
			if (model.IsNewModel)
			{
				ViewBag.FormSubmitAction = CommonResources.GenericCreateText;
				ViewBag.PageTitleAction = CommonResources.GenericCreateText;
			}
			else
			{
				ViewBag.FormSubmitAction = CommonResources.GenericSaveText;
				ViewBag.PageTitleAction = CommonResources.GenericEditText;
			}

			Blueprints.GetLookupsForBlueprint(model);
			return View("Edit", model);
		}

		#endregion
		
	}
}
