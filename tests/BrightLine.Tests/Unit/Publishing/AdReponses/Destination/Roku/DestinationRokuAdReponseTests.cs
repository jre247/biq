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
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.ResourceType;
using Newtonsoft.Json.Linq;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.Roku;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Services.Destination.Platforms;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class DestinationRokuAdReponseTests
	{
		#region Variables

		IocRegistration Container;
		Guid PublishId;
		int AdTagId = 12;
		Ad Ad;
		int CampaignId = 111;
		string Key = "111"; // Assign Key to be same as CampaignId
		int AdId = 1;
		int CreativeId = 1;
		string AdName = "Test Ad";
		string PlatformManifestName = PlatformConstants.ManifestNames.Roku;
		string PlatformName = PlatformConstants.PlatformNames.Roku.ToLower();
		int PlatformId;
		string AdTypeManifestName = AdTypeConstants.ManifestNames.BrandDestination;
		string AdTypeName = AdTypeConstants.AdTypeNames.BrandDestination;
		int AdTypeId;
		int FeatureId1 = 111;
		string FeatureName1 = "Feature 1";
		int FeatureId2 = 222;
		string FeatureName2 = "Feature 2";
		string BlueprintManifestName1 = "video-gallery";
		string BlueprintManifestName2 = "info-video-gallery";
		string BlueprintName1 = "video1";
		string BlueprintName2 = "video2";
		int PageId1 = 1;
		int PageId2 = 2;
		string TargetEnv = PublishConstants.TargetEnvironments.Production;
		string MappedTargetEnv = PublishConstants.DestinationAdResponse.Roku.MappedEnvironments.Production;
		int ModelInstanceId1 = 222;
		int ModelInstanceId2 = 444;
		int ModelId1 = 111;
		string ModelName1 = "Choice1";
		string ModelName2 = "Choice2";
		int ModelId2 = 222;
		int SettingInstanceId1 = 333;
		int SettingInstanceId2 = 555;
		int SettingId1 = 333;
		int SettingId2 = 444;
		string ModelInstance1FieldName1 = "thumbnailSrc";
		string ModelInstance2FieldName1 = "width";
		string ModelInstance1FieldValue1 = "i-300-452.png";
		string ModelInstance2FieldValue1 = "133";
		string SettingInstance1FieldName1 = "thumbnailSrc";
		string SettingInstance1FieldValue1 = "i-500-654.png";
		string SettingInstance2FieldName1 = "height";
		string SettingInstance2FieldValue1 = "655";
		int InactivityThreshold = 500;
		int CompanionAdId = 23;
		int CompanionAdTagId = 24;
		string RepoName = "Test-Repo-Name";
		int Generation = 2;

		#endregion //Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();
			Container.Register<IEnvironmentHelper, EnvironmentHelper>();

			PlatformId = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			AdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			PublishId = Guid.NewGuid();	
			SetupObjectGraph();

		}

		#endregion //Setup

		#region Tests

		[Test(Description = "(BL-462) Destination Roku Ad Responses count is correct")]
		public void Destination_Roku_AdResponses_Count_Is_Correct()
		{
			// Act
			var RokuDestinationAdResponse = new RokuDestinationAdResponse(Ad, TargetEnv, PublishId);
			var adResponses = RokuDestinationAdResponse.GetAdResponse();

			// Assert
			Assert.IsNotNull(adResponses.AdResponseBody.DI, "Ad Response DI should not be null.");
			Assert.IsNotNull(adResponses.AdResponseBody.RAF, "Ad Response RAF should not be null.");
			Assert.IsNull(adResponses.AdResponseBody.Default, "Ad Response Default should be null.");
		}

		[Test(Description = "(BL-462) Destination Roku Ad Response metadata is correct")]
		public void Destination_Roku_AdResponse_Metadata_Is_Correct()
		{
			// Act
			var RokuDestinationAdResponse = new RokuDestinationAdResponse(Ad, TargetEnv, PublishId);
			var adResponses = RokuDestinationAdResponse.GetAdResponse();

			Assert.AreEqual(adResponses.Metadata.ad.id, AdId, "Ad Response Metadata Ad Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adType.manifestName, AdTypeManifestName, "Ad Response Metadata AdType ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.platform.manifestName, PlatformManifestName, "Ad Response Metadata Platform ManifestName is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.adTagId, AdTagId, "Ad Response Metadata Ad Tag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.responseType, PublishConstants.ResponseTypes.Json, "Ad Response Metadata responseType is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.companionAd.id, CompanionAdId, "Ad Response Metadata CompanionAd Id is not correct.");
			Assert.IsNotNull(adResponses.Metadata.ad.companionAd.adTagId, "Ad Response Metadata CompanionAd AdTag Id is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.campaign.id, CampaignId, "Ad Response Metadata Ad CampaignId is not correct.");
			Assert.AreEqual(adResponses.Metadata.ad.creative.id, CreativeId, "Ad Response Metadata Ad CreativeId is not correct.");
		}

		[Test(Description = "(BL-462) Destination Roku RAF Ad Response Key is correct")]
		public void Destination_Roku__RAF_AdResponse_Key_Is_Correct()
		{
			// Act
			var RokuDestinationAdResponse = new RokuDestinationAdResponse(Ad, TargetEnv, PublishId);
			var adResponses = RokuDestinationAdResponse.GetAdResponse();

			// Assert
			var key = adResponses.Key;
			Assert.AreEqual(key, string.Format("{0}_{1}", PublishConstants.TargetEnvironments.Production, AdTagId), "Ad Response Key is not correct.");
		}

		#region DI Tests
	
		[Test(Description = "(BL-462) Destination Roku DI Ad Response Poco is correct")]
		public void Destination_Roku_DI_AdResponse_Body_Is_Correct()
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Act
			var RokuDestinationAdResponse = new RokuDestinationAdResponse(Ad, TargetEnv, PublishId);
			var adResponses = RokuDestinationAdResponse.GetAdResponse();

			// Assert
			var adResponsePoco = (RokuDestinationViewModel)adResponses.AdResponseBody.DI;

			Assert.AreEqual(adResponsePoco.isDirect, true, "isDirect not correct.");
			AssertMedia(PublishId, AdId, AdName, adResponsePoco);
			AssertAnalytics(MappedTargetEnv, PlatformName, AdId, adResponsePoco);
			AssertCms(settings, TargetEnv, Key, adResponsePoco);
			AssertUrls(settings, AdId, CampaignId, adResponsePoco);
			Assert.AreEqual(adResponsePoco.layout, "blueprint-layout", "Layout is not correct.");
			Assert.AreEqual(adResponsePoco.nav, "blueprint-horizontal-nav", "Nav is not correct.");
			Assert.AreEqual(adResponsePoco.appSettings, "app_settings", "AppSettings is not correct.");
			Assert.AreEqual(adResponsePoco.inactivityThreshold, InactivityThreshold, "inactivityThreshold is not correct.");
			AssertFeatures(adResponsePoco);

		}

		#endregion  //DI

		#region RAF Tests

		[Test(Description = "(BL-462) Destination Roku RAF Ad Response Poco is correct")]
		public void Destination_Roku_AdResponse_Body_Is_Correct()
		{
			var settings = IoC.Resolve<ISettingsService>();

			// Act
			var RokuDestinationAdResponse = new RokuDestinationAdResponse(Ad, TargetEnv, PublishId);
			var adResponses = RokuDestinationAdResponse.GetAdResponse();

			// Assert
			var adResponsePoco = (RokuDestinationViewModel)adResponses.AdResponseBody.RAF;

			Assert.AreEqual(adResponsePoco.isDirect, false, "isDirect not correct.");
			AssertMedia(PublishId, AdId, AdName, adResponsePoco);
			AssertAnalytics(MappedTargetEnv, PlatformName, AdId, adResponsePoco);
			AssertCms(settings, TargetEnv, Key, adResponsePoco);
			AssertUrls(settings, AdId, CampaignId, adResponsePoco);
			Assert.AreEqual(adResponsePoco.layout, "blueprint-layout", "Layout is not correct.");
			Assert.AreEqual(adResponsePoco.nav, "blueprint-horizontal-nav", "Nav is not correct.");
			Assert.AreEqual(adResponsePoco.appSettings, "app_settings", "AppSettings is not correct.");
			Assert.AreEqual(adResponsePoco.inactivityThreshold, InactivityThreshold, "inactivityThreshold is not correct.");
			AssertFeatures(adResponsePoco);

		}

		#endregion // RAF

		#endregion //Tests

		#region Assertions

		/// <summary>
		/// Assert Features
		/// </summary>
		/// <param name="adResponse"></param>
		private void AssertFeatures(RokuDestinationViewModel adResponse)
		{
			AssertFeaturePrimaryFields(FeatureId1, FeatureName1, BlueprintManifestName1, BlueprintName1, PageId1, adResponse);
			AssertFeaturePrimaryFields(FeatureId2, FeatureName2, BlueprintManifestName2, BlueprintName2, PageId2, adResponse);

			AssertFeatureModels(adResponse);
			AssertFeatureSettings(adResponse);
		}

		/// <summary>
		/// Assert Feature Models
		/// </summary>
		/// <param name="adResponse"></param>
		private void AssertFeatureModels(RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.features[FeatureId1].models.Count(), 1, "Features Models count is not correct.");
			AssertFeatureModel(adResponse, FeatureId1, ModelName1, 0, ModelInstanceId1, ModelId1, ModelInstance1FieldName1, ModelInstance1FieldValue1);

			Assert.AreEqual(adResponse.features[FeatureId2].models.Count(), 1, "Features Models count is not correct.");
			AssertFeatureModel(adResponse, FeatureId2, ModelName2, 0, ModelInstanceId2, ModelId2, ModelInstance2FieldName1, ModelInstance2FieldValue1);
		}

		/// <summary>
		/// Assert Feature Settings
		/// </summary>
		/// <param name="adResponse"></param>
		private void AssertFeatureSettings(RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.features[FeatureId1].settings.Count(), 1, "Features Settings count is not correct.");
			AssertFeatureSetting(adResponse, FeatureId1, 0, SettingInstanceId1, SettingId1, SettingInstance1FieldName1, SettingInstance1FieldValue1);

			Assert.AreEqual(adResponse.features[FeatureId2].settings.Count(), 1, "Features Settings count is not correct.");
			AssertFeatureSetting(adResponse, FeatureId2, 0, SettingInstanceId2, SettingId2, SettingInstance2FieldName1, SettingInstance2FieldValue1);
		}
	
		/// <summary>
		/// Assert Primary Fields for a given Feature.
		/// </summary>
		/// <param name="featureId"></param>
		/// <param name="featureName"></param>
		/// <param name="blueprintManifestName"></param>
		/// <param name="blueprintName"></param>
		/// <param name="pageId"></param>
		/// <param name="adResponse"></param>
		private static void AssertFeaturePrimaryFields(int featureId, string featureName, string blueprintManifestName, string blueprintName, int pageId, RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.features[featureId].blueprint, blueprintManifestName, "Features Blueprint is not correct.");
			Assert.AreEqual(adResponse.features[featureId].id, featureId, "Features Id is not correct.");
			Assert.AreEqual(adResponse.features[featureId].name, blueprintName, "Features Name is not correct.");
			Assert.AreEqual(adResponse.features[featureId].pages[pageId].page_id, pageId, "Features Page is not correct.");
		}

		/// <summary>
		/// Assert for an individual Model for a given Feature
		/// </summary>
		/// <param name="adResponse"></param>
		/// <param name="featureId"></param>
		/// <param name="modelIndex"></param>
		/// <param name="modelInstanceId"></param>
		/// <param name="modelId"></param>
		/// <param name="modelInstanceFieldName"></param>
		/// <param name="modelInstanceFieldValue"></param>
		private void AssertFeatureModel(RokuDestinationViewModel adResponse, int featureId, string modelName, int modelIndex, int modelInstanceId, int modelId, string modelInstanceFieldName, string modelInstanceFieldValue)
		{
			var modelInstancesGroup = adResponse.features[featureId].models[modelName];
			var modelInstance = modelInstancesGroup.ElementAt(modelIndex);

			Assert.AreEqual(modelInstance.Count(), 4, "Features Models Instance Properties count is incorrect.");

			// Assert Model Id property
			Assert.AreEqual(modelInstance[CmsPublishConstants.ModelInstanceJsonProperties.Id], modelInstanceId.ToString(), "Features Models Id is not correct.");

			// Assert Model BL Property
			AssertFeatureModelBLProperty(featureId, modelInstanceId, modelId, modelInstance);

			// Assert Model Instance Field property 
			//	*Note: a Model Instance can have 1 or more fields and each of these fields will be a property on the Model dictionary
			Assert.AreEqual(modelInstance[modelInstanceFieldName], modelInstanceFieldValue, "Features Models Instance Field is not correct.");
		}

		/// <summary>
		/// Assert for an individual Setting for a given Feature
		/// </summary>
		/// <param name="adResponse"></param>
		/// <param name="featureId"></param>
		/// <param name="modelIndex"></param>
		/// <param name="modelInstanceId"></param>
		/// <param name="modelId"></param>
		/// <param name="modelInstanceFieldName"></param>
		/// <param name="modelInstanceFieldValue"></param>
		private void AssertFeatureSetting(RokuDestinationViewModel adResponse, int featureId, int settingIndex, int settingInstanceId, int settingId, string settingInstanceFieldName, string settingInstanceFieldValue)
		{
			var setting = adResponse.features[featureId].settings[settingIndex];

			Assert.AreEqual(setting.Count(), 2, "Features Settings Instance Properties count is incorrect.");

			// Assert Setting Id property
			Assert.AreEqual(setting[CmsPublishConstants.ModelInstanceJsonProperties.Id], settingInstanceId.ToString(), "Features Settings Id is not correct.");

			// Assert Setting Instance Field property 
			//	*Note: a Setting Instance can have 1 or more fields and each of these fields will be a property on the Setting dictionary
			Assert.AreEqual(setting[settingInstanceFieldName], settingInstanceFieldValue, "Features Settings Instance Field is not correct.");
		}

		/// <summary>
		/// Assert for the BL Property for a given Feature Model
		/// </summary>
		/// <param name="featureId"></param>
		/// <param name="modelInstanceId"></param>
		/// <param name="modelId"></param>
		/// <param name="model"></param>
		private void AssertFeatureModelBLProperty(int featureId, int modelInstanceId, int modelId, Dictionary<string, object> model)
		{
			var blDeserialized = (BLViewModel)(model[CmsPublishConstants.ModelInstanceJsonProperties.BL]);

			Assert.AreEqual(blDeserialized.campaignId, CampaignId, "Features Models BL campaignId is not correct.");
			Assert.AreEqual(blDeserialized.creativeId, CreativeId, "Features Models BL creativeId is not correct.");
			Assert.AreEqual(blDeserialized.featureId, featureId, "Features Models BL featureId is not correct.");
			Assert.AreEqual(blDeserialized.instanceId, modelInstanceId, "Features Models BL instanceId is not correct.");
			Assert.AreEqual(blDeserialized.modelId, modelId, "Features Models BL modelId is not correct.");
		}

		/// <summary>
		/// Assert the Urls object that resides on an Ad Response for a Generation 1 Campaign
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="AdId"></param>
		/// <param name="CampaignId"></param>
		/// <param name="adResponse"></param>
		private void AssertUrls(ISettingsService settings, int AdId, int CampaignId, RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.urls.analytics, settings.TrackingUrl, "Urls analytics is not correct.");
			Assert.AreEqual(adResponse.urls.images, string.Format("{0}/campaigns/{1}/images", settings.MediaCDNBaseUrl, CampaignId), "Urls images is not correct.");
			Assert.AreEqual(adResponse.urls.videos, string.Format("{0}/campaigns/{1}/videos", settings.MediaCDNBaseUrl, CampaignId), "Urls videos is not correct.");
			Assert.AreEqual(adResponse.urls.fonts, string.Format("{0}/campaigns/{1}/fonts", settings.MediaCDNBaseUrl, CampaignId), "Urls fonts is not correct.");
		}

		/// <summary>
		/// Assert the Cms object that resides on the Ad Response
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="TargetEnv"></param>
		/// <param name="Key"></param>
		/// <param name="adResponse"></param>
		private void AssertCms(ISettingsService settings, string TargetEnv, string Key, RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.cms.baseUrl, string.Format("{0}/{1}/{2}", settings.CmsBaseUrl, TargetEnv, CampaignId), "Cms baseUrl is not correct.");
			Assert.AreEqual(adResponse.cms.key, Key, "Cms Key is not correct.");
		}

		/// <summary>
		/// Assert Analytics object that resides on the Ad Response
		/// </summary>
		/// <param name="TargetEnv"></param>
		/// <param name="PlatformName"></param>
		/// <param name="AdId"></param>
		/// <param name="adResponse"></param>
		private void AssertAnalytics(string TargetEnv, string PlatformName, int AdId, RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.analytics.ad_id, AdId, "Analytics ad_id is not correct.");
			Assert.AreEqual(adResponse.analytics.env, TargetEnv, "Analytics env is not correct.");
			Assert.AreEqual(adResponse.analytics.platform, PlatformName, "Analytics platform is not correct.");
			Assert.AreEqual(adResponse.analytics.valid, true, "Analytics valid should be true.");
		}

		/// <summary>
		/// Asser the Media object that resides on the Ad Response
		/// </summary>
		/// <param name="publishedId"></param>
		/// <param name="AdId"></param>
		/// <param name="AdName"></param>
		/// <param name="adResponse"></param>
		private void AssertMedia(Guid publishedId, int AdId, string AdName, RokuDestinationViewModel adResponse)
		{
			Assert.AreEqual(adResponse.media.cacheVer, publishedId.ToString(), "Media cacheVer is not correct.");
			Assert.AreEqual(adResponse.media.name, AdName, "Media name is not correct.");
			Assert.AreEqual(adResponse.media.shortName, AdId.ToString(), "Media shortName is not correct.");
			Assert.AreEqual(adResponse.media.ver, "1.0", "Media ver is not correct.");
		}

		#endregion

		#region Helper Methods

		private void SetupObjectGraph()
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var settings = IoC.Resolve<ISettingsService>();
			
			var adTypeId = Lookups.AdTypes.HashByName[AdTypeName];
			var creativeName = "Creative 1";
			var creativeDescription = "Creative Description 1";
			var xCoordinateHd = 111;
			var yCoordinateHd = 222;
			var xCoordinateSd = 333;
			var yCoordinateSd = 444;		

			Ad = new Ad
			{
				Id = AdId,
				Name = AdName,
				AdType = new AdType { Id = Lookups.AdTypes.HashByName[AdTypeName], Name = AdTypeName, ManifestName = AdTypeConstants.ManifestNames.BrandDestination},
				AdTag = new AdTag { Id = AdTagId },
				Creative = new Creative { Id = CreativeId, Name = creativeName, Description = creativeDescription, InactivityThreshold = InactivityThreshold },
				Platform = new Platform { Id = PlatformId, Name = PlatformName, ManifestName = PlatformConstants.ManifestNames.Roku },
				Campaign = new Campaign { Id = CampaignId, Generation = Generation },
				XCoordinateHd = xCoordinateHd,
				YCoordinateHd = yCoordinateHd,
				XCoordinateSd = xCoordinateSd,
				YCoordinateSd = yCoordinateSd,
				CompanionAd = new Ad { Id = CompanionAdId, AdTag = new AdTag { Id = CompanionAdTagId } },
				RepoName = RepoName
			};

			CreateFeaturesForAd();
			SetupObjectGraphForAd();

		}

		public void CreateFeaturesForAd()
		{
			var campaign = new Campaign
			{
				Id = CampaignId,
				Name = "Test Campaign"
			};

			Ad.Features = new List<Feature>
			{
				new Feature
				{
					Id = FeatureId1, 
					Name = FeatureName1, 
					Blueprint = new Blueprint
					{
						ManifestName = BlueprintManifestName1,
						Name = BlueprintName1
					},
					Campaign = campaign,
					Pages = new List<Page>
					{
						new Page{Id = PageId1}
					}
				},
				new Feature
				{
					Id = FeatureId2, 
					Name = FeatureName2, 
					Blueprint = new Blueprint
					{
						ManifestName = BlueprintManifestName2,
						Name = BlueprintName2
					},
					Campaign = campaign,
					Pages = new List<Page>
					{
						new Page{Id = PageId2}
					}
				}
			};
		}

		private void SetupObjectGraphForAd()
		{
			var adTypes = IoC.Resolve<IRepository<AdType>>();
			var creatives = IoC.Resolve<ICreativeService>();
			var placements = IoC.Resolve<IPlacementService>();
			var campaigns = IoC.Resolve<ICampaignService>();

			var resourceId = 1;
			var resourceWidth = 11;
			var resourceHeight = 22;
			var resourceBitrate = 1000;
			var resourceFilename = "file1.mp4";
			var resourceDuration = 160;
			var resourceExtensionId = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];
			var resourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage];
			
			// Create new Campaign
			var campaignNew = new Campaign{Id = CampaignId, Name = "Test Campaign"};
			campaigns.Create(campaignNew);

			// Set up campaign to have ads to pass Ad Validation
			var campaign = campaigns.Get(CampaignId);
			campaign.Ads = new List<Ad>();
			campaign.Ads.Add(Ad);
	
			// Create Resources for Ad
			var resource = new Resource{
				Id = resourceId, 
				ResourceType = new ResourceType{Id = resourceTypeId}, 
				Extension = new FileType{Id = resourceExtensionId }, 
				Width = resourceWidth, 
				Height = resourceHeight, 
				Bitrate = resourceBitrate,
				Filename = resourceFilename,
				Duration = resourceDuration
			};
			Ad.Creative.Resources = new List<Resource>{resource};
			
			// Create Model Instances for Ad's Campaign
			BuildModelInstance(ModelInstanceId1, ModelId1, ModelName1, FeatureId1, ModelInstance1FieldName1, ModelInstance1FieldValue1);
			BuildModelInstance(ModelInstanceId2, ModelId2, ModelName2, FeatureId2, ModelInstance2FieldName1, ModelInstance2FieldValue1);
				
			// Create Setting Instances for Ad's Campaign
			BuildSettingInstance(SettingInstanceId1, SettingId1, FeatureId1, SettingInstance1FieldName1, SettingInstance1FieldValue1);
			BuildSettingInstance(SettingInstanceId2, SettingId2, FeatureId2, SettingInstance2FieldName1, SettingInstance2FieldValue1);

		}

		private void BuildModelInstance(int modelInstanceId, int modelId, string modelName, int featureId, string fieldName, string fieldValue)
		{
			var cmsModelInstances = IoC.Resolve<IRepository<CmsModelInstance>>();

			var model = new CmsModel
			{
				Id = modelId,
				Name = modelName,
				Feature = new Feature
				{
					Id = featureId,
					Creative = new Creative
					{
						Id = CreativeId
					},
					Campaign = new Campaign
					{
						Id = CampaignId
					}
				}
			};

			var modelInstance = new CmsModelInstance
			{
				Id = modelInstanceId,
				Model = model
			};

			var publishedJson = new Dictionary<string, object>();

			// Setup PublishedJson Id Property
			publishedJson.Add(CmsPublishConstants.ModelInstanceJsonProperties.Id, modelInstanceId.ToString());
			publishedJson.Add(CmsPublishConstants.ModelInstanceJsonProperties.ModelName, modelName);

			// Setup PublishedJson BL Property
			var blViewModel = BLViewModel.Parse(modelInstance);
			var blJObject = BLViewModel.ToJObject(blViewModel);
			publishedJson.Add(CmsPublishConstants.ModelInstanceJsonProperties.BL, blJObject.ToString());

			// Setup PublishedJson Instance Field Property
			publishedJson.Add(fieldName, new string[1]{fieldValue});

			// Set PublishedJson value
			modelInstance.PublishedJson = JObject.FromObject(publishedJson).ToString();

			cmsModelInstances.Insert(modelInstance);
			cmsModelInstances.Save();
		}

		private void BuildSettingInstance(int settingInstanceId, int settingId, int featureId, string fieldName, string fieldValue)
		{
			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();

			var setting = new CmsSetting
			{
				Id = settingId,
				Feature = new Feature
				{
					Id = featureId,
					Creative = new Creative
					{
						Id = CreativeId
					},
					Campaign = new Campaign
					{
						Id = CampaignId
					}
				}
			};

			var settingInstance = new CmsSettingInstance
			{
				Id = settingInstanceId,
				Setting_Id = settingId
			};

			var publishedJson = new Dictionary<string, object>();

			// Setup PublishedJson Id Property
			publishedJson.Add(CmsPublishConstants.SettingInstanceJsonProperties.Id, settingInstanceId.ToString());

			// Setup PublishedJson Instance Field Property
			publishedJson.Add(fieldName, new string[1]{fieldValue});

			// Set PublishedJson value
			settingInstance.PublishedJson = JObject.FromObject(publishedJson).ToString();

			cmsSettingInstances.Insert(settingInstance);
			cmsSettingInstances.Save();

			cmsSettings.Insert(setting);
			cmsSettings.Save();
		}

		#endregion

	}
}
