using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.FieldType;
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

		public static List<Setting> CreateSettings()
		{
			var recs = new List<Setting>
			{
				new Setting
				{
					Id = 1,	
					Key = "Test A",
					Value = "Test 1"			
				},
				new Setting
				{
					Id = 2,	
					Key = "Test B",
					Value = "Test 2"				
				}
			};

			return recs;
		}

		

	}

}