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

		public static ModelInstanceSaveViewModel GetSettingInstanceSaveViewModel(int id, string name, int settingId = 1)
		{
			return new ModelInstanceSaveViewModel
			{
				id = id,
				name = name,
				settingId = settingId,
				fields = new List<FieldSaveViewModel>()
			};
		}

		internal static List<CmsSettingInstance> CreateSettingInstances()
		{
			var settingInstances = new List<CmsSettingInstance>();

			settingInstances.Add(new CmsSettingInstance
			{
				Id = 1,
				Name = "Setting 1",
				Setting_Id = 1,
				Json = JsonConvert.SerializeObject(new ModelInstanceJsonViewModel
				{
					id = 1,
					name = "Setting 1",
					fields = new List<FieldViewModel> { 
						MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "abc", "abcd"),
						MockEntities.GetFieldViewModel(2, "test", FieldTypeConstants.FieldTypeNames.Datetime, "05/20/14", "05/21/14"),
						MockEntities.GetFieldViewModel(3, "test", FieldTypeConstants.FieldTypeNames.Bool, "true", null),
						MockEntities.GetFieldViewModel(4, "test", FieldTypeConstants.FieldTypeNames.Integer, "1234", "12345"),
						MockEntities.GetFieldViewModel(4, "test", FieldTypeConstants.FieldTypeNames.Float, "1234.12", "12345.12") 
					}

				})
			});

			settingInstances.Add(new CmsSettingInstance
			{
				Id = 2,
				Name = "Setting 2",
				Setting_Id = 1,
				Json = JsonConvert.SerializeObject(new ModelInstanceJsonViewModel
				{
					id = 2,
					name = "Setting 2",
					fields = new List<FieldViewModel> { 
						MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "bce", "bcef") 
					}

				})
			});

			settingInstances.Add(new CmsSettingInstance
			{
				Id = 3,
				Name = "Setting 3",
				Setting_Id = 2,
				Json = JsonConvert.SerializeObject(new ModelInstanceJsonViewModel
				{
					id = 3,
					name = "Setting 3",
					fields = new List<FieldViewModel> { 
						MockEntities.GetFieldViewModelForImage(GetFileType()) 
					}

				})
			});

			return settingInstances;
		}

		

	}

}