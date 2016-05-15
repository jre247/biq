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
using System.Web;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ImageValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private IRepository<Resource> ResourceRepo { get; set; }
		IocRegistration Container;
		CmsField ImageField;
		ModelInstanceLookups ModelInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();

			CmsModelInstance = cmsModelInstances.Get(2);
			ResourceRepo = IoC.Resolve<IRepository<Resource>>();
			ImageField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}


		
		[Test(Description = "Image Validator against valid user resource Idinput.")]
		public void Cms_ImageValidator_Against_Valid_ResourceId_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Image Validator is required against empty string user input.")]
		public void Cms_ImageValidator_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "";
			ImageField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Image Validator not required against empty string user input.")]
		public void Cms_ImageValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string resourceId = null;
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Image validating resource with incorrect extension.")]
		public void Cms_ImageValidator_Against_Invalid_Resource_Extension()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "4";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "Resource to be registered has an invalid extension.");
		}

		[Test(Description = "Image Validator Min Width against invalid resource width.")]
		public void Cms_ImageValidator_Min_Width_Against_Invalid_Resource_Width()
		{
			var validation = MockEntities.GetValidationInstanceForMinWidth("1234567");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Image Validator Min Width against valid resource width.")]
		public void Cms_ImageValidator_Min_Width_Against_Valid_Resource_Width()
		{
			var validation = MockEntities.GetValidationInstanceForMinWidth("123");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Image Validator Max Width against invalid resource width.")]
		public void Cms_ImageValidator_Max_Width_Against_Invalid_Resource_Width()
		{
			var validation = MockEntities.GetValidationInstanceForMaxWidth("12");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Image Validator Max Width against valid resource width.")]
		public void Cms_ImageValidator_Max_Width_Against_Valid_Resource_Width()
		{
			var validation = MockEntities.GetValidationInstanceForMaxWidth("1234567");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Image Validator Min Height against invalid resource height.")]
		public void Cms_ImageValidator_Min_Height_Against_Invalid_Resource_Height()
		{
			var validation = MockEntities.GetValidationInstanceForMinHeight("1234567");
			var resourceId = "2";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Image Validator Min Height against valid resource height.")]
		public void Cms_ImageValidator_Min_Height_Against_Valid_Resource_Height()
		{
			var validation = MockEntities.GetValidationInstanceForMinHeight("123");
			var resourceId = "1";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Image Validator Max Height against invalid resource height.")]
		public void Cms_ImageValidator_Max_Height_Against_Invalid_Resource_Height()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("12");
			var resourceId = "2";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Image Validator Max Height against valid resource height.")]
		public void Cms_ImageValidator_Max_Height_Against_Valid_Resource_Height()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			var resourceId = "2";
			var validatorService = CreateImageValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		private static ImageValidatorService CreateImageValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new ImageValidatorService(validatorServiceParams);
			return validatorService;
		}

		private static ImageValidatorService CreateImageValidatorServiceForSettingInstance(Validation validation, CmsField cmsField, int settingId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = settingId, InstanceType = InstanceTypeEnum.SettingInstance };
			var validatorService = new ImageValidatorService(validatorServiceParams);
			return validatorService;
		}

    }
}
