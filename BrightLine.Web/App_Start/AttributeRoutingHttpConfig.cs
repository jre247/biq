using AttributeRouting.Web.Http.WebHost;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Models.Enums;
using BrightLine.Web.Controllers;
using System.Web.Http;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BrightLine.Web.AttributeRoutingHttpConfig), "Start")]

namespace BrightLine.Web
{
	public static class AttributeRoutingHttpConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes)
		{
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd
			routes.MapHttpAttributeRoutes(
				config =>
				{
					try
					{
						config.AddRoutesFromAssemblyOf<BaseController>();
						config.InlineRouteConstraints.Add("DeleteTypes", typeof(AttributeRouting.Web.Constraints.EnumRouteConstraint<DeleteTypes>));
						config.InlineRouteConstraints.Add("AnalyticsInterval", typeof(AttributeRouting.Web.Constraints.EnumRouteConstraint<AnalyticsInterval>));
					}
					catch (System.Reflection.ReflectionTypeLoadException ex)
					{
						LogMissingType(ex);
					}
				});
		}

		public static void Start()
		{
			RegisterRoutes(GlobalConfiguration.Configuration.Routes);
		}

		internal static void LogMissingType(System.Reflection.ReflectionTypeLoadException rtlex)
		{
			var sb = new System.Text.StringBuilder("Top TypeLoadException:" + rtlex.Message);
			sb.AppendLine();
			if (rtlex.LoaderExceptions == null)
			{
				BrightLine.Utility.Log.Debug("No file info:" + sb.ToString());
				return;
			}

			foreach (var innerException in rtlex.LoaderExceptions)
			{
				sb.AppendLine(innerException.Message);
				var fex = innerException as System.IO.FileNotFoundException;
				if (fex != null && !string.IsNullOrEmpty(fex.FusionLog))
				{
					sb.AppendLine("Fusion Log:");
					sb.AppendLine(fex.FusionLog);
				}
				sb.AppendLine();
			}
			string errorMessage = sb.ToString();
			BrightLine.Utility.Log.Debug(errorMessage);
		}
	}
}