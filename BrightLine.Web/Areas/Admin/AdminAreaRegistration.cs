using LowercaseRoutesMVC;
using System.Web.Http;
using System.Web.Mvc;

namespace BrightLine.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRouteLowercase(
				"AdminResourceDoc",
				"Admin/resourcedoc/{action}/{id}",
				new
					{
						controller = "ResourceDoc",
						action = "Index",
						id = UrlParameter.Optional
					},
				null,
				new[] { "BrightLine.Web.Controllers" }
				);

			context.MapRouteLowercase(
				"AdminSpecificEntity",
				"admin/{action}/{model}/{id}",
				new { controller = "Entity" },
				new
					{
						model = "[a-zA-Z]+",
						action = "details|get|delete|edit",
						id = "\\d+"
					},
				new[] { "BrightLine.Web.Controllers" }
				);

			context.MapRouteLowercase(
				"AdminGeneralEntity",
				"admin/{action}/{model}",
				new { controller = "Entity" },
				new
					{
						model = "[a-zA-Z]+",
						action = "create|save|list|getall"
					},
				new[] { "BrightLine.Web.Controllers" }
				);

			context.MapRouteLowercase(
				"AdminAgencyEntity",
				"admin/agencies/{action}",
				new { controller = "Agencies" },
				new
				{
					action = "create|save|list"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminSpecificAgency",
				"admin/agencies/{action}/{id}",
				new { controller = "Agencies" },
				new
				{
					action = "edit",
					id = "\\d+"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminMediaPartnerEntity",
				"admin/mediapartners/{action}",
				new { controller = "MediaPartners" },
				new
				{
					action = "create|save|list"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminSpecificMediaPartner",
				"admin/mediaPartners/{action}/{id}",
				new { controller = "MediaPartners" },
				new
				{
					action = "edit",
					id = "\\d+"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminPlacementEntity",
				"admin/placements/{action}",
				new { controller = "Placements" },
				new
				{
					action = "create|save|list"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminSpecificPlacement",
				"admin/placements/{action}/{id}",
				new { controller = "Placements" },
				new
				{
					action = "edit",
					id = "\\d+"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminAppEntity",
				"admin/apps/{action}",
				new { controller = "Apps" },
				new
				{
					action = "create|save|list"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.MapRouteLowercase(
				"AdminSpecificApp",
				"admin/apps/{action}/{id}",
				new { controller = "Apps" },
				new
				{
					action = "edit",
					id = "\\d+"
				},
				new[] { "BrightLine.Web.Controllers" }
			);

			context.Routes.MapHttpRoute(
				name: "AdminApi_default",
				routeTemplate: "api/admin/{action}",
				defaults: new { controller = "AdminApi" }
				);

			context.MapRouteLowercase(
				"Admin_default",
				"Admin/{controller}/{action}/{id}",
				new
					{
						controller = "admin",
						action = "index",
						id = UrlParameter.Optional
					}
				);

			context.MapRouteLowercase(
				"Users",
				"admin/users/{isInternal}",
				new { controller = "Users" },
				new
				{
					controller = "user",
					action = "index"
				}
			);
		}
	}
}
