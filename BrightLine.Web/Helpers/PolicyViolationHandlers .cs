using FluentSecurity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Web.Helpers
{
	public class DenyAnonymousAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			var uri = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery);
			return new RedirectToRouteResult(
				new RouteValueDictionary(new
				{
					area = "",
					controller = "Account",
					action = "Index",
					redirect = uri
				})
			);
		}
	}

	public class DenyAuthenticatedAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			return new RedirectToRouteResult(
				new RouteValueDictionary(new
				{
					area = "",
					controller = "Account",
					action = "Redirect"
				})
			);
		}
	}
}