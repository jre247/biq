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
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Core;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class PageRefValidatorTests
	{
		private CmsField CmsField { get; set; }
		private EntityRepositoryInMemory<CmsModelInstance> CmsModelInstanceRepo { get; set; }
		private ModelInstanceSaveViewModel ViewModel { get; set; }
		private FieldSaveViewModel FieldSaveViewModel { get; set; }
		private CmsModelInstance CmsModelInstance { get;set;}
		private CmsModelInstance CmsModelInstanceReference { get; set; }
		private string UserInput { get;set;}
		IocRegistration Container;
		private IModelInstanceService ModelInstanceService { get; set; }
		ModelInstanceLookups ModelInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			CmsModelInstanceRepo = new EntityRepositoryInMemory<CmsModelInstance>();
	
			FieldSaveViewModel = MockEntities.GetFieldSaveVmForPageRef(2);
			FieldSaveViewModel.value.Add(MockEntities.GetFieldValueForPageRef(2));
			ViewModel = GetModelInstanceSaveViewModel(new List<FieldSaveViewModel> { FieldSaveViewModel });

			MockUtilities.SetupIoCContainer(Container);

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();


			CmsField = MockEntities.GetFieldForPageRef();
			UserInput = MockEntities.GetFieldValueForPageRef(2);

			CmsModelInstanceReference = cmsModelInstances.Get(9);
			CmsModelInstance = cmsModelInstances.Get(10);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}

		[Test(Description = "Page Ref Validator for max length against invalid input.")]
		public void Cms_PageRefValidator_For_Max_Length_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxLength("1");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator for max length against valid input.")]
		public void Cms_PageRefValidator_For_Max_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxLength("2");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator for min length against invalid input.")]
		public void Cms_PageRefValidator_For_Min_Length_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMinLength("3");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator for min length against valid input.")]
		public void Cms_PageRefValidator_For_Min_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMinLength("1");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator for length against invalid input.")]
		public void Cms_PageRefValidator_For_Length_Against_invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForLength("1");
			var cmsField = MockEntities.GetFieldForModelRef();
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator for length against valid input.")]
		public void Cms_PageRefValidator_For_Length_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForLength("2");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "Page Ref Validator against empty user input.")]
		public void Cms_PageRefValidator_For_Required_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator against existing user input.")]
		public void Cms_PageRefValidator_For_Required_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator not required against empty string user input.")]
		public void Cms_PageRefValidator_Not_Required_Against_Empty_String_Input()
		{
			//remove required validation --TODO: this is a hack and should be replaced with not automatically adding required validation to begin with
			var requiredValidation = CmsField.Validations.Single(v => v.ValidationType.Id == Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required]);
			CmsField.Validations.Remove(requiredValidation);

			var validation = MockEntities.GetValidationInstanceForLength("2");
			string userInput = null;
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, null, null, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Page Ref Validator against invalid page reference.")]
		public void Cms_PageRefValidator_Against_Invalid_Page_Reference()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			UserInput = MockEntities.GetFieldValueForPageRef(2123);
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "Page doesn't exist for field value pageId.");
		}

		[Test(Description = "Page Ref Validator against invalid page creative reference.")]
		public void Cms_PageRefValidator_Against_Invalid_Page_Creative_Reference()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			UserInput = MockEntities.GetFieldValueForPageRef(3); //set field value to be page id that references diff creative than original instance's creative
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "Field value has a page id that references a page that is of a different creative than the original model instance.");
		}

		[Test(Description = "Page Ref Validator against valid user input.")]
		public void Cms_PageRefValidator_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var validatorService = CreatePageRefValidatorServiceForModelInstance(validation, CmsField, FieldSaveViewModel, ViewModel, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(UserInput);

			Assert.IsTrue(boolMessage.Success);
		}

		#region private methods

		private static PageRefValidatorService CreatePageRefValidatorServiceForModelInstance(Validation validation, CmsField cmsField, FieldSaveViewModel fieldSaveViewModel, ModelInstanceSaveViewModel viewModel, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = viewModel, CmsField = cmsField, InstanceField = fieldSaveViewModel, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new PageRefValidatorService(validatorServiceParams);
			return validatorService;
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

		#endregion
	}
}
