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
    public class MediaPartnersController : BaseController
    {
		private IMediaPartnerService MediaPartners { get;set;}

		public MediaPartnersController()
		{
			MediaPartners = IoC.Resolve<IMediaPartnerService>();
		}

		[System.Web.Http.HttpGet]
		public ActionResult List()
		{
			var mediaPartners = MediaPartners.GetAll().ToList().Select(c => new MediaPartnerViewModel(c)).ToList();
			
			return View("List", mediaPartners);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Edit(int id)
		{
			try
			{
				ViewBag.FormSubmitAction = "Save";
				ViewBag.PageTitleAction = "Edit";

				var mediaPartner = MediaPartners.Get(id);
						
				var vm = MediaPartners.GetViewModel(mediaPartner);

				return View(vm);
				
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting mediaPartner to edit." });
			}
		}

		[System.Web.Http.HttpGet]
		public ActionResult Create()
		{
			try
			{
				ViewBag.FormSubmitAction = "Create";
				ViewBag.PageTitleAction = "Create";

				var vm = MediaPartners.GetViewModel();

				return View("Edit", vm);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting mediaPartner to edit." });
			}

		}

		[System.Web.Http.HttpPost]
		public ActionResult Save(MediaPartnerViewModel model)
		{
			try
			{
				MediaPartner mediaPartner = null;
				var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

				if (ModelState.IsValid)
				{
					mediaPartner = MediaPartners.Save(model);
					flashMessageExtensions.Success("Media Partner saved", true);
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

					MediaPartners.FillSelectListsForViewModel(model);
					return View("Edit", model);
				}
					
				return RedirectToAction("Edit", new {id = mediaPartner.Id});
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error getting mediaPartner to edit." });
			}
		}

		

		

    }
}
