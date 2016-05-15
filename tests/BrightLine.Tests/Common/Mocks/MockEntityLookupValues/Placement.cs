using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Platform;
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
		/// <summary>
		/// Gets all platforms
		/// </summary>
		/// <returns></returns>
		public static List<Placement> CreatePlacements()
		{
			var recs = new List<Placement>()
				{
					new Placement() {Id = 1, Name = "Test 1"},
					new Placement() {Id = 2, Name = "Test 2"},
					
				};
			return recs;
		}


	}

}