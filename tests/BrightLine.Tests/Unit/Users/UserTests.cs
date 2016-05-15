using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Users;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace BrightLine.Tests.Unit
{
	[TestFixture]
	class UserTests
	{
		IocRegistration _container;
		ICampaignService Campaigns {get;set;}
		IUserService Users { get;set;}

		[SetUp]
		public void Setup()
		{
			_container = new IocRegistration();

			_container.RegisterSingleton(() => MockBuilder<Role>.GetPopulatedRepository(MockEntities.GetRoles));
			_container.RegisterSingleton(() => MockBuilder<User>.GetPopulatedRepository(MockEntities.GetUsers));
			_container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(MockEntities.GetCampaigns));
			_container.RegisterSingleton(() => MockBuilder<Advertiser>.GetPopulatedRepository(MockEntities.GetAdvertisers));
			_container.RegisterSingleton(() => MockBuilder<AccountInvitation>.GetPopulatedRepository(MockEntities.GetAccountInvitations));
			_container.RegisterSingleton(() => MockBuilder<Agency>.GetPopulatedRepository(MockEntities.CreateAgencies));
			_container.RegisterSingleton(() => MockBuilder<MediaPartner>.GetPopulatedRepository(MockEntities.CreateMediaPartners));

			var mockEmailService = new Mock<IEmailService>();
			_container.Register<IEmailService>(() => mockEmailService.Object);

			_container.Register<ICache, Cache>();
			_container.Register<IUserService, UserService>();
			_container.Register<IRepository<CampaignContentSchema>, CrudRepository<CampaignContentSchema>>();
			_container.Register<ICampaignService, CampaignService>();
			_container.Register<IRoleService, RoleService>();
			_container.Register<IFileHelper, FileHelper>();
			_container.Register<IAccountService, AccountService>();
			_container.Register<ISettingsService, MockSettingService>();
			_container.Register<IAgencyService, AgencyService>();
			_container.Register<IMediaPartnerService, MediaPartnerService>();

			IocSetup.Setup(_container);

			MockHelper.BuildMockLookups();


			Campaigns = IoC.Resolve<ICampaignService>();
			Users = IoC.Resolve<IUserService>();
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void CreateUserAdvertiser()
		{
			var firstName = "Jeff";
			var lastName = "Edwards";
			var email = "Jeff@Jeff.com";
			var advertiserId = 3;
			var mediaAgencyId = -1;
			var mediaPartnerId = -1;
			var timeZoneId = "Eastern Standard Time";
			var isInternal = false;
			var roleId = Lookups.Roles.HashByName[RoleConstants.RoleNames.Client];
			var roles = new List<EntityLookup>{new EntityLookup{Id = roleId, Name = RoleConstants.RoleNames.Client}};

			var userViewModel = CreateUserViewModel(firstName, lastName, email, advertiserId, mediaAgencyId, mediaPartnerId, timeZoneId, isInternal, roles);

			var userViewModelJson = JObject.FromObject(userViewModel);
			var user = Users.SaveUser(0, userViewModelJson.ToString());

			Assert.IsTrue(user.Id > 0, "User Id is not correct.");
			Assert.IsTrue(user.FirstName == firstName, "User First Name is not correct.");
			Assert.IsTrue(user.LastName == lastName, "User Last Name is not correct.");
			Assert.IsTrue(user.Email == email, "User Email is not correct.");
			Assert.IsTrue(user.Advertiser.Id == advertiserId, "User Advertiser is not correct.");
			Assert.IsNull(user.MediaPartner, "User Media Partner should be null.");
			Assert.IsNull(user.MediaAgency, "User Media Agency should be null.");
			Assert.IsTrue(user.TimeZoneId == timeZoneId, "User Time Zone Id is not correct.");
			Assert.IsTrue(user.Internal == isInternal, "User IsInternal is not correct.");
			Assert.IsTrue(user.Roles.Count == roles.Count, "User Roles count is not correct.");
			var userRoles = user.Roles.ToEntities();
			Assert.IsTrue(userRoles[0].Id == roleId, "User Role Id is not correct.");
		}

		

		[Test]
		public void CreateUserMediaAgency()
		{
			var firstName = "Jeff";
			var lastName = "Edwards";
			var email = "Jeff@Jeff.com";
			var advertiserId = -1;
			var mediaAgencyId = 1;
			var mediaPartnerId = -1;
			var timeZoneId = "Eastern Standard Time";
			var isInternal = false;
			var roleId = Lookups.Roles.HashByName[RoleConstants.RoleNames.AgencyPartner];
			var roles = new List<EntityLookup> { new EntityLookup { Id = roleId, Name = RoleConstants.RoleNames.AgencyPartner } };

			var userViewModel = CreateUserViewModel(firstName, lastName, email, advertiserId, mediaAgencyId, mediaPartnerId, timeZoneId, isInternal, roles);

			var userViewModelJson = JObject.FromObject(userViewModel);
			var user = Users.SaveUser(0, userViewModelJson.ToString());

			Assert.IsTrue(user.Id > 0, "User Id is not correct.");
			Assert.IsTrue(user.FirstName == firstName, "User First Name is not correct.");
			Assert.IsTrue(user.LastName == lastName, "User Last Name is not correct.");
			Assert.IsTrue(user.Email == email, "User Email is not correct.");
			Assert.IsNull(user.Advertiser, "User Advertiser should be null.");
			Assert.IsNull(user.MediaPartner, "User Media Partner should be null.");
			Assert.IsTrue(user.MediaAgency.Id == mediaAgencyId, "User Media Agency is not correct.");
			Assert.IsTrue(user.TimeZoneId == timeZoneId, "User Time Zone Id is not correct.");
			Assert.IsTrue(user.Internal == isInternal, "User IsInternal is not correct.");
			Assert.IsTrue(user.Roles.Count == roles.Count, "User Roles count is not correct.");
			var userRoles = user.Roles.ToEntities();
			Assert.IsTrue(userRoles[0].Id == roleId, "User Role Id is not correct.");
		}

		[Test]
		public void CreateUserMediaPartner()
		{
			var firstName = "Jeff";
			var lastName = "Edwards";
			var email = "Jeff@Jeff.com";
			var advertiserId = -1;
			var mediaAgencyId = -1;
			var mediaPartnerId = 1;
			var timeZoneId = "Eastern Standard Time";
			var isInternal = false;
			var roleId = Lookups.Roles.HashByName[RoleConstants.RoleNames.MediaPartner];
			var roles = new List<EntityLookup> { new EntityLookup { Id = roleId, Name = RoleConstants.RoleNames.MediaPartner } };

			var userViewModel = CreateUserViewModel(firstName, lastName, email, advertiserId, mediaAgencyId, mediaPartnerId, timeZoneId, isInternal, roles);

			var userViewModelJson = JObject.FromObject(userViewModel);
			var user = Users.SaveUser(0, userViewModelJson.ToString());

			Assert.IsTrue(user.Id > 0, "User Id is not correct.");
			Assert.IsTrue(user.FirstName == firstName, "User First Name is not correct.");
			Assert.IsTrue(user.LastName == lastName, "User Last Name is not correct.");
			Assert.IsTrue(user.Email == email, "User Email is not correct.");
			Assert.IsNull(user.Advertiser, "User Advertiser should be null.");
			Assert.IsTrue(user.MediaPartner.Id == mediaPartnerId, "User Media Partner is not correct.");
			Assert.IsNull(user.MediaAgency, "User Media Agency should be null.");
			Assert.IsTrue(user.TimeZoneId == timeZoneId, "User Time Zone Id is not correct.");
			Assert.IsTrue(user.Internal == isInternal, "User IsInternal is not correct.");
			Assert.IsTrue(user.Roles.Count == roles.Count, "User Roles count is not correct.");
			var userRoles = user.Roles.ToEntities();
			Assert.IsTrue(userRoles[0].Id == roleId, "User Role Id is not correct.");
		}

		
		[Test]
		public void ToggleFavoriteAdd()
		{
			var campaign = Campaigns.Get(1);
			var user = Users.Get(1);
			Users.ToggleFavorite(user, campaign);
			bool hasCampaign = user.CampaignFavorites.Contains(campaign);
			Assert.IsTrue(hasCampaign, "Campaign not added to user CampaignFavorites");
		}

		[Test]
		public void ToggleFavoriteRemove()
		{
			var campaign = Campaigns.Get(1);
			var user = Users.Get(1);
			user.CampaignFavorites.Add(campaign);
			Users.ToggleFavorite(user, campaign);
			bool hasCampaign = user.CampaignFavorites.Contains(campaign);
			Assert.IsFalse(hasCampaign, "Campaign not removed to user CampaignFavorites");
		}
		
		private static UserViewModel CreateUserViewModel(string firstName, string lastName, string email, int advertiserId, int mediaAgencyId, int mediaPartnerId, string timeZoneId, bool isInternal, List<EntityLookup> roles)
		{
			var userViewModel = new UserViewModel
			{
				FirstName = firstName,
				LastName = lastName,
				Advertiser = advertiserId,
				Email = email,
				MediaAgency = mediaAgencyId,
				MediaPartner = mediaPartnerId,
				TimeZoneId = timeZoneId,
				Internal = isInternal,
				Roles = roles
			};
			return userViewModel;
		}


	}
}