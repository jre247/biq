using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.ValidationType;
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
		public static List<Vertical> GetVerticals()
		{
			var verticals = new List<Vertical>();
			verticals.Add(new Vertical { Id = 1, Name = VerticalConstants.VerticalNames.ConsumerPackagedGood });
			verticals.Add(new Vertical { Id = 2, Name = VerticalConstants.VerticalNames.PharmaAndHealthcare });

			return verticals;
		}
	}

}