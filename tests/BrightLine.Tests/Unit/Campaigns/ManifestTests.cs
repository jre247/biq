using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	class ManifestTests
	{
		private ManifestViewModel ManifestVersion1 { get; set; }
		private ManifestViewModel ManifestVersion2 { get; set; }
		private const string DEFAULT_PAGE_KEY = "null";

		[SetUp]
		// Setup will create 2 manifest versions:
		//	1) a manifest version that is json reference deserialized into ManifestViewModel
		//		*it is assumed that this is a correct representation of the manifest and has been vetted
		//	2) a c# in memory representation of that json reference file
		//		*a campaign and user objects are created and passed as params when instantiating ManifestViewModel
		//		*transformations will happen within ManifestViewModel at each layer's Parse function
		//		*if this transformation at every level of ManifestViewModel is successful then it assumed that every property of the transformed ManifestViewModel instantiated obj will match up with the reference
		public void Setup()
		{
			MockHelper.BuildMockLookups();

			var json = ResourceLoader.ReadText("Campaigns.Manifest.json");
			ManifestVersion1 = JsonConvert.DeserializeObject<ManifestViewModel>(json);
			ManifestVersion2 = GetManifestModel();
		}

		[TearDown]
		public void TearDown()
		{ }

		#region Tests

		[Test(Description = "Manifest constructor with null parameters.")]
		public void ManifestWithNullConstructorParameters()
		{
			var manifest = new ManifestViewModel(null, null);

			Assert.AreEqual(manifest.campaign, default(ManifestViewModel.CampaignViewModel), "Manifest campaign is not equal to null.");
			Assert.AreEqual(manifest.manifestId, default(Guid), "Manifest manifestId is not equal to default Guid.");
			Assert.AreEqual(manifest.createdOn, default(DateTime), "Manifest createdOn is not equal to default DateTime.");
			Assert.AreEqual(manifest.status, default(CompletionStatus), "Manifest status is not equal to default CompletionStatus.");
		}

		[Test(Description = "Manifest JObject with null parameter.")]
		public void ManifestJObjectWithNullParameter()
		{
			var manifestJson = ManifestViewModel.ToJObject(null);
			Assert.IsNull(manifestJson, "Manifest JObject should be null.");
		}

		[Test(Description = "Manifest with empty campaign.")]
		public void ManifestFromEmptyCampaign()
		{
			var campaign = new Campaign();
			var user = new User()
			{
				Email = "test@brightline.tv",
				FirstName = "Test",
				LastName = "Tester"
			};
			var manifest = new ManifestViewModel(campaign, user);
			Assert.AreEqual(manifest.campaign.id, 0, "Manifest campaign id is not 0.");
		}

		[Test(Description = "Manifest Campaign first level properties.")]
		public void ManifestCampaignFirstLevelProperties()
		{
			Assert.AreEqual(ManifestVersion1.campaign.id, ManifestVersion2.campaign.id, "Manifest campaign id does not match reference.");
			Assert.AreEqual(ManifestVersion1.campaign.name, ManifestVersion2.campaign.name, "Manifest campaign name does not match reference.");
			Assert.AreEqual(ManifestVersion1.campaign.product.id, ManifestVersion2.campaign.product.id, "Manifest campaign product id does not match reference.");
			Assert.AreEqual(ManifestVersion1.campaign.product.name, ManifestVersion2.campaign.product.name, "Manifest campaign product name does not match reference.");
			Assert.AreEqual(ManifestVersion1.campaign.cms.key, ManifestVersion2.campaign.cms.key, "Manifest campaign cms key does not match reference.");
		}

		[Test(Description = "Manifest Campaign Ads properties.")]
		public void ManifestCampaignAds()
		{
			Assert.AreEqual(ManifestVersion1.campaign.ads.Length, ManifestVersion2.campaign.ads.Length, "Manifest campaign ads length does not match reference.");

			for (var a = 0; a < ManifestVersion1.campaign.ads.Length; a++)
			{
				var manifestVersion1Ad = ManifestVersion1.campaign.ads[a];
				var manifestVersion2Ad = ManifestVersion2.campaign.ads[a];

				//top level ad properties
				Assert.AreEqual(manifestVersion1Ad.name, manifestVersion2Ad.name, "Manifest campaign ad name does not match reference.");
				Assert.AreEqual(manifestVersion1Ad.ad_id, manifestVersion2Ad.ad_id, "Manifest campaign ad ad_id does not match reference.");
				Assert.AreEqual(manifestVersion1Ad.adType.id, manifestVersion2Ad.adType.id, "Manifest campaign ad adType id does not match reference.");
				Assert.AreEqual(manifestVersion1Ad.platform.id, manifestVersion2Ad.platform.id, "Manifest campaign ad platform id does not match reference.");
				Assert.AreEqual(manifestVersion1Ad.platform.name, manifestVersion2Ad.platform.name, "Manifest campaign ad platform name does not match reference.");

				//MediaPartner
				if (manifestVersion1Ad.mediaPartner != null && manifestVersion2Ad.mediaPartner != null)
				{
					Assert.AreEqual(manifestVersion1Ad.mediaPartner.id, manifestVersion2Ad.mediaPartner.id, "Manifest campaign ad MediaPartner id does not match reference.");
					Assert.AreEqual(manifestVersion1Ad.mediaPartner.name, manifestVersion2Ad.mediaPartner.name, "Manifest campaign ad MediaPartner name does not match reference.");
				}

				//placement
				if (manifestVersion1Ad.placement != null && manifestVersion2Ad.placement != null)
				{
					Assert.AreEqual(manifestVersion1Ad.placement.id, manifestVersion2Ad.placement.id, "Manifest campaign ad placement id does not match reference.");
					Assert.AreEqual(manifestVersion1Ad.placement.name, manifestVersion2Ad.placement.name, "Manifest campaign ad placement name does not match reference.");
				}

				//creative
				if (manifestVersion1Ad.creative != null && manifestVersion2Ad.creative != null)
				{
					Assert.AreEqual(manifestVersion1Ad.creative.id, manifestVersion2Ad.creative.id, "Manifest campaign ad creative id does not match reference.");
					Assert.AreEqual(manifestVersion1Ad.creative.name, manifestVersion2Ad.creative.name, "Manifest campaign ad creative name does not match reference.");
					Assert.AreEqual(JsonConvert.SerializeObject(manifestVersion1Ad.creative.resources), JsonConvert.SerializeObject(manifestVersion2Ad.creative.resources), "Manifest campaign ad creative resources does not match reference.");
					Assert.AreEqual(JsonConvert.SerializeObject(manifestVersion1Ad.creative.adType), JsonConvert.SerializeObject(manifestVersion2Ad.creative.adType), "Manifest campaign ad creative Ad Type does not match reference.");
				}

				//destination ad
				if (manifestVersion1Ad.destinationAdId != null && manifestVersion2Ad.destinationAdId != null)
					Assert.AreEqual(manifestVersion1Ad.destinationAdId, manifestVersion2Ad.destinationAdId, "Manifest campaign ad destination ad does not match reference.");

				if (manifestVersion1Ad.coordinates != null && manifestVersion2Ad.coordinates != null)
				{
					if (manifestVersion1Ad.coordinates.sd != null && manifestVersion2Ad.coordinates.sd != null)
					{
						Assert.AreEqual(manifestVersion1Ad.coordinates.sd.x, manifestVersion2Ad.coordinates.sd.x, "Manifest campaign ad SD X Coordinate does not match reference.");
						Assert.AreEqual(manifestVersion1Ad.coordinates.sd.y, manifestVersion2Ad.coordinates.sd.y, "Manifest campaign ad SD Y Coordinate does not match reference.");
					}
					
					Assert.AreEqual(manifestVersion1Ad.coordinates.hd.x, manifestVersion2Ad.coordinates.hd.x, "Manifest campaign ad HD X Coordinate does not match reference.");
					Assert.AreEqual(manifestVersion1Ad.coordinates.hd.y, manifestVersion2Ad.coordinates.hd.y, "Manifest campaign ad HD Y Coordinate does not match reference.");
				}
			}
		}

		[Test(Description = "Manifest Campaign Ad Features properties.")]
		public void ManifestCampaignAdFeatures()
		{
			for (var a = 0; a < ManifestVersion1.campaign.ads.Length; a++)
			{
				var manifestVersion1Ad = ManifestVersion1.campaign.ads[a];
				var manifestVersion2Ad = ManifestVersion2.campaign.ads[a];

				if (manifestVersion1Ad.features != null)
				{
					for (var f = 0; f < manifestVersion1Ad.features.Length; f++)
					{
						var manifestVersion1Feature = manifestVersion1Ad.features[f];
						var manifestVersion2Feature = manifestVersion2Ad.features[f];

						if (manifestVersion1Feature.pages != null)
						{
							for (var p = 0; p < manifestVersion1Feature.pages.Count; p++)
							{
								var manifestVersion1FeaturePage = manifestVersion1Feature.pages["1"];
								var manifestVersion2FeaturePage = manifestVersion2Feature.pages["1"];

								Assert.AreEqual(manifestVersion1FeaturePage.page_id, manifestVersion2FeaturePage.page_id, "Manifest campaign ad feature page id does not match reference.");
								Assert.AreEqual(manifestVersion1FeaturePage.name, manifestVersion2FeaturePage.name, "Manifest campaign ad feature page name does not match reference.");
								Assert.AreEqual(manifestVersion1FeaturePage.url, manifestVersion2FeaturePage.url, "Manifest campaign ad feature page url does not match reference.");
							}
						}
						
						Assert.AreEqual(manifestVersion1Feature.id, manifestVersion2Feature.id, "Manifest campaign ad feature id does not match reference.");
						Assert.AreEqual(manifestVersion1Feature.name, manifestVersion2Feature.name, "Manifest campaign ad feature name does not match reference.");

						Assert.AreEqual(manifestVersion1Feature.blueprint.id, manifestVersion2Feature.blueprint.id, "Manifest campaign ad feature blueprint id does not match reference.");
						Assert.AreEqual(manifestVersion1Feature.blueprint.name, manifestVersion2Feature.blueprint.name, "Manifest campaign ad feature blueprint name does not match reference.");
						Assert.AreEqual(manifestVersion1Feature.blueprint.key, manifestVersion2Feature.blueprint.key, "Manifest campaign ad feature blueprint key does not match reference.");
					}
				}
			}
		}

		[Test(Description = "Manifest Campaign Ad has companionAdId when AdType is not Commercial Spot or Brand Destination.")]
		public void Manifest_Campaign_Ad_With_Invalid_CompanionAd()
		{
			var manifest = GetManifestModelForAdTests();

			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var brandingDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			for (var a = 0; a < manifest.campaign.ads.Length; a++)
			{
				var ad = manifest.campaign.ads[a];
				var adType = ad.adType.id;

				if (adType != brandingDestinationAdTypeId && adType != commercialSpotAdTypeId)
					Assert.IsFalse(ad.companionAdId.HasValue);
				else
					Assert.IsTrue(ad.companionAdId.HasValue && ad.companionAdId.Value > 0 || !ad.companionAdId.HasValue);
			}
		}

		[Test(Description = "Manifest Campaign Ad does not have companionAdId if Ad's AdType is not Commercial Spot or Brand Destination.")]
		public void Manifest_Campaign_Ad_With_Valid_CompanionAd()
		{
			var manifest = GetManifestModelForAdTests();
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var brandingDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];
			manifest.campaign.ads[0].companionAdId = 12;

			for (var a = 0; a < manifest.campaign.ads.Length; a++)
			{
				var ad = manifest.campaign.ads[a];
				var adType = ad.adType.id;

				if (adType == brandingDestinationAdTypeId || adType == commercialSpotAdTypeId)
					Assert.IsTrue(ad.companionAdId.HasValue && ad.companionAdId.Value > 0 || !ad.companionAdId.HasValue);
				else
					Assert.IsFalse(ad.companionAdId.HasValue);
			}
		}

		[Test(Description = "Manifest Campaign Ad does not have destinationAdId if Ad's AdType is Commercial Spot or Brand Destination.")]
		public void Manifest_Campaign_Ad_With_Valid_DestinationAd()
		{
			var manifest = GetManifestModelForAdTests();
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var brandingDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			for (var a = 0; a < manifest.campaign.ads.Length; a++)
			{
				var ad = manifest.campaign.ads[a];
				var adType = ad.adType.id;

				if (adType == brandingDestinationAdTypeId || adType == commercialSpotAdTypeId)
					Assert.IsFalse(ad.destinationAdId.HasValue);
				else
					Assert.IsTrue(ad.destinationAdId.HasValue && ad.destinationAdId.Value > 0 || !ad.destinationAdId.HasValue);
			}
		}


		[Test(Description = "Manifest Campaign Ad features is present if Ad's AdType is Brand Destination.")]
		public void Manifest_Campaign_Ad_Does_Not_Have_Features_If_Not_BrandDestination_AdType()
		{
			var manifest = GetManifestModelForAdTests();
			var commercialSpotAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot];
			var brandingDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];
	
			for (var a = 0; a < manifest.campaign.ads.Length; a++)
			{
				var ad = manifest.campaign.ads[a];
				var adType = ad.adType.id;

				if (adType != brandingDestinationAdTypeId)
					Assert.IsNull(ad.features);
			}
		}

		#endregion //Tests

		#region helper functions

		private static User GetUserObject()
		{
			var user = new User()
			{
				Email = "test@brightline.tv",
				FirstName = "Test",
				LastName = "Tester"
			};
			return user;
		}

		private static ManifestViewModel GetManifestModelForAdTests()
		{
			var campaign = BuildCampaign();

			campaign.Ads.Add(new Ad()
			{
				Id = 263561,
				Name = "4G LTE",
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot]
				},
				Features = new List<Feature>(),
				CompanionAd = new Ad
				{
					Id = 122,
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot]
					},
					Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] }
				},
				DestinationAd = new Ad
				{
					Id = 13,
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner]
					},
					Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] }
				},
				XCoordinateSd = 11,
				YCoordinateSd = 22,
				XCoordinateHd = 33,
				YCoordinateHd = 44,
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] }
			});

			campaign.Ads.Add(new Ad()
			{
				Id = 263562,
				Name = "4G LTE",
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]
				},
				Features = new List<Feature>(),
				CompanionAd = new Ad
				{
					Id = 123,
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]
					},
					Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] }
				},
				DestinationAd = new Ad
				{
					Id = 14,
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner]
					},
					Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku]}
				},
				Platform = new Platform { Id = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku] }
			});

			var user = GetUserObject();
			var manifest = new ManifestViewModel(campaign, user);
			return manifest;
		}

		private static ManifestViewModel GetManifestModel()
		{
			var campaign = BuildCampaign();

			campaign.Ads.Add(new Ad()
			{
				Id = 263561,
				Name = "4G LTE",
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.DedicatedBrandApp]
				},
				Features = new List<Feature>()
                {
                    new Feature()
                    {
                        Id = 1361,
                        Name = "Life with 4G LTE",
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint()
						{
                            Id = 7,
                            Name = "Information",
							ManifestName="blueprint-combo-gallery"
                        },
                        Pages = new List<Page>(){
                            new Page(){
                                Id = 101185,
								PageDefinition = new PageDefinition{Key = 1},
                                Name = "Life with 4G LTE",
                                RelativeUrl = "life-with-4g-lte"
                            }
                        }

                    },
                    new Feature()
                    {
                        Id = 1363,
                        Name = "Benefits of 4G LTE",
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint(){
                            Id = 7,
                            Name = "Information",
							ManifestName="blueprint-combo-gallery"
                        },
                        Pages = new List<Page>(){
                            new Page(){
                                Id = 101195,
                                Name = "Benefits of 4G LTE",
								PageDefinition = new PageDefinition{Key = 1},
                                RelativeUrl = "benefits-of-4g-lte"
                            }
                        }
                    }
                },
				Creative = new Creative(){
					Id = 1,
					Name = "4G LTE",
					Resources = new List<Resource> { new Resource { Id = 1, Name = "orange.png", Filename = "orange.png", MD5Hash = "3jde-aieej-acee", ResourceType = new ResourceType{ Id = 1, Name = "SD Image" }, Width = 255, Height = 233 } },
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.DedicatedBrandApp]
					}
				
				},
				Platform = new Platform()
				{
					Id = 29,
					Name = "Opera",
					ManifestName = "Opera"
				},
				DestinationAd = new Ad { Id = 12 },
				XCoordinateSd = null,
				YCoordinateSd = null,
				XCoordinateHd = 33,
				YCoordinateHd = 44
			});

			campaign.Ads.Add(new Ad()
			{
				Id = 263570,
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.ImageBanner]
				},
				Features = new List<Feature>(),
				Platform = new Platform()
				{
					Id = 16,
					Name = "Samsung",
					ManifestName = "Samsung"
				},
				Placement = new Placement()
				{
					Id = 4,
					Name = "Samsung AdHub Image Banner",
					MediaPartner = new MediaPartner()
					{
						Id = 3,
						Name = "Samsung AdHub",
						ManifestName = "Samsung AdHub"
					}
				},
				DestinationAd = new Ad { Id = 14 }
			});
			campaign.Ads.Add(new Ad()
			{
				Id = 263579,
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.Overlay]
				},
				Features = new List<Feature>{
					new Feature()
                    {
                        Id = 0,
                        Name = null,
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint()
						{
                            Id = -1,
                            ManifestName = "blueprint-overlay",
							Name="layout"
                        }
                    }
				},
				Platform = new Platform()
				{
					Id = 16,
					Name = "Samsung",
					ManifestName = "Samsung"
				},
				Placement = new Placement()
				{
					Id = 4,
					Name = "Samsung AdHub Image Banner",
					MediaPartner = new MediaPartner()
					{
						Id = 3,
						Name = "Samsung AdHub",
						ManifestName = "Samsung AdHub"
					}
				},
				DestinationAd = new Ad { Id = 14 }
			});
			campaign.Ads.Add(new Ad()
			{
				Id = 263565,
				Name = "4G LTE",
				AdType = new AdType()
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination]
				},
				Features = new List<Feature>{
					new Feature()
                    {
                        Id = 3357,
                        Name = "Diagnostic Feature",
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint()
						{
                            Id = 29,
                            ManifestName = "blueprint-diagnostic",
							Name="blueprint-diagnostic"
                        },
						Pages = new List<Page>
						{
							new Page{Id = 104399, Name = "Page 1",  PageDefinition = new PageDefinition{Key = 0}},
							new Page{Id = 104400, Name = "Page 2",  PageDefinition = new PageDefinition{Key = 1}}
						}
                    },
					new Feature()
                    {
                        Id = 0,
                        Name = null,
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint()
						{
                            Id = -1,
                            ManifestName = "blueprint-layout",
							Name="layout"
                        }
                    },
					new Feature()
                    {
                        Id = 0,
                        Name = null,
						FeatureType = new FeatureType{Id = 1},
                        Blueprint = new Blueprint()
						{
                            Id = -1,
                            ManifestName = "blueprint-horizontal-nav",
							Name="nav"
                        }
                    }
				},
				Creative = new Creative()
				{
					Id = 1,
					Name = "4G LTE",
					Resources = new List<Resource> { new Resource { Id = 1, Name = "orange.png", Filename = "orange.png", MD5Hash = "3jde-aieej-acee", ResourceType = new ResourceType { Id = 1, Name = "SD Image" }, Width = 255, Height = 233 } },
					AdType = new AdType()
					{
						Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.DedicatedBrandApp]
					}

				},
				Platform = new Platform()
				{
					Id = 29,
					Name = "Opera",
					ManifestName = "Opera"
				},
				DestinationAd = new Ad { Id = 12 },
				XCoordinateSd = 11,
				YCoordinateSd = 22,
				XCoordinateHd = 33,
				YCoordinateHd = 44
			});


			var user = GetUserObject();
			var manifestV2 = new ManifestViewModel(campaign, user);
			return manifestV2;
		}

		private static Campaign BuildCampaign()
		{
			var campaign = new Campaign()
			{
				Id = 20198,
				Name = "OnStar",
				CmsKey = "20198",
				Product = new Product()
				{
					Id = 142,
					Name = "OnStar"
				},
				Ads = new List<Ad>()
			};
			return campaign;
		}

		#endregion // helper functions
	}
}