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
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.CMS.Services;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using Moq;
using BrightLine.Common.Utility.Enums;
using System.Web;
using BrightLine.Service;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class BoolValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private CmsSettingInstance CmsSettingInstance { get;set;}
		IocRegistration Container;
		private IModelInstanceService ModelInstanceService = null;
		CmsField BoolField;
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

			CmsModelInstance = cmsModelInstances.Get(2);
			CmsSettingInstance = cmsSettingInstances.Get(2);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();

			BoolField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
			SettingInstanceLookups = settingInstanceLookupsService.CreateSettingInstanceLookups(CmsSettingInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}

		
		[Test(Description = "Bool Validator against empty string user input.")]
		public void Cms_BoolValidator_Against_Empty_String_Input()
		{
			BoolField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateBoolValidatorServiceForModelInstance(validation, BoolField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Bool Validator against valid user input.")]
		public void Cms_BoolValidator_Against_Valid_Input()
		{
			BoolField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "true";
			var validatorService = CreateBoolValidatorServiceForModelInstance(validation, BoolField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Bool Validator not required against empty string user input.")]
		public void CmsBoolTimeValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string userInput = null;
			var validatorService = CreateBoolValidatorServiceForModelInstance(validation, BoolField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Bool model instance unique Validator against invalid user input.")]
		public void Cms_BoolValidator_Model_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "true";
			var validatorService = CreateBoolValidatorServiceForModelInstance(validation, BoolField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Bool setting instance unique Validator against invalid user input.")]
		public void Cms_BoolValidator_Setting_Instance_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "true";
			var settingId = 1;
			var validatorService = CreateBoolValidatorServiceForSettingInstance(validation, BoolField, settingId);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		private static BoolValidatorService CreateBoolValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new BoolValidatorService(validatorServiceParams);
			return validatorService;
		}

		private static BoolValidatorService CreateBoolValidatorServiceForSettingInstance(Validation validation, CmsField cmsField, int settingId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = settingId, InstanceType = InstanceTypeEnum.SettingInstance };
			var validatorService = new BoolValidatorService(validatorServiceParams);
			return validatorService;
		}


    }
}
