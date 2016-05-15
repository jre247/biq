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
	public class DateTimeValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private CmsSettingInstance CmsSettingInstance { get;set;}
		IocRegistration Container;
		CmsField DateField;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		SettingInstanceLookups SettingInstanceLookups { get; set; }
		int SettingId { get;set;}

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

			DateField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			SettingInstanceLookups = settingInstanceLookupsService.CreateSettingInstanceLookups(CmsSettingInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}

		[Test(Description = "DateTime Validator for min against invalid user input.")]
		public void Cms_DateTimeValidator_For_Min_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "12/22/2000",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinDatetime],
					SystemType = ValidationTypeSystemTypeConstants.DATETIME,
					Name = ValidationTypeNameConstants.MIN
				}
			};
			var userInput = "12/20/1998";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator for min against valid user input.")]
		public void Cms_DateTimeValidator_For_Min_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "12/22/2000",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinDatetime],
					SystemType = ValidationTypeSystemTypeConstants.DATETIME,
					Name = ValidationTypeNameConstants.MIN
				}
			};
			var userInput = "12/23/2000";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator for max against invalid user input.")]
		public void Cms_DateTimeValidator_For_Max_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "12/22/2000",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxDatetime],
					SystemType = ValidationTypeSystemTypeConstants.DATETIME,
					Name = ValidationTypeNameConstants.MAX
				}
			};
			var userInput = "12/20/2002";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator for max against valid user input.")]
		public void Cms_DateTimeValidator_For_Max_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "12/22/2000",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxDatetime],
					SystemType = ValidationTypeSystemTypeConstants.DATETIME,
					Name = ValidationTypeNameConstants.MAX
				}
			};
			var userInput = "12/20/2000";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator for required against invalid user input.")]
		public void Cms_DateTimeValidator_For_Required_Against_Invalid_Input()
		{
			DateField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator for required against valid user input.")]
		public void Cms_DateTimeValidator_For_Required_Against_Valid_Input()
		{
			DateField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "12/20/2000";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "DateTime Validator not required against empty string user input.")]
		public void Cms_DateTimeValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string userInput = null;
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "DateTime model instance unique Validator against invalid user input.")]
		public void Cms_DateTimeValidator_Model_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "2/3/14";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "DateTime model instance unique Validator against valid user input.")]
		public void Cms_DateTimeValidator_Model_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "2/21/14";
			var validatorService = CreateDateTimeValidatorServiceForModelInstance(validation, DateField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "DateTime setting instance unique Validator against invalid user input.")]
		public void Cms_DateTimeValidator_Setting_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "5/21/14";
			var validatorService = CreateDateTimeValidatorServiceForSettingInstance(validation, DateField, SettingId);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "DateTime setting instance unique Validator against valid user input.")]
		public void Cms_DateTimeValidator_Setting_Instance_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "2/21/14";
			var validatorService = CreateDateTimeValidatorServiceForSettingInstance(validation, DateField, SettingId);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		private static DateTimeValidatorService CreateDateTimeValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new DateTimeValidatorService(validatorServiceParams);
			return validatorService;
		}

		private static DateTimeValidatorService CreateDateTimeValidatorServiceForSettingInstance(Validation validation, CmsField cmsField, int settingId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = settingId, InstanceType = InstanceTypeEnum.SettingInstance };
			var validatorService = new DateTimeValidatorService(validatorServiceParams);
			return validatorService;
		}

		

    }
}
