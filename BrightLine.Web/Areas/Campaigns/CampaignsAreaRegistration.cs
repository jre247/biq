using System.Web.Http;
using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace BrightLine.Web.Areas.Campaigns
{
	public class CampaignsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Campaigns";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.MapHttpRoute(
				name: "PlanningApi_default",
				routeTemplate: "api/campaigns/planning/{action}/{id}",
				defaults: new { controller = "PlanningApi", id = UrlParameter.Optional }
			);

			context.MapRouteLowercase(
				"Campaigns_default",
				"campaigns/{action}/{id}",
				new { controller = "Campaigns", action = "Index", id = UrlParameter.Optional },
				constraints: new
				{
					action = "Create|Edit|Save|Details|Implementation|NewLaunch|CreateLaunch|Administer|Creatives|create|edit|save|details|implementation|newlaunch|createlaunch|administer|creatives|RegisterResource|History|Publish"
				});

			context.Routes.MapHttpRoute(
				name: "CampaignsApi_default",
				routeTemplate: "api/campaigns/{action}",
				defaults: new { controller = "CampaignsApi" }
			);

			context.Routes.MapRouteLowercase(
				"CampaignSpaDefault",
				"campaigns",
				defaults: new
				{
					controller = "Base",
					action = "Spa"
				});

			context.MapRouteLowercase(
				"CampaignSpaGeneric",
				"campaigns/{id}/{p1}/{p2}/{p3}/{p4}/{p5}/{p6}/{p7}/{p8}/{p9}/{p10}",
				new
				{
					controller = "Base",
					action = "Spa",
					id = UrlParameter.Optional,
					p1 = UrlParameter.Optional,
					p2 = UrlParameter.Optional,
					p3 = UrlParameter.Optional,
					p4 = UrlParameter.Optional,
					p5 = UrlParameter.Optional,
					p6 = UrlParameter.Optional,
					p7 = UrlParameter.Optional,
					p8 = UrlParameter.Optional,
					p9 = UrlParameter.Optional,
					p10 = UrlParameter.Optional
				},
				null, new[] { "BrightLine.Web.Controllers" }
			);
		}
	}
}