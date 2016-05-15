using BrightLine.Common.ViewModels.Developer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	public class MediaBucketsController : Controller
    {
        public ActionResult Index()
        {
			return View();
        }

    }
}
