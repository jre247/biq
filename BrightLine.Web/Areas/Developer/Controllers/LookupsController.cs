using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
    public class LookupsController : DeveloperController
    {
        //
        // GET: /Developer/Lookups/

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Refresh()
		{
			try
			{
				Lookups.Refresh();
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not refresh Lookups.", exception: ex);
			}

			return RedirectToAction("Index", "Lookups");
		}

    }
}
