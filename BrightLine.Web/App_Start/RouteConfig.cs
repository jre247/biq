using LowercaseRoutesMVC;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.MapRouteLowercase(
				"SignOut",
				"SignOut",
				new { controller = "Account", action = "SignOut", id = UrlParameter.Optional },
				null, new[] { "BrightLine.Web.Controllers" }
			);

			routes.MapRouteLowercase(
				"SpecificEntity",
				"{action}/{model}/{id}",
				new { controller = "Entity" },
				new { model = "campaign|experience|checklist|creative", action = "details|get|delete|edit|copy", id = "\\d*" },
				 new[] { "BrightLine.Web.Controllers" }
			);

			routes.MapRouteLowercase(
				"GeneralEntity",
				"{action}/{model}",
				new { controller = "Entity" },
				new { model = "campaign|experience|checklist|creative", action = "create|save|list|getall|copy" },
				 new[] { "BrightLine.Web.Controllers" }
			);

			routes.MapRouteLowercase(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Account", action = "Index", id = UrlParameter.Optional }
				, null, new[] { "BrightLine.Web.Controllers" }
			);

			routes.MapRouteLowercase(
				"DefaultSignIn",
				"{controller}/{action}/{id}",
				new { controller = "Account", action = "SignIn", id = UrlParameter.Optional },
				null, new[] { "BrightLine.Web.Controllers" }
			);
		}
	}
}