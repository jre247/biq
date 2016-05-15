using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Service;
using BrightLine.Tests.Common;
using Moq;
using NUnit.Framework;
using System;

namespace BrightLine.Tests.Unit.Campaigns
{
    [TestFixture]
	public class CampaignRuleTests
    {
        public const int TEST_ADVERTISER_ID = 12;
		IocRegistration Container;

		[SetUp]
		public void SetUp()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
		}

        [Test]
        public void IsAdvertiserOfCampaign()
        {
            ShouldPassCampaignRule(TEST_ADVERTISER_ID, 12, AuthConstants.Roles.AgencyPartner, (svc, campaign) => svc.IsCampaignAdvertiser(campaign));
        }


        [Test]
        public void IsNotAdvertiserOfCampaignDueToDifferentAdvertiserId()
        {
            ShouldFailCampaignRule( TEST_ADVERTISER_ID, 14, AuthConstants.Roles.AgencyPartner, (svc, campaign) => svc.IsCampaignAdvertiser(campaign));
        }


        [Test]
        public void IsAccessibleAsAgencyParter()
        {
            ShouldPassCampaignRule(TEST_ADVERTISER_ID, 12, AuthConstants.Roles.AgencyPartner, (svc, campaign) => svc.IsAccessible(campaign));
        }


        [Test]
        public void IsAccessibleAsAdmin()
        {
            ShouldPassCampaignRule(TEST_ADVERTISER_ID, 14, AuthConstants.Roles.Admin, (svc, campaign) => svc.IsAccessible(campaign));
        }


        [Test]
        public void IsAccessibleAsEmployee()
        {
            ShouldPassCampaignRule(TEST_ADVERTISER_ID, 16, AuthConstants.Roles.Employee, (svc, campaign) => svc.IsAccessible(campaign));
        }


        [Test]
        public void IsNotAccessibleDueToRole()
        {
            ShouldFailCampaignRule(TEST_ADVERTISER_ID, 12, AuthConstants.Roles.CMSAdmin, (svc, campaign) => svc.IsAccessible(campaign));
        }


        [Test]
        public void IsNotAccessibleDueToDifferentAdvertiserId()
        {
            ShouldFailCampaignRule(TEST_ADVERTISER_ID, 14, AuthConstants.Roles.AgencyPartner, (svc, campaign) => svc.IsAccessible(campaign), 2);
        }


        [Test]
        public void IsUserAllowedToCreate()
        {
			ShouldFailCampaignRule(TEST_ADVERTISER_ID, 12, AuthConstants.Roles.AgencyPartner, (svc, campaign) => svc.IsUserAllowedToCreate());
        }


        [Test]
        public void IsUserNotAllowedToCreate()
        {
            ShouldFailCampaignRule(TEST_ADVERTISER_ID, 12, AuthConstants.Roles.CMSEditor, (svc, campaign) => svc.IsUserAllowedToCreate());
        }


        [Test]
        public void IsNotAccessibleDueToDeleted()
        {
            ShouldFailCampaignRule(TEST_ADVERTISER_ID, 14, AuthConstants.Roles.AgencyPartner, (svc, campaign) =>
            {
                    campaign.IsDeleted = true;
                    return svc.IsAccessible(campaign); 
            });
        }


        private void ShouldPassCampaignRule(int advertiserUserId, int userId, string role, Func<CampaignService, Campaign, bool> checker)
        {
            var result = CheckCampaignRule(advertiserUserId, userId, role, checker);
            Assert.IsTrue(result);
        }


		private void ShouldFailCampaignRule(int advertiserUserId, int userId, string role, Func<CampaignService, Campaign, bool> checker, int campaignAgencyId = 1, int userAgencyId = 1)
        {
			var result = CheckCampaignRule(advertiserUserId, userId, role, checker, campaignAgencyId, userAgencyId);
            Assert.IsFalse(result);
        }


        private bool CheckCampaignRule( int advertiserUserId, int userId, string role, Func<CampaignService, Campaign, bool> checker, int campaignAgencyId = 1, int userAgencyId = 1)
        {
            // 1. Create the campaign
            var campaign = new Campaign();
            campaign.Product = new Product();
            campaign.Product.Brand = new Brand();
            campaign.Product.Brand.Advertiser = new Advertiser();
            campaign.Product.Brand.Advertiser.Id = advertiserUserId;
			campaign.MediaAgency = new Agency { Id = campaignAgencyId };

            // 2. Create sample user for authentication
            var authUser = new User() {Display = "user1", TimeZoneId = "Eastern Standard Time"};
            authUser.Advertiser = new Advertiser();
            authUser.Advertiser.Id = userId;
			authUser.MediaAgency = new Agency { Id = userAgencyId };
            Auth.Init(new AuthForUnitTests(true, userId, "user1", role, (name) => authUser));

            // 3. Now check.
			var mockFileHelper = new Mock<IFileHelper>();
			var mockResource = new Mock<Resource>();
			mockResource.Setup(c => c.Id).Returns(1);
			mockFileHelper.Setup(c => c.GetCloudFileDownloadUrl(mockResource.Object)).Returns("/download");
			Container.Register<IFileHelper>(() => mockFileHelper.Object);

            var svc = new CampaignService(new EntityRepositoryInMemory<Campaign>());
            var result = checker(svc, campaign);
            return result;
        }
    }
}
