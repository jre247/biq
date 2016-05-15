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
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrightLine.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Services;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class ModelsRetrievalTests
	{
		IocRegistration Container;
		IModelService ModelService { get;set;}
		IRepository<CmsModel> CmsModels { get;set;}
		ICreativeService Creatives { get; set; }

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupIoCContainer(Container);

			ModelService = IoC.Resolve<IModelService>();
			CmsModels = IoC.Resolve<IRepository<CmsModel>>();
			Creatives = IoC.Resolve<ICreativeService>();

			InsertModelsInRepository();
			CreateFeaturesForCreatives();
		}


		[Test(Description = "Retrieving list of Models is null.")]
		public void Cms_Models_Retrieval_Is_Null()
		{
			var cmsService = new CmsService();

			var models = ModelService.GetModelsForCreative(134);

			Assert.IsNull(models);
		}

		[Test(Description = "Retrieving list of Models is not null.")]
		public void Cms_Models_Retrieval_Is_Not_Null()
		{
			var cmsService = new CmsService();

			var models = ModelService.GetModelsForCreative(1);

			Assert.IsNotNull(models);
		}

		[Test(Description = "Retrieving list of Models has correct features count.")]
		public void Cms_Models_Retrieval_Has_Correct_Features_Count()
		{
			var cmsService = new CmsService();

			var models = ModelService.GetModelsForCreative(1);

			Assert.IsTrue(models.features.Count() == 2);
		}

		[Test(Description = "Retrieving list of Models has correct models count.")]
		public void Cms_Models_Retrieval_Has_Correct_Models_Count()
		{
			var cmsService = new CmsService();

			var creative = ModelService.GetModelsForCreative(1);

			Assert.IsTrue(creative.features[1].models.Count() == 1);
			Assert.IsTrue(creative.features[2].models.Count() == 1);
		}

		[Test(Description = "Retrieving list of Models has correct feature hash keys.")]
		public void Cms_Models_Retrieval_Has_Correct_Feature_Hash_Keys()
		{
			var cmsService = new CmsService();

			var creative = ModelService.GetModelsForCreative(1);
			var creative2 = ModelService.GetModelsForCreative(2);

			Assert.IsTrue(creative.features.ContainsKey(1), "Creative does not contain feature with key '1'");
			Assert.IsTrue(creative.features.ContainsKey(2), "Creative does not contain feature with key '2'");
			Assert.IsTrue(creative2.features.ContainsKey(3), "Creative does not contain feature with key '3'");
			Assert.IsTrue(creative2.features.ContainsKey(4), "Creative does not contain feature with key '4'");
			Assert.IsTrue(creative2.features.ContainsKey(5), "Creative does not contain feature with key '5'");
		}

		[Test(Description = "Retrieving list of Models has correct model hash keys.")]
		public void Cms_Models_Retrieval_Has_Correct_Model_Hash_Keys()
		{
			var cmsService = new CmsService();

			var creative = ModelService.GetModelsForCreative(1);
			var creative2 = ModelService.GetModelsForCreative(2);

			Assert.IsTrue(creative.features[1].models.ContainsKey(1), "Feature does not contain model with key '1'");
			Assert.IsTrue(creative.features[2].models.ContainsKey(2), "Feature does not contain model with key '2'");
			Assert.IsTrue(creative2.features[3].models.ContainsKey(3), "Feature does not contain model with key '3'");
			Assert.IsTrue(creative2.features[4].models.ContainsKey(4), "Feature does not contain model with key '4'");
			Assert.IsTrue(creative2.features[5].models.ContainsKey(5), "Feature does not contain model with key '5'");
		}

		[Test(Description = "Retrieving list of Models has correct feature ids.")]
		public void Cms_Models_Retrieval_Has_Correct_Feature_Ids()
		{
			var cmsService = new CmsService();

			var creative1 = ModelService.GetModelsForCreative(1);
			var creative2 = ModelService.GetModelsForCreative(2);

			Assert.IsTrue(creative1.features[1].id == 1, "Feature does not have id equal to 1.");
			Assert.IsTrue(creative1.features[2].id == 2, "Feature does not have id equal to 2.");
			Assert.IsTrue(creative2.features[3].id == 3, "Feature does not have id equal to 3.");
			Assert.IsTrue(creative2.features[4].id == 4, "Feature does not have id equal to 2.");
			Assert.IsTrue(creative2.features[5].id == 5, "Feature does not have id equal to 5.");
		}

		[Test(Description = "Retrieving list of Models has correct model names.")]
		public void Cms_Models_Retrieval_Has_Correct_Model_Ids()
		{
			var cmsService = new CmsService();

			var creative = ModelService.GetModelsForCreative(1);
			var creative2 = ModelService.GetModelsForCreative(2);

			Assert.IsTrue(creative.features[1].models[1].name == "test model 1", "Model does not have name equal to 'test model 1'.");
			Assert.IsTrue(creative.features[2].models[2].name == "test model 2", "Model does not have name equal to 'test model 2'.");
			Assert.IsTrue(creative2.features[3].models[3].name == "test model 3", "Model does not have name equal to 'test model 3'.");
			Assert.IsTrue(creative2.features[4].models[4].name == "test model 4", "Model does not have name equal to 'test model 4'.");
			Assert.IsTrue(creative2.features[5].models[5].name == "test model 5", "Model does not have name equal to 'test model 5'.");
		}

		[Test(Description = "Retrieving list of Models has correct model definition ids.")]
		public void Cms_Models_Retrieval_Has_Correct_ModelDefinition_Ids()
		{
			var cmsService = new CmsService();

			var creative = ModelService.GetModelsForCreative(1);
			var creative2 = ModelService.GetModelsForCreative(2);

			Assert.IsTrue(creative.features[1].models[1].modelDefinitionId == 1, "Model does not model definition id equal to '1'.");
			Assert.IsTrue(creative.features[2].models[2].modelDefinitionId == 1, "Model does not model definition id equal to '1'.");
			Assert.IsTrue(creative2.features[3].models[3].modelDefinitionId == 1, "Model does not model definition id equal to '1'.");
			Assert.IsTrue(creative2.features[4].models[4].modelDefinitionId == 2, "Model does not model definition id equal to '2'.");
			Assert.IsTrue(creative2.features[5].models[5].modelDefinitionId == 2, "Model does not model definition id equal to '2'.");
		}

	

		#region Private Methods

		private void InsertModelsInRepository()
		{
			var model1 = CmsModels.Get(1);
			MockEntities.SetModelProperties(ref model1, "test model 1", 10, 1, 1, 1);

			var model2 = CmsModels.Get(2);
			MockEntities.SetModelProperties(ref model2, "test model 2", 10, 1, 2, 1);

			var model3 = CmsModels.Get(3);
			MockEntities.SetModelProperties(ref model3, "test model 3", 10, 2, 3, 1);

			var model4 = CmsModels.Get(4);
			MockEntities.SetModelProperties(ref model4, "test model 4", 10, 2, 4, 2);

			var model5 = CmsModels.Get(5);
			MockEntities.SetModelProperties(ref model5, "test model 5", 10, 2, 5, 2);

			CmsModels.Save();
		}

		private void CreateFeaturesForCreatives()
		{
			var creative1 = Creatives.Get(1);
			creative1.Features = new List<Feature>();
			creative1.Features.Add(MockEntities.BuildFeature(1, "testFeature1", 1));
			creative1.Features.Add(MockEntities.BuildFeature(2, "testFeature2", 1));

			var creative2 = Creatives.Get(2);
			creative2.Features = new List<Feature>();
			creative2.Features.Add(MockEntities.BuildFeature(3, "testFeature3", 2));
			creative2.Features.Add(MockEntities.BuildFeature(4, "testFeature4", 2));
			creative2.Features.Add(MockEntities.BuildFeature(5, "testFeature5", 2));

			Creatives.Save();
		}

		#endregion //Private Methods
	}
}
