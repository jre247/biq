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
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Framework;
using BrightLine.CMS.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Services;
using System.Web;
using Moq;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Core;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ModelRefValidatorTests
	{
		private CmsField CmsField { get; set; }
		private EntityRepositoryInMemory<CmsModelInstance> CmsModelInstanceRepo { get; set; }
		private ModelInstanceSaveViewModel ViewModel { get; set; }
		private FieldSaveViewModel FieldSaveViewModel { get; set; }
		private CmsModelInstance CmsModelInstance { get; set; }
		private CmsSettingInstance CmsSettingInstance { get;set;}
		private CmsModelInstance CmsModelInstanceReference { get; set; }
		private IModelInstanceService ModelInstanceService { get; set; }
		private string UserInput { get; set; }
		IocRegistration Container;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		private SettingInstanceLookups SettingInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			CmsModelInstanceRepo = new EntityRepositoryInMemory<CmsModelInstance>();
			CmsField = MockEntities.GetFieldForModelRef();
			UserInput = MockEntities.GetFieldValueForRefType("choices", 9);

			FieldSaveViewModel = MockEntities.GetFieldSaveVmForRefType("choices", 9);
			FieldSaveViewModel.value.Add(MockEntities.GetFieldValueForRefType("choice", 9));
			ViewModel = GetModelInstanceSaveViewModel(new List<FieldSaveViewModel> { FieldSaveViewModel });

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();

			CmsModelInstanceReference = cmsModelInstances.Get(9);
			CmsModelInstance = cmsModelInstances.Get(10);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
			ModelInstanceLookups = new ModelInstanceLookups();
			ModelInstanceLookups.ModelInstance = CmsModelInstance;

			CmsModelInstanceReference = cmsModelInstances.Get(9);
			CmsModelInstance = cmsModelInstances.Get(10);
			CmsSettingInstance = cmsSettingInstances.Get(1);

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			SettingInstanceLookups = settingInstanceLookupsService.CreateSettingInstanceLookups(CmsSettingInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}


		[Test(Description = "Model Ref Validator for max length against invalid input.")]
		public void Cms_ModelRefValidator_For_Max_Length_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxLength("1");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator for max length against valid input.")]
		public void Cms_ModelRefValidator_For_Max_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxLength("2");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator for min length against invalid input.")]
		public void Cms_ModelRefValidator_For_Min_Length_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMinLength("3");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator for min length against valid input.")]
		public void Cms_ModelRefValidator_For_Min_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMinLength("1");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator for length against invalid input.")]
		public void Cms_ModelRefValidator_For_Length_Against_invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForLength("1");
			var cmsField = MockEntities.GetFieldForModelRef();
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, cmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator for length against valid input.")]
		public void Cms_ModelRefValidator_For_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "Model Ref Validator against empty user input.")]
		public void Cms_ModelRefValidator_For_Required_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator against existing user input.")]
		public void Cms_ModelRefValidator_For_Required_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator not required against empty string user input.")]
		public void Cms_ModelRefValidator_Not_Required_Against_Empty_String_Input()
		{
			//remove required validation --TODO: this is a hack and should be replaced with not automatically adding required validation to begin with
			var requiredValidation = CmsField.Validations.Single(v => v.ValidationType.Id == Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required]);
			CmsField.Validations.Remove(requiredValidation);

			var validation = MockEntities.GetValidationInstanceForLength("2");
			string userInput = null;
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, null, null, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator against invalid user input.")]
		public void Cms_ModelRefValidator_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = MockEntities.GetFieldValueForRefType("model1", 45);
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Model Ref Validator against valid user input.")]
		public void Cms_ModelRefValidator_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance known ref field has instanceId that references correct model.")]
		public void Cms_Model_Instance_Known_Ref_Field_Value_Is_Valid()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeKnown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance known ref field has instanceId that does not reference correct model.")]
		public void Cms_Model_Instance_Known_Ref_Field_Value_Has_Invalid_Model()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			CmsModelInstanceReference.Model.CmsModelDefinition.Id = 5565; //make referenced instance have invalid model 
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeKnown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance known ref field has instanceId that does not reference model with correct model definition id.")]
		public void Cms_Model_Instance_Known_Ref_Field_Value_Has_InstanceId_Belonging_To_Invalid_ModelDefinition()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			CmsModelInstanceReference.Model.CmsModelDefinition.Id = 5565; //make referenced instance have invalid model definition
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeKnown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance unknown ref field is valid.")]
		public void Cms_Model_Instance_Unknown_Ref_Field_Value_Is_Valid()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeUnknown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance unknown ref field has instanceId that does not reference model with correct model definition id.")]
		public void Cms_Model_Instance_Unknown_Ref_Field_Value_Has_InstanceId_Belonging_To_Invalid_ModelDefinition()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			CmsModelInstanceReference.Model.CmsModelDefinition.Id = 5565; //make referenced instance have invalid model definition
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeUnknown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Cms Model Instance unknown ref field has instanceId that does not reference model with correct creative id.")]
		public void Cms_Model_Instance_Unknown_Ref_Field_Value_Has_InstanceId_Belonging_To_Invalid_Creative()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			CmsModelInstanceReference.Model.Feature.Creative.Id = 555; //make referenced instance have invalid creative
			var validatorService = CreateModelRefValidatorServiceForModelInstance(validation, BuildCmsFieldForRefTypeUnknown(), FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}


		private static ModelInstanceSaveViewModel GetModelInstanceSaveViewModel(List<FieldSaveViewModel> fields)
		{
			var viewModel = new ModelInstanceSaveViewModel
			{
				id = 1,
				name = "Question",
				fields = fields
			};
			return viewModel;
		}

		private CmsField BuildCmsFieldForRefTypeKnown(int modelDefinitionId = 1)
		{
			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			return MockEntities.GetFieldForModelRef("true", CmsRefTypeknownId, "known", modelDefinitionId);
		}

		private CmsField BuildCmsFieldForRefTypeUnknown(int modelDefinitionId = 1)
		{
			var CmsRefTypeUnknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Unknown];
			return MockEntities.GetFieldForModelRef("true", CmsRefTypeUnknownId, "unknown", modelDefinitionId);
		}

		private static ModelRefValidatorService CreateModelRefValidatorServiceForModelInstance(Validation validation, CmsField cmsField, FieldSaveViewModel fieldSaveViewModel, ModelInstanceSaveViewModel viewModel, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = viewModel, CmsField = cmsField, InstanceField = fieldSaveViewModel, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new ModelRefValidatorService(validatorServiceParams);
			return validatorService;
		}

	}
}
