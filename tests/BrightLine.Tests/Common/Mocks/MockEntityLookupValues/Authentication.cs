using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		
		public static List<Role> GetRoles()
		{
			// create some sample roles.
			var admin = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.Admin],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.Admin,
				DisplayName = "Administrator",
				Users = new List<User>()
			};
			var developer = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.Developer],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.Developer,
				DisplayName = AuthConstants.Roles.Developer,
				Users = new List<User>()
			};
			var employee = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.Employee],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.Employee,
				DisplayName = AuthConstants.Roles.Employee,
				Users = new List<User>()
			};
			var client = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.Client],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.Client,
				DisplayName = AuthConstants.Roles.Client,
				Users = new List<User>()
			};
			var agencyPartner = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.AgencyPartner],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.AgencyPartner,
				DisplayName = "Agency Partner",
				Users = new List<User>()
			};
			var mediaPartner = new Role()
			{
				Id = Lookups.Roles.HashByName[AuthConstants.Roles.MediaPartner],
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				Name = AuthConstants.Roles.MediaPartner,
				DisplayName = "Media Partner",
				Users = new List<User>()
			};

			var items = new List<Role>() { admin, developer, employee, client, agencyPartner, mediaPartner };
			return items;
		}

		public static List<User> GetUsers()
		{
			var roles = IoC.Resolve<IRoleService>();

			// create some sample users.
			var admin = new User()
			{
				Id = 1,
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				FirstName = "Admin",
				LastName = "Admin Test",
				Email = "admin@bl-test.tv",
				Password = "password123",
				Salt = "mmmm",
				IsApproved = true,
				Roles = new List<Role>() { roles.Get(1) },
				AccountInvitations = new Collection<AccountInvitation>(),
				AccountRetrievalRequests = new Collection<AccountRetrievalRequest>(),
				CampaignFavorites = new Collection<Campaign>()
			};
			var adminId = Lookups.Roles.HashByName[AuthConstants.Roles.Admin];
			roles.Get(adminId).Users.Add(admin);
			var developer = new User()
			{
				Id = 2,
				IsDeleted = false,
				DateCreated = DateTime.UtcNow,
				FirstName = "Developer",
				LastName = "Developer Test",
				Email = "developer@bl-test.tv",
				Password = "password123",
				Salt = "mmmm",
				IsApproved = true,
				Roles = new List<Role>() { roles.Get(2) },
				AccountInvitations = new Collection<AccountInvitation>(),
				AccountRetrievalRequests = new Collection<AccountRetrievalRequest>(),
				CampaignFavorites = new Collection<Campaign>()
			};
			var developerId = Lookups.Roles.HashByName[AuthConstants.Roles.Developer];
			roles.Get(developerId).Users.Add(developer);
			var employee = new User()
			{
				Id = 3,
				DateCreated = DateTime.UtcNow,
				FirstName = "Employee",
				LastName = "Employee Test",
				Email = "employee@bl-test.tv",
				Password = "password123",
				Salt = "mmmm",
				IsApproved = true,
				Roles = new List<Role>() { roles.Get(3) },
				AccountInvitations = new Collection<AccountInvitation>(),
				AccountRetrievalRequests = new Collection<AccountRetrievalRequest>(),
				CampaignFavorites = new Collection<Campaign>()
			};
			var employeeId = Lookups.Roles.HashByName[AuthConstants.Roles.Employee];
			roles.Get(employeeId).Users.Add(employee);
			var client = new User()
			{
				Id = 4,
				IsDeleted = true,
				DateCreated = DateTime.UtcNow,
				FirstName = "Client",
				LastName = "Client Test",
				Email = "client@bl-test.tv",
				Password = "password123",
				Salt = "mmmm",
				IsApproved = true,
				Roles = new List<Role>() { roles.Get(4) },
				AccountInvitations = new Collection<AccountInvitation>(),
				AccountRetrievalRequests = new Collection<AccountRetrievalRequest>(),
				CampaignFavorites = new Collection<Campaign>()
			};
			var clientId = Lookups.Roles.HashByName[AuthConstants.Roles.Client];
			roles.Get(clientId).Users.Add(client);
			var agencyPartner = new User()
			{
				Id = 5,
				IsDeleted = true,
				DateCreated = DateTime.UtcNow,
				FirstName = "Agency",
				LastName = "Partner",
				Email = "agencyPartner@bl-test.tv",
				Password = "password123",
				Salt = "mmmm",
				IsApproved = true,
				Roles = new List<Role>() { roles.Get(5) },
				AccountInvitations = new Collection<AccountInvitation>(),
				AccountRetrievalRequests = new Collection<AccountRetrievalRequest>(),
				CampaignFavorites = new Collection<Campaign>()
			};
			var agencyPartnerId = Lookups.Roles.HashByName[AuthConstants.Roles.AgencyPartner];
			roles.Get(agencyPartnerId).Users.Add(agencyPartner);

			var items = new List<User>() { admin, developer, employee, client, agencyPartner };
			return items;
		}

		public static List<AccountInvitation> GetAccountInvitations()
		{
			var settings = IoC.Resolve<ISettingsService>();

			var invitation = new AccountInvitation
				{
					InvitedUser = new User{Id = 1},
					CreatingUser = new User { Id = 2 },
					DateIssued = DateTime.UtcNow,
					DateExpired = DateTime.UtcNow.AddDays(settings.InvitationExpirationDayCount),
					Salt = "abc",
					TokenHash = "abcd",
					SecondaryToken = Guid.NewGuid()
				};

			return new List<AccountInvitation>(){invitation};
		}
		


	}

}