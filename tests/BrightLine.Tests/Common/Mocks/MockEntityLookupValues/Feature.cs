using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Expose;
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
		public static List<Feature> GetFeatures(int creativeId = 1)
		{
			var campaigns = IoC.Resolve<ICampaignService>();
			var campaign = campaigns.Get(1);

			var recs = new List<Feature>()
				{
					new Feature() 
					{
						Id = 1, 
						Campaign = campaign, 
						Creative = new Creative
						{
							 Id = creativeId
						},
						//Pages = ps1, 
						Name = "Hello", 
						ButtonName = "Lovely World", 
						Blueprint = new Blueprint
						{
							Id = 1,
							Name = "test blueprint 1",
							ModelDefinitions = new List<CmsModelDefinition>
							{
								new CmsModelDefinition
								{
									Id = 1,
									Name = "model 1",
									DisplayFieldName = "field 1",
									DisplayFieldType = new FieldType{ Id = (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], Name = FieldTypeConstants.FieldTypeNames.Image},
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								},
								new CmsModelDefinition
								{
									Id = 2,
									Name = "model 2",
									DisplayFieldName = "field 2",
									DisplayFieldType = new FieldType{ Id = (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], Name = FieldTypeConstants.FieldTypeNames.Image},
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								}
							},
							SettingDefinitions = new List<CmsSettingDefinition>
							{
								new CmsSettingDefinition
								{
									Id = 1,
									Name = "setting 1",
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								}
							}
						}
					},
					new Feature() 
					{
						Id = 2, 
						Creative = new Creative
						{
							 Id = creativeId
						},
						Campaign = campaign, 
						//Pages = ps2, 
						Name = "Goodbye", 
						ButtonName = "Cruel World",
						Blueprint = new Blueprint
						{
							Id = 2,
							Name = "test blueprint 2",
							ModelDefinitions = new List<CmsModelDefinition>
							{
								new CmsModelDefinition
								{
									Id = 3,
									Name = "model 3",
									DisplayFieldName = "field 3",
									DisplayFieldType = new FieldType{ Id = (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], Name = FieldTypeConstants.FieldTypeNames.Image},
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								},
								new CmsModelDefinition
								{
									Id = 4,
									Name = "model 4",
									DisplayFieldName = "field 4",
									DisplayFieldType = new FieldType{ Id = (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], Name = FieldTypeConstants.FieldTypeNames.Image},
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}

								},
								new CmsModelDefinition
								{
									Id = 5,
									Name = "model 5",
									DisplayFieldName = "field 5",
									DisplayFieldType = new FieldType{ Id = (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], Name = FieldTypeConstants.FieldTypeNames.Image},
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 5", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								}
							},
							SettingDefinitions = new List<CmsSettingDefinition>
							{
								new CmsSettingDefinition
								{
									Id = 2,
									Name = "setting 2",
									Fields = new List<CmsField>{
										MockEntities.GetCmsFieldForType("field 1", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 2", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),
										MockEntities.GetCmsFieldForType("field 3", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 3, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question"),										
									}
								}  
							}
						}
					},
					new Feature() 
					{
						Id = 3, 
						Campaign = campaign, 
						//Pages = ps3, 
						Name = "Nice", 
						ButtonName = "Meet us",
						Blueprint = new Blueprint
						{
							Id = 3
						}
					},
				};
			return recs;
		}

		public static List<FeatureType> CreateFeatureTypes()
		{
			var recs = new List<FeatureType>()
				{
					new FeatureType{
						Id = 1,
						Name = "Test 1",
						Display = "Test 1",
						FeatureTypeGroup = new FeatureTypeGroup{
							Id = 1
						}
					}
				};

			return recs;
		}

		public static List<FeatureTypeGroup> CreateFeatureTypeGroups()
		{
			var recs = new List<FeatureTypeGroup>()
				{
					new FeatureTypeGroup{
						Id = 1,
						Name = "Test 1",
						Display = "Test 1",
						
					}
				};

			return recs;
		}

		public static Feature BuildFeature(int featureId, string featureName, int creativeId, int campaignId = 1)
		{
			return new Feature { Id = featureId, Name = featureName, Models = new List<CmsModel>(), Creative = new Creative { Id = creativeId, Campaign = new Campaign { Id = campaignId } } };
		}
	}

}