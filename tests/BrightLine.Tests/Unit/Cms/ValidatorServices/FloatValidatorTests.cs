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
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.CMS.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Services;
using Moq;
using BrightLine.Common.Utility.Enums;
using System.Web;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Service;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class FloatValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private CmsSettingInstance CmsSettingInstance { get; set; }
		IocRegistration Container;
		private IModelInstanceService ModelInstanceService = null;
		private ISettingInstanceService SettingInstanceService = null;
		CmsField FloatField;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		SettingInstanceLookups SettingInstanceLookups { get; set; }
		private int SettingId { get;set;}

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

			CmsModelInstance = cmsModelInstances.Get(2);
			CmsSettingInstance = cmsSettingInstances.Get(1);
			SettingId = 1;

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
			ModelInstanceLookups = new ModelInstanceLookups();
			ModelInstanceLookups.ModelInstance = CmsModelInstance;

			FloatField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			SettingInstanceLookups = settingInstanceLookupsService.CreateSettingInstanceLookups(CmsSettingInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}

		[Test(Description = "Float Validator for max width against invalid user input.")]
		public void Cms_FloatValidator_For_Max_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_WIDTH
				}
			};
			var userInput = "abc";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for max width against valid user input.")]
		public void Cms_FloatValidator_For_Max_Width_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_WIDTH
				}
			};

			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for min width against invalid user input.")]
		public void Cms_FloatValidator_For_Min_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_WIDTH
				}
			};

			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for min width against valid user input.")]
		public void Cms_FloatValidator_For_Min_Width_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_WIDTH
				}
			};

			var userInput = "300.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for width against invalid user input.")]
		public void Cms_FloatValidator_For_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.WIDTH
				}
			};

			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for width against valid user input.")]
		public void Cms_FloatValidator_For_Width_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.WIDTH
				}
			};

			var userInput = "200.54";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for required against invalid user input.")]
		public void Cms_FloatValidator_For_Required_Against_Invalid_Input()
		{
			FloatField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for required against valid user input.")]
		public void Cms_FloatValidator_For_Required_Against_Valid_Input()
		{
			FloatField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "143.22";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator not required against empty string user input.")]
		public void Cms_FloatValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string userInput = null;
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for max height against invalid user input.")]
		public void Cms_FloatValidator_For_Max_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_HEIGHT
				}
			};

			var userInput = "300.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for max height against invalid user input.")]
		public void Cms_FloatValidator_For_Max_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MAX_HEIGHT
				}
			};

			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for min height against invalid user input.")]
		public void Cms_FloatValidator_For_Min_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_HEIGHT
				}
			};
			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for min height against invalid user input.")]
		public void Cms_FloatValidator_For_Min_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.MIN_HEIGHT
				}
			};
			var userInput = "300.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float Validator for height against invalid user input.")]
		public void Cms_FloatValidator_For_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.HEIGHT
				}
			};
			var userInput = "100.32";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float Validator for height against valid user input.")]
		public void Cms_FloatValidator_For_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200.54",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height],
					SystemType = ValidationTypeSystemTypeConstants.FLOAT,
					Name = ValidationTypeNameConstants.HEIGHT
				}
			};
			var userInput = "200.54";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Float model instance unique Validator against invalid user input.")]
		public void Cms_FloatValidator_Model_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "12.2";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float model instance Validator against valid user input.")]
		public void Cms_FloatValidator_Model_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "15.43";
			var validatorService = CreateFloatValidatorServiceForModelInstance(validation, FloatField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "Float setting instance unique Validator against invalid user input.")]
		public void Cms_FloatValidator_Setting_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "12345.12";
			var validatorService = CreateFloatValidatorServiceForSettingInstance(validation, FloatField, SettingId);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Float setting instance Validator against valid user input.")]
		public void Cms_FloatValidator_Setting_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "15.43";
			var validatorService = CreateFloatValidatorServiceForSettingInstance(validation, FloatField, SettingId);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		private static FloatValidatorService CreateFloatValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new FloatValidatorService(validatorServiceParams);
			return validatorService;
		}

		private static FloatValidatorService CreateFloatValidatorServiceForSettingInstance(Validation validation, CmsField cmsField, int settingId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = settingId, InstanceType = InstanceTypeEnum.SettingInstance };
			var validatorService = new FloatValidatorService(validatorServiceParams);
			return validatorService;
		}

    }
}
