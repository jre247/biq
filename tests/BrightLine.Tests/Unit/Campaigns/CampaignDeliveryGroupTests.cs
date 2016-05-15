using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using BrightLine.Service;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using NUnit.Framework;
using System;

namespace BrightLine.Tests.Unit.Campaigns
{
    [TestFixture]
	public class CampaignDeliveryGroupTests
    {
		IocRegistration Container;
		IRepository<DeliveryGroup> DeliveryGroups { get;set;}
		ICampaignService Campaigns { get; set; }

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth();

			MockUtilities.SetupIoCContainer(Container);

			DeliveryGroups = IoC.Resolve<IRepository<DeliveryGroup>>();
			Campaigns = IoC.Resolve<ICampaignService>();

			var dateHelperService = IoC.Resolve<IDateHelperService>();
			dateHelperService.CurrentDate = new DateTime(2015, 5, 1);
		}

        [Test]
        public void Campaign_Delivery_Groups_Exists()
        {
           var campaignDeliveryGroups = DeliveryGroups.Where(c => c.Campaign.Id == 1).ToEntities();

		   Assert.IsTrue(campaignDeliveryGroups.Count == 2);
        }

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Count_For_Campaign()
		{
			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			Assert.IsTrue(campaignDeliveryGroups.Count == 2);
		}

		[Test]
		public void Campaign_Delivery_Groups_Is_Null_For_Campaign()
		{
			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(123245);

			Assert.IsTrue(campaignDeliveryGroups.Count == 0);
		}

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Begin_And_End_Dates()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 1, 12), new DateTime(2015, 5, 22)));
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 2, 23), new DateTime(2015, 5, 20)));

			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.beginDate == "01/12/2015 12:00:00 AM", "Delivery Group Begin Date is incorrect.");
			Assert.IsTrue(deliveryGroupCompare.endDate == "05/22/2015 12:00:00 AM", "Delivery Group End Date is incorrect.");
		}

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Non_Computed_Properties()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 1, 12), new DateTime(2015, 5, 22)));
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 2, 23), new DateTime(2015, 5, 20)));

			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.id == 1, "Delivery Group Id is incorrect.");
			Assert.IsTrue(deliveryGroupCompare.name == "Delivery Group 1", "Delivery Group Name is incorrect.");
			Assert.IsTrue(deliveryGroupCompare.impressionGoal == 10, "Delivery Group Impressions Count is incorrect.");
			Assert.IsTrue(deliveryGroupCompare.mediaPartnerId == 1, "Delivery Group mediaPartner Id is incorrect.");
		}

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Delivering_Status()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 1, 12), new DateTime(2015, 10, 22)));
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 2, 23), new DateTime(2015, 9, 20)));

			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.Status == CampaignStatus.Delivering.ToString(), "Delivery Group Status should be Delivering.");		
		}

		[Test]
		public void Campaign_Delivery_Groups_With_Empty_Ads_Has_Correct_Upcoming_Status()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			
			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.Status == CampaignStatus.Upcoming.ToString(), "Delivery Group Status should be Delivering.");
		}

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Completed_Status()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 1, 12), new DateTime(2015, 3, 22)));
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 2, 23), new DateTime(2015, 3, 20)));

			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.Status == CampaignStatus.Completed.ToString(), "Delivery Group Status should be Completed.");
		}

		[Test]
		public void Campaign_Delivery_Groups_Has_Correct_Upcoming_Status()
		{
			var deliveryGroup = DeliveryGroups.Get(1);
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 7, 12), new DateTime(2015, 9, 22)));
			deliveryGroup.Ads.Add(MockEntities.CreateAd(1, new DateTime(2015, 7, 23), new DateTime(2015, 9, 20)));

			var campaignDeliveryGroups = Campaigns.GetCampaignDeliveryGroups(1);

			var deliveryGroupCompare = campaignDeliveryGroups[1];
			Assert.IsTrue(deliveryGroupCompare.Status == CampaignStatus.Upcoming.ToString(), "Delivery Group Status should be Upcoming.");
		}
        
    }
}
