using System.Web.Http;

namespace BrightLine.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Filters.Add(new AuthorizeAttribute());

			config.Routes.MapHttpRoute(
				name: "EntityGetAllApi_default",
				routeTemplate: "api/{action}/{model}",
				defaults: new { controller = "EntityApi", action = "getall|getlookup" }
				);

			config.Routes.MapHttpRoute(
				name: "EntityApi_default",
				routeTemplate: "api/{action}/{model}/{id}",
				defaults: new { controller = "EntityApi", action = "get|copy|delete" }
				);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}",
				defaults: new { controller = "EntityApi" }
				);
		}
	}
}