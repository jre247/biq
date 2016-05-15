using AttributeRouting.Web.Constraints;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Models.Enums;
using BrightLine.Web.Controllers;
using System.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BrightLine.Web.AttributeRoutingConfig), "Start")]
namespace BrightLine.Web
{
	public static class AttributeRoutingConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd
			routes.MapAttributeRoutes(
				config =>
				{
					try
					{
						config.AddRoutesFromAssemblyOf<BaseController>();
						config.InlineRouteConstraints.Add("DeleteTypes", typeof(EnumRouteConstraint<DeleteTypes>));
						config.InlineRouteConstraints.Add("AnalyticsInterval", typeof(EnumRouteConstraint<AnalyticsInterval>));
					}
					catch (System.Reflection.ReflectionTypeLoadException ex)
					{
						AttributeRoutingHttpConfig.LogMissingType(ex);
					}
				});
		}

		public static void Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}
	}
}
