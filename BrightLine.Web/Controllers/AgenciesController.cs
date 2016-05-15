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
    public class AgenciesController : BaseController
    {
		private IAgencyService Agencies { get;set;}

		public AgenciesController()
		{
			Agencies = IoC.Resolve<IAgencyService>();
		}

		[System.Web.Http.HttpGet]
		public ActionResult List()
		{
			var agencies = Agencies.GetAll().ToList().Select(c => new AgencyViewModel(c)).ToList();
			
			return View("List", agencies);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Edit(int id)
		{
			try
			{
				ViewBag.FormSubmitAction = "Save";
				ViewBag.PageTitleAction = "Edit";

				var agency = Agencies.Get(id);

				var vm = Agencies.GetViewModel(agency);

				return View(vm);
				
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting agency to edit." });
			}
		}

		[System.Web.Http.HttpGet]
		public ActionResult Create()
		{
			try
			{
				ViewBag.FormSubmitAction = "Create";
				ViewBag.PageTitleAction = "Create";

				var vm = Agencies.GetViewModel();

				return View("Edit", vm);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting agency to edit." });
			}

		}

		[System.Web.Http.HttpPost]
		public ActionResult Save(AgencyViewModel model)
		{
			try
			{
				Agency agency = null;
				var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

				if (ModelState.IsValid)
				{
					agency = Agencies.Save(model);
					flashMessageExtensions.Success("Agency saved", true);
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

					return View("Edit", model);
				}
					
				return RedirectToAction("Edit", new {id = agency.Id});
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Agency to edit." });
			}
		}

		

		

    }
}
