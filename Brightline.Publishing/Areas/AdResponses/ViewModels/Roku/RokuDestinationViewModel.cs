using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Publishing.Areas.AdResponses.Enums;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.ViewModels.Roku
{
	public class RokuDestinationViewModel
	{
		#region Members

		public bool isDirect;
		public MediaViewModel media;
		public AnalyticsViewModel analytics;
		public CmsViewModel cms;
		public UrlsViewModel urls;
		public string layout;
		public string nav;
		public string appSettings;
		public Dictionary<int, FeaturesViewModel.FeatureViewModel> features;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? inactivityThreshold;
		
		#endregion

		#region Init

		//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
		[JsonConstructor]
		private RokuDestinationViewModel()
		{ }

		public RokuDestinationViewModel(Ad ad, Guid publishId, string targetEnv, RokuSdk rokuSdk)
		{
			isDirect = rokuSdk == RokuSdk.DirectIntegration ? true : false;
			media = MediaViewModel.Parse(ad, publishId);
			analytics = AnalyticsViewModel.Parse(ad, targetEnv);
			cms = CmsViewModel.Parse(ad, targetEnv);
			urls = UrlsViewModel.Parse(ad);
			layout = "blueprint-layout"; // hardcoded for now
			nav = "blueprint-horizontal-nav"; // hardcoded for now
			appSettings = "app_settings"; // hardcoded for now
			features = FeaturesViewModel.Parse(ad.Features);
			inactivityThreshold = ad.Creative.InactivityThreshold;
		}

		#endregion

		#region Public Methods

		public static JObject ToJObject(RokuDestinationViewModel rokuDestinationViewModel)
		{
			if (rokuDestinationViewModel == null)
				return null;

			var json = JObject.FromObject(rokuDestinationViewModel);
			return json;
		}

		#endregion

		#region Subclasses

		public class MediaViewModel
		{
			#region Members

			public string name;
			public string shortName;
			public string ver;
			public string cacheVer;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private MediaViewModel()
			{ }

			private MediaViewModel(Ad ad, Guid publishId) 
			{
				ver = "1.0"; // this is hardcoded 
				name = ad.Name;
				shortName = ad.Id.ToString();
				cacheVer = publishId.ToString();
			}

			#endregion

			#region Public Methods

			public static MediaViewModel Parse(Ad ad, Guid publishId)
			{
				if (ad == null)
					return null;

				return new MediaViewModel(ad, publishId);
			}

			#endregion
		}

		public class AnalyticsViewModel
		{
			#region Members

			public int ad_id;
			public string platform;
			public string env;
			public bool valid;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private AnalyticsViewModel()
			{ }

			private AnalyticsViewModel(Ad ad, string targetEnv)
			{
				ad_id = ad.Id;
				platform = ad.Platform.Name.ToLower();
				env = RokuDestinationAdResponseHelper.GetMappedEnvironment(targetEnv);
				valid = true; // Valid: true means the traffic is real traffic. IQ should always set this to true.
			}

			#endregion

			#region Public Methods

			public static AnalyticsViewModel Parse(Ad ad, string targetEnv)
			{
				if (ad == null || string.IsNullOrEmpty(targetEnv))
					return null;

				return new AnalyticsViewModel(ad, targetEnv);
			}

			#endregion
		}

		public class CmsViewModel
		{
			#region Members

			public string baseUrl;
			public string key;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CmsViewModel()
			{ }

			private CmsViewModel(Ad ad, string targetEnv)
			{
				var settings = IoC.Resolve<ISettingsService>();

				if (settings.CMSVersion == 1)
					baseUrl = string.Format("{0}/api/", settings.CmsBaseUrl);
				else
					baseUrl = string.Format("{0}/{1}/{2}", settings.CmsBaseUrl, targetEnv, ad.Campaign.Id);

				key = ad.Campaign.Id.ToString();
			}

			#endregion

			#region Public Methods

			public static CmsViewModel Parse(Ad ad, string targetEnv)
			{
				if (ad == null || string.IsNullOrEmpty(targetEnv))
					return null;

				return new CmsViewModel(ad, targetEnv);
			}

			#endregion
		}

		public class UrlsViewModel
		{
			#region Members

			public string analytics;
			public string images;
			public string fonts;
			public string videos;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private UrlsViewModel()
			{ }

			private UrlsViewModel(Ad ad)
			{
				var settings = IoC.Resolve<ISettingsService>();

				analytics = settings.TrackingUrl;

				var urls = new DestinationAdResponseUrls(ad);
				images = urls.images;
				fonts = urls.fonts;
				videos = urls.videos;		
			}

			#endregion

			#region Public Methods

			public static UrlsViewModel Parse(Ad ad)
			{
				return new UrlsViewModel(ad);
			}

			#endregion
		}

		public class FeaturesViewModel
		{
			#region Members

			public Dictionary<int, FeaturesViewModel.FeatureViewModel> features;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private FeaturesViewModel()
			{ }

			private FeaturesViewModel(IEnumerable<Feature> featuresIn)
			{
				features = featuresIn.Select(f => FeaturesViewModel.FeatureViewModel.Parse(f)).ToDictionary(f => f.id, f => f);
			}

			#endregion

			#region Public Methods

			public static Dictionary<int, FeaturesViewModel.FeatureViewModel> Parse(IEnumerable<Feature> features)
			{
				var featuresViewModel = new FeaturesViewModel(features);
				return featuresViewModel.features;
			}

			#endregion

			#region Subclasses

			public class FeatureViewModel
			{
				#region Members

				public string blueprint;
				public string name;
				public Dictionary<int, FeatureViewModel.PageViewModel> pages;
				public int id;
				public Dictionary<string, Dictionary<string, object>[]> models;
				public Dictionary<string, object>[] settings;

				#endregion

				#region Init

				//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
				[JsonConstructor]
				private FeatureViewModel()
				{ }

				private FeatureViewModel(Feature feature)
				{
					blueprint = feature.Blueprint.ManifestName;
					name = feature.Blueprint.Name;
					id = feature.Id;
					pages = FeatureViewModel.PagesViewModel.Parse(feature.Pages);

					// Build up all Model Instances grouped by Model Name for this feature
					models = ModelViewModel.Parse(feature);

					// Build up all Setting Instances for this Feature
					settings = SettingViewModel.Parse(feature);
				}

				#endregion

				#region Public Methods

				public static FeatureViewModel Parse(Feature feature)
				{
					return new FeatureViewModel(feature);
				}

				#endregion

				#region Subclasses

				public class PagesViewModel
				{
					#region Members

					public Dictionary<int, FeatureViewModel.PageViewModel> pages;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private PagesViewModel()
					{ }

					private PagesViewModel(IEnumerable<Page> pagesIn)
					{
						pages = pagesIn.Select(p => FeatureViewModel.PageViewModel.Parse(p)).ToDictionary(s => s.page_id, s => s);
					}

					#endregion

					#region Public Methods

					public static Dictionary<int, FeatureViewModel.PageViewModel> Parse(IEnumerable<Page> pages)
					{
						var pagesViewModel = new PagesViewModel(pages);
						return pagesViewModel.pages;
					}

					#endregion
				}

				public class PageViewModel
				{
					#region Members

					public int page_id;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private PageViewModel()
					{ }

					private PageViewModel(Page page)
					{
						page_id = page.Id;
					}

					#endregion

					#region Public Methods

					public static PageViewModel Parse(Page page)
					{
						return new PageViewModel(page);
					}

					#endregion
				}

				public class ModelViewModel
				{
					#region Members

					public Dictionary<string, object> modelInstance;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private ModelViewModel()
					{ }

					private ModelViewModel(CmsModelInstance modelInstanceIn)
					{
						var modelInstanceDeserialized = new Dictionary<string, object>();
						var modelInstancePublishedJsonService = IoC.Resolve<IModelInstancePublishedJsonService>();

						var modelInstanceRaw = JsonConvert.DeserializeObject<Dictionary<string, object>>(modelInstanceIn.PublishedJson);
					
						// Deserialize all of the Model Instance values
						foreach (var item in modelInstanceRaw)
						{
							var valueDeserialized = modelInstancePublishedJsonService.DeserializeModelInstancePropertyValue(item);

							modelInstanceDeserialized.Add(item.Key, valueDeserialized);
						}

						modelInstance = modelInstanceDeserialized;
					}

					#endregion

					#region Public Methods

					public static Dictionary<string, Dictionary<string, object>[]> Parse(Feature feature)
					{
						var cmsModelInstancesRepo = IoC.Resolve<IRepository<CmsModelInstance>>();

						var modelInstances = cmsModelInstancesRepo.Where(m => m.Model.Feature.Id == feature.Id).ToList();
						if (modelInstances == null || modelInstances.Count() == 0)
							return null;

						var modelInstancesFlattened = modelInstances.Select(modelInstance => new ModelViewModel(modelInstance).modelInstance).ToArray();

						// Group the flattened Model Instances by Model Name
						var modelInstancesGroupedInitial = new Dictionary<string, ModelGroupAsList>();
						foreach (var modelInstance in modelInstancesFlattened)
						{
							// Get the Model Name to use as a key for the group
							var modelName = modelInstance[CmsPublishConstants.ModelInstanceJsonProperties.ModelName].ToString();

							// Initialize the list for a model group
							if (!modelInstancesGroupedInitial.ContainsKey(modelName))
								modelInstancesGroupedInitial[modelName] = new ModelGroupAsList();

							// Add the Model Instance to a group
							modelInstancesGroupedInitial[modelName].ModelInstances.Add(modelInstance);
						}

						var modelInstancesGrouped = new Dictionary<string, Dictionary<string, object>[]>();
						foreach (var group in modelInstancesGroupedInitial)
							modelInstancesGrouped.Add(group.Key, ModelGroupAsArray.Parse(group.Value));

						return modelInstancesGrouped;
					}

					#endregion

					#region Subclasses

					public class ModelGroupAsList
					{
						public List<Dictionary<string, object>> ModelInstances { get; set; }

						public ModelGroupAsList()
						{
							ModelInstances = new List<Dictionary<string, object>>();
						}
					}

					public class ModelGroupAsArray
					{
						public static Dictionary<string, object>[] Parse(ModelGroupAsList group)
						{
							return group.ModelInstances.ToArray();
						}
					}

					#endregion
				}

				public class SettingViewModel
				{
					#region Members

					public Dictionary<string, object> settingInstance;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private SettingViewModel()
					{ }

					private SettingViewModel(CmsSettingInstance settingInstanceIn)
					{
						if (!string.IsNullOrEmpty(settingInstanceIn.PublishedJson))
						{
							var settingInstanceDeserialized = new Dictionary<string, object>();
							var settingInstancePublishedJsonService = IoC.Resolve<ISettingInstancePublishedJsonService>();

							var settingInstanceRaw = JsonConvert.DeserializeObject<Dictionary<string, object>>(settingInstanceIn.PublishedJson);

							// Deserialize all of the Model Instance values
							foreach (var item in settingInstanceRaw)
							{
								var valueDeserialized = settingInstancePublishedJsonService.DeserializeSettingnstancePropertyValue(item);

								settingInstanceDeserialized.Add(item.Key, valueDeserialized);
							}

							settingInstance = settingInstanceDeserialized;
						}
					}

					#endregion

					#region Public Methods

					public static Dictionary<string, object>[] Parse(Feature feature)
					{
						var cmsSettingsRepo = IoC.Resolve<IRepository<CmsSetting>>();
						var cmsSettingInstancesRepo = IoC.Resolve<IRepository<CmsSettingInstance>>();

						var cmsSettings = cmsSettingsRepo.Where(m => m.Feature.Id == feature.Id).Select(s => s.Id).ToList();
						if (cmsSettings == null && cmsSettings.Count() == 0)
							return null;

						// Find all Cms Setting Instances for all Cms Settings that are part of this specific Feature
						var cmsSettingsHash = new HashSet<int>(cmsSettings);
						var cmsSettingInstances = cmsSettingInstancesRepo.Where(c => cmsSettingsHash.Contains(c.Setting_Id)).ToList();

						return cmsSettingInstances.Select(settingInstance => new SettingViewModel(settingInstance).settingInstance).ToArray();
					}

					#endregion
				}

				#endregion
			}
		
			#endregion
		}

		#endregion
	}
}
