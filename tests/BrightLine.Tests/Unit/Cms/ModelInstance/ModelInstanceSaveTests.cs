using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BrightLine.Tests.Common;
using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Utility;
using BrightLine.Common.ViewModels;
using Newtonsoft.Json;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using Moq;
using System.Web;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ModelInstanceSaveTests
	{
		#region Private Variables

		private CmsModelInstance CmsModelInstance { get; set; }
		private ICmsService CmsService { get; set; }
		private const int USER_ID = 12;
		private const string ROLE = AuthConstants.Roles.Employee;
		private const int TEST_ADVERTISER_ID = 12;
		IocRegistration Container;
		private ModelInstanceLookups ModelInstanceLookups { get;set;}
		private ICmsModelInstanceService CmsModelInstances { get;set;}
		private IModelInstanceSaveService ModelInstanceSaveService { get; set; }
		private IModelInstanceService ModelInstanceService { get; set; }
		private IRepository<CmsModelInstanceField> CmsModelInstanceFields { get; set; }
		private ICreativeService Creatives { get; set; }
		private IRepository<CmsModelInstanceFieldValue> CmsModelInstanceFieldValues { get; set; }

		#endregion //Private Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth(USER_ID, ROLE, 1);
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			CmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			ModelInstanceSaveService = IoC.Resolve<IModelInstanceSaveService>();
			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
			CmsModelInstanceFields = IoC.Resolve<IRepository<CmsModelInstanceField>>();
			Creatives = IoC.Resolve<ICreativeService>();
			CmsModelInstanceFieldValues = IoC.Resolve<IRepository<CmsModelInstanceFieldValue>>();

			CmsModelInstance = MockEntities.GetModelInstance(123, "question", 1, "choices");
			
			CreateCampaign();
			CreativeCreative();
			CreateModel();

			CmsService = IoC.Resolve<ICmsService>();
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "Cms Model Instance saving creates new instance if no instance exists for viewmodel's instance id.")]
		public void Cms_Model_Instance_Saving_Creates_New_Instance()
		{
			var viewModel = CreateViewModelForModelRefFieldValue("1432", 0, 6);

			CmsService.SaveModelInstance(viewModel);

			var modelInstances = CmsModelInstances.GetAll();
			Assert.IsTrue(modelInstances.Count() == 13);
		}

		[Test(Description = "Cms Model Instance saving creates new instance with returned instance id.")]
		public void Cms_Model_Instance_Saving_Creates_New_Instance_With_Returned_Id()
		{
			var viewModel = CreateViewModelForFieldValue("2", 0, 6);

			var boolMessage = CmsService.SaveModelInstance(viewModel);

			var modelInstance = CmsModelInstances.Get(13);
			Assert.IsTrue(modelInstance.Id == boolMessage.EntityId);
		}


		[Test(Description = "Cms Model Instance saving does not create new instance if viewmodel references existing instance id.")]
		public void Cms_Model_Instance_Saving_Does_Not_Create_New_Instance()
		{
			var viewModel = CreateViewModelForModelRefFieldValue("1432", 1);
			var cmsModelInstance = CmsModelInstances.Get(1);
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 1)); 

			CmsService.SaveModelInstance(viewModel);

			var modelInstances = CmsModelInstances.GetAll();
			Assert.IsTrue(modelInstances.Count() == 12);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence insertion for a number field value.")]
		public void Cms_Model_Instance_Sql_Service_Insertion_For_Number()
		{
			SetupCmsFieldForNumberType("What is your favorite prime number");
			var viewModel = CreateViewModelForFieldValue("1432");
			var cmsModelInstance = MockEntities.GetModelInstance(0, "question", 1, "choices");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.NumberValue == int.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence Update for a number field value.")]
		public void Cms_Model_Instance_Sql_Service_Update_For_Number()
		{
			SetupCmsFieldForNumberType("What is your favorite prime number");
			var viewModel = CreateViewModelForFieldValue("1432");
			CreateInstanceFieldAndFieldValue("What is your favorite prime number");
			var cmsModelInstance = MockEntities.GetModelInstance(0, "question", 1, "choices");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.NumberValue == int.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence insertion for a DateTime field value.")]
		public void Cms_Model_Instance_Sql_Service_Insertion_For_DateTime()
		{
			SetupCmsFieldForDateTimeType();
			var viewModel = CreateViewModelForFieldValue("4/5/2014");
			var cmsModelInstance = MockEntities.GetModelInstance(0, "question", 1, "choices");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], FieldTypeConstants.FieldTypeNames.Datetime, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.DateValue == DateTime.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Validate Cms Model Instance Sql Server Persistence Update for a DateTime field value.")]
		public void Cms_Model_Instance_Sql_Service_Update_For_DateTime()
		{
			SetupCmsFieldForDateTimeType();
			var viewModel = CreateViewModelForFieldValue("4/5/2014");
			CreateInstanceFieldAndFieldValue("Which date is your favorite holiday");
			var cmsModelInstance = MockEntities.GetModelInstance(0, "question", 1, "choices");
			cmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], FieldTypeConstants.FieldTypeNames.Datetime, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.DateValue == DateTime.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence insertion for a String field value.")]
		public void Cms_Model_Instance_Sql_Service_Insertion_For_String()
		{
			SetupCmsFieldForStringType();
			var viewModel = CreateViewModelForFieldValue("pileofblah");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.StringValue == GetViewModelFieldValue(viewModel));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence Update for a String field value.")]
		public void Cms_Model_Instance_Sql_Service_Update_For_String()
		{
			SetupCmsFieldForStringType();
			var viewModel = CreateViewModelForFieldValue("pileofblah");
			CreateInstanceFieldAndFieldValue("What is the name of your favorite holiday");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.StringValue == GetViewModelFieldValue(viewModel));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence insertion for a Bool field value.")]
		public void Cms_Model_Instance_Sql_Service_Insertion_For_Bool()
		{
			SetupCmsFieldForBoolType();
			var viewModel = CreateViewModelForFieldValue("true");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], FieldTypeConstants.FieldTypeNames.Bool, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.BoolValue == bool.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Sql Server Persistence update for a Bool field value.")]
		public void Cms_Model_Instance_Sql_Service_Update_For_Bool()
		{
			SetupCmsFieldForBoolType();
			var viewModel = CreateViewModelForFieldValue("true");
			CreateInstanceFieldAndFieldValue("Is your favorite holiday christmas");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetCmsFieldForType("test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], FieldTypeConstants.FieldTypeNames.Bool, 2)); 

			BoolMessageItem boolMessage;
			CmsModelInstanceFieldValue modelInstanceFieldValue;
			SaveModelInstance(viewModel, out boolMessage, out modelInstanceFieldValue);

			Assert.IsTrue(modelInstanceFieldValue.BoolValue == bool.Parse(GetViewModelFieldValue(viewModel)));
			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance Json with a field that is of type refToModel.")]
		public void Cms_Model_Instance_Json_With_RefToModel_Field()
		{
			var instanceId = 123;
			var modelInstanceJsonCompare = MockEntities.GetModelInstanceViewModelWithEmptyFields(instanceId);
			modelInstanceJsonCompare.fields.Add(MockEntities.GetFieldViewModelForRefModel("choices", 9, 1, "choices", "Choices", "A list of choices to choose from for the question", true));
			var modelInstanceSaveVm = CreateViewModelForModelRefFieldValue("choice 1", instanceId);
			CmsModelInstances.Create(CmsModelInstance);
			MockEntities.SetupCmsFieldRefToModelForModelInstance(CmsModelInstance);

			var boolMessage = ModelInstanceSaveService.Save(modelInstanceSaveVm);

			var modelInstance = CmsModelInstances.Get(123);
			var modelInstanceJson = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.Json);
			Assert.IsTrue(JsonConvert.SerializeObject(modelInstanceJson) == JsonConvert.SerializeObject(modelInstanceJsonCompare));
		}

		[Test(Description = "Cms Model Instance Json with a field that is of type refToPage.")]
		public void Cms_Model_Instance_Json_With_RefToPage_Field()
		{
			var instanceId = 123;
			var modelInstanceJsonCompare = MockEntities.GetModelInstanceViewModelWithEmptyFields(instanceId);
			modelInstanceJsonCompare.fields.Add(MockEntities.GetFieldViewModelForRefPage(1, 1, "choices", "Choices", "A list of choices to choose from for the question", true));
			var modelInstanceSaveVm = CreateViewModelForPageRefFieldValue(1, instanceId);
			CmsModelInstances.Create(CmsModelInstance);
			MockEntities.SetupCmsFieldRefToPageForModelInstance(CmsModelInstance);

			var boolMessage = ModelInstanceSaveService.Save(modelInstanceSaveVm);

			var modelInstance = CmsModelInstances.Get(123);
			var modelInstanceJson = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.Json);
			Assert.IsTrue(JsonConvert.SerializeObject(modelInstanceJson) == JsonConvert.SerializeObject(modelInstanceJsonCompare));
		}

		[Test(Description = "Cms Model Instance Json with a field that is of type image.")]
		public void Cms_Model_Instance_Json_With_Image_Field()
		{
			var instanceId = 123;
			var modelInstanceJsonCompare = MockEntities.GetModelInstanceViewModelWithEmptyFields(instanceId);

			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			modelInstanceJsonCompare.fields.Add(MockEntities.GetFieldViewModelForImage(MockEntities.GetFileType(), 1, "apple.jpeg", null, 1, "choices", "Choices", "A list of choices to choose from for the question", true, CmsRefTypeknownId, "known", 1, 
				"test page", "Test Page", 1, "apple"));
			var modelInstanceSaveVm = CreateViewModelForImageFieldValue("1", "apple.jpeg", null, instanceId);
			CmsModelInstances.Create(CmsModelInstance);
			MockEntities.SetupCmsFieldImageForModelInstance(CmsModelInstance);

			var boolMessage = ModelInstanceSaveService.Save(modelInstanceSaveVm);

			var modelInstance = CmsModelInstances.Get(123);
			var modelInstanceJson = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.Json);
			var modelInstanceJsonCompareSerialized = JsonConvert.SerializeObject(modelInstanceJsonCompare);
			var modelInstanceJsonSerialized = JsonConvert.SerializeObject(modelInstanceJson);
			Assert.IsTrue(modelInstanceJsonSerialized == modelInstanceJsonCompareSerialized);
		}

		[Test(Description = "Cms Model Instance with unique validator returns invalid.")]
		public void Cms_Model_Instance_Save_With_Unique_Validator_Is_False()
		{
			var modelInstanceJsonCompare = MockEntities.GetModelInstanceViewModelWithEmptyFields(123);
			modelInstanceJsonCompare.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.Bool, "true"));

			var modelInstanceSaveVm = CreateViewModelForUniqueFieldValue("true", 123, 1);

			//add unique validator to model instance's model definition
			var cmsField = MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], ValidationTypeSystemTypeConstants.BOOL, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], ValidationTypeSystemTypeConstants.BOOL, ValidationTypeNameConstants.UNIQUE, "true");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);

			CmsModelInstances.Create(CmsModelInstance);

			var boolMessage = ModelInstanceService.Save(modelInstanceSaveVm);
			Assert.IsFalse(boolMessage.Success);
			
		}

		[Test(Description = "Cms Model Instance with unique validator returns invalid.")]
		public void Cms_Model_Instance_Save_With_Unique_Validator_Is_True()
		{
			var modelInstanceJsonCompare = MockEntities.GetModelInstanceViewModelWithEmptyFields(123);
			modelInstanceJsonCompare.fields.Add(MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "abcd123"));

			var modelInstanceSaveVm = CreateViewModelForUniqueFieldValue("abcd123", 123, 1);

			//add unique validator to model instance's model definition
			var cmsField = MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], ValidationTypeSystemTypeConstants.STRING, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], ValidationTypeSystemTypeConstants.BOOL, ValidationTypeNameConstants.UNIQUE, "true");
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);

			CmsModelInstances.Create(CmsModelInstance);

			var boolMessage = ModelInstanceService.Save(modelInstanceSaveVm);
			Assert.IsTrue(boolMessage.Success);
		}

		
		#endregion //Tests

		#region Private Methods

		private void SetupCmsFieldForNumberType(string fieldName)
		{
			var cmsField = MockEntities.GetCmsFieldForNumberType(fieldName);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		private void SaveModelInstance(ModelInstanceSaveViewModel viewModel, out Utility.BoolMessageItem boolMessage, out CmsModelInstanceFieldValue modelInstanceFieldValue)
		{
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);
			boolMessage = ModelInstanceSaveService.SaveModelInstance(viewModel, CmsModelInstance);

			var modelInstanceField = CmsModelInstance.Fields.First();
			modelInstanceFieldValue = modelInstanceField.Values.Where(f => f.CmsModelInstanceField == modelInstanceField).Single();
		}

		private void CreativeCreative()
		{
			Creatives.Create(new Creative { Id = 0, Name = "Test", Campaign = new Campaign{Id = 1}, AdType = new AdType{Id = 1} });
		}

		private void CreateModel()
		{
			var cmsModels = IoC.Resolve<IRepository<CmsModel>>();

			cmsModels.Insert(new CmsModel { Id = 0, CmsModelDefinition = MockEntities.GetModelDefinition(), Feature = new Feature { Id = 1, Campaign = new Campaign { Id = 10 }, Creative = new Creative { Id = 1 } } });
		}


		private void CreateCampaign()
		{
			var campaigns = IoC.Resolve<ICampaignService>();

			campaigns.Upsert(MockEntities.CreateCampaign(0, "test", TEST_ADVERTISER_ID));
		}

		private static string GetViewModelFieldValue(ModelInstanceSaveViewModel viewModel)
		{
			return viewModel.fields[0].value[0];
		}

		private static ModelInstanceSaveViewModel CreateViewModelForFieldValue(string fieldValue, int instanceId = 1, int modelId = 1)
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(instanceId, "question", modelId);
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(1, fieldValue));
			return viewModel;
		}

		private static ModelInstanceSaveViewModel CreateViewModelForModelRefFieldValue(string fieldValue, int instanceId = 1, int modelId = 1)
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(instanceId, "question", modelId);
			viewModel.fields.Add(MockEntities.GetFieldSaveVmForRefType("choices", 9));
			return viewModel;
		}

		private static ModelInstanceSaveViewModel CreateViewModelForPageRefFieldValue(int fieldValue, int instanceId = 1)
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(instanceId, "question");
			viewModel.fields.Add(MockEntities.GetFieldSaveVmForPageRef(fieldValue));
			return viewModel;
		}

		private static ModelInstanceSaveViewModel CreateViewModelForUniqueFieldValue(string fieldValue, int instanceId = 1, int fieldId = 1)
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(instanceId, "question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(fieldId, fieldValue));
			return viewModel;
		}

		private static ModelInstanceSaveViewModel CreateViewModelForImageFieldValue(string resourceId = "1", string filename = "test.png", string url = "test/test.png", int instanceId = 1)
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(instanceId, "question");
			viewModel.fields.Add(MockEntities.GetFieldSaveVmForResource(resourceId));
			return viewModel;
		}
		

		private void SetupCmsFieldForDateTimeType()
		{
			var cmsField = MockEntities.GetCmsFieldForType("Which date is your favorite holiday", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], "datetime", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "4/7/2016", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxFloat], ValidationTypeSystemTypeConstants.DATETIME, ValidationTypeNameConstants.MAX));
			cmsField.Validations.Add(MockEntities.GetValidationForType(2, "1/2/1990", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], ValidationTypeSystemTypeConstants.DATETIME, ValidationTypeNameConstants.MIN));

			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		private void SetupCmsFieldForStringType()
		{
			var cmsField = MockEntities.GetCmsFieldForType("What is the name of your favorite holiday", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], "string", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			cmsField.Validations.Add(MockEntities.GetValidationForType(1, "5", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MAX_LENGTH));
			cmsField.Validations.Add(MockEntities.GetValidationForType(2, "2", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));

			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		private void SetupCmsFieldForBoolType()
		{
			var cmsField = MockEntities.GetCmsFieldForType("Is your favorite holiday christmas", (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], "string", 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "true");

			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		private void CreateInstanceFieldAndFieldValue(string fieldName)
		{
			//create cms model instance field
			var cmsModelInstanceField = new CmsModelInstanceField
			{
				Name = fieldName,
				ModelInstance = CmsModelInstance
			};
			CmsModelInstanceFields.Insert(cmsModelInstanceField);

			//create cms model instance field value
			var cmsModelInstanceFieldValue = new CmsModelInstanceFieldValue
			{
				CmsModelInstanceField = cmsModelInstanceField
			};
			CmsModelInstanceFieldValues.Insert(cmsModelInstanceFieldValue);
		}

		#endregion //Private Methods



		
	}
}
