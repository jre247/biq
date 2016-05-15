using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Core;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class CampaignLookupsTests
	{
		IocRegistration Container;
		

		[SetUp]
		public void SetUp()
		{
			Container = MockUtilities.SetupIoCContainer(Container);

			
		}

		[Test]
		public void Campaign_Lookups_Platform_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, adFormatId = 1, platformId = 23, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch");
			IoC.Ads.Create(ad);
			var lookupEntities = new EntityLookups {Name = "platforms", Values = IoC.Platforms.GetAll(), CampaignId = campaignId};

			var hash = EntityLookups.BuildHashOfEntitiesInCampaign(lookupEntities);

			Assert.IsTrue(hash.Contains(platformId));
		}

		[Test]
		public void Campaign_Lookups_Platform_Does_Not_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, campaignId2 = 300, adFormatId = 1, platformId = 23, platformId2 = 44, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch");
			IoC.Ads.Create(ad);
			var ad2 = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId2, adFormatId, "Format 1", adTagId, platformId2, "Product Watch");
			IoC.Ads.Create(ad2);
			var lookupEntities = new EntityLookups { Name = "platforms", Values = IoC.Platforms.GetAll(), CampaignId = campaignId };

			var hash = EntityLookups.BuildHashOfEntitiesInCampaign(lookupEntities);

			Assert.IsFalse(hash.Contains(platformId2));
		}

		[Test]
		public void Campaign_Lookups_Channel_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, adFormatId = 1, platformId = 23, channelId = 2, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch");
			ad.Placement.Channel = new Channel { Id = channelId }; 
			IoC.Ads.Create(ad);
			var lookupEntities = new EntityLookups { Name = "channels", Values = MockEntities.CreateChannels().ToLookups("Name"), CampaignId = campaignId };

			var hash = EntityLookups.BuildHashOfEntitiesInCampaign(lookupEntities);

			//assert hash of entities in campaign has platform with id 23
			Assert.AreEqual(hash.Count(), 1);
			Assert.IsTrue(hash.Contains(channelId));
		}

		[Test]
		public void Campaign_Lookups_Channel_Does_Not_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, campaignId2 = 300, adFormatId = 1, platformId = 23, channelId = 2, channelId2 = 3, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch");
			ad.Placement.Channel = new Channel { Id = channelId }; 
			IoC.Ads.Create(ad);
			var ad2 = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId2, adFormatId, "Format 1", adTagId, platformId, "Product Watch");
			ad2.Placement.Channel = new Channel { Id = channelId2 }; 
			IoC.Ads.Create(ad2);
			var lookupEntities = new EntityLookups { Name = "channels", Values = MockEntities.CreateChannels().ToLookups("Name"), CampaignId = campaignId };

			var hash = EntityLookups.BuildHashOfEntitiesInCampaign(lookupEntities);

			Assert.IsFalse(hash.Contains(channelId2));
		}

		[Test]
		public void Campaign_Lookups_AdType_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, adFormatId = 1, platformId = 23, channelId = 2, adTypeId = 10002, creativeId = 1, placementId = 1, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch", placementId, "placement 1", channelId, "channel 1", creativeId, "creative 1", adTypeId);
			IoC.Ads.Create(ad);

			var hash = IoC.Campaigns.BuildHashOfAdTypesInCampaign(campaignId, MockEntities.GetAdTypes());

			//assert hash of entities in campaign has platform with id 23
			Assert.AreEqual(hash.Count(), 1);
			Assert.IsTrue(hash.Contains(adTypeId));
		}

		[Test]
		public void Campaign_Lookups_AdType_Does_Not_Exists_In_Lookups_Hash()
		{
			int campaignId = 211, campaignId2 = 300, adFormatId = 1, platformId = 23, channelId = 2, adTypeId = 10002, adTypeId2 = 10001, creativeId = 1, placementId = 1, adTagId = 1;
			var ad = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId, adFormatId, "Format 1", adTagId, platformId, "Product Watch", placementId, "placement 1", channelId, "channel 1", creativeId, "creative 1", adTypeId);
			IoC.Ads.Create(ad);
			var ad2 = MockEntities.CreateAd(0, "test", new DateTime(2015, 4, 3), new DateTime(2015, 8, 3), campaignId2, adFormatId, "Format 1", adTagId, platformId, "Product Watch", placementId, "placement 1", channelId, "channel 1", creativeId, "creative 1", adTypeId2);
			IoC.Ads.Create(ad2);

			var hash = IoC.Campaigns.BuildHashOfAdTypesInCampaign(campaignId, MockEntities.GetAdTypes());

			//assert hash of entities in campaign has platform with id 23
			Assert.IsFalse(hash.Contains(adTypeId2));
		}

	}
}
