using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Core;
using BrightLine.Web.Controllers;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Admin.Controllers
{
	public class AdminController : BaseController
	{
		public AdminController()
			: base()
		{

		}

		public ActionResult ResourceTypes()
		{
			var ResourceTypesRepo = IoC.Resolve <IRepository<ResourceType>>();
			var resourceTypes = ResourceTypesRepo.GetAll();
			return View("ResourceTypes", resourceTypes);
		}
	}
}
