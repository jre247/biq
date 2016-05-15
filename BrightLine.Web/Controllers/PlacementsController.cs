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
    public class PlacementsController : BaseController
    {
		private IPlacementService Placements { get; set; }

		public PlacementsController()
		{
			Placements = IoC.Resolve<IPlacementService>();
		}

		[System.Web.Http.HttpGet]
		public ActionResult List()
		{
			var placements = Placements.GetAll().ToList().Select(c => new PlacementViewModel(c)).ToList();

			return View("List", placements);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Edit(int id)
		{
			try
			{
				ViewBag.FormSubmitAction = "Save";
				ViewBag.PageTitleAction = "Edit";

				var placement = Placements.Get(id);

				var vm = Placements.GetViewModel(placement);

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

				var vm = Placements.GetViewModel();

				return View("Edit", vm);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Placement to edit." });
			}

		}

		[System.Web.Http.HttpPost]
		public ActionResult Save(PlacementViewModel model)
		{
			try
			{
				Placement Placement = null;
				var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

				if (ModelState.IsValid)
				{
					Placement = Placements.Save(model);
					flashMessageExtensions.Success("Placement saved", true);
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

					Placements.FillSelectListsForViewModel(model);
					return View("Edit", model);
				}
					
				return RedirectToAction("Edit", new {id = Placement.Id});
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting Placement to edit." });
			}
		}

		

		

    }
}
