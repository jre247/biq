using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Web.Areas.Developer.Models;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Developer.Controllers
{
	public class CacheController : DeveloperController
	{
		public ActionResult Index()
		{
			var model = GetCacheModel();
			return View(model);
		}

		public ActionResult Refresh(string key)
		{
			IoC.Cache.Refresh(key);
			var model = GetCacheModel();
			return View("Index", model);
		}

		public ActionResult Remove(string key)
		{
			IoC.Cache.Remove(key);
			return RedirectToAction("Index", "Cache");
		}

		public ActionResult Purge(string key, string contains)
		{
			IoC.Cache.Purge();
			return RedirectToAction("Index", "Cache");
		}

		private CacheModel GetCacheModel()
		{
			var model = new CacheModel
			{
				Keys = MemoryCache.Default.OrderBy(o => o.Key).Select(o => o.Key).ToList()
			};

			// Get detailed cache info.
			model.CacheMetaItems = IoC.Cache.GetCacheEntries();

			// Convert timezones 
			// NOTE: Not doing it in ICache because it would introduce a dependency on Datehelper.
			if (model.CacheMetaItems != null && model.CacheMetaItems.Count > 0)
			{
				foreach (var item in model.CacheMetaItems)
				{
					item.Expires = DateHelper.ToUserTimezone(item.Expires);
					item.LastUpdated = DateHelper.ToUserTimezone(item.LastUpdated);
					item.LastAccessTime = DateHelper.ToUserTimezone(item.LastAccessTime);
				}
			}
			return model;
		}
	}
}
