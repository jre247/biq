using BrightLine.Common.Core;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace BrightLine.Common.ViewModels.Campaigns
{
	public class ManifestViewModel
	{
		public PublishViewModel publish;
		public CampaignViewModel campaign;
		public Guid manifestId;
		public DateTime createdOn;
		public string targetEnv;
		public string key;
		[JsonConverter(typeof(StringEnumConverter))]
		public CompletionStatus status;

		//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
		[JsonConstructor]
		private ManifestViewModel()
		{ }

		public ManifestViewModel(Campaign campaignIn, User user)
		{
			SetManifestViewModelProperties(campaignIn, user, null);
		}

		public ManifestViewModel(Campaign campaignIn, User user, CmsPublish cmsPublish)
		{
			SetManifestViewModelProperties(campaignIn, user, cmsPublish);
		}

		public static JObject ToJObject(ManifestViewModel manifest)
		{
			if (manifest == null)
				return null;

			var json = JObject.FromObject(manifest);
			return json;
		}

		#region Private Methods

		private void SetManifestViewModelProperties(Campaign campaignIn, User user, CmsPublish cmsPublish)
		{
			if (campaignIn == null)
				return;

			campaign = CampaignViewModel.Parse(campaignIn);
			manifestId = Guid.NewGuid();
			createdOn = DateTime.UtcNow;
			status = CompletionStatus.Created;
			publish = PublishViewModel.Parse(user, cmsPublish);
			targetEnv = null; // TODO: fill in
			key = null; // TODO: fill in
		}


		#endregion


		#region ViewModel classes

		public class PublishViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private PublishViewModel()
			{ }

			private PublishViewModel(User userIn, CmsPublish cmsPublish)
			{
				if (cmsPublish != null)
				{
					targetEnv = cmsPublish.TargetEnvironment;
					publishId = cmsPublish.PublishId.ToString();
				}
				
				user = userIn.Email;
			}

			public string targetEnv;
			public string publishId;
			public string user;

			internal static PublishViewModel Parse(User user, CmsPublish cmsPublish)
			{
				return new PublishViewModel(user, cmsPublish);
			}
		}

		public class CampaignViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CampaignViewModel()
			{ }

			private CampaignViewModel(Campaign campaign, IEnumerable<Ad> manifestAds)
			{
				id = campaign.Id;
				name = campaign.Name;
				product = LookupViewModel.Parse(campaign.Product);
				analytics = AnalyticsViewModel.Parse(campaign);
				cms = CmsViewModel.Parse(campaign);
				ads = AdViewModel.Parse(manifestAds);
			}

			public int id;
			public string name;
			public LookupViewModel product;
			public AnalyticsViewModel analytics;
			public CmsViewModel cms;
			public AdViewModel[] ads;

			internal static CampaignViewModel Parse(Campaign campaign)
			{
				if (campaign == null)
					return null;

				var ads = campaign.Ads;
				return new CampaignViewModel(campaign, ads);
			}
		}

		public class AnalyticsViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private AnalyticsViewModel(Campaign campaign) //TODO: campaign analytics issue. #352
			{ //TODO: add manifest analytics object data
			}

			internal static AnalyticsViewModel Parse(Campaign campaign)
			{
				if (campaign == null)
					return null;

				return new AnalyticsViewModel(campaign);
			}
		}

		public class CmsViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CmsViewModel()
			{ }

			private CmsViewModel(Campaign campaign)
			{ //TODO: add Campaign.Cms base, key, version
				@base = "";
				key = campaign.CmsKey;
				version = "";
			}

			public string @base;
			public string key;
			public string version;

			internal static CmsViewModel Parse(Campaign campaign)
			{
				if (campaign == null)
					return null;

				return new CmsViewModel(campaign);
			}
		}

		public class AdViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private AdViewModel()
			{ }

			private AdViewModel(Ad ad)
			{
				ad_id = ad.Id;
				if (ad.AdTag != null)
					tag = ad.AdTag.Id;

				name = ad.Name;

				if (ad.Creative != null)
					creative = CreativeViewModel.Parse(ad.Creative);

				adType = AdTypeViewModel.Parse(ad.AdType);

				features = FeatureViewModel.Parse(ad.Features);

				AddGeneralFeaturesToFeaturesArray(ad);
					
				var plat = EntityLookup.ToLookup(ad.Platform, "ManifestName");
				platform = LookupViewModel.Parse(plat);
				var place = EntityLookup.ToLookup(ad.Placement, "Name");
				placement = LookupViewModel.Parse(place);
				if (placement != null)
				{
					var c = EntityLookup.ToLookup(ad.Placement.MediaPartner, "ManifestName");
					mediaPartner = LookupViewModel.Parse(c);
					var a = EntityLookup.ToLookup(ad.Placement.App, "Name");
					app = LookupViewModel.Parse(a);
				}

				//only set ad's Destination Ad if ad's Ad Type is not Commercial Spot or Brand Destination
				if (ad.DestinationAd != null &&
					ad.AdType.Id != Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot] &&
					ad.AdType.Id != Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination])
					destinationAdId = ad.DestinationAd.Id;

				if (ad.CompanionAd != null)
					companionAdId = ad.CompanionAd.Id;

				coordinates = CoordinatesResolutionViewModel.Parse(ad);
				
			}

			public string name;
			public int ad_id;
			public int? tag;
			public int? companionAdId;
			public int? destinationAdId;
			public AdTypeViewModel adType;
			public FeatureViewModel[] features;
			public CoordinatesResolutionViewModel coordinates;

			public LookupViewModel placement;
			public LookupViewModel platform;
			public LookupViewModel mediaPartner;
			public LookupViewModel app;
			public CreativeViewModel creative;

			internal static AdViewModel[] Parse(IEnumerable<Ad> ads)
			{
				if (ads == null || !ads.Any())
					return null;

				return ads.Select(ad => new AdViewModel(ad)).ToArray();
			}

			/// <summary>
			/// Create hard coded features for the following Ad Types:
			///		1) Overlay: 
			///			Create a feature that will point to "blueprint-overlay" blueprint
			///		2) Brand Destination:
			///			Create 2 features: 
			///				a) One feature that points to the "blueprint-layout" blueprint 
			///				b) Another feature that points to the "blueprint-horizontal-nav" blueprint	
			/// </summary>
			/// <param name="adType"></param>
			/// <returns></returns>
			public FeatureViewModel[] GetGeneralBlueprintFeatures(AdType adType)
			{
				var features = new List<FeatureViewModel>();

				if (adType.Id == Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay])
				{
					var blueprintOverlay = new BlueprintViewModel(-1, "blueprint-overlay", "layout");
					var featureForBlueprintOverlay = new FeatureViewModel(blueprintOverlay);
					features.Add(featureForBlueprintOverlay);
				}
				else if (adType.Id == Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination])
				{

					var blueprintLayout = new BlueprintViewModel(-1, "blueprint-layout", "layout");
					var featureForBlueprintLayout = new FeatureViewModel(blueprintLayout);
					features.Add(featureForBlueprintLayout);

					var blueprintHorizontalNav = new BlueprintViewModel(-1, "blueprint-horizontal-nav", "nav");
					var featureForBlueprintHorizontalNav = new FeatureViewModel(blueprintHorizontalNav);
					features.Add(featureForBlueprintHorizontalNav);
				}

				return features.ToArray();
			}


			/// <summary>
			// Tack on hard coded features to ad's features array.
			// These hard coded features will only have a filled in blueprint object for ads that are Overlay or Brand Destination.
			// The rest of the properties in these hard coded features will be null.
			/// </summary>
			/// <param name="ad"></param>
			private void AddGeneralFeaturesToFeaturesArray(Ad ad)
			{
				var generalFeatures = GetGeneralBlueprintFeatures(ad.AdType);
				if (generalFeatures.Count() > 0)
				{
					if (features == null)
						features = generalFeatures;
					else
					{
						var featuresList = features.ToList();
						foreach (var generalFeature in generalFeatures)
							featuresList.Add(generalFeature);

						features = featuresList.ToArray();
					}
				}
			}
		}

		public class AdTypeViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private AdTypeViewModel()
			{ }

			private AdTypeViewModel(AdType adType)
			{
				id = adType.Id;
				name = adType.ManifestName;
				isPromo = adType.IsPromo;
			}

			public int id;
			public string name;
			public bool isPromo;

			internal static AdTypeViewModel Parse(AdType adType)
			{
				if (adType == null)
					return null;

				return new AdTypeViewModel(adType);
			}
		}

		public class CoordinatesResolutionViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CoordinatesResolutionViewModel()
			{ }

			private CoordinatesResolutionViewModel(Ad ad)
			{
				// only set SD X/Y values if the Ad's Platform is Roku
				if(ad.Platform.Id == Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku])
					sd = CoordinatePositionViewModel.Parse(ad.XCoordinateSd, ad.YCoordinateSd);

				hd = CoordinatePositionViewModel.Parse(ad.XCoordinateHd, ad.YCoordinateHd);
			}

			[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
			public CoordinatePositionViewModel sd;

			public CoordinatePositionViewModel hd;

			internal static CoordinatesResolutionViewModel Parse(Ad ad)
			{
				if (ad.XCoordinateSd == null && ad.YCoordinateSd == null && ad.XCoordinateHd == null && ad.YCoordinateHd == null)
					return null;

				return new CoordinatesResolutionViewModel(ad);
			}
		}

		public class CoordinatePositionViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CoordinatePositionViewModel()
			{ }

			private CoordinatePositionViewModel(int? x, int? y)
			{
				this.x = x.HasValue ? x : null;
				this.y = y.HasValue ? y : null;
			}

			public int? x;
			public int? y;

			internal static CoordinatePositionViewModel Parse(int? x, int? y)
			{
				return new CoordinatePositionViewModel(x, y);
			}
		}

		public class CreativeViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private CreativeViewModel()
			{ }

			private CreativeViewModel(Creative creative)
			{
				id = creative.Id;
				name = creative.Name;
				adType = AdTypeViewModel.Parse(creative.AdType);
				if (creative.Resources != null)
					resources = creative.Resources.Where(r => !r.IsDeleted).Select(r => new ResourceViewModel(r)).ToList();
			}

			public int id;
			public string name;
			public AdTypeViewModel adType;
			public List<ResourceViewModel> resources;

			internal static CreativeViewModel Parse(Creative creative)
			{
				if (creative == null)
					return null;

				return new CreativeViewModel(creative);
			}
		}

		public class ResourceViewModel
		{
			public int id { get; set; }
			public string name { get; set; }
			public ResourceTypeViewModel resourceType { get; set; }
			public string fileName { get;set;}
			public string md5Hash { get; set; }
			public int? height { get; set; }
			public int? width { get; set; }

			public ResourceViewModel(Resource resource)
			{
				if (resource == null)
					return;

				id = resource.Id;
				name = resource.Name;
				fileName = resource.Filename;
				md5Hash = resource.MD5Hash;
				resourceType = new ResourceTypeViewModel(resource.ResourceType);
				height = resource.Height;
				width = resource.Width;
			}
		}

		public class ResourceTypeViewModel
		{
			public int id { get; set; }
			public string name { get; set; }

			public ResourceTypeViewModel(ResourceType resourceType)
			{
				if (resourceType == null)
					return;

				id = resourceType.Id;
				name = resourceType.Name;
			}
		}

		public class FeatureViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private FeatureViewModel()
			{ }

			private FeatureViewModel(Feature feature)
			{
				id = feature.Id;
				featureTypeId = feature.FeatureType.Id;
				name = feature.Name;
				cmsCollection = ""; //TODO: manifest cms collection
				blueprint = BlueprintViewModel.Parse(feature.Blueprint);
				var pagesList = PageViewModel.Parse(feature.Pages);

				if (pagesList == null)
					return;

				//a feature contains a dictionary of pages where key is page definition key
				//only pages with page definition key are added to list
				pages = new Dictionary<string, PageViewModel>();
				foreach (var page in pagesList)
				{
					string pageKey;

					if (page.key == null)
						continue;

					pageKey = page.key.Value.ToString();
					pages.Add(pageKey, page);
				}
			}

			public FeatureViewModel(BlueprintViewModel blueprint)
			{
				this.blueprint = blueprint;
			}

			public int id;
			public int featureTypeId;
			public string name;
			public string cmsCollection;
			public BlueprintViewModel blueprint;
			public Dictionary<string, PageViewModel> pages;

			internal static FeatureViewModel[] Parse(IEnumerable<Feature> features)
			{
				if (!features.Any())
					return null;

				return features.Select(feature => new FeatureViewModel(feature)).ToArray();
			}
		}

		public class PageViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private PageViewModel()
			{ }

			private PageViewModel(Page page)
			{
				if (page == null)
					return;

				page_id = page.Id;
				name = page.Name;
				url = page.RelativeUrl;

				if (page.PageDefinition != null)
					key = page.PageDefinition.Key;
			}

			public int page_id;
			public int? key;
			public string name;
			public string url;

			internal static IEnumerable<PageViewModel> Parse(IEnumerable<Page> pages)
			{
				if (pages == null || !pages.Any())
					return null;

				return pages.Select(page => new PageViewModel(page)).ToArray();
			}
		}

		public class BlueprintViewModel
		{
			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private BlueprintViewModel()
			{ }

			public BlueprintViewModel(int? id, string key, string name)
			{
				this.id = id;
				this.key = key;
				this.name = name;
			}

			public int? id;
			public string name;
			public string key;

			internal static BlueprintViewModel Parse(Blueprint blueprint)
			{
				if (blueprint == null)
					return null;

				var bp = new BlueprintViewModel()
				{
					id = blueprint.Id,
					name = blueprint.Name,
					key = blueprint.ManifestName
				};

				return bp;
			}
		}

		#endregion

		#region Validation methods

		/// <summary>
		/// Checks the manifest against the manifest schema.
		/// </summary>
		/// <param name="manifest">Teh manifest to check for validity.</param>
		/// <returns>True for a valid manifest, false otherwise.</returns>
		private static bool ValidateManifest(JObject manifest)
		{
			//TODO: manifest json schema
			var schema = @"{
				'type': 'object',
				'properties': {
					'publishedBy': {
							'type':'object',
							'properties': {
								'email': {'type': 'string'},
								'name': {'type': 'string'}
							}
					},
					'campaign': {
						'type': 'object',
						'properties': {
							'id': {'type': 'number'},
							'name': {'type': 'string'},
							'product': {
								'type':'object',
								'properties': {
									'id': {'type': 'number'},
									'name': {'type': 'string'}
								}
							}
							'analytics': {'type': 'object', 'properties': {}},
							'cms': {
								'type':'object',
								'properties': {
									'base': {'type': 'string'},
									'key': {'type': 'string'},
									'version': {'type': 'string'}
								}
							},
							'ads': {
								'type':'array',
								'items': {
									'type':'object',
									'properties': {
										'ad_id': {'type': 'number'},
										'tag': {'type': 'string'},
										'url': {'type': 'string'},
										'destinationAdId': {'type': 'string'},
										'adType': {
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'},
												'isPromo': {'type': 'boolean'}
											}
										},
										'feature': {
											'type':'array',
											'items': {
												'type':'object',
												'properties': {
													'id': {'type': 'number'},
													'featureAdId': {'type': 'number'},
													'name': {'type': 'string'},
													'cmsCollection': {'type': 'string'},
													'blueprint': {
														'type':'object',
														'properties': {
															'id': {'type': 'number'},
															'name': {'type': 'string'}
														}
													}
													'pages':{
														'type':'object',
														'properties': {
															'key': {
																'type': 'array',
																'items': {
																	'type':'object',
																	'properties': {
																		'page_id': {'type': 'number'},
																		'name': {'type': 'string'},
																		'url': {'type': 'string'}
																	}
																}
															}
														}
													}									
												}
											}
										},
										'placement':{
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'}
											}
										},
										'platform':{
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'}
											}
										},
										'mediaPartner':{
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'}
											}
										},
										'app':{
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'}
											}
										},
										'blueprint': {
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'}
											}
										},
										'creative':{
											'type':'object',
											'properties': {
												'id': {'type': 'number'},
												'name': {'type': 'string'},
												'resources: {
													'type': 'array',
													'items': {
														'type':'object',
														'properties': {
															
														}
													}
												}
											}
										},
										
									}
								}
							}
							
						}
					}
				}
			}";
			//var schema = JsonSchema.Parse(CampaignManifestViewModel.Schema);
			//IList<string> messages;
			//var valid = manifest.IsValid(schema, out messages);
			//if (!valid)
			//{
			//	var m = string.Join(", ", messages);
			//	IoC.Log.Error(m);
			//	throw new ArgumentException("Manifest is not in the proper Json format.", "manifest");
			//}
			return true;
		}

		#endregion

	}
}

