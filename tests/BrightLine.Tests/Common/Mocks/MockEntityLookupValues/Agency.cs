using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		
		public static List<Agency> CreateAgencies()
		{
			var items = new List<Agency>()
				{
					new Agency() {Id = Lookups.Agencies.HashByName[AgencyConstants.AgencyNames.BBDO], Name = AgencyConstants.AgencyNames.BBDO},
					new Agency() {Id = Lookups.Agencies.HashByName[AgencyConstants.AgencyNames.CaratNY], Name = AgencyConstants.AgencyNames.CaratNY}
				};
			return items;
		}

		
	}

}