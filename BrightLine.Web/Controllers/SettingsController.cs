using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Web.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/

        public ActionResult Index()
        {
			var settings = IoC.Resolve<ISettingsService>();

			var vm = new SettingsViewModel(settings.AllSettings);

			return View(vm);
        }


		[HttpPost]
		public RedirectResult Save(SettingsViewModel model)
		{
			var settings = IoC.Resolve<ISettingsService>();

			var settingKeys = model.Settings.Select(s => s.Key).ToList();
			var settingsHash = settings.Repo.Where(s => settingKeys.Contains(s.Key)).ToDictionary(s => s.Key, s => s);

			foreach (var setting in model.Settings)
			{
				var settingDb = settingsHash[setting.Key];
				settingDb.Value = setting.Value;
			}

			settings.Repo.Save();

			settings.ClearCache();

			return Redirect("Index");
		}

    }
}
