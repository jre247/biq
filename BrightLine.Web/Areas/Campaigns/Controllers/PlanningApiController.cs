using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Framework;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BrightLine.Web.Areas.Campaigns.Controllers
{
	[RoutePrefix("api/campaigns/planning")]
	[CamelCase]
	public class PlanningApiController : ApiController
	{
		[GET("productlines")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public void ProductLines()
		{
			IoC.Log.Error("Product Line is being referenced in the api call to get Product Lines, however, ProductLine entity has been removed from object graph.");
		}
	}
}