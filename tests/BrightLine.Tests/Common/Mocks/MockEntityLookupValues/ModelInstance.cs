using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.CmsRefType;
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
		public static CmsModelInstance GetModelInstance(int id = 1, string name = "question", int modelId = 1, string modelName = "choice", string display = "testDisplay")
		{
			return new CmsModelInstance
			{
				Id = id,
				Name = name,
				Display = display,
				Model = new CmsModel
				{
					Id = modelId,
					Name = modelName,
					CmsModelDefinition = new CmsModelDefinition
					{
						Id = 1,
						Fields = new List<CmsField>()
					},
					Feature = new Feature
					{
						Id = 1,
						Campaign = new Campaign
						{
							Id = 10
						},
						Creative = new Creative
						{
							Id = 1
						}
					}
				}
			};
		}

		public static ModelInstanceJsonViewModel GetModelInstanceViewModel(int id = 1, string name = "question")
		{
			var modelInstanceJsonCompare = new ModelInstanceJsonViewModel
			{
				id = id,
				name = name,
				fields = new List<FieldViewModel>
				{
					GetStaticFieldViewModel()
				}
			};
			return modelInstanceJsonCompare;
		}

		public static ModelInstanceJsonViewModel GetModelInstanceViewModelWithEmptyFields(int id = 1)
		{
			var modelInstanceJsonCompare = new ModelInstanceJsonViewModel
			{
				id = id,
				name = "question",
				fields = new List<FieldViewModel>()
			};
			return modelInstanceJsonCompare;
		}


		public static ModelInstanceJsonViewModel GetModelInstanceViewModelWithAddedField(int instanceId = 1, string instanceName = "question", int fieldId = 1, string fieldType = FieldTypeConstants.FieldTypeNames.Integer, string fieldName = "test", string fieldValue = "5")
		{
			var modelInstanceJsonCompare = new ModelInstanceJsonViewModel
			{
				id = instanceId,
				name = instanceName,
				fields = new List<FieldViewModel>
				{
					GetStaticFieldViewModel(),
					GetFieldViewModel(fieldId, fieldName, fieldType, fieldValue)
				}
			};
			return modelInstanceJsonCompare;
		}

		public static ModelInstanceSaveViewModel GetModelInstanceSaveViewModel(int id, string name, int modelId = 1)
		{
			return new ModelInstanceSaveViewModel
			{
				id = id,
				name = name,
				modelId = modelId,
				fields = new List<FieldSaveViewModel>()
			};
		}

		public static void SetupCmsFieldRefToModelForModelInstance(CmsModelInstance cmsModelInstance)
		{
			var cmsField = MockEntities.GetCmsFieldForType("choices", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel], "refToModel", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question");
			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			cmsField.CmsRef = MockEntities.GetCmsRef(1, CmsRefTypeknownId, "known", 1, "choice", "Choice");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		public static void SetupCmsFieldRefToPageForModelInstance(CmsModelInstance cmsModelInstance)
		{
			var cmsField = MockEntities.GetCmsFieldForType("choices", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage], "refToPage", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question");
			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			cmsField.CmsRef = MockEntities.GetCmsRef(1, CmsRefTypeknownId, "known", 1, "choice", "Choice");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

	
		public static void SetupCmsFieldImageForModelInstance(CmsModelInstance cmsModelInstance)
		{
			var cmsField = MockEntities.GetCmsFieldForType("choices", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], FieldTypeConstants.FieldTypeNames.Image, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Both], "Both", "true", true, "Choices", "A list of choices to choose from for the question");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		public static CmsModelInstanceField CreateCmsModelInstanceField(string fieldName, CmsModelInstance cmsModelInstance, int fieldTypeId, int id = 1)
		{
			return new CmsModelInstanceField
			{
				Id = id,
				ModelInstance = cmsModelInstance,
				Name = fieldName,
				FieldType = new FieldType
				{
					Id = fieldTypeId
				}
			};
		}

		public static CmsModelInstanceFieldValue CreateCmsModelInstanceFieldNumberValue(int fieldValue, CmsModelInstanceField field, int id = 1)
		{
			return new CmsModelInstanceFieldValue
			{
				Id = id,
				NumberValue = fieldValue,
				CmsModelInstanceField = field
			};
		}

		public static CmsModelInstanceFieldValue CreateCmsModelInstanceFieldBoolValue(bool fieldValue, CmsModelInstanceField field, int id = 1)
		{
			return new CmsModelInstanceFieldValue
			{
				Id = id,
				BoolValue = fieldValue,
				CmsModelInstanceField = field
			};
		}

		public static CmsModelInstanceFieldValue CreateCmsModelInstanceFieldStringValue(string fieldValue, CmsModelInstanceField field, int id = 1)
		{
			return new CmsModelInstanceFieldValue
			{
				Id = id,
				StringValue = fieldValue,
				CmsModelInstanceField = field
			};
		}

		public static CmsModelInstanceFieldValue CreateCmsModelInstanceFieldDateValue(DateTime fieldValue, CmsModelInstanceField field, int id = 1)
		{
			return new CmsModelInstanceFieldValue
			{
				Id = id,
				DateValue = fieldValue,
				CmsModelInstanceField = field
			};
		}

		public static CmsModelInstance GetCmsModelInstance(int id, string name, int modelId, string modelName = null, string displayName = null, string displayFieldName = "display", int displayFieldTypeId = 1, 
		  string displayFieldTypeName = FieldTypeConstants.FieldTypeNames.Image)
		{
			return new CmsModelInstance{
				Id = id,
				Name = name,
				DisplayName = displayName,
				Model = new CmsModel
				{
					Id = modelId,
					Name = modelName,
					CmsModelDefinition = new CmsModelDefinition
					{
						Id = 1,
						DisplayFieldName = displayFieldName,
						DisplayFieldType = new FieldType { Id = displayFieldTypeId, Name = displayFieldTypeName },
						Fields = new List<CmsField>()
					}
				}					
			};
		}

		public static void InsertModelInstanceInRepository(ModelInstanceJsonViewModel vm, IRepository<CmsModelInstance> cmsModelInstanceRepo, int instanceId, string instanceName = "question", int modelId = 1, string modelName = "choices", int modelDefinitionId = 1)
		{
			var cmsModelInstance = MockEntities.GetModelInstance(instanceId, instanceName, modelId, modelName);
			cmsModelInstance.Json = JsonConvert.SerializeObject(vm);
			cmsModelInstance.Model = new CmsModel { Id = modelId, Name = modelName, Feature = new Feature{ Id = 1, Campaign = new Campaign{ Id = 1}, Creative = new Creative { Id = 1, Campaign = new Campaign{ Id = 1} } }, CmsModelDefinition = new CmsModelDefinition { Id = modelDefinitionId } };
			cmsModelInstanceRepo.Insert(cmsModelInstance);
		}

		public static void InsertModelInstanceInRepository(ModelInstanceJsonViewModel vm, List<CmsModelInstance> cmsModelInstanceRepo, int instanceId, string instanceName = "question", int modelId = 1, string modelName = "choices", int modelDefinitionId = 1)
		{
			var cmsModelInstance = MockEntities.GetModelInstance(instanceId, instanceName, modelId, modelName);
			cmsModelInstance.Json = JsonConvert.SerializeObject(vm);
			cmsModelInstance.Model = new CmsModel { Id = modelId, Name = modelName, Feature = new Feature { Id = 1, Campaign = new Campaign { Id = 1 }, Creative = new Creative { Id = 1, Campaign = new Campaign { Id = 1 } } }, CmsModelDefinition = new CmsModelDefinition { Id = modelDefinitionId, Fields = new List<CmsField>() } };
			cmsModelInstanceRepo.Add(cmsModelInstance);
		}

		public static void CreateModelInstances(IRepository<CmsModelInstance> cmsModelInstanceRepo)
		{
			var vm = MockEntities.GetModelInstanceViewModelWithEmptyFields(1);
			vm.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Bool, "true", "false"));
			MockEntities.InsertModelInstanceInRepository(vm, cmsModelInstanceRepo, 1);

			var vm3 = MockEntities.GetModelInstanceViewModelWithEmptyFields(3);
			vm3.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "orange", "banana"));
			MockEntities.InsertModelInstanceInRepository(vm3, cmsModelInstanceRepo, 3);

			var vm4 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm4.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "apple", "pear"));
			MockEntities.InsertModelInstanceInRepository(vm4, cmsModelInstanceRepo, 4);

			var vm5 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm5.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, "2/3/14", "2/5/14"));
			MockEntities.InsertModelInstanceInRepository(vm5, cmsModelInstanceRepo, 5);

			var vm6 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm6.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, "1/3/15", "1/5/14"));
			MockEntities.InsertModelInstanceInRepository(vm6, cmsModelInstanceRepo, 6);

			var vm7 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm7.fields.Add(MockEntities.GetFieldViewModel(1, "test", "float", "12.2", "14.3"));
			MockEntities.InsertModelInstanceInRepository(vm7, cmsModelInstanceRepo, 7);

			var vm8 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm8.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Integer, "12", "14"));
			MockEntities.InsertModelInstanceInRepository(vm8, cmsModelInstanceRepo, 8);

			var vm9 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm9.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.RefToModel, "model1", "model2"));
			MockEntities.InsertModelInstanceInRepository(vm9, cmsModelInstanceRepo, 9);

			var vm10 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm10.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.RefToModel, "model1", "model2"));
			MockEntities.InsertModelInstanceInRepository(vm10, cmsModelInstanceRepo, 10, "question", 2, "choices");

			var vm11 = MockEntities.GetModelInstanceViewModelWithEmptyFields(11);
			vm11.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.Image, "apple.jpeg", "banana.png"));
			MockEntities.InsertModelInstanceInRepository(vm11, cmsModelInstanceRepo, 11, "question", 2, "choices");

			var vm12 = MockEntities.GetModelInstanceViewModelWithEmptyFields(12);
			vm12.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.Image, "orange.jpeg", "grapefruit.png"));
			MockEntities.InsertModelInstanceInRepository(vm12, cmsModelInstanceRepo, 12, "question", 2, "choices");
		}

		public static List<CmsModelInstance> CreateModelInstances()
		{
			var cmsModelInstanceRepo = new List<CmsModelInstance>();

			var vm = MockEntities.GetModelInstanceViewModelWithEmptyFields(1);
			vm.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Bool, "true", "false"));
			vm.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Bool, null, null));
			MockEntities.InsertModelInstanceInRepository(vm, cmsModelInstanceRepo, 1);

			var vm2 = MockEntities.GetModelInstanceViewModelWithEmptyFields(3);
			vm2.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "grapefruit", "blueberries"));
			vm2.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, null, "blueberries2"));
			MockEntities.InsertModelInstanceInRepository(vm2, cmsModelInstanceRepo, 2);

			var vm3 = MockEntities.GetModelInstanceViewModelWithEmptyFields(3);
			vm3.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "orange", "banana"));
			vm3.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, null, "banana2"));
			MockEntities.InsertModelInstanceInRepository(vm3, cmsModelInstanceRepo, 3);

			var vm4 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm4.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "apple", "pear"));
			vm4.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, null, "pear2"));
			MockEntities.InsertModelInstanceInRepository(vm4, cmsModelInstanceRepo, 4);

			var vm5 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm5.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, "2/3/14", "2/5/14"));
			vm5.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, null, "2/16/14"));
			MockEntities.InsertModelInstanceInRepository(vm5, cmsModelInstanceRepo, 5);

			var vm6 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm6.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, "1/3/15", "1/5/14"));
			vm6.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Datetime, null, "1/25/14"));
			MockEntities.InsertModelInstanceInRepository(vm6, cmsModelInstanceRepo, 6);

			var vm7 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm7.fields.Add(MockEntities.GetFieldViewModel(1, "test", "float", "12.2", "14.3"));
			vm7.fields.Add(MockEntities.GetFieldViewModel(1, "test", "float", null, "14.23"));
			MockEntities.InsertModelInstanceInRepository(vm7, cmsModelInstanceRepo, 7);

			var vm8 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm8.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Integer, "12", "14"));
			vm8.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Integer, null, "15"));
			MockEntities.InsertModelInstanceInRepository(vm8, cmsModelInstanceRepo, 8);

			var vm9 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm9.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.RefToModel, "model1", "model2"));
			vm9.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.RefToModel, null, "model3"));
			MockEntities.InsertModelInstanceInRepository(vm9, cmsModelInstanceRepo, 9);

			var vm10 = MockEntities.GetModelInstanceViewModelWithEmptyFields(4);
			vm10.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.RefToModel, "model1", "model2"));
			MockEntities.InsertModelInstanceInRepository(vm10, cmsModelInstanceRepo, 10, "question", 2, "choices");

			var vm11 = MockEntities.GetModelInstanceViewModelWithEmptyFields(11);
			vm11.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.Image, "apple.jpeg", "banana.png"));
			MockEntities.InsertModelInstanceInRepository(vm11, cmsModelInstanceRepo, 11, "question", 2, "choices");

			var vm12 = MockEntities.GetModelInstanceViewModelWithEmptyFields(12);
			vm12.fields.Add(MockEntities.GetFieldViewModel(1, "choices", FieldTypeConstants.FieldTypeNames.Image, "orange.jpeg", "grapefruit.png"));
			MockEntities.InsertModelInstanceInRepository(vm12, cmsModelInstanceRepo, 12, "question", 2, "choices");

			return cmsModelInstanceRepo;
		}
	}

}