/*  
    1. all entities are using id as primary key, with the exception of ad (ad_id) and page (page_id)
    2. all properties are to use camelCase with the exception of properties that are reported for analytics, which use underscore (ad_id, placement_id, mediaPartner_id, app_platform_placement, page_id)
    3. all elements in the "ads" array indicate an ad that is new or has had a change made that requires rebuilding the ad.
    4. assume that all pages of a feature are implemented on all ads that use that feature (and feature_ad_page_id is not needed or provided)
     
    Questions:
    1. Should any additional feature data be added (e.g. FeatureTypeGroup, FeatureType, FeatureCategory)? This could be helpful from the publishing side or from the recieving side. 
    2. Should we send ads that are associates with a mediaPartner placement? I assume we will not serve these. Verify that assumption and consider logic to no include ads with mediaPartner_ids (and mediaPartner_platform_placement)
*/
//{
//	"campaign": {
//		"id": 238,
//		"name": "AMEX NOW",
//		"product": {
//			"id": 2,
//			"name": "American Express Cards"
//		},
//		"analytics": {
//			"google": "xxxx",
//			"keen": {
//				"production": {
//					"projectId": "xxxx",
//					"writeKey": "xxxx"
//				}
//			}
//		},
//		"cms": {
//			"key": "amex"
//		},
//		"ads": [{
//				"ad_id": 100002, /* Example of a non-promotional ad with features */
//				"adType": {
//					"id": 10010,
//					"name": "Branded Destination", /* AdType.ManifestName (doesn't exist yet) */
//					"isPromo": false
//				},
//				"platform": {
//					"id": 16,
//					"name": "Samsung" /* Platform.ManifestName */
//				},
//				"features": [
//					{
//						"featureAdId": 1,
//						"featureId": 230, /* FeatureAd.Module_Id */
//						"name": "Intro (Autoplay)",
//						"blueprint": {
//							"id": 566,
//							"name": "feature-gallery-video" /* BluePrint.AppId (rename to ManifestName) */
//						},
//						"pages": [
//							{
//								"page_id": 100001, /* Page.Id */
//								"name": "Intro - Main Page",
//								"url": "intro"
//							}
//						]
//					}
//				]
//			}, {
//				"ad_id": 100038, /* Example of a promotional ad */
//				"placement_id": 2,
//				"mediaPartner_id": 21,
//				"app_platform_placement": 2,
//				 "blueprint": {
//							"id": 550,
//							"name": "ad-image-banner-horizontal" /* BluePrint.AppId (rename to ManifestName) */
//				},
//				"adType": {
//					"id": 10001,
//					"name": "Image Banner", /* AdType.ManifestName (doesn't exist yet) */
//					"isPromo": true
//				},
//				"platform": {
//					"id": 16,
//					"name": "Samsung" /* Platform.ManifestName */
//				}
//			},{
//				"ad_id": 100040, /* Example of a promotional ad */
//				"placement_id": 5,
//				"mediaPartner_id": 39,
//				"app_platform_placement": 76,
//				"blueprint": {
//							"id": 412,
//							"name": "ad-video-overlay-horizontal-bottom" /* BluePrint.AppId (rename to ManifestName) */
//				},
//				"adType": {
//					"id": 4,
//					"name": "Embedded Video Overlay", /* AdType.ManifestName (doesn't exist yet) */
//					"isPromo": true
//				},
//				"platform": {
//					"id": 16,
//					"name": "Samsung" /* Platform.ManifestName */
//				},
//				 "features": [{
//						"featureAdId": 13,
//						"featureId": 270, /* FeatureAd.Module_Id */
//						"name": "Landing Screen",
//						"blueprint": {
//							"id": 450,
//							"name": "feature-overlay-landing" /* BluePrint.AppId (rename to ManifestName) */
//						},
//						"pages": [
//							{
//								"page_id": 100321, /* Page.Id */
//								"name": "Landing",
//								"url": "home"
//							}
//						]
//					}
//				]
//			}
//		]
//	}
//}