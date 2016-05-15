using System.Web.Http;
using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace BrightLine.Web.Areas.Developer
{
	public class DeveloperAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Developer";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRouteLowercase(
				"Developer_default",
				"Developer/{controller}/{action}/{id}",
				new { action = "Index", controller="home", id = UrlParameter.Optional }
			);

			context.Routes.MapHttpRoute(
				name: "NightwatchApi_default",
				routeTemplate: "api/NightwatchTests/{action}",
				defaults: new { controller = "NightwatchTestsApi" }
			);

			context.Routes.MapHttpRoute(
				name: "MediaServerRsyncApi_default",
				routeTemplate: "api/MediaServerRsync/{action}",
				defaults: new { controller = "MediaServerRsyncApi" }
			);
		}
	}
}
