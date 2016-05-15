using BrightLine.Common.Framework;
using BrightLine.Common.Resources;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using BrightLine.Common.Utility;
using Common.Logging;
using BrightLine.Common.Services;

namespace BrightLine.Web.Controllers
{
	[CompressAttribute]
	public class BaseController : Controller
	{
		protected ILog Logger { get;set;}
		protected IFileHelper FileHelper { get; set; }
		protected IFlashMessageExtensions FlashMessage { get; set; }

		public BaseController()
		{
			var logHelper = IoC.Resolve<ILogHelper>();
			var fileHelper = IoC.Resolve<IFileHelper>();
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			Logger = logHelper.GetLogger();
			FileHelper = fileHelper;
			FlashMessage = flashMessageExtensions;
		}

		protected ActionResult ReturnInaccessible(int id, string action = "Index", string controller = "Campaigns")
		{
			IoC.Log.Warn("Inaccessible campaign: " + id + ". User: " + Auth.UserName);
			return RedirectToAction(action, controller).Error("The requested campaign is inaccessible.");
		}

		protected ContentResult JsonContent(object content, bool success = true, string message = "")
		{
			var response = new JsonResponse
				{
					Data = content,
					Success = success,
					Message = message
				};

			var settings = new JsonSerializerSettings()
				{
					NullValueHandling = NullValueHandling.Include,
					PreserveReferencesHandling = PreserveReferencesHandling.None,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				};
			return Content(JsonConvert.SerializeObject(response, settings), "application/json");
		}

		protected ContentResult JsonContentRaw(string content, bool format)
		{
			if (string.IsNullOrEmpty(content))
			{
				return Content("Data not available for current campaign");
			}
			if (format)
			{
				string json = content;
				JToken jt = JToken.Parse(json);
				string formatted = jt.ToString(Newtonsoft.Json.Formatting.Indented);
				return Content(formatted, "application/json");
			}
			return Content(content, "application/json");
		}

		public ActionResult Spa()
		{
			return View();
		}

		public void AddGenericError()
		{
			ModelState.AddModelError(CommonResources.GenericErrorKey, CommonResources.GenericErrorMessage);
		}
	}
}