using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
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
		public static List<CmsModelDefinition> CreateCmsModelDefinitions()
		{
			var cmsModelDefinitions = new List<CmsModelDefinition>();
			cmsModelDefinitions.Add(new CmsModelDefinition { Id = 1, Name = "Test 1", Blueprint = new Blueprint { Id = 1029 } });
			cmsModelDefinitions.Add(new CmsModelDefinition { Id = 2, Name = "Test 2", Blueprint = new Blueprint { Id = 1029 } });

			return cmsModelDefinitions;
		}


		public static CmsModelDefinition GetModelDefinition()
		{
			var field1 = MockEntities.GetCmsFieldForType("What is your favorite prime number", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			field1.Validations.Add(MockEntities.GetValidationForType(1, "445", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MAX_WIDTH));
			field1.Validations.Add(MockEntities.GetValidationForType(2, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MIN_WIDTH));

			var field2 = MockEntities.GetCmsFieldForType("What is your favorite odd number", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			field2.Validations.Add(MockEntities.GetValidationForType(1, "500", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MAX_WIDTH));
			field2.Validations.Add(MockEntities.GetValidationForType(2, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MIN_WIDTH));

			var modelDefinition = new CmsModelDefinition
			{
				Id = 1,
				Fields = new List<CmsField> { field1, field2 }
			};

			return modelDefinition;
		}

		public static void SetModelProperties(ref CmsModel model, string name, int campaignId, int creativeId, int featureId, int modelDefinitionId)
		{
			model.Name = name;
			model.Feature = new Feature
			{
				Id = featureId,
				Creative = new Creative
				{
					Id = creativeId
				},
				Campaign = new Campaign
				{
					Id = campaignId
				}
			};
			model.CmsModelDefinition = new CmsModelDefinition
			{
				Id = modelDefinitionId
			};	
		}


		public static CmsModel GetCmsModel(int id, string name)
		{
			return new CmsModel { Id = id, Name = name };
		}

		internal static List<CmsModel> CreateModels()
		{
			var models = new List<CmsModel>();

			models.Add(new CmsModel
			{
				Id = 1,
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 }, Campaign = MockEntities.CreateCampaign(10, "test", 1) },
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1
				},
				Name = "Test 1"
			});

			models.Add(new CmsModel
			{
				Id = 2,
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 }, Campaign = MockEntities.CreateCampaign(10, "test", 1) },
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1
				},
				Name = "Test 2"
			});

			models.Add(new CmsModel
			{
				Id = 3,
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 }, Campaign = MockEntities.CreateCampaign(10, "test", 1) },
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1
				},
				Name = "Test 3"
			});

			models.Add(new CmsModel
			{
				Id = 4,
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 }, Campaign = MockEntities.CreateCampaign(10, "test", 1) },
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1
				},
				Name = "Test 4"
			});

			models.Add(new CmsModel
			{
				Id = 5,
				Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 }, Campaign = MockEntities.CreateCampaign(10, "test", 1) },
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = 1
				},
				Name = "Test 5"
			});

			return models;

		}

	}

}