using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace BrightLine.Web.App_Start
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			var path_vendor_iq_js = "~/Content/app/javascripts/vendor_iq.js";
			var path_vendor_css = "~/Content/app/stylesheets/vendor.css";
			var path_app_css = "~/Content/app/stylesheets/app.css";

			bundles.Add(new ScriptBundle("~/scripts/assets").Include(
						path_vendor_iq_js,
						"~/Content/lib/bootstrap-3.1.1/js/bootstrap.min.js",
						"~/Content/lib/bootstrap-3.1.1/js/Application.js",
						"~/Content/lib/bootstrap-3.1.1/msgGrowl/js/msgGrowl.js",
						"~/Content/lib/bootstrap-3.1.1/msgbox/jquery.msgbox.js",
						"~/Content/lib/datepicker/js/datepicker.js",
						"~/assets/javascripts/analytics.js",
						"~/assets/javascripts/base.js"));

			bundles.Add(new StyleBundle("~/css/assets").Include(
						path_vendor_css,
					    "~/Content/lib/datepicker/css/datepicker/base.css",
						"~/Content/lib/datepicker/css/datepicker/clean.css",
						"~/Content/lib/bootstrap-3.1.1/css/bootstrap.css",
						"~/Content/lib/bootstrap-3.1.1/css/bootstrap-base-admin-3.css",
						"~/Content/lib/bootstrap-3.1.1/msgGrowl/css/msgGrowl.css",
						"~/Content/lib/bootstrap-3.1.1/msgbox/jquery.msgbox.css",
						"~/Content/lib/bootstrap-3.1.1/css/custom.css",
						"~/assets/stylesheets/base.css",
						"~/assets/stylesheets/base_old.css",
						path_app_css));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = true;
		}
	}
}