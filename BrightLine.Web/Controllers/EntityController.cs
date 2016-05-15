using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using BrightLine.Web.Helpers;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace BrightLine.Web.Controllers
{
	public class EntityController : BaseController
	{
		[System.Web.Http.HttpGet]
		public ActionResult List(string model)
		{
			var em = EntityManager.GetManager(model);
			var instances = em.GetAll();
			return View("List", instances);
		}

		[System.Web.Http.HttpGet]
		public ActionResult Details(string model, int id)
		{
			var type = ReflectionHelper.TryGetType(model);
			if (type == null)
				throw new TypeLoadException("Could not retrieve details for " + model + " type (Id: " + id + ".");

			var em = EntityManager.GetManager(type);
			var instance = em.Get(id);
			var custom = type.GetCustomAttribute<EntityAttribute>();
			var editor = (custom != null) ? custom.Details ?? "Details" : "Details";
			return View(editor, instance);
		}

		[System.Web.Http.HttpGet]
		[RestoreModelState]
		public ActionResult Create(string model)
		{
			var type = ReflectionHelper.TryGetType(model);
			if (type == null)
				throw new TypeLoadException("Could not edit " + model + " type.");

			var em = EntityManager.GetManager(model);
			var custom = type.GetCustomAttribute<EntityAttribute>();
			var editor = (custom != null) ? custom.Editor ?? "Editor" : "Editor";
			var entity = TempData[model] ?? em.GetOrNew(0);
			return View(editor, entity);
		}

		[System.Web.Http.HttpGet]
		[RestoreModelState]
		public ActionResult Edit(string model, int id)
		{
			var type = ReflectionHelper.TryGetType(model);
			if (type == null)
				throw new TypeLoadException("Could not edit " + model + " type.");

			var em = EntityManager.GetManager(model);
			var custom = type.GetCustomAttribute<EntityAttribute>();
			var editor = (custom != null) ? custom.Editor ?? "Editor" : "Editor";
			var entity = TempData[model] ?? em.GetOrNew(id);
			return View(editor, entity);
		}

		[System.Web.Http.HttpPost]
		[PersistModelState]
		public ActionResult Save(string model)
		{
			var fileHelper = IoC.Resolve<IFileHelper>();
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			int id;
			if (!int.TryParse(Request["Id"], out id))
				return JsonContent(model + " requires an Id.", false);

			var em = EntityManager.GetManager(model);
			var instance = em.GetOrNew(id);
			try
			{
				if (ModelState.IsValid && instance.IsValid())
				{
					var nvc = new NameValueCollection(Request.Form);
					var resource = fileHelper.CreateFile(Request.Files);
					if (em.IsType(typeof(Resource)))
					{
						instance = resource ?? instance; // if the resource is null, probably editing, just want to update props.
						nvc.Remove("Id");
						ReflectionHelper.TrySetProperties(instance, nvc);
					}
					else
					{
						if (resource != null)
							nvc.Add("resource", resource.Id.ToString(CultureInfo.InvariantCulture));

						ReflectionHelper.TrySetProperties(instance, nvc);
					}

					instance = em.Upsert(instance);
					flashMessageExtensions.Success(model + " saved", true);
				}
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				flashMessageExtensions.Error("Error saving " + model, true, ex);
			}

			var returnUrl = Request.Form["ReturnUrl"];
			if (!string.IsNullOrWhiteSpace(returnUrl))
				return Redirect(returnUrl);

			TempData[model] = instance;
			return RedirectToAction("Edit", new { model, id = instance.Id });
		}

		
	}
}