using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Web.Areas.Campaigns.Controllers;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class CampaignSummaryTests
	{
		IocRegistration Container;
		private int CampaignId = 2;
		private int UserId = 1;
		private Mock<IResourceHelper> ResourceHelper { get; set; }
		private Stream FileStream;
		private CampaignService CampaignService { get; set; }
		private Resource Resource1 { get; set; }
		private Resource Resource2 { get; set; }
		private Resource Resource3 { get; set; }
		private const string Md5Hash = "abc-123";

		[SetUp]
		public void Setup()
		{
			MockHelper.BuildMockLookups();

			var dateTime1 = new DateTime(2016, 5, 4);
			var dateTime2 = new DateTime(2016, 5, 14);

			Resource1 = MockEntities.CreateResource(5, "blueberries1", "blueberries1.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ResourceTypeConstants.ResourceTypeNames.SdVideo, 2, 3, 4, 5, 4, 2, dateTime1);
			Resource2 = MockEntities.CreateResource(6, "blueberries2", "blueberries2.png", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Png], FileTypeConstants.FileTypeNames.Png, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, 4, 2, dateTime2);
			Resource3 = MockEntities.CreateResource(7, "blueberries3", "blueberries3.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Jpg], FileTypeConstants.FileTypeNames.Jpg, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, 4, 543534, dateTime2);

			Container = new IocRegistration();

			Container.RegisterSingleton(() => MockBuilder<Product>.GetPopulatedRepository(GetProducts));
			Container.RegisterSingleton(() => MockBuilder<Brand>.GetPopulatedRepository(GetBrands));
			Container.RegisterSingleton(() => MockBuilder<Advertiser>.GetPopulatedRepository(MockEntities.GetAdvertisers));
			Container.RegisterSingleton(() => MockBuilder<SubSegment>.GetPopulatedRepository(GetSubSegments));
			Container.RegisterSingleton(() => MockBuilder<Agency>.GetPopulatedRepository(GetAgencies));
			Container.RegisterSingleton(() => MockBuilder<Segment>.GetPopulatedRepository(GetSegments));
			Container.RegisterSingleton(() => MockBuilder<Vertical>.GetPopulatedRepository(MockEntities.GetVerticals));
			Container.RegisterSingleton(() => MockBuilder<Resource>.GetPopulatedRepository(GetResources));
			Container.RegisterSingleton(() => MockBuilder<CampaignContentSchema>.GetPopulatedRepository(GetCampaignContentSchemas));
			Container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(GetCampaigns));
			Container.RegisterSingleton(() => MockBuilder<DeliveryGroup>.GetPopulatedRepository(GetDeliveryGroups));
			Container.RegisterSingleton(() => MockBuilder<MediaPartner>.GetPopulatedRepository(GetMediaPartners));
			Container.RegisterSingleton(() => MockBuilder<Placement>.GetPopulatedRepository(GetPlacements));
			Container.RegisterSingleton(() => MockBuilder<Ad>.GetPopulatedRepository(GetAds));
			Container.RegisterSingleton(() => MockBuilder<Category>.GetPopulatedRepository(MockEntities.CreateCategories));
			Container.RegisterSingleton(() => MockBuilder<AdTypeGroup>.GetPopulatedRepository(MockEntities.CreateAdTypeGroups));

			Container.Register<IUserService, UserService>();
			Container.Register<ICampaignService, CampaignService>();
			Container.Register<IRoleService, RoleService>();
			Container.Register<IAgencyService, AgencyService>();
			Container.Register<IMediaPartnerService, MediaPartnerService>();
			Container.Register<IResourceService, ResourceService>();
			Container.Register<IAdService, AdService>();
			Container.Register<IPlacementService, PlacementService>();
			Container.Register<ICampaignLookupsService, CampaignLookupsService>();
			Container.Register<ICampaignPermissionsService, CampaignPermissionsService>();
			Container.Register<ICampaignsListingService, CampaignsListingService>();
			Container.Register<ICampaignSummaryService, CampaignSummaryService>();

			var ResourceHelper = new Mock<IResourceHelper>();
			var fileStream = new Mock<FileStream>();
			ResourceHelper.Setup(c => c.GenerateMd5HashForFile(FileStream)).Returns(Md5Hash);
			Container.Register<IResourceHelper>(() => ResourceHelper.Object);

			IocSetup.Setup(Container);

			var repo = IoC.Resolve<IRepository<Campaign>>();
			CampaignService = new CampaignService(repo);
		}

		[Test]
		public void Campaign_Summary_Campaign_Properties_Are_Correct_For_Employee_For_Campaign1()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId1 = 1;
			var beginDate = DateHelper.ToString(new DateTime(2015, 1, 11));
			var endDate = DateHelper.ToString(new DateTime(2015, 9, 2));
			var platforms1 = new List<int>{2, 4};
			var mediaPartners1 = new List<int> { 1, 2 };
			var summary1 = CampaignService.GetSummary(campaignId1);
	
			AssertCampaignPropertyValues(summary1, campaignId1, "Campaign 1", "desc 1", "sales 1", 5, "blueberries1", "blueberries1.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ProductConstants.ProductNames.AmexDeo, BrandConstants.BrandNames.Abreva,
				AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein, false, platforms1, mediaPartners1, beginDate, endDate);

		}

		[Test]
		public void Campaign_Summary_Campaign_Properties_Are_Correct_For_Employee_For_Campaign2()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId2 = 2;
			var beginDate = DateHelper.ToString(new DateTime(2015, 4, 3));
			var endDate = DateHelper.ToString(new DateTime(2016, 12, 21));
			var platforms2 = new List<int> { 6 };
			var mediaPartners2 = new List<int> { 3, 4 };
			var summary2 = CampaignService.GetSummary(campaignId2);

			AssertCampaignPropertyValues(summary2, campaignId2, "Campaign 2", "desc 2", "sales 2", 6, "blueberries2", "blueberries2.png", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ProductConstants.ProductNames.AxeMasterbrand, BrandConstants.BrandNames.Abreva,
				AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein, false, platforms2, mediaPartners2, beginDate, endDate);
		}

		[Test]
		public void Campaign_Summary_Campaign_Properties_Are_Correct_For_MediaPartner_And_Children()
		{
			var mediaPartnerId = 1;
			var childMediaPartnerId = 2;
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.MediaPartner, null, mediaPartnerId);

			var campaignId = 1;
			var beginDate = DateHelper.ToString(new DateTime(2015, 1, 11));
			var endDate = DateHelper.ToString(new DateTime(2015, 9, 2));
			var platforms = new List<int> { 2, 4 };
			var mediaPartners = new List<int> { mediaPartnerId, childMediaPartnerId};
			var summary = CampaignService.GetSummary(campaignId);

			AssertCampaignPropertyValues(summary, campaignId, "Campaign 1", "desc 1", "sales 1", 5, "blueberries1", "blueberries1.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ProductConstants.ProductNames.AmexDeo, BrandConstants.BrandNames.Abreva,
				AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein, false, platforms, mediaPartners, beginDate, endDate);
		}

		public void Campaign_Summary_Campaign_Is_Not_Accessible_For_MediaPartner()
		{
			var mediaPartnerId = 3;
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.MediaPartner, null, mediaPartnerId);

			var campaignId = 2;
			var beginDate = DateHelper.ToString(new DateTime(2015, 4, 3));
			var endDate = DateHelper.ToString(new DateTime(2016, 12, 21));
			var platforms = new List<int> { 6 };
			var mediaPartners = new List<int> { mediaPartnerId };
			var summary = CampaignService.GetSummary(campaignId);

			// campaign summary should be null since there are no delivery groups for this current campaign that match this current media partner
			Assert.IsNull(summary);
		}

		[Test]
		public void Campaign_Summary_Campaign_Properties_Are_Correct_For_MediaPartner_Without_Children()
		{
			var mediaPartnerId = 4;
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.MediaPartner, null, mediaPartnerId);

			var campaignId = 2;
			var beginDate = DateHelper.ToString(new DateTime(2015, 4, 3));
			var endDate = DateHelper.ToString(new DateTime(2016, 12, 21));
			var platforms = new List<int> { 6 };
			var mediaPartners = new List<int> { mediaPartnerId };
			var summary = CampaignService.GetSummary(campaignId);

			AssertCampaignPropertyValues(summary, campaignId, "Campaign 2", "desc 2", "sales 2", 6, "blueberries2", "blueberries2.png", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ProductConstants.ProductNames.AxeMasterbrand, BrandConstants.BrandNames.Abreva,
				AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein, false, platforms, mediaPartners, beginDate, endDate);
		}


		#region Private Methods

		private void AssertCampaignPropertyValues(CampaignSummaryViewModel campaign, int campaignId, string campaignName, string description, string salesForceId, int resourceId, string resourceName, string resourceFilename, int reourceType, string productName, string brandName,
			string advertiserName, bool isDeleted, List<int> platforms, List<int> mediaPartners, string beginDate, string endDate)
		{
			Assert.That(campaign.id == campaignId, "Campaign Id is not correct.");
			Assert.That(campaign.name == campaignName, "Campaign Name is not correct.");
			Assert.That(campaign.description == description, "Campaign Description is not correct.");
			Assert.That(campaign.salesForceId == salesForceId, "Campaign SalesForceId Id is not correct.");
			Assert.That(campaign.resourceId == resourceId, "Campaign Resource Id is not correct.");
			Assert.That(campaign.resourceName == resourceName, "Campaign Resource Name is not correct.");
			Assert.That(campaign.resourceFilename == resourceFilename, "Campaign Resource Filename is not correct.");
			Assert.That(campaign.resourceType == reourceType, "Campaign Resource Type is not correct.");
			Assert.That(campaign.productName == productName, "Campaign Product Name is not correct.");
			Assert.That(campaign.brandName == brandName, "Campaign Brand Name is not correct.");
			Assert.That(campaign.advertiserName == advertiserName, "Campaign Advertiser Name is not correct.");
			Assert.That(campaign.isDeleted == isDeleted, "Campaign IsDeleted is not correct.");
			Assert.That(JArray.FromObject(campaign.platforms).ToString() == JArray.FromObject(platforms).ToString(), "Campaign Platforms are not correct.");
			Assert.That(JArray.FromObject(campaign.mediaPartners).ToString() == JArray.FromObject(mediaPartners).ToString(), "Campaign MediaPartners are not correct.");
			Assert.That(campaign.beginDate == beginDate, "Campaign BeginDate is not correct.");
			Assert.That(campaign.endDate == endDate, "Campaign BeginDate is not correct.");
		}

		private List<Placement> GetPlacements()
		{
			var placements = new List<Placement>();
			placements.Add(new Placement{Id = 1, MediaPartner = new MediaPartner{Id = 1}});
			placements.Add(new Placement { Id = 2, MediaPartner = new MediaPartner { Id = 2 } });
			placements.Add(new Placement { Id = 3, MediaPartner = new MediaPartner { Id = 3 } });
			placements.Add(new Placement { Id = 4, MediaPartner = new MediaPartner { Id = 4 } });
			return placements;
		}

		private List<Ad> GetAds()
		{
			var ads = new List<Ad>();
			ads.Add(new Ad
			{
				Id = 1,
				BeginDate = new DateTime(2015, 4, 3),
				EndDate = new DateTime(2015, 9, 2),
				Platform = new Platform{Id = 2}, 
				Campaign = new Campaign{Id = 1}, 
				Placement = new Placement{Id = 1}
			});
			ads.Add(new Ad 
			{ 
				Id = 2, 
				Platform = new Platform { Id = 4 },
				BeginDate = new DateTime(2015, 1, 11),
				EndDate = new DateTime(2015, 7, 12),
				Campaign = new Campaign { Id = 1 }, 
				Placement = new Placement { Id = 2 } 
			});
			ads.Add(new Ad 
			{ 
				Id = 3,
				BeginDate = new DateTime(2015, 12, 6),
				EndDate = new DateTime(2016, 12, 21),
				Platform = new Platform { Id = 6 }, 
				Campaign = new Campaign { Id = 2 }, 
				Placement = new Placement { Id = 3 } 
			});
			ads.Add(new Ad 
			{ 
				Id = 4,
				BeginDate = new DateTime(2015, 4, 3),
				EndDate = new DateTime(2016, 1, 9),
				Platform = new Platform { Id = 6 }, 
				Campaign = new Campaign { Id = 2 }, 
				Placement = new Placement { Id = 4 } 
			});
			return ads;
		}

		private List<Resource> GetResources()
		{
			var resources = new List<Resource> { Resource1, Resource2, Resource3 };

			return resources;
		}

		private List<CampaignContentSchema> GetCampaignContentSchemas()
		{
			var schemas = new List<CampaignContentSchema>();
			schemas.Add(new CampaignContentSchema { Id = 1, Campaign = new Campaign { Id = CampaignId } });

			return schemas;
		}

		private List<Campaign> GetCampaigns()
		{

			var campaigns = new List<Campaign>();
			campaigns.Add(new Campaign { Id = 1, Name = "Campaign 1", Description = "desc 1", SalesForceId = "sales 1", Internal = false, MediaAgency = new Agency { Id = 1 }, Thumbnail = Resource1, UsersFavorite = new List<User>(), Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo] } });
			campaigns.Add(new Campaign { Id = 2, Name = "Campaign 2", Description = "desc 2", SalesForceId = "sales 2", Internal = true, MediaAgency = new Agency { Id = 2 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AxeMasterbrand] } });
			campaigns.Add(new Campaign { Id = 3, Name = "Campaign 3", Description = "desc 3", SalesForceId = "sales 3", Internal = true, MediaAgency = new Agency { Id = 3 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.Beano] } });

			return campaigns;
		}

		private List<Agency> GetAgencies()
		{
			var agencies = new List<Agency>();
			agencies.Add(new Agency { Id = 1 });
			agencies.Add(new Agency { Id = 2, Parent_Id = 1 });
			agencies.Add(new Agency { Id = 3 });

			return agencies;
		}

		private List<DeliveryGroup> GetDeliveryGroups()
		{
			var deliveryGroups = new List<DeliveryGroup>();
			deliveryGroups.Add(new DeliveryGroup { Id = 1, Campaign = new Campaign { Id = 2 }, MediaPartner = new MediaPartner { Id = 1 } });
			deliveryGroups.Add(new DeliveryGroup { Id = 2, Campaign = new Campaign { Id = 2 }, MediaPartner = new MediaPartner { Id = 2 } });
			deliveryGroups.Add(new DeliveryGroup { Id = 3, Campaign = new Campaign { Id = 1 }, MediaPartner = new MediaPartner { Id = 3 } });
			deliveryGroups.Add(new DeliveryGroup { Id = 4, Campaign = new Campaign { Id = 2 }, MediaPartner = new MediaPartner { Id = 4 } });
			deliveryGroups.Add(new DeliveryGroup { Id = 5, Campaign = new Campaign { Id = 1 }, MediaPartner = new MediaPartner { Id = 1 } });
			deliveryGroups.Add(new DeliveryGroup { Id = 6, Campaign = new Campaign { Id = 1 }, MediaPartner = new MediaPartner { Id = 2 } });
			return deliveryGroups;
		}

		private List<MediaPartner> GetMediaPartners()
		{
			var mediaPartners = new List<MediaPartner>();
			mediaPartners.Add(new MediaPartner { Id = 1 });
			mediaPartners.Add(new MediaPartner { Id = 2, Parent_Id = 1 });
			mediaPartners.Add(new MediaPartner { Id = 3});
			mediaPartners.Add(new MediaPartner { Id = 4 });
			return mediaPartners;
		}

		private static List<Product> GetProducts()
		{
			var items = new List<Product>()
				{
					new Product() {
						Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo], 
						Name = ProductConstants.ProductNames.AmexDeo, 
						Brand = new Brand
						{
							Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.Abreva]
						}, 
						SubSegment = new SubSegment{Id = 1}
					},
					new Product() {
						Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AxeMasterbrand], 
						Name = ProductConstants.ProductNames.AxeMasterbrand, 
						Brand = new Brand
						{
							Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.Abreva]
						}, 
						SubSegment = new SubSegment{Id = 2}
					},
					new Product() {
						Id = Lookups.Products.HashByName[ProductConstants.ProductNames.Beano], 
						Name = ProductConstants.ProductNames.Beano, 
						Brand = new Brand
						{
							Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.Bertolli]
						}, 
						SubSegment = new SubSegment{Id = 2}
					}
				};
			return items;
		}

		private static List<SubSegment> GetSubSegments()
		{
			var subsegments = new List<SubSegment>();
			subsegments.Add(new SubSegment { Id = 1, Name = "Consumer Packaged Goods", Segment = new Segment { Id = 1 } });
			subsegments.Add(new SubSegment { Id = 2, Name = "Food & Beverage", Segment = new Segment { Id = 1 } });

			return subsegments;
		}

		private static List<Segment> GetSegments()
		{
			var segments = new List<Segment>();
			segments.Add(new Segment { Id = 1, Name = "Consumer Packaged Goods", Vertical = new Vertical { Id = 1, Name = VerticalConstants.VerticalNames.ConsumerPackagedGood } });
			segments.Add(new Segment { Id = 2, Name = "Food & Beverage", Vertical = new Vertical { Id = 2, Name = VerticalConstants.VerticalNames.PharmaAndHealthcare } });

			return segments;
		}

		private static List<Brand> GetBrands()
		{
			var brands = new List<Brand>();
			brands.Add(new Brand
			{
				Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.Abreva],
				Name = BrandConstants.BrandNames.Abreva,
				Advertiser = new Advertiser
				{
					Id = Lookups.Advertisers.HashByName[AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein],
					Name = AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein
				}
			});
			brands.Add(new Brand
			{
				Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.AmericanExpressCards],
				Name = BrandConstants.BrandNames.AmericanExpressCards,
				Advertiser = new Advertiser
				{
					Id = Lookups.Advertisers.HashByName[AdvertiserConstants.AdvertiserNames.AmericanExpress],
					Name = AdvertiserConstants.AdvertiserNames.AmericanExpress
				}
			});
			brands.Add(new Brand
			{
				Id = Lookups.Brands.HashByName[BrandConstants.BrandNames.Bertolli],
				Name = BrandConstants.BrandNames.Bertolli,
				Advertiser = new Advertiser
				{
					Id = Lookups.Advertisers.HashByName[AdvertiserConstants.AdvertiserNames.Unilever],
					Name = AdvertiserConstants.AdvertiserNames.Unilever
				}
			});
			return brands;
		}

		#endregion

	}
}
