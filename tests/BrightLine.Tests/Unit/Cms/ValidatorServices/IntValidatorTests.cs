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
using BrightLine.Common.Framework;
using BrightLine.CMS.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Services;
using System.Web;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class IntValidatorTests
	{
		private CmsModelInstance CmsModelInstance { get; set; }
		IocRegistration Container;
		private IModelInstanceService ModelInstanceService { get; set; }
		CmsField IntField {get;set;}
		ModelInstanceLookups ModelInstanceLookups { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var cmsModelInstances = IoC.Resolve<ICmsModelInstanceService>();
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();

			CmsModelInstance = cmsModelInstances.Get(2);

			ModelInstanceService = IoC.Resolve<IModelInstanceService>();
			ModelInstanceLookups = new ModelInstanceLookups();
			ModelInstanceLookups.ModelInstance = CmsModelInstance;

			IntField = MockEntities.GetCmsFieldForTypeWithoutRequired();

			ModelInstanceLookups = modelInstanceLookupsService.CreateModelInstanceLookups(CmsModelInstance);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}


		[Test(Description = "Int Validator for max width against invalid user input width.")]
		public void Cms_IntValidator_For_Max_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "445",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MAX_WIDTH
				}
			};
			var userInput = "462";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for max width against valid user input width.")]
		public void Cms_IntValidator_For_Max_Width_Against_valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "445",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MAX_WIDTH
				}
			};
			var userInput = "432";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator for min width against valid user input width.")]
		public void Cms_IntValidator_For_Min_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "445",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MIN_WIDTH
				}
			};
			var userInput = "432";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for min width against valid user input width.")]
		public void Cms_IntValidator_For_Min_Width_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "445",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MIN_WIDTH
				}
			};
			var userInput = "455";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator for width against invalid user input width.")]
		public void Cms_IntValidator_For_Width_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "432",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.WIDTH
				}
			};
			var userInput = "450";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for width against valid user input width.")]
		public void Cms_IntValidator_For_Width_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "432",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.WIDTH
				}
			};
			var userInput = "432";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator for required against invalid user input.")]
		public void Cms_IntValidator_For_Required_Against_Invalid_Input()
		{
			IntField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for required against valid user input.")]
		public void Cms_IntValidator_For_Required_Against_Valid_Input()
		{
			IntField.Validations.Add(MockEntities.GetValidationInstanceForRequired("true"));
			var validation = MockEntities.GetValidationInstanceForRequired("true");
			var userInput = "143";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator not required against empty string user input.")]
		public void Cms_IntValidator_Not_Required_Against_Empty_String_Input()
		{
			var validation = MockEntities.GetValidationInstanceForMaxHeight("1234356");
			string userInput = null;
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}


		[Test(Description = "Int Validator for max height against invalid user input.")]
		public void Cms_IntValidator_For_Max_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MAX_HEIGHT
				}
			};
			var userInput = "300";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for max height against invalid user input.")]
		public void Cms_IntValidator_For_Max_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MAX_HEIGHT
				}
			};
			var userInput = "100";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator for min height against invalid user input.")]
		public void Cms_IntValidator_For_Min_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MIN_HEIGHT
				}
			};
			var userInput = "100";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for min height against invalid user input.")]
		public void Cms_IntValidator_For_Min_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.MIN_HEIGHT
				}
			};
			var userInput = "300";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator for height against invalid user input.")]
		public void Cms_IntValidator_For_Height_Against_Invalid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.HEIGHT
				}
			};
			var userInput = "100";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator for height against valid user input.")]
		public void Cms_FloatValidator_For_Height_Against_Valid_Input()
		{
			var validation = new Validation
			{
				Id = 1,
				Value = "200",
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height],
					SystemType = FieldTypeConstants.FieldTypeNames.Integer,
					Name = ValidationTypeNameConstants.HEIGHT
				}
			};
			var userInput = "200";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		[Test(Description = "Int Validator against invalid user input.")]
		public void Cms_IntValidator_Unique_Against_Invalid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "12";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsFalse(boolMessage.Success);
		}

		[Test(Description = "Int Validator against valid user input.")]
		public void Cms_IntValidator_Unique_Against_Valid_Input()
		{
			var validation = MockEntities.GetValidationInstanceForUnique("true");
			var userInput = "19";
			var validatorService = CreateIntValidatorServiceForModelInstance(validation, IntField, CmsModelInstance.Id);

			var boolMessage = validatorService.Validate(userInput);

			Assert.IsTrue(boolMessage.Success);
		}

		private static IntValidatorService CreateIntValidatorServiceForModelInstance(Validation validation, CmsField cmsField, int modelInstanceId = 1)
		{
			var validatorServiceParams = new ValidatorServiceParams { Validation = validation, InstanceViewModel = null, CmsField = cmsField, InstanceField = null, InstanceId = modelInstanceId, InstanceType = InstanceTypeEnum.ModelInstance };
			var validatorService = new IntValidatorService(validatorServiceParams);
			return validatorService;
		}


    }
}
