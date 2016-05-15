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
using BrightLine.Common.Services;
using System.Web;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.ResourceType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class VideoValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		IocRegistration Container;
		CmsField VideoField;
		ModelInstanceLookups ModelInstanceLookups { get; set; }
		IResourceService Resources { get;set;}

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupIoCContainer(Container);
			Resources = IoC.Resolve<IResourceService>();

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			CmsModelInstance = cmsModelInstances.Get(2);

			VideoField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();
			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}

		[Test(Description = "Video Validator against empty string user input.")]
		public void Cms_VideoValidator_Against_Empty_String_Input()
		{
			VideoField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "";
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Video Validator not required against empty string user input.")]
		public void Cms_VideoValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string resourceId = null;
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Video Validator against valid user resource Idinput.")]
		public void Cms_VideoValidator_Against_Valid_ResourceId_Input()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "5";
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Cms Video validating resource with incorrect extension.")]
		public void Cms_VideoValidator_Against_Invalid_Resource_Extension()
		{
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var resourceId = "7";
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "Resource to be registered has an invalid extension.");
		}

		[Test(Description = "Video Validator Max Duration against invalid resource duration.")]
		public void Cms_VideoValidator_Max_Duration_Against_Invalid_Resource_Duration()
		{
			var validation = MockEntities.GetValidationInstanceForMaxDuration("12398192");
			var newResource = MockEntities.CreateResource(0, "blueberries2.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo], ResourceTypeConstants.ResourceTypeNames.HdVideo, 10000000);
			var resource = Resources.Create(newResource);
			var resourceId = resource.Id.ToString();
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Video Validator Max Duration against valid resource duration.")]
		public void Cms_VideoValidator_Max_Duration_Against_Valid_Resource_Duration()
		{
			var validation = MockEntities.GetValidationInstanceForMaxDuration("100000");
			var newResource = MockEntities.CreateResource(0, "blueberries2.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo], ResourceTypeConstants.ResourceTypeNames.HdVideo, 1000);
			var resource = Resources.Create(newResource);
			var resourceId = resource.Id.ToString();
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Video Validator Max Size against invalid resource size.")]
		public void Cms_VideoValidator_Max_Size_Against_Invalid_Resource_Size()
		{
			var validation = MockEntities.GetValidationInstanceForMaxSize("1000000");
			var newResource = MockEntities.CreateResource(0, "blueberries2.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo], ResourceTypeConstants.ResourceTypeNames.HdVideo, 1000, 10000000);
			var resource = Resources.Create(newResource);
			var resourceId = resource.Id.ToString();
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Video Validator Max Size against valid resource size.")]
		public void Cms_VideoValidator_Max_Size_Against_Valid_Resource_Size()
		{
			var validation = MockEntities.GetValidationInstanceForMaxSize("1000000");
			var newResource = MockEntities.CreateResource(0, "blueberries2.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo], ResourceTypeConstants.ResourceTypeNames.HdVideo, 1000, 1000);
			var resource = Resources.Create(newResource);
			var resourceId = resource.Id.ToString();
			var validatorService = CreateVideoValidatorServiceForModelInstance(validation, VideoField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(resourceId);

			Assert.IsTrue(boolMessage.Success);
		}

		private static VideoValidatorService CreateVideoValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new VideoValidatorService(validatorServiceParams);
			return validatorService;
		}

    }
}
