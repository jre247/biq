using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BrightLine.Tests.Common;


using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using Newtonsoft.Json;
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ModelInstanceRetrievalTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		IocRegistration Container;
		private const int USER_ID = 12;
		private const string ROLE = AuthConstants.Roles.AgencyPartner;
		public const int TEST_ADVERTISER_ID = 12;
		IModelInstanceService ModelInstanceService { get;set;}
		ICmsModelInstanceService CmsModelInstances { get; set; }
		IRepository<CmsModelInstanceField> CmsModelInstanceFields { get; set; }
		IRepository<CmsModelInstanceFieldValue> CmsModelInstanceFieldValues { get; set; }

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth(USER_ID, ROLE);

			Container = MockUtilities.SetupIoCContainer(Container);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
			CmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			CmsModelInstanceFields = IoC.Resolve<IRepository<CmsModelInstanceField>>();
			CmsModelInstanceFieldValues = IoC.Resolve<IRepository<CmsModelInstanceFieldValue>>();

			CmsModelInstance = MockEntities.GetCmsModelInstance(123, "question", 1);
		}


		[Test(Description = "Retrieving an existing Model Instance is not null.")]
		public void Cms_Model_Instance_Retrieval_Against_Existing_Instance_Is_Not_Null()
		{
			var cmsService = new CmsService();
			var vm = MockEntities.GetModelInstanceViewModel();
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123);

			Assert.IsNotNull(modelInstance);
		}

		[Test(Description = "Retrieving a Model Instance is not null when it has a field with its expose property set to 'server').")]
		public void Cms_Model_Instance_Retrieval_With_Field_Expose_Server_Is_Not_Null()
		{
			var fieldName = "random choice";
			int fieldValue = 5;
			var cmsService = new CmsService();
			SetupCmsFieldForNumberType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModel();
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForNumber(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123);

			Assert.IsNotNull(modelInstance);
		}

		[Test(Description = "Retrieving a Model Instance has correct number of fields returned when this instance has a field with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_Without_Field_Expose_Server_Has_Correct_Fields_Count()
		{
			var cmsService = new CmsService();
			var vm = MockEntities.GetModelInstanceViewModel(123, "question");
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			Assert.IsTrue(modelInstanceDeserialized.fields.Count() == 1);
		}

		[Test(Description = "Validating base level properties for Model Instance with a field that has its expose property set to 'server' and this field value is of type number).")]
		public void Cms_Model_Instance_Retrieval_Field_Expose_Server_Has_Valid_Base_Level_Properties()
		{
			var fieldName = "random choice";
			int fieldValue = 5;
			var cmsService = new CmsService();
			SetupCmsFieldForNumberType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, FieldTypeConstants.FieldTypeNames.Integer, fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForNumber(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			Assert.IsTrue(modelInstanceDeserialized.id == CmsModelInstance.Id);
			Assert.IsTrue(modelInstanceDeserialized.name == CmsModelInstance.Name);
		}

		[Test(Description = "Retrieving a Model Instance has correct number of fields returned when this instance has a field with its expose property set to 'server' or 'both'.")]
		public void Cms_Model_Instance_Retrieval_With_Field_Expose_Server_Has_Correct_Fields_Count()
		{
			var fieldName = "random choice";
			int fieldValue = 5;
			var cmsService = new CmsService();
			SetupCmsFieldForNumberType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, FieldTypeConstants.FieldTypeNames.Integer, fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForNumber(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			Assert.IsTrue(modelInstanceDeserialized.fields.Count() == 2);
		}

		[Test(Description = "Retrieving a Model Instance that has field of type number and with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_Has_Valid_Number_Field_With_Expose_Server()
		{
			var fieldName = "random choice";
			int fieldValue = 5;
			var cmsService = new CmsService();
			SetupCmsFieldForNumberType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, FieldTypeConstants.FieldTypeNames.Integer, fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForNumber(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[1];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer);
			AssertFieldProperties(fieldValue, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance has a field with type bool with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_Has_Valid_Bool_Field_With_Expose_Server()
		{
			var fieldName = "random choice";
			var fieldValue = true;
			var cmsService = new CmsService();
			SetupCmsFieldForBoolType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "bool", fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForBool(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[1];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], FieldTypeConstants.FieldTypeNames.Bool);
			AssertFieldProperties(fieldValue, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance has a field of type string with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_Has_Valid_String_Field_With_Expose_Server()
		{
			var fieldName = "random choice";
			var fieldValue = "apple";
			var cmsService = new CmsService();
			SetupCmsFieldForStringType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "string", fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForString(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[1];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String);
			AssertFieldProperties(fieldValue, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance has a field of type date with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_Has_Valid_Date_Field_With_Expose_Server()
		{
			var fieldName = "random choice";
			var fieldValue = new DateTime(2014, 11, 2);
			var cmsService = new CmsService();
			SetupCmsFieldForDateType(fieldName);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "datetime", fieldName, fieldValue.ToString());
			InsertModelInstanceInRepository(vm);
			CreateInstanceFieldAndInstanceValueForDate(fieldName, fieldValue);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[1];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], FieldTypeConstants.FieldTypeNames.Datetime);
			AssertFieldProperties(fieldValue, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance that has multiple fields already and also has a field of type date with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_With_Multiple_Instance_Fields_Has_Valid_Date_Field_With_Expose_Server()
		{
			var cmsService = new CmsService();
			var fieldValue = new DateTime(2014, 11, 2);
			var fieldName = "random choice";
			SetupCmsFieldForDateType(fieldName);
			CreateInstanceFieldAndInstanceValueForDate(fieldName, fieldValue);
			var fieldName2 = "random choice 2";
			SetupCmsFieldForDateType(fieldName2);
			var fieldValue2 = new DateTime(2013, 10, 3);
			CreateInstanceFieldAndInstanceValueForDate(fieldName2, fieldValue2, 2);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "datetime", fieldName, fieldValue.ToString());
			vm.fields.Add(MockEntities.GetFieldViewModel(2, fieldName2, "datetime", fieldValue2.ToString()));
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[2];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName2, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], FieldTypeConstants.FieldTypeNames.Datetime, 2);
			AssertFieldProperties(fieldValue2, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance that has multiple fields already and also has a field of type number with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_With_Multiple_Instance_Fields_Has_Valid_Number_Field_With_Expose_Server()
		{
			var cmsService = new CmsService();
			var fieldValue = 5;
			var fieldName = "random choice";
			SetupCmsFieldForNumberType(fieldName);
			CreateInstanceFieldAndInstanceValueForNumber(fieldName, fieldValue);
			var fieldName2 = "random choice 2";
			SetupCmsFieldForNumberType(fieldName2);
			var fieldValue2 = 6;
			CreateInstanceFieldAndInstanceValueForNumber(fieldName2, fieldValue2, 2);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, FieldTypeConstants.FieldTypeNames.Integer, fieldName, fieldValue.ToString());
			vm.fields.Add(MockEntities.GetFieldViewModel(2, fieldName2, FieldTypeConstants.FieldTypeNames.Integer, fieldValue2.ToString()));
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[2];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName2, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 2);
			AssertFieldProperties(fieldValue2, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance that has multiple fields already and also has a field of type string with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_With_Multiple_Instance_Fields_Has_Valid_String_Field_With_Expose_Server()
		{
			var cmsService = new CmsService();
			var fieldValue = "apple";
			var fieldName = "random choice";
			SetupCmsFieldForStringType(fieldName);
			CreateInstanceFieldAndInstanceValueForString(fieldName, fieldValue);
			var fieldName2 = "random choice 2";
			SetupCmsFieldForStringType(fieldName2);
			var fieldValue2 = "blueberries";
			CreateInstanceFieldAndInstanceValueForString(fieldName2, fieldValue2, 2);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "string", fieldName, fieldValue.ToString());
			vm.fields.Add(MockEntities.GetFieldViewModel(2, fieldName2, "string", fieldValue2.ToString()));
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[2];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName2, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String, 2);
			AssertFieldProperties(fieldValue2, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance that has multiple fields already and also has a field of type bool with its expose property set to 'server'.")]
		public void Cms_Model_Instance_Retrieval_With_Multiple_Instance_Fields_Has_Valid_Bool_Field_With_Expose_Server()
		{
			var cmsService = new CmsService();
			var fieldValue = true;
			var fieldName = "random choice";
			SetupCmsFieldForBoolType(fieldName);
			CreateInstanceFieldAndInstanceValueForBool(fieldName, fieldValue);
			var fieldName2 = "random choice 2";
			SetupCmsFieldForBoolType(fieldName2);
			var fieldValue2 = false;
			CreateInstanceFieldAndInstanceValueForBool(fieldName2, fieldValue2, 2);
			var vm = MockEntities.GetModelInstanceViewModelWithAddedField(123, "question", 1, "bool", fieldName, fieldValue.ToString());
			vm.fields.Add(MockEntities.GetFieldViewModel(2, fieldName2, "bool", fieldValue2.ToString()));
			InsertModelInstanceInRepository(vm);

			var modelInstance = CmsModelInstances.Get(123).Json;
			var modelInstanceDeserialized = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(modelInstance.ToString());

			var field = modelInstanceDeserialized.fields[2];
			var fieldCompare = MockEntities.GetCmsFieldForType(fieldName2, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], FieldTypeConstants.FieldTypeNames.Bool, 2);
			AssertFieldProperties(fieldValue2, field, fieldCompare);
		}

		[Test(Description = "Retrieving a Model Instance list not null.")]
		public void Cms_Model_Instance_List_Retrieval_Not_Null()
		{
			var modelInstances = ModelInstanceService.GetModelInstancesForModel(1);

			Assert.IsNotNull(modelInstances);
		}

		[Test(Description = "Retrieving a Model Instance list is null.")]
		public void Cms_Model_Instance_List_Retrieval_Is_Null()
		{
			var modelInstances = ModelInstanceService.GetModelInstancesForModel(21234);

			Assert.IsNull(modelInstances);
		}

		[Test(Description = "Retrieving a Model Instance list has correct count.")]
		public void Cms_Model_Instance_List_Retrieval_Has_Correct_Count()
		{
			var modelInstances = ModelInstanceService.GetModelInstancesForModel(1);

			Assert.IsTrue(modelInstances.Count() == 9);
		}

		[Test(Description = "Retrieving a Model Instance list has item with correct properties.")]
		public void Cms_Model_Instance_List_Retrieval_Has_Correct_Item_Properties()
		{
			var modelInstance = ModelInstanceService.GetModelInstancesForModel(1).ElementAt(0);

			Assert.IsTrue(modelInstance.Key == 1, "Model Instance key is not correct.");
			Assert.IsTrue(modelInstance.Value.id == 1, "Model Instance id is not correct.");
			Assert.IsTrue(modelInstance.Value.name == "question", "Model Instance name is not correct.");
			Assert.IsTrue(modelInstance.Value.display == "testDisplay", "Model Instance display is not correct.");
		}

		[Test(Description = "Retrieving a Model Instance verbose list has correct properties.")]
		public void Cms_Model_Instance_Verbose_List_Retrieval_Has_Correct_Item_Properties()
		{
			var cmsModels = IoC.Resolve<IRepository<CmsModel>>();

			var cmsModelInstance = MockEntities.GetCmsModelInstance(123, "test1", 14, "model1", "testDisplay", "apple");
			cmsModelInstance.Json = JsonConvert.SerializeObject(MockEntities.GetModelInstanceViewModel(123));
			CmsModelInstances.Create(cmsModelInstance);
			cmsModels.Insert(MockEntities.GetCmsModel(14, "model1"));

			var modelInstance = ModelInstanceService.GetModelInstancesForModel(14, true).ElementAt(0);

			Assert.IsTrue(modelInstance.Key == 123, "Model Instance key is not correct.");
			Assert.IsTrue(modelInstance.Value.id == 123, "Model Instance id is not correct.");
			Assert.IsTrue(modelInstance.Value.name == "test1", "Model Instance name is not correct.");
			Assert.IsTrue(modelInstance.Value.display == "testDisplay", "Model Instance display is not correct.");
			Assert.IsTrue(modelInstance.Value.displayField == "apple", "Model Instance displayField is not correct.");
			Assert.IsTrue(modelInstance.Value.displayFieldType == "image", "Model Instance displayFieldType is not correct.");
		}

		[Test(Description = "Retrieving a Model Instance raw list does not have verbose properties.")]
		public void Cms_Model_Instance_Raw_List_Retrieval_Has_Null_DisplayField_And_DisplayFieldType()
		{
			var modelInstance = ModelInstanceService.GetModelInstancesForModel(1, false).ElementAt(0);

			Assert.IsNull(modelInstance.Value.displayField, "Model Instance displayField is not null.");
			Assert.IsNull(modelInstance.Value.displayFieldType, "Model Instance displayField is not null.");
		}


		#region Private Methods

		private static void AssertFieldProperties<T>(T fieldValue, FieldViewModel field, CmsField fieldCompare)
		{
			Assert.IsTrue(field.id == fieldCompare.Id, "fields don't match on id property.");
			Assert.IsTrue(field.name == fieldCompare.Name, "fields don't match on name property.");
			Assert.IsTrue(field.values[0].ToString() == fieldValue.ToString(), "fields don't match on value property.");
			Assert.IsTrue(field.type == fieldCompare.Type.Name, "fields don't match on type property.");
			Assert.IsNotNull(field.type, "field type is null.");
			Assert.IsTrue(field.list == fieldCompare.List, "fields don't match on list property.");
			Assert.IsTrue(field.validations.Count() == fieldCompare.Validations.Count(), "fields don't match on validations count."); //TODO: maybe validate all validation objs as well?
		}

		//make reusable function since it's referenced elsewhere
		private void SetupCmsFieldForNumberType(string fieldName)
		{
			var cmsField = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		//make reusable function since it's referenced elsewhere
		private void SetupCmsFieldForBoolType(string fieldName)
		{
			var cmsField = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], FieldTypeConstants.FieldTypeNames.Bool);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		//make reusable function since it's referenced elsewhere
		private void SetupCmsFieldForStringType(string fieldName)
		{
			var cmsField = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], FieldTypeConstants.FieldTypeNames.String);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		//make reusable function since it's referenced elsewhere
		private void SetupCmsFieldForDateType(string fieldName)
		{
			var cmsField = MockEntities.GetCmsFieldForType(fieldName, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], FieldTypeConstants.FieldTypeNames.Datetime);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(cmsField);
		}

		private void CreateInstanceFieldAndInstanceValueForBool(string fieldName, bool fieldValue, int id = 1)
		{
			var field = MockEntities.CreateCmsModelInstanceField(fieldName, CmsModelInstance, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], id);
			CmsModelInstanceFields.Insert(field);
			CmsModelInstanceFieldValues.Insert(MockEntities.CreateCmsModelInstanceFieldBoolValue(fieldValue, field, id));
		}

		private void CreateInstanceFieldAndInstanceValueForString(string fieldName, string fieldValue, int id = 1)
		{
			var field = MockEntities.CreateCmsModelInstanceField(fieldName, CmsModelInstance, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], id);
			CmsModelInstanceFields.Insert(field);
			CmsModelInstanceFieldValues.Insert(MockEntities.CreateCmsModelInstanceFieldStringValue(fieldValue, field, id));
		}

		private void CreateInstanceFieldAndInstanceValueForDate(string fieldName, DateTime fieldValue, int id = 1)
		{
			var field = MockEntities.CreateCmsModelInstanceField(fieldName, CmsModelInstance, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], id);
			CmsModelInstanceFields.Insert(field);
			CmsModelInstanceFieldValues.Insert(MockEntities.CreateCmsModelInstanceFieldDateValue(fieldValue, field, id));
		}

		private void CreateInstanceFieldAndInstanceValueForNumber(string fieldName, int fieldValue, int id = 1)
		{
			var field = MockEntities.CreateCmsModelInstanceField(fieldName, CmsModelInstance, Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], id);
			CmsModelInstanceFields.Insert(field);
			CmsModelInstanceFieldValues.Insert(MockEntities.CreateCmsModelInstanceFieldNumberValue(fieldValue, field, id));
		}



		private void InsertModelInstanceInRepository(ModelInstanceJsonViewModel vm)
		{
			CmsModelInstance.Json = JsonConvert.SerializeObject(vm);
			CmsModelInstances.Create(CmsModelInstance);
		}


		#endregion //Private Methods
	}
}
