using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.SqlServer;
using BrightLine.Core;
using BrightLine.Publishing.Html5BrandDestination.Services;
using BrightLine.Publishing.Areas;
using BrightLine.Service;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Common.Services;
using BrightLine.Publishing.Areas.AdResponses.Services;
using BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces;

namespace BrightLine.Publishing
{
	public static partial class Bootstrapper
	{
		public static void InitializeContainer(Container container)
		{

			#region OLTP CrudService Registrations

			container.Register<IPublishService, PublishService>();
			container.Register<IAdResponsesService, AdResponsesService>();
			container.Register<IHtml5BrandDestinationService, Html5BrandDestinationService>();

			#endregion
		}

	}
}