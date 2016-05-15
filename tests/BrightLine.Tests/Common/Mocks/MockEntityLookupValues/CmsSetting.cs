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
		internal static void BuildSettings(EntityRepositoryInMemory<CmsSetting> _cmsSettingRepo)
		{
			_cmsSettingRepo.Insert(MockEntities.GetSetting(1, "test setting 1", 1, 1));
			_cmsSettingRepo.Insert(MockEntities.GetSetting(2, "test setting 2", 2, 1));
		}

		private static CmsSetting GetSetting(int settingId, string settingName, int featureId, int settingDefinitionId)
		{
			return new CmsSetting
			{
				Id = settingId,
				Name = settingName,
				CmsSettingDefinition = new CmsSettingDefinition
				{
					Id = settingDefinitionId
				},
				Feature = new Feature
				{
					Id = featureId
				}
			};
		}

		internal static List<CmsSetting> CreateCmsSettings()
		{
			var settings = new List<CmsSetting>();
			var campaign = new Campaign { Id = 1 };

			settings.Add(new CmsSetting
			{
				Id = 1,
				Name = "Setting 1",
				CmsSettingInstance = new CmsSettingInstance { Id = 1 },
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1, Campaign = campaign }, Campaign = campaign },
				CmsSettingDefinition = new CmsSettingDefinition
				{
					Id = 1,
					Name = "Setting 1",
					Fields = new List<CmsField>
					{
						MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1)
					}
				}
			});

			settings.Add(new CmsSetting
			{
				Id = 2,
				Name = "Setting 2",
				CmsSettingInstance = new CmsSettingInstance { Id = 1 },
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1, Campaign = campaign }, Campaign = campaign },
				CmsSettingDefinition = new CmsSettingDefinition
				{
					Id = 1,
					Name = "Setting 2",
					Fields = new List<CmsField>
					{
						MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1)
					}
				}
			});

			settings.Add(new CmsSetting
			{
				Id = 3,
				CmsSettingInstance = new CmsSettingInstance { Id = 1 },
				Name = "Setting 3",
				Feature = new Feature { Id = 2, Creative = new Creative { Id = 2, Campaign = campaign }, Campaign = campaign },
				CmsSettingDefinition = new CmsSettingDefinition
				{
					Id = 2,
					Name = "Setting 3",
					Fields = new List<CmsField>
					{
						MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1)
					}
				}
			});


			return settings;
		}


		public static List<CmsSettingDefinition> CreateCmsSettingDefinitions()
		{
			var recs = new List<CmsSettingDefinition>()
				{
					new CmsSettingDefinition{
						Id = 1,
						Name = "Test 1",
						Display = "Test 1",
						Blueprint = new Blueprint {Id = 10000}
					}
				};

			return recs;
		}


	}

}