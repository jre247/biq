using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BrightLine.Web.Controllers
{
    public class AppsController : BaseController
    {
		private IAppService Apps { get;set;}

		public AppsController()
		{
			Apps = IoC.Resolve<IAppService>();
		}

		[System.Web.Http.HttpGet]
		public ActionResult List()
		{
			var apps = Apps.GetAll().ToList().Select(c => new AppViewModel(c)).ToList();

			return View("List", apps);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Edit(int id)
		{
			try
			{
				ViewBag.FormSubmitAction = "Save";
				ViewBag.PageTitleAction = "Edit";

				var app = Apps.Get(id);

				var vm = Apps.GetViewModel(app);

				return View(vm);
				
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Placement to edit." });
			}
		}

		[System.Web.Http.HttpGet]
		public ActionResult Create()
		{
			try
			{
				ViewBag.FormSubmitAction = "Create";
				ViewBag.PageTitleAction = "Create";

				var vm = Apps.GetViewModel();

				return View("Edit", vm);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Placement to edit." });
			}

		}

		[System.Web.Http.HttpPost]
		public ActionResult Save(AppViewModel model)
		{
			try
			{
				App app = null;
				var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

				if (ModelState.IsValid)
				{
					app = Apps.Save(model);
					flashMessageExtensions.Success("App saved", true);
				}
				else
				{
					//if editing placement
					if (model.Id > 0)
					{
						ViewBag.FormSubmitAction = "Save";
						ViewBag.PageTitleAction = "Edit";
					}
					else
					{
						ViewBag.FormSubmitAction = "Create";
						ViewBag.PageTitleAction = "Create";
					}

					Apps.FillSelectListsForViewModel(model);
					return View("Edit", model);
				}
					
				return RedirectToAction("Edit", new {id = app.Id});
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Placement to edit." });
			}
		}

		

		

    }
}
