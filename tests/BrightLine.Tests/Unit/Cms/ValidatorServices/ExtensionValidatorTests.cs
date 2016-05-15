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
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using System.Web;
using BrightLine.Common.Services;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ExtensionValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		private IResourceService ResourceRepo { get; set; }
		IocRegistration Container;
		CmsField ImageField;
		int ImageFieldTypeId;
		int ResourcesCount;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		SettingInstanceLookups SettingInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);

			Container.Register<IResourceHelper, ResourceHelper>();
			Container.Register<IEnvironmentHelper, EnvironmentHelper>();

			ResourceRepo = IoC.Resolve<IResourceService>();

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			
			CmsModelInstance = cmsModelInstances.Get(2);
		
			ImageField = MockEntities.GetCmsFieldForTypeWithoutRequired();
			ImageFieldTypeId = Lookups.FieldTypes.HashByName["image"];
			ResourcesCount = ResourceRepo.GetAll().Count();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}

		
		[Test(Description = "Extension Validator against valid user resource Id input.")]
		public void Cms_ExtensionValidator_Against_Valid_ResourceId_Input()
		{
			var validation = MockEntities.GetValidationInstanceForExtension();
			CreateFileTypeValidations(validation);
			var resourceId = "1";
			var validatorService = CreateExtensionValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Extension Validator is required against empty string user input.")]
		public void Cms_ExtensionValidator_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForExtension();
			CreateFileTypeValidations(validation);
			var resourceId = "";
			ImageField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validatorService = CreateExtensionValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Extension Validator not required against empty string user input.")]
		public void Cms_ExtensionValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForExtension();
			CreateFileTypeValidations(validation);
			string resourceId = null;
			var validatorService = CreateExtensionValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Image validating resource with incorrect extension.")]
		public void Cms_ExtensionValidator_Against_Invalid_Resource_Extension()
		{
			var validation = MockEntities.GetValidationInstanceForExtension();
			CreateFileTypeValidations(validation);
			var resourceId = ResourcesCount + 1;
			var resourceInvalidExtension = MockEntities.CreateResource(resourceId, "orange.abc", 42323, "abc", 1, "fewjoifj");
			ResourceRepo.Create(resourceInvalidExtension);
			var validatorService = CreateExtensionValidatorServiceForModelInstance(validation, ImageField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId.ToString());

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "Resource extension is not valid for extension: abc.");
		}

		
		private void CreateFileTypeValidations(Validation validation)
		{
			validation.FileTypeValidations = new List<FileTypeValidation> { MockEntities.CreateFileTypeValidation(1, ImageFieldTypeId, validation.Id), MockEntities.CreateFileTypeValidation(2, ImageFieldTypeId, validation.Id) };
		}

		private static ExtensionValidatorService CreateExtensionValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new ExtensionValidatorService(validatorServiceParams);
			return validatorService;
		}

    }
}
