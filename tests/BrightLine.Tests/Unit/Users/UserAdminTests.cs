using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Service;
using BrightLine.Tests.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Core;
using BrightLine.Common.ViewModels.Users;
using BrightLine.Common.Core;
using Newtonsoft.Json;
using BrightLine.Common.Utility.Authentication;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class UserAdminTests
	{
		IocRegistration _container;
		private string Email {get;set;}
		private int ClientRoleId { get;set;}
		private int EmployeeRoleId { get; set; }
		private int AdminRoleId { get; set; }
		private int MediaPartnerRoleId { get;set;}
		IUserService Users { get;set;}

		[SetUp]
		public void SetUp()
		{
			_container = MockUtilities.SetupIoCContainer(_container);

			var mockAccountService = new Mock<IAccountService>();
			_container.Register<IAccountService>(() => mockAccountService.Object);

			Users = IoC.Resolve<IUserService>();

			Email = string.Format("{0}@brightline.tv", Guid.NewGuid());
			ClientRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.Client];
			EmployeeRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.Employee];
			AdminRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.Admin];
			MediaPartnerRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.MediaPartner];
		}

		[Test]
		public void UserAdmin_Employee_Role_Is_Selected()
		{
			var userId = 0;
			var adminRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.Admin];
			var employeeRoleId = Lookups.Roles.HashByName[AuthConstants.Roles.Employee];
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = adminRoleId,
					Name = AuthConstants.Roles.Admin
				},
				new EntityLookup
				{
					Id = employeeRoleId,
					Name = AuthConstants.Roles.Employee
				}
			};
		
			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);

			Assert.AreEqual(user.Roles.Count(), 2, "User Roles count is not correct.");
			Assert.IsTrue(user.Roles.Any(r => r.Id == adminRoleId), "Admin Role does not exist for user.");
			Assert.IsTrue(user.Roles.Any(r => r.Id == employeeRoleId), "Employee Role does not exist for user.");
		}

		[Test]
		public void UserAdmin_Employee_Role_Is_Not_Selected()
		{
			var userId = 0;
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = ClientRoleId,
					Name = AuthConstants.Roles.Client
				}
			};
				
			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);

			Assert.AreEqual(user.Roles.Count(), 1, "User Roles count is not correct.");
			Assert.IsTrue(user.Roles.Any(r => r.Id == ClientRoleId), "Client Role does not exist for user.");
			Assert.IsFalse(user.Roles.Any(r => r.Id == EmployeeRoleId), "Employee Role should not exist for user.");
		}
		
		[Test]
		[ExpectedException(typeof(ValidationException))]
		public void UserAdmin_External_And_Employee_Roles_Selected_Throws_Exception()
		{
			var userId = 0;
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = ClientRoleId,
					Name = AuthConstants.Roles.Client
				},
				new EntityLookup
				{
					Id = EmployeeRoleId,
					Name = AuthConstants.Roles.Employee
				}
			};
			
			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);
		}

		[Test]
		[ExpectedException(typeof(ValidationException))]
		public void UserAdmin_Internal_And_External_Roles_Selected_Throws_Exception()
		{
			var userId = 0;
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = ClientRoleId,
					Name = AuthConstants.Roles.Client
				},
				new EntityLookup
				{
					Id = AdminRoleId,
					Name = AuthConstants.Roles.Admin
				}
			};
			
			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);
		}

		[Test]
		public void UserAdmin_MediaPartner_Role_Is_Selected()
		{
			var userId = 0;
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = MediaPartnerRoleId,
					Name = AuthConstants.Roles.MediaPartner
				}
			};

			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);

			Assert.AreEqual(user.Roles.Count(), 1, "User Roles count is not correct.");
			Assert.IsTrue(user.Roles.Any(r => r.Id == MediaPartnerRoleId), "Media Partner Role does not exist for user.");
		}

		[Test]
		public void UserAdmin_MediaPartner_Role_Is_Not_Selected()
		{
			var userId = 0;
			var userViewModel = BuildUserViewModel(userId, Email);
			userViewModel.Roles = new List<EntityLookup>
			{
				new EntityLookup
				{
					Id = ClientRoleId,
					Name = AuthConstants.Roles.Client
				}
			};

			var jsonData = JsonConvert.SerializeObject(userViewModel);

			var user = Users.SaveUser(userId, jsonData);

			Assert.AreEqual(user.Roles.Count(), 1, "User Roles count is not correct.");
			Assert.IsFalse(user.Roles.Any(r => r.Id == MediaPartnerRoleId), "Media Partner Role should not exist for user.");
		}

		private SaveUserViewModel BuildUserViewModel(int userId, string email, string firstName = "Bob", string lastName = "Evans")
		{
			return new SaveUserViewModel
			{
				Id = userId,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				TimeZoneId = "Eastern Standard Time",
				Advertiser = 1
			};
		}

	}
}
