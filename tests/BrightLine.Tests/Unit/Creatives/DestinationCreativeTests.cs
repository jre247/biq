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
using BrightLine.Service;
using BrightLine.Common.Services;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Utility;
using BrightLine.Common.Framework;
using BrightLine.Common.ViewModels.Campaigns;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Utility;
using Moq;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class DestinationCreativeTests
	{
		IocRegistration Container;
		int CreativesCount, CreativeId;
		private IRepository<CmsModel> CmsModels { get;set;}
		private ICreativeService Creatives { get;set;}
		private IRepository<CmsSetting> CmsSettings { get; set; }
		private IResourceService Resources { get; set; }

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			CmsModels = IoC.Resolve<IRepository<CmsModel>>();
			Creatives = IoC.Resolve<ICreativeService>();
			CmsSettings = IoC.Resolve<IRepository<CmsSetting>>();
			Resources = IoC.Resolve<IResourceService>();

			CreativesCount = Creatives.GetAll().Count();
			CreativeId = CreativesCount + 1;
		}

		[Test(Description = "Destination Creative save does not save a creative when another creative has the same name.")]
		[ExpectedException("BrightLine.Common.Framework.Exceptions.ValidationException")]
		public void Destination_Creative_Save_Does_Not_Create_Creative_With_Existing_Name()
		{
			var creative = MockEntities.BuildCreative(0, "Creative 1");

			Creatives.Create(creative);
		}

		[Test(Description = "Destination Creative save creates new creative.")]
		public void Destination_Creative_Save_Creates_New_Creative()
		{
			var creative = MockEntities.BuildCreative(0, "test12345");

			var newCreative = Creatives.Create(creative);

			Assert.IsTrue(Creatives.GetAll().Count() == CreativesCount + 1, "Creatives count is not correct.");
		}

		[Test(Description = "Destination Creative save creates new creative with correct properties.")]
		public void Destination_Creative_Save_Creates_New_Creative_With_Correct_Properties()
		{
			var CreativeId = CreativesCount + 1;
			var creative = MockEntities.BuildCreative(0, "test12345");
			creative.Features = MockEntities.GetFeatures();

			Creatives.Create(creative);
			var newCreative = Creatives.Get(CreativeId);

			Assert.IsTrue(newCreative.Id == CreativeId, "New Creative has incorrect Id.");
			Assert.IsTrue(newCreative.Name == "test12345", "New Creative has incorrect Name.");
			Assert.IsTrue(newCreative.Features.Count() == 3, "New Creative has incorrect number of Features.");
			Assert.IsTrue(newCreative.AdType.IsPromo == false, "New Creative has incorrect IsPromo property.");
			Assert.IsTrue(newCreative.Campaign.Id == 10, "New Creative has incorrect Campaign Id.");
		}


		[Test(Description = "Destination Creative save does not create Models for Promotional.")]
		public void Destination_Creative_Save_Does_Not_Create_Models_For_Promotional()
		{
			var creative = MockEntities.BuildCreative(0, "test1234", true);
			var modelsInitialCount = CmsModels.GetAll().Count();

			var creativeReturned = Creatives.Create(creative);

			var modelsPostCount = CmsModels.GetAll().Count();
			Assert.IsTrue(modelsInitialCount == modelsPostCount, "Models count is not correct after creating promotional creative.");
		}

		[Test(Description = "Destination Creative save does not create Settings for Promotional.")]
		public void Destination_Creative_Save_Does_Not_Create_Settings_For_Promotional()
		{
			var creative = MockEntities.BuildCreative(0, "test1234", true);
			var settingsInitialCount = CmsSettings.GetAll().Count();

			var creativeReturned = Creatives.Create(creative);

			var settingsPostCount = CmsSettings.GetAll().Count();
			Assert.IsTrue(settingsInitialCount == settingsPostCount, "Settings count is not correct after creating promotional creative.");
		}

		[Test(Description = "Destination Creative save creates correct Models count.")]
		public void Destination_Creative_Save_Creates_Correct_Models_Count()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var modelsCount = CmsModels.GetAll().Count();
			Assert.IsTrue(modelsCount == 10, "Models count is not correct after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative save creates correct Settings count.")]
		public void Destination_Creative_Save_Creates_Correct_Settings_Count()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var settingsCount = CmsSettings.GetAll().Count();
			Assert.IsTrue(settingsCount == 5, "Settings count is not correct after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative save creates Models for correct Feature.")]
		public void Destination_Creative_Save_Creates_Correct_Models_For_Feature()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var model1 = CmsModels.Get(6);
			var model2 = CmsModels.Get(7);
			var model3 = CmsModels.Get(8);
			Assert.IsTrue(model1.Feature.Id == 1, "Model does not belong to correct Feature after creating Destination Creative.");
			Assert.IsTrue(model2.Feature.Id == 1, "Model does not belong to correct Feature after creating Destination Creative.");
			Assert.IsTrue(model3.Feature.Id == 2, "Model does not belong to correct Feature after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative save creates Settings for correct Feature.")]
		public void Destination_Creative_Save_Creates_Correct_Settings_For_Feature()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var setting1 = CmsSettings.Get(4);
			var setting2 = CmsSettings.Get(5);
			Assert.IsTrue(setting1.Feature.Id == 1, "Setting does not belong to correct Feature after creating Destination Creative.");
			Assert.IsTrue(setting2.Feature.Id == 2, "Setting does not belong to correct Feature after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative save creates Settings for correct Setting Definition.")]
		public void Destination_Creative_Save_Creates_Correct_Settings_For_SettingDefinition()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var setting1 = CmsSettings.Get(4);
			var setting2 = CmsSettings.Get(5);
			Assert.IsTrue(setting1.CmsSettingDefinition.Id == 1, "Setting does not belong to correct Setting Definition after creating Destination Creative.");
			Assert.IsTrue(setting2.CmsSettingDefinition.Id == 2, "Setting does not belong to correct Setting Definition after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative save creates Models for correct Model Definition.")]
		public void Destination_Creative_Save_Creates_Correct_Models_For_ModelDefinition()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var model1 = CmsModels.Get(6);
			var model2 = CmsModels.Get(7);
			var model3 = CmsModels.Get(8);
			Assert.IsTrue(model1.CmsModelDefinition.Id == 1, "Model does not belong to correct Model Definition after creating Destination Creative.");
			Assert.IsTrue(model2.CmsModelDefinition.Id == 2, "Model does not belong to correct Model Definition after creating Destination Creative.");
			Assert.IsTrue(model3.CmsModelDefinition.Id == 3, "Model does not belong to correct Model Definition after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative saves display field for Models correctly.")]
		public void Destination_Creative_Saves_Display_Field_For_Models()
		{
			var creative = MockEntities.BuildCreative(0, "test1234");
			creative.Features = MockEntities.GetFeatures();

			var creativeReturned = Creatives.Create(creative);

			var model1 = CmsModels.Get(6);
			var model2 = CmsModels.Get(7);
			var model3 = CmsModels.Get(8);
			Assert.IsTrue(model1.CmsModelDefinition.DisplayFieldName == "field 1", "Model does not have correct DisplayField value after creating Destination Creative.");
			Assert.IsTrue(model2.CmsModelDefinition.DisplayFieldName == "field 2", "Model does not have correct DisplayField value after creating Destination Creative.");
			Assert.IsTrue(model3.CmsModelDefinition.DisplayFieldName == "field 3", "Model does not have correct DisplayField value after creating Destination Creative.");
		}

		[Test(Description = "Destination Creative get does not return null for existing destination creative.")]
		public void Destination_Creative_Get_Not_Null()
		{
			var creativeReturned = Creatives.GetDestinationCreative(4);

			Assert.IsNotNull(creativeReturned, "Destination Creative is not null.");
		}

		[Test(Description = "Destination Creative get returns correct properties.")]
		public void Destination_Creative_Get_Returns_Correct_Properties()
		{
			var creative = Creatives.GetDestinationCreative(4);
			//creative.Features = new List<DestinationCreativeViewModel.DestinationCreativeFeatureViewModel>{new DestinationCreativeViewModel.DestinationCreativeFeatureViewModel{Id = 1}};

			var creativeAsJson = JObject.FromObject(creative).ToString();
			var creativeCompare = DestinationCreativeViewModel.FromCreative(Creatives.Get(4));
			var creativeAsJsonCompare = JObject.FromObject(creativeCompare).ToString();

			Assert.IsTrue(creativeAsJson == creativeAsJsonCompare, "Destination Creative properties are incorrect.");
		}

		[Test(Description = "Destination Creative save assigns CreativeId to appropriate resources.")]
		public void Destination_Creative_Assigns_Creative_To_New_Resources()
		{
			var creative = MockEntities.BuildCreative(0, "test1", true, 10, "3");

			var newCreative = Creatives.Create(creative);

			var resource1 = Resources.Get(3);
			Assert.IsTrue(resource1.Creative.Id == CreativeId, "Resource not assigned correct creative.");
		}

		[Test(Description = "Destination Creative resource initializes to activated.")]
		public void Destination_Creative_Resource_Initializes_To_Activated()
		{
			var creative = MockEntities.BuildCreative(CreativeId, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = CreativeId } };
			Resources.Create(resource);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == false, "Creative Resource is not activated.");
		}

		[Test(Description = "Destination Creative deactivates old resources for Create.")]
		public void Destination_Creative_Deactivates_Old_Resources_For_Create()
		{
			var creative = MockEntities.BuildCreative(0, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = CreativeId } };
			Resources.Create(resource);

			var newCreative = Creatives.Create(creative);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == true, "Creative Resource is not deactivated.");
		}

		[Test(Description = "Destination Creative deactivates old resources for Update.")]
		public void Destination_Creative_Deactivates_Old_Resources_For_Update()
		{
			var creative = MockEntities.BuildCreative(4, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = 4 } };
			Resources.Create(resource);

			var newCreative = Creatives.Update(creative);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == true, "Creative Resource is not deactivated.");
		}

	}
}


