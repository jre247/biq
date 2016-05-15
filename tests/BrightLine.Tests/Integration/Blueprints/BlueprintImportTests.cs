using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Service.BlueprintImport;
using BrightLine.Tests.Common;
using BrightLine.Utility;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Tests.Integration.Blueprints
{
	[TestFixture]
	public class BlueprintImportTests
	{
		IocRegistration _container;
		private int _blueprintId = 0;
		private BoolMessageItem _boolMessage = null;
		private int _choiceModelDefinitionId = 0;
		private int _questionModelDefinitionId = 0;
		private int _resultPageDefinitionId = 1002;
		private int _resultPageRefFieldIndex = 16;
		private int _choiceModelRefIndex = 0;
		private int _settingDefinitionId = 0;
		private CmsModelDefinition _choiceModelDefinition = null;
		private CmsModelDefinition _questionModelDefinition = null;
		private CmsSettingDefinition _settingDefinition = null;
		private IBlueprintService BlueprintService { get;set;}
		private Mock<HttpContextBase> Context { get; set; }
		private ICmsFieldService CmsFields { get;set;}
		private IBlueprintService Blueprints { get;set;}
		private IBlueprintImportService BlueprintImportService { get; set; }
		private ICmsModelDefinitionService CmsModelDefinitions { get; set; }
		private IRepository<CmsSettingDefinition> CmsSettingDefinitions { get; set; }
		private IHttpContextHelper HttpContextHelper { get; set; }
		private IValidationService Validations { get; set; }
		private IBlueprintImportModelsService BlueprintImportModelsService { get; set; }
		private IRepository<CmsRef> CmsRefs { get; set; }
		private IBlueprintImportSettingsService BlueprintImportSettingsService { get; set; }
		private IFileTypeValidationService FileTypeValidations { get; set; }
		private IPageDefinitionService PageDefinitions { get; set; }

		[SetUp]
		public void Setup()
		{
			_container = MockUtilities.SetupIoCContainer(_container);
			
			var mockOltpContextHelper = new Mock<IOLTPContextHelper>();
			var mockFileHelper = new Mock<IFileHelper>();
			_container.Register<IOLTPContextHelper>(() => mockOltpContextHelper.Object);
			_container.Register<IFileHelper>(() => mockFileHelper.Object);
			_container.Register<IBlueprintImportLookupsService, BlueprintImportLookupsService>();

			CmsFields = IoC.Resolve<ICmsFieldService>();
			Blueprints = IoC.Resolve<IBlueprintService>();
			BlueprintImportService = IoC.Resolve<IBlueprintImportService>();
			CmsModelDefinitions = IoC.Resolve<ICmsModelDefinitionService>();
			CmsSettingDefinitions = IoC.Resolve<IRepository<CmsSettingDefinition>>();
			HttpContextHelper = IoC.Resolve<IHttpContextHelper>();
			Validations = IoC.Resolve<IValidationService>();
			BlueprintImportModelsService = IoC.Resolve<IBlueprintImportModelsService>();
			CmsRefs = IoC.Resolve<IRepository<CmsRef>>();
			BlueprintImportSettingsService = IoC.Resolve<IBlueprintImportSettingsService>();
			FileTypeValidations = IoC.Resolve<IFileTypeValidationService>();
			PageDefinitions = IoC.Resolve<IPageDefinitionService>();

			var blueprintsRepo = Blueprints.ConvertToInMemoryRepository<Blueprint, ICrudService<Blueprint>>();
			BlueprintService = new BlueprintService(blueprintsRepo);

			var manifestName = "blueprint-test";
			var testBlueprint = new Blueprint
			{
				ManifestName = manifestName,
				Name = "blueprint test"
			};
			testBlueprint = BlueprintService.Create(testBlueprint);
			_blueprintId = testBlueprint.Id;

			var result = BlueprintImportService.ImportBlueprint(testBlueprint.Id, manifestName);
			result.Wait();
			_boolMessage = result.Result;

			_choiceModelDefinition = CmsModelDefinitions.Where(c =>  c.Blueprint_Id == _blueprintId).ToList()[0];
			_questionModelDefinition = CmsModelDefinitions.Where(c => c.Blueprint_Id == _blueprintId).ToList()[1];
			_settingDefinition = CmsSettingDefinitions.Where(c => c.Blueprint_Id == _blueprintId).ToList()[0];
			_choiceModelDefinitionId = _choiceModelDefinition.Id;
			_questionModelDefinitionId = _questionModelDefinition.Id;
			_settingDefinitionId = _settingDefinition.Id;
		}

		[TearDown]
		public void TearDown()
		{
			HttpContextHelper.RemoveItem(BlueprintImportConstants.BlueprintImportLookupsKey);
		}


		[Test(Description = "Test importing Blueprint-test repo from github returns true.")]
		public void Blueprint_Import_Returns_True()
		{
			Assert.IsTrue(_boolMessage.Success);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Model Definitions count.")]
		public void Blueprint_Import_Correct_Model_Definitions_Count_Imported()
		{
			var modelDefinitions = CmsModelDefinitions.Where(m => m.Blueprint_Id == _blueprintId).ToList();
			Assert.IsTrue(modelDefinitions.Count() == 2, "Blueprint import did not import the correct count of Model Definitions.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Setting Definitions count.")]
		public void Blueprint_Import_Correct_Setting_Definitions_Count_Imported()
		{
			var modelDefinitions = CmsSettingDefinitions.Where(m => m.Blueprint_Id == _blueprintId).ToList();
			Assert.IsTrue(modelDefinitions.Count() == 1, "Blueprint import did not import the correct count of Setting Definitions.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Choice Model Definition properties.")]
		public void Blueprint_Import_Correct_Choice_Model_Definitions_Properties()
		{
			Assert.IsTrue(_choiceModelDefinition.Name == "choice", "Choice model has incorrect imported Name.");
			Assert.IsTrue(_choiceModelDefinition.Display == "Choice", "Choice model has incorrect imported Display.");
			Assert.IsTrue(_choiceModelDefinition.DisplayFieldName == "thumbnailSrc", "Choice model has incorrect imported DisplayFieldName.");
			Assert.IsTrue(_choiceModelDefinition.DisplayFieldType_Id == Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], "Choice model has incorrect imported DisplayFieldType.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Definition properties.")]
		public void Blueprint_Import_Correct_Question_Model_Definitions_Properties()
		{
			Assert.IsTrue(_questionModelDefinition.Name == "question", "Question model has incorrect imported Name.");
			Assert.IsTrue(_questionModelDefinition.Display == "Question", "Question model has incorrect imported Display.");
			Assert.IsTrue(_questionModelDefinition.DisplayFieldName == "display", "Question model has incorrect imported DisplayFieldName.");
			Assert.IsTrue(_questionModelDefinition.DisplayFieldType_Id == Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], "Question model has incorrect imported DisplayFieldType.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Setting Definition properties.")]
		public void Blueprint_Import_Correct_Choice_Setting_Definitions_Properties()
		{
			Assert.IsTrue(_settingDefinition.Name == "settings", "Settings has incorrect imported Name.");
			Assert.IsTrue(_settingDefinition.Display == "Feature Settings", "Settings has incorrect imported Display.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Choice Model Fields count.")]
		public void Blueprint_Import_Correct_Choice_Model_Fields_Count()
		{
			var choiceModelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).ToList();
			Assert.IsTrue(choiceModelFields.Count() == 1, "Choice model has incorrect imported Fields count.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Fields count.")]
		public void Blueprint_Import_Correct_Question_Model_Fields_Count()
		{
			var questionModelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList();
			Assert.IsTrue(questionModelFields.Count() == 17, "Choice model has incorrect imported Fields count.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Settings Fields count.")]
		public void Blueprint_Import_Correct_Settings_Fields_Count()
		{
			var settingFields = CmsFields.Where(m => m.CmsSettingDefinition != null && m.CmsSettingDefinition.Id == _settingDefinitionId).ToList();
			Assert.IsTrue(settingFields.Count() == 3, "Setting incorrect imported Fields count.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Choice Model Fields High Level Properties.")]
		public void Blueprint_Import_Correct_Choice_Model_Fields_High_Level_Properties()
		{
			var choiceModelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).ToList();
			AssertChoiceFieldProperties(choiceModelFields, 0, "thumbnailSrc", "Choice Image", "Sprite to use for the choice", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Fields High Level Properties.")]
		public void Blueprint_Import_Correct_Question_Model_Fields_High_Level_Properties()
		{
			var questionModelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList();
			AssertQuestionFieldProperties(questionModelFields, 0, "choices", "Choices", "Model Ref test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel], true, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 1, "String test", "String Test", "String Test (min length and max length validation)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 2, "String test 2", "String Test", "String Test (length validation)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 3, "ordinality", "Ordinality", "Integer test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 4, "headerSrc", "Header Image", "Extension test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 5, "date test", "Date Test", "A date test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 6, "float test", "Float Test", "A float test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Float], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 7, "required false test", "Required Falst Test", "A required false test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Float], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 8, "Image test", "Image Test", "Image Test (validations: min height, max height, maxImageSize, required)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 9, "Image test 2", "Image Test", "Image Test (validations: height, required)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 10, "Image test 3", "Image Test", "Image Test (validations: width, required)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 11, "Image test 4", "Image Test", "Image Test (validations: min width, max width, required)", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 12, "Video test", "Video Test", "Video Test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Video], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 13, "Unique test", "Unique Test", "Unique Test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 14, "Extension Test", "Extension Test", "Extension Test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertQuestionFieldProperties(questionModelFields, 15, "Expose Test", "Expose Test", "Expose Test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Server]);
			AssertQuestionFieldProperties(questionModelFields, 16, "Page Ref Test", "Page Ref Test", "Page Ref Test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage], true, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Settings Fields High Level Properties.")]
		public void Blueprint_Import_Correct_Choice_Settings_Fields_High_Level_Properties()
		{
			var settingFields = CmsFields.Where(m => m.CmsSettingDefinition != null && m.CmsSettingDefinition.Id == _settingDefinitionId).ToList();
			AssertSettingFieldProperties(settingFields, 0, "headerSrc", "Header Image", "Image to use for the header of the feature", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertSettingFieldProperties(settingFields, 1, "arrowSrc", "Arrow Image", "Image of an UPward arrow that will be used for the up/down arrows of the calculator", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
			AssertSettingFieldProperties(settingFields, 2, "ctaSrc", "CTA Image", "Image for call to action", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image], false, Lookups.Exposes.HashByName[ExposeConstants.ExposeNames.Client]);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Choice Model Fields Validations Count.")]
		public void Blueprint_Import_Correct_Choice_Model_Fields_Validations_Count()
		{
			var choiceModelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).ToList();
			var fieldIndex = 0;
			var field = choiceModelFields[fieldIndex];
			var validations = Validations.Where(v => v.CmsField.Id == field.Id).ToList();
			Assert.IsTrue(validations.Count() == 4, string.Format("Choice model has incorrect number of Validations for field {0}.", fieldIndex));
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Fields Validations Count.")]
		public void Blueprint_Import_Correct_Question_Model_Fields_Validations_Count()
		{
			var modelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList();

			AssertFieldValidationsCount(modelFields, 0, 3);
			AssertFieldValidationsCount(modelFields, 1, 2);
			AssertFieldValidationsCount(modelFields, 2, 2);
			AssertFieldValidationsCount(modelFields, 3, 1);
			AssertFieldValidationsCount(modelFields, 4, 4);
			AssertFieldValidationsCount(modelFields, 5, 3);
			AssertFieldValidationsCount(modelFields, 6, 3);
			AssertFieldValidationsCount(modelFields, 7, 3);
			AssertFieldValidationsCount(modelFields, 8, 4);
			AssertFieldValidationsCount(modelFields, 9, 2);
			AssertFieldValidationsCount(modelFields, 10, 2);
			AssertFieldValidationsCount(modelFields, 11, 3);
			AssertFieldValidationsCount(modelFields, 12, 3);
			AssertFieldValidationsCount(modelFields, 13, 2);
			AssertFieldValidationsCount(modelFields, 14, 4);
			AssertFieldValidationsCount(modelFields, 15, 2);
			AssertFieldValidationsCount(modelFields, 16, 0);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Setting Fields Validations Count.")]
		public void Blueprint_Import_Correct_Setting_Fields_Validations_Count()
		{
			var modelFields = CmsFields.Where(m => m.CmsSettingDefinition != null && m.CmsSettingDefinition.Id == _settingDefinitionId).ToList();

			AssertFieldValidationsCount(modelFields, 0, 2);
			AssertFieldValidationsCount(modelFields, 1, 2);
			AssertFieldValidationsCount(modelFields, 2, 2);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Choice Model Fields Validation properties for Field.")]
		public void Blueprint_Import_Correct_Question_Model_Fields_Has_Correct_Validation_Properties_For_Field()
		{
			//field 1
			AssertFieldValidation(0, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], "2");
			AssertFieldValidation(0, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], "10");
			AssertFieldValidation(0, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 2
			AssertFieldValidation(1, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], "5");
			AssertFieldValidation(1, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], "1");

			//field 3
			AssertFieldValidation(2, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Length], "5");
			AssertFieldValidation(2, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 4
			AssertFieldValidation(3, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 5
			AssertFieldValidation(4, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(4, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "408");
			AssertFieldValidation(4, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "108");

			//field 6
			AssertFieldValidation(5, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinDatetime], "02/05/15");
			AssertFieldValidation(5, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxDatetime], "10/04/17");
			AssertFieldValidation(5, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 7
			AssertFieldValidation(6, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], "24.32");
			AssertFieldValidation(6, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxFloat], "102.87");
			AssertFieldValidation(6, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 8
			AssertFieldValidation(7, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinFloat], "24.32");
			AssertFieldValidation(7, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxFloat], "102.87");
			AssertFieldValidation(7, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "False");

			//field 9
			AssertFieldValidation(8, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxImageSize], "1002");
			AssertFieldValidation(8, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(8, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight], "345");
			AssertFieldValidation(8, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight], "900");

			//field 10
			AssertFieldValidation(9, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(9, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "345");

			//field 11
			AssertFieldValidation(10, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(10, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "345");

			//field 12
			AssertFieldValidation(11, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(11, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], "345");
			AssertFieldValidation(11, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], "566");

			//field 13
			AssertFieldValidation(12, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxVideoDuration], "6002");
			AssertFieldValidation(12, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxVideoSize], "24");
			AssertFieldValidation(12, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 14
			AssertFieldValidation(13, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], "24");
			AssertFieldValidation(13, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");

			//field 15
			AssertFieldValidation(14, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
			AssertFieldValidation(14, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "408");
			AssertFieldValidation(14, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "108");

			//field 16
			AssertFieldValidation(15, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], "24");
			AssertFieldValidation(15, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required], "True");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Setting Fields Validations Properties.")]
		public void Blueprint_Import_Correct_Choice_Setting_Fields_Validations_Properties()
		{
			//field 1
			AssertSettingFieldValidation(0, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "1920");
			AssertSettingFieldValidation(0, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "144");

			//field 2
			AssertSettingFieldValidation(1, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "96");
			AssertSettingFieldValidation(1, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "54");

			//field 3
			AssertSettingFieldValidation(2, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Width], "276");
			AssertSettingFieldValidation(2, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Height], "108");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Field Extensions Count.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Extensions_Count()
		{
			var modelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList();

			AssertExtensionsCountForField(modelFields, 4);
			AssertExtensionsCountForField(modelFields, 14);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Extensions Properties.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Extensions_Properties()
		{
			var modelFields = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList();

			AssertExtensionPropertiesForField(modelFields, 4);
			AssertExtensionPropertiesForField(modelFields, 14);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Model Ref Count.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Model_Ref_Correct_Count()
		{
			var cmsRefs = CmsRefs.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).ToList();

			Assert.AreEqual(cmsRefs.Count(), 1, "Cms Model Ref count is incorrect.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Question Model Model Ref Properties.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Model_Ref_Correct_Properties()
		{
			var cmsRef = CmsRefs.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).Single();

			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			Assert.AreEqual(cmsRef.CmsRefType_Id, CmsRefTypeknownId, "Cms Model Ref Type is incorrect.");
			Assert.IsNull(cmsRef.PageDefinition, "Cms Model Ref Page Definition should be null.");
		}


		[Test(Description = "Test importing Blueprint-test repo from github has field that references the correct Model.")]
		public void Blueprint_Import_Correct_Question_Model_Field_References_Correct_Model()
		{
			var cmsRef = CmsRefs.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _choiceModelDefinitionId).Single();
			var modelRefField = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList()[_choiceModelRefIndex];

			Assert.AreEqual(cmsRef.Id, modelRefField.CmsRef.Id, "Cms Model Ref is incorrect.");
		}
		[Test(Description = "Test importing Blueprint-test repo from github has correct Page Page Ref Count.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Page_Ref_Correct_Count()
		{
			var cmsRefs = CmsRefs.Where(m => m.PageDefinition_Id == _resultPageDefinitionId).ToList();

			Assert.AreEqual(cmsRefs.Count(), 1, "Cms Page Ref count is incorrect.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct Page Ref Properties.")]
		public void Blueprint_Import_Correct_Question_Model_Field_Page_Ref_Correct_Properties()
		{
			var cmsRef = CmsRefs.Where(m => m.PageDefinition_Id == _resultPageDefinitionId).Single();

			var CmsRefTypeknownId = Lookups.CmsRefTypes.HashByName[CmsRefTypeConstants.CmsRefTypeNames.Known];
			Assert.AreEqual(cmsRef.CmsRefType_Id, CmsRefTypeknownId, "Cms Page Ref Type is incorrect.");
		}

		[Test(Description = "Test importing Blueprint-test repo from github has field that references the correct Page.")]
		public void Blueprint_Import_Correct_Question_Model_Field_References_Correct_Page()
		{
			var cmsRef = CmsRefs.Where(m => m.PageDefinition_Id == _resultPageDefinitionId).Single();
			var pageRefField = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList()[_resultPageRefFieldIndex];

			Assert.AreEqual(cmsRef.Id, pageRefField.CmsRef.Id, "Cms Page Ref is incorrect.");	
		}

		[Test(Description = "Test importing Blueprint-test repo from github matches on model name.")]
		public void Blueprint_Import_Model_Filename_From_Github_Match()
		{
			var isModelMatch = BlueprintImportModelsService.CheckIfModelFilename("cms.abc123.model.json");
			Assert.IsTrue(isModelMatch);
		}

		[Test(Description = "Test importing Blueprint-test repo from github matches on settings name.")]
		public void Blueprint_Import_Settings_Filename_From_Github_Match()
		{
			var isSettingMatch = BlueprintImportSettingsService.CheckIfSettingsFilename("cms.settings.json");
			Assert.IsTrue(isSettingMatch);
		}

		[Test(Description = "Test importing Blueprint-test repo from github does not match on model name.")]
		public void Blueprint_Import_Model_Filename_From_Github_Does_Not_Match()
		{
			var isModelMatch = BlueprintImportModelsService.CheckIfModelFilename("cms.abc123.aeamodel.json");
			Assert.IsFalse(isModelMatch);
		}

		[Test(Description = "Test importing Blueprint-test repo from github does not match on settings name.")]
		public void Blueprint_Import_Settings_Filename_From_Github_Is_Not_Match()
		{
			var isSettingMatch = BlueprintImportSettingsService.CheckIfSettingsFilename("cms.abc123.awsettings.json");
			Assert.IsFalse(isSettingMatch);
		}

		[Test(Description = "Test importing Blueprint-test repo from github has correct PageDefinitions Count.")]
		public void Blueprint_Import_Correct_PageDefinitions_Count()
		{
			var pageDefinitionsCount = PageDefinitions.Where(p => p.Blueprint_Id == _blueprintId).Count();

			Assert.AreEqual(pageDefinitionsCount, 2);
		}

		#region Private Methods

		private static void AssertQuestionFieldProperties(List<CmsField> questionModelFields, int fieldIndex, string name, string displayName, string description, int typeId, bool isList, int exposeId)
		{
			Assert.IsTrue(questionModelFields[fieldIndex].Name == name, string.Format("Question field {0} has incorrect imported Name.", fieldIndex));
			Assert.IsTrue(questionModelFields[fieldIndex].Display == displayName, string.Format("Question field {0} has incorrect imported DisplayName.", fieldIndex));
			Assert.IsTrue(questionModelFields[fieldIndex].Description == description, string.Format("Question field {0} has incorrect imported Description.", fieldIndex));
			Assert.IsTrue(questionModelFields[fieldIndex].Type_Id == typeId, string.Format("Question field {0} has incorrect imported Name.", fieldIndex));
			Assert.IsTrue(questionModelFields[fieldIndex].List == isList, string.Format("Question field {0} has incorrect imported List.", fieldIndex));
			Assert.IsTrue(questionModelFields[fieldIndex].Expose_Id == exposeId, string.Format("Question field {0} has incorrect imported Expose.", fieldIndex));
		}

		private static void AssertChoiceFieldProperties(List<CmsField> choiceModelFields, int fieldIndex, string name, string displayName, string description, int typeId, bool isList, int exposeId)
		{
			Assert.IsTrue(choiceModelFields[fieldIndex].Name == name, "Choice field 0 has incorrect imported Name.");
			Assert.IsTrue(choiceModelFields[fieldIndex].Display == displayName, "Choice field 0 has incorrect imported DisplayName.");
			Assert.IsTrue(choiceModelFields[fieldIndex].Description == description, "Choice field 0 has incorrect imported Description.");
			Assert.IsTrue(choiceModelFields[fieldIndex].Type_Id == typeId, "Choice field 0 has incorrect imported Name.");
			Assert.IsTrue(choiceModelFields[fieldIndex].List == isList, "Choice field 0 has incorrect imported List.");
			Assert.IsTrue(choiceModelFields[fieldIndex].Expose_Id == exposeId, "Choice field 0 has incorrect imported Expose.");
			Assert.IsNull(choiceModelFields[fieldIndex].CmsSettingDefinition, "Choice field 0 has incorrect imported CmsSettingDefinition (should be null).");

		}

		private static void AssertSettingFieldProperties(List<CmsField> settingFields, int fieldIndex, string name, string displayName, string description, int typeId, bool isList, int exposeId)
		{
			Assert.IsTrue(settingFields[fieldIndex].Name == name, "Setting field 0 has incorrect imported Name.");
			Assert.IsTrue(settingFields[fieldIndex].Display == displayName, "Setting field 0 has incorrect imported DisplayName.");
			Assert.IsTrue(settingFields[fieldIndex].Description == description, "Setting field 0 has incorrect imported Description.");
			Assert.IsTrue(settingFields[fieldIndex].Type_Id == typeId, "Setting field 0 has incorrect imported Name.");
			Assert.IsTrue(settingFields[fieldIndex].List == isList, "Setting field 0 has incorrect imported List.");
			Assert.IsTrue(settingFields[fieldIndex].Expose_Id == exposeId, "Setting field 0 has incorrect imported Expose.");
			Assert.IsNull(settingFields[fieldIndex].CmsModelDefinition, "Setting field 0 has incorrect imported CmsModelDefinition (should be null).");

		}
		
		private void AssertFieldValidationsCount(List<CmsField> modelFields, int fieldIndex, int validationsCount)
		{
			var field = modelFields[fieldIndex];
			var validations = Validations.Where(v => v.CmsField.Id == field.Id).ToList();
			Assert.IsTrue(validations.Count() == validationsCount, string.Format("Incorrect number of Validations for field {0}.", fieldIndex));
		}

		private void AssertFieldValidation(int fieldIndex, int validationTypeId, string validationValue)
		{
			var field = CmsFields.Where(m => m.CmsModelDefinition != null && m.CmsModelDefinition.Id == _questionModelDefinitionId).ToList()[fieldIndex];
			var validationsForFields = Validations.Where(v => v.CmsField.Id == field.Id).ToList();
			var validation = validationsForFields.Where(v => v.ValidationType_Id == validationTypeId).Single();

			Assert.IsTrue(validation.ValidationType_Id == validationTypeId, string.Format("Field {0} has incorrect value for validation {1}", fieldIndex, validationTypeId));
			Assert.IsTrue(validation.Value == validationValue, string.Format("Field {0} has incorrect value for validation {1}", fieldIndex, validationTypeId));
		}

		private void AssertSettingFieldValidation(int fieldIndex, int validationTypeId, string validationValue)
		{
			var field = CmsFields.Where(m => m.CmsSettingDefinition != null && m.CmsSettingDefinition.Id == _settingDefinitionId).ToList()[fieldIndex];
			var validationsForFields = Validations.Where(v => v.CmsField.Id == field.Id).ToList();
			var validation = validationsForFields.Where(v => v.ValidationType_Id == validationTypeId).Single();

			Assert.IsTrue(validation.ValidationType_Id == validationTypeId, string.Format("Field {0} has incorrect value for validation {1}", fieldIndex, validationTypeId));
			Assert.IsTrue(validation.Value == validationValue, string.Format("Field {0} has incorrect value for validation {1}", fieldIndex, validationTypeId));
		}

		private void AssertExtensionsCountForField(List<CmsField> modelFields, int fieldIndex)
		{
			var field = modelFields[fieldIndex];
			var fileTypeValidations = FileTypeValidations.Where(f => f.Validation.CmsField.Id == field.Id).ToList();
			Assert.AreEqual(fileTypeValidations.Count(), 4, string.Format("Extensions count is not correct for field {0}", fieldIndex));
		}

		private void AssertExtensionPropertiesForField(List<CmsField> modelFields, int fieldIndex)
		{
			var field = modelFields[fieldIndex];
			var fileTypeValidations = FileTypeValidations.Where(f => f.Validation.CmsField.Id == field.Id).ToList();

			Assert.AreEqual(fileTypeValidations[0].FileType_Id, Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Jpg], string.Format("Extension File Type is not correct for field {0}", fieldIndex));
			Assert.AreEqual(fileTypeValidations[1].FileType_Id, Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Jpeg], string.Format("Extension File Type is not correct for field {0}", fieldIndex));
			Assert.AreEqual(fileTypeValidations[2].FileType_Id, Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Png], string.Format("Extension File Type is not correct for field {0}", fieldIndex));
			Assert.AreEqual(fileTypeValidations[3].FileType_Id, Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Gif], string.Format("Extension File Type is not correct for field {0}", fieldIndex));
		}

		//private static void AssertChoiceFieldCmsRefIsNull(List<CmsField> choiceModelFields, int fieldIndex)
		//{
		//	Assert.IsNull(choiceModelFields[fieldIndex].CmsRef, "Choice field 0 has incorrect imported CmsRef (should be null).");
		//}

		//private static void AssertChoiceFieldCmsSettingIsNull(List<CmsField> choiceModelFields, int fieldIndex)
		//{
		//	Assert.IsNull(choiceModelFields[fieldIndex].CmsSettingDefinition, string.Format("Choice field {0} has incorrect imported CmsSettingDefinition (should be null).", fieldIndex);
		//}


		#endregion
	}
}
