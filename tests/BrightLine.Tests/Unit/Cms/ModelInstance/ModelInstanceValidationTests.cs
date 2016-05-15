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
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.Common.Utility;
using BrightLine.Common.Services;
using System.Web;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ModelInstanceValidationTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		IocRegistration Container;
		ModelInstanceLookups ModelInstanceLookups { get;set;}
		ICmsModelInstanceService CmsModelInstances { get;set;}
		IModelInstanceLookupsService ModelInstanceLookupsService { get; set; }
		IModelInstanceValidationService ModelInstanceValidationService { get; set; }

		[SetUp]
		public void Setup()
		{
			CmsModelInstance = MockEntities.GetModelInstance(123, "question", 1, "choices");

			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			CmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			ModelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			ModelInstanceValidationService = IoC.Resolve<IModelInstanceValidationService>();

			ModelInstanceLookups = ModelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}


		[Test(Description = "Validate Cms Model Instance fields against invalid fields.")]
		public void Cms_Model_Instance_Fields_Validation_Against_Invalid_Fields()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(1, "Abc"));
			BuildCmsFieldsForCommonType();
			var cmsService = new CmsService();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Validate Cms Model Instance fields against valid fields.")]
		public void Cms_Model_Instance_Fields_Validation_Against_Valid_Fields()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(1, "40"));
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(2, "50"));
			BuildCmsFieldsForCommonType();
			var cmsService = new CmsService();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance ref field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Ref_Field_Not_Required_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput());
			CmsModelInstance.Model.Name = "choices";
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsRefFieldWithoutRequiredValidator();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance number field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Number_Field_Not_Required_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test number"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsNumberFieldWithoutRequiredValidator();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance number field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Number_Field_Required_False_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test number"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsNumberFieldWithRequiredValidatorFalse();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance string field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_String_Field_Not_Required_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test number"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsStringFieldWithoutRequiredValidator();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance string field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_String_Field_Required_False_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test string"));
			CmsModelInstance.Model.Name = "choices";
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsStringFieldWithRequiredValidatorFalse();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance bool field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Bool_Field_Not_Required_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test bool"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsBoolFieldWithoutRequiredValidator();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance bool field with the following conditions is still valid: field required validator has value of false but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Bool_Field_Required_False_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test bool"));
			CmsModelInstance.Model.Name = "choices";
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsBoolFieldWithRequiredValidatorFalse();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance datetime field with the following conditions is still valid: field value is not required but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Date_Field_Not_Required_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test datetime"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsDateFieldWithoutRequiredValidator();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance datetime field with the following conditions is still valid: field required validator has value of false but field has other validators, field value has empty input.")]
		public void Cms_Model_Instance_Date_Field_Required_False_And_With_Other_Validators_Is_Still_Valid()
		{
			var viewModel = MockEntities.GetModelInstanceSaveViewModel(1, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithEmptyInput(1, "test datetime"));
			CmsModelInstances.Create(CmsModelInstance);
			var modelInstanceService = ModelInstanceRetrievalService();
			BuildCmsDateFieldWithRequiredValidatorFalse();
			ModelInstanceLookups.BuildInstanceFieldsDictionary(viewModel, CmsModelInstance.Id);

			var boolMessage = ModelInstanceValidationService.ValidateModelInstanceFields(CmsModelInstance, viewModel);

			Assert.IsTrue(boolMessage.Success);
		}

		


		#region Private Methods
		
		private IModelInstanceService ModelInstanceRetrievalService()
		{
			return new ModelInstanceService();
		}

		private void BuildCmsFieldsForCommonType()
		{
			var field1 = MockEntities.GetCmsFieldForType("What is your favorite prime number", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 1, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			field1.Validations.Add(MockEntities.GetValidationForType(1, "445", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth],  FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MAX_WIDTH));
			field1.Validations.Add(MockEntities.GetValidationForType(2, "39", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth],  FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MIN_WIDTH));

			var field2 = MockEntities.GetCmsFieldForType("What is your favorite odd number", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], FieldTypeConstants.FieldTypeNames.Integer, 2, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server], "Server", "false");
			field2.Validations.Add(MockEntities.GetValidationForType(1, "500", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MAX_WIDTH));
			field2.Validations.Add(MockEntities.GetValidationForType(2, "1", Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], FieldTypeConstants.FieldTypeNames.Integer, ValidationTypeNameConstants.MIN_WIDTH));
			
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(field1);
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(field2); 
		}


		private void BuildCmsRefFieldWithoutRequiredValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldForRefTypeWithoutRequiredValidator());
		}

		private void BuildCmsRefFieldWithRequiredValidatorFalse()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldForModelRef("false"));
		}

		private void BuildCmsNumberFieldWithoutRequiredValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Float], ValidationTypeSystemTypeConstants.FLOAT, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MIN));
		}

		private void BuildCmsNumberFieldWithRequiredValidatorFalse()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithRequiredValidatorFalse(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Float], ValidationTypeSystemTypeConstants.FLOAT, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MIN));
		}

		private void BuildCmsStringFieldWithoutRequiredValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], ValidationTypeSystemTypeConstants.STRING, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));
		}

		private void BuildCmsStringFieldWithRequiredValidatorFalse()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithRequiredValidatorFalse(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], ValidationTypeSystemTypeConstants.STRING, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.STRING, ValidationTypeNameConstants.MIN_LENGTH));
		}

		private void BuildCmsBoolFieldWithoutRequiredValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], ValidationTypeSystemTypeConstants.BOOL, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], ValidationTypeSystemTypeConstants.BOOL, ValidationTypeNameConstants.REQUIRED));
		}

		private void BuildCmsBoolFieldWithRequiredValidatorFalse()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithRequiredValidatorFalse(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool], ValidationTypeSystemTypeConstants.BOOL, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], ValidationTypeSystemTypeConstants.BOOL, ValidationTypeNameConstants.REQUIRED));
		}

		private void BuildCmsDateFieldWithoutRequiredValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], ValidationTypeSystemTypeConstants.DATETIME, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], ValidationTypeSystemTypeConstants.DATETIME, ValidationTypeNameConstants.MIN));
		}

		private void BuildCmsDateFieldWithRequiredValidatorFalse()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithRequiredValidatorFalse(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], ValidationTypeSystemTypeConstants.DATETIME, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], ValidationTypeSystemTypeConstants.DATETIME, ValidationTypeNameConstants.MIN));
		}

		private void BuildCmsDateFieldWithUniqueValidator()
		{
			CmsModelInstance.Model.CmsModelDefinition.Fields.Add(MockEntities.GetFieldWithoutRequiredValidator(1, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], ValidationTypeSystemTypeConstants.DATETIME, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], ValidationTypeSystemTypeConstants.DATETIME, ValidationTypeNameConstants.UNIQUE, "true"));
		}

		#endregion //Private Methods
	}
}
