using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Developer;
using BrightLine.Web.Areas.Developer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	public class AdsController : DeveloperController
    {
        //
        // GET: /Ads/

        public ActionResult Index()
        {
			return View();
        }

		public ActionResult CleanupAds()
		{
			try
			{
				var ads = IoC.Resolve<IAdService>();
				ads.CleanupDirtyAds();
			}
			catch (Exception ex)
			{
				IoC.Log.Error("Could not cleanup Ads.", exception: ex);
				return View("Index");
			}

			return View("Index");
		}

    }
}
