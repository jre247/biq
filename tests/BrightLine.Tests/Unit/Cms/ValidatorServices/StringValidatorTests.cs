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
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.CMS.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Services;
using Moq;
using BrightLine.Common.Utility.Enums;
using System.Web;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class StringValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private CmsSettingInstance CmsSettingInstance { get;set;}
		private IocRegistration Container {get;set;}
		private IModelInstanceService ModelInstanceService = null;
		CmsField StringField;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		SettingInstanceLookups SettingInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();

			CmsModelInstance = cmsModelInstances.Get(1);
			CmsSettingInstance = cmsSettingInstances.Get(1);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();

			ModelInstanceLookups = new ModelInstanceLookups();
			ModelInstanceLookups.ModelInstance = CmsModelInstance;

			StringField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			SettingInstanceLookups = settingInstanceLookupsService.CreateSettingInstanceLookups(CmsSettingInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}


		[Test(Description = "String Validator for max length against invalid user input.")]
		public void Cms_StringValidator_For_Max_Length_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_LENGTH
				}
			};
			var userInput = "hamburger";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String Validator for max length against valid user input.")]
		public void Cms_StringValidator_For_Max_Length_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_LENGTH
				}
			};
			var userInput = "app";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "String Validator for min length against invalid user input.")]
		public void Cms_StringValidator_For_Min_Length_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_LENGTH
				}
			};
			var userInput = "app";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String Validator for min length against valid user input.")]
		public void Cms_StringValidator_For_Min_Length_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_LENGTH
				}
			};
			var userInput = "hamburger";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "String Validator for length against invalid user input.")]
		public void Cms_StringValidator_For_Length_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Length],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.LENGTH
				}
			};
			var userInput = "hamburger";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String Validator for length against valid user input.")]
		public void Cms_StringValidator_For_Length_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "4",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Length],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.LENGTH
				}
			};
			var userInput = "hamb";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "String Validator for required against invalid user input.")]
		public void Cms_StringValidator_For_Required_Against_Invalid_Input()
		{
			StringField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String Validator not required against empty string user input.")]
		public void Cms_StringValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string userInput = null;
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "String Validator for required against valid user input.")]
		public void Cms_StringValidator_For_Required_Against_Valid_Input()
		{
			StringField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "143";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "String model instance unique Validator against invalid user input.")]
		public void Cms_StringValidator_Model_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "orange";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String model instance Validator against valid user input.")]
		public void Cms_StringValidator_Model_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "turtle";
			var validatorService = CreateStringValidatorServiceForModelInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "String setting instance unique Validator against invalid user input.")]
		public void Cms_StringValidator_Setting_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "abc";
			var validatorService = CreateStringValidatorServiceForSettingInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "String setting instance Validator against valid user input.")]
		public void Cms_StringValidator_Setting_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "turtle";
			var validatorService = CreateStringValidatorServiceForSettingInstance(validation, StringField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		private static StringValidatorService CreateStringValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new StringValidatorService(validatorServiceParams);
			return validatorService;
		}

		private static StringValidatorService CreateStringValidatorServiceForSettingInstance(Validation validation, CmsField cmsField, int settingId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = settingId, InstanceType = InstanceTypeEnum.SettingInstance };
			var validatorService = new StringValidatorService(validatorServiceParams);
			return validatorService;
		}
    }
}
