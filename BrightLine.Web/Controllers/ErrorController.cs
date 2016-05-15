using BrightLine.Common.Framework;
using System.Web.Mvc;

namespace BrightLine.Web.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult Index()
		{
			return View("Error");
		}

		public ActionResult Error()
		{
			return View();
		}

		public ActionResult ResourceNotFoundError()
		{
			return View();
		}

		public ActionResult HttpError404()
		{
			return View("ResourceNotFoundError");
		}

		public ActionResult HttpError505()
		{
			return View("Error");
		}

		public void LogJavaScriptError(string message)
		{
			var jsex = new JavaScriptException(message);
			IoC.Log.Error(jsex);
		}
	}
}
