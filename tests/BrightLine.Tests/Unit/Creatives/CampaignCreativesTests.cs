using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdType;
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
	public class CampaignCreativesTests
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
			var creativeId1 = 1;
			var creativeId2 = 2;
			var creativeId3 = 3;

			Resource1 = MockEntities.CreateResource(5, "blueberries1", "blueberries1.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ResourceTypeConstants.ResourceTypeNames.SdVideo, 2, 3, 4, 5, creativeId1, CampaignId, dateTime1);
			Resource2 = MockEntities.CreateResource(6, "blueberries2", "blueberries2.png", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Png], FileTypeConstants.FileTypeNames.Png, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, creativeId2, CampaignId, dateTime2);
			Resource3 = MockEntities.CreateResource(7, "blueberries3", "blueberries3.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Jpg], FileTypeConstants.FileTypeNames.Jpg, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], ResourceTypeConstants.ResourceTypeNames.SdImage, 2, 3, 4, 5, creativeId3, 543534, dateTime2);

			Container = new IocRegistration();

			Container.RegisterSingleton(() => MockBuilder<Resource>.GetPopulatedRepository(GetResources));
			Container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(GetCampaigns));
			Container.RegisterSingleton(() => MockBuilder<AdFunction>.GetPopulatedRepository(GetAdFunctions));
			Container.RegisterSingleton(() => MockBuilder<AdType>.GetPopulatedRepository(GetAdTypes));
			Container.RegisterSingleton(() => MockBuilder<Creative>.GetPopulatedRepository(GetCreatives));
			Container.RegisterSingleton(() => MockBuilder<Feature>.GetPopulatedRepository(GetFeatures));
			Container.RegisterSingleton(() => MockBuilder<CampaignContentSchema>.GetPopulatedRepository(GetCampaignContentSchemas));

			Container.Register<IUserService, UserService>();
			Container.Register<ICampaignService, CampaignService>();
			Container.Register<IRoleService, RoleService>();
			Container.Register<IResourceService, ResourceService>();
			Container.Register<ICreativeService, CreativeService>();
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
		public void Creatives_Listing_Destination_Creatives_Does_Not_Contain_Promo_Creatives()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId = 1;
	
			var creatives = CampaignService.GetDestinationCreatives(campaignId);
			var promoCreatives = creatives.FindAll(c => c.isPromo);
			
			Assert.IsEmpty(promoCreatives);
		}

		[Test]
		public void Creatives_Listing_Promo_Creatives_Does_Not_Contain_Destination_Creatives()
		{
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var campaignId = 2;

			var creatives = CampaignService.GetPromotionalCreatives(campaignId);
			var destinationCreatives = creatives.FindAll(c => !c.isPromo);

			Assert.IsEmpty(destinationCreatives);
		}

		[Test]
		public void Creatives_Listing_Destination_Properties_Are_Correct()
		{
			var campaignId = 1;
			var features1 = new List<int>{1,2};
	
			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var creatives = CampaignService.GetDestinationCreatives(campaignId);
			var creative1 = creatives.Find(c => c.id == 1);
			var creative2 = creatives.Find(c => c.id == 2);

			AssertCreativePropertyValues(creative1, 1, "Creative 1", false, campaignId, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeConstants.AdTypeNames.BrandDestination, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
				AdFunctionConstants.AdFunctionNames.ClickToJump, false, 5, "blueberries1", "blueberries1.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], features1);

			AssertCreativePropertyValues(creative2, 2, "Creative 2", false, campaignId, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], AdTypeConstants.AdTypeNames.BrandDestination, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
				AdFunctionConstants.AdFunctionNames.OnDemand, false, 6, "blueberries2", "blueberries2.png", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], features1);
		}

		[Test]
		public void Creatives_Listing_Promo_Properties_Are_Correct()
		{
			var campaignId = 2;
			var features1 = new List<int> {3 };

			MockUtilities.SetupAuth(UserId, AuthConstants.Roles.Employee);

			var creatives = CampaignService.GetPromotionalCreatives(campaignId);
			var creative1 = creatives.Find(c => c.id == 3);

			AssertCreativePropertyValues(creative1, 3, "Creative 3", false, campaignId, Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], AdTypeConstants.AdTypeNames.CommercialSpot, Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
				AdFunctionConstants.AdFunctionNames.ClickToJump, true, 7, "blueberries3", "blueberries3.mp4", Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage], features1);
		}


		#region Private Methods

		private void AssertCreativePropertyValues(CampaignCreativeViewModel creative, int id, string name, bool isDeleted, int campaignId, int adTypeId, string adTypeName, int adFunctionId, string adFunctionName, bool isPromo, int resourceId,
			string resourceName, string resourceFilename, int resourceType, List<int> features)
		{
			Assert.That(creative.id == id, "Creative Id is not correct.");
			Assert.That(creative.name == name, "Creative Name is not correct.");
			Assert.That(creative.isDeleted == isDeleted, "Creative IsDeleted is not correct.");
			Assert.That(creative.campaignId == campaignId, "Creative CampaignId is not correct.");
			Assert.That(creative.adTypeId == adTypeId, "Creative AdTypeId is not correct.");
			Assert.That(creative.adTypeName == adTypeName, "Creative AdTypeName is not correct.");
			Assert.That(creative.adFunctionId == adFunctionId, "Creative AdFunctionId is not correct.");
			Assert.That(creative.adFunctionName == adFunctionName, "Creative AdFunctionName is not correct.");
			Assert.That(creative.isPromo == isPromo, "Creative IsPromo is not correct.");
			Assert.That(creative.resourceId == resourceId, "Creative ResourceId is not correct.");
			Assert.That(creative.resourceName == resourceName, "Creative ResourceName is not correct.");
			Assert.That(creative.resourceFilename == resourceFilename, "Creative ResourceFilename is not correct.");
			Assert.That(creative.resourceType == resourceType, "Creative ResourceType is not correct.");
			Assert.That(JArray.FromObject(creative.features).ToString() == JArray.FromObject(features).ToString(), "Creative Features are not correct.");
			// *Note: not testing Begin/End Dates because those are computed fields by the database. I'm also not testing LastModifiedDate because that relies on Begin/End Dates.
		}

		private List<Resource> GetResources()
		{
			var resources = new List<Resource> { Resource1, Resource2, Resource3 };

			return resources;
		}

		private List<CampaignContentSchema> GetCampaignContentSchemas()
		{
			var schemas = new List<CampaignContentSchema>();
			schemas.Add(new CampaignContentSchema { Id = 1, Campaign = new Campaign { Id = 1 } });

			return schemas;
		}

		private List<Creative> GetCreatives()
		{
			var creatives = new List<Creative>();
			creatives.Add(new Creative 
			{ 
				Id = 1, 
				Name = "Creative 1",
				Campaign = new Campaign 
				{ 
					Id = 1 
				}, 
				AdType = new AdType
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], 
					Name = AdTypeConstants.AdTypeNames.BrandDestination,
					IsPromo = false
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
					Name = AdFunctionConstants.AdFunctionNames.ClickToJump
				}
			});
			creatives.Add(new Creative 
			{ 
				Id = 2,
				Name = "Creative 2",
				Campaign = new Campaign 
				{ 
					Id = 1 
				}, 
				AdType = new AdType 
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination],
					Name = AdTypeConstants.AdTypeNames.BrandDestination,
					IsPromo = false
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand],
					Name = AdFunctionConstants.AdFunctionNames.OnDemand
				}
			});
			creatives.Add(new Creative 
			{ 
				Id = 3,
				Name = "Creative 3",
				Campaign = new Campaign 
				{ 
					Id = 2 
				}, 
				AdType = new AdType 
				{
					Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot],
					Name = AdTypeConstants.AdTypeNames.CommercialSpot,
					IsPromo = true
				},
				AdFunction = new AdFunction
				{
					Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump],
					Name = AdFunctionConstants.AdFunctionNames.ClickToJump
				}
			});

			return creatives;
		}

		private List<Campaign> GetCampaigns()
		{

			var campaigns = new List<Campaign>();
			campaigns.Add(new Campaign { Id = 1, Name = "Campaign 1", Internal = false, MediaAgency = new Agency { Id = 1 }, Thumbnail = Resource1, UsersFavorite = new List<User>(), Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo] } });
			campaigns.Add(new Campaign { Id = 2, Name = "Campaign 2", Internal = true, MediaAgency = new Agency { Id = 2 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AxeMasterbrand] } });
			campaigns.Add(new Campaign { Id = 3, Name = "Campaign 3", Internal = true, MediaAgency = new Agency { Id = 3 }, Thumbnail = Resource2, UsersFavorite = new List<User> { new User { Id = UserId } }, Product = new Product { Id = Lookups.Products.HashByName[ProductConstants.ProductNames.Beano] } });

			return campaigns;
		}

		private List<AdType> GetAdTypes()
		{
			var adTypes = new List<AdType>();
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.CommercialSpot], Name = AdTypeConstants.AdTypeNames.CommercialSpot, IsPromo = true });
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.DedicatedBrandApp], Name = AdTypeConstants.AdTypeNames.DedicatedBrandApp });
			adTypes.Add(new AdType { Id = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination], Name = AdTypeConstants.AdTypeNames.BrandDestination });

			return adTypes;
		}

		private List<AdFunction> GetAdFunctions()
		{
			var adFunctions = new List<AdFunction>();
			adFunctions.Add(new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.OnDemand], Name = AdFunctionConstants.AdFunctionNames.OnDemand });
			adFunctions.Add(new AdFunction { Id = Lookups.AdFunctions.HashByName[AdFunctionConstants.AdFunctionNames.ClickToJump], Name = AdFunctionConstants.AdFunctionNames.ClickToJump });

			return adFunctions;
		}

		private List<Feature> GetFeatures()
		{
			var features = new List<Feature>();
			features.Add(new Feature 
			{ 
				Id = 1,
				Creative = new Creative { Id = 1},
				Campaign = new Campaign { Id = 1}
			});
			features.Add(new Feature 
			{ 
				Id = 2,
				Creative = new Creative { Id = 2 },
				Campaign = new Campaign { Id = 1}
			});
			features.Add(new Feature 
			{ 
				Id = 3,
				Creative = new Creative { Id = 3 },
				Campaign = new Campaign { Id = 2}
			});

			return features;
		}

		
		#endregion

	}
}
