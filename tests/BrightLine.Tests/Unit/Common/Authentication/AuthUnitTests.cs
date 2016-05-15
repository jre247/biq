using BrightLine.Common.Models;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Tests.Common;
using NUnit.Framework;


namespace BrightLine.Tests.Unit.Common.Authentication
{
	[TestFixture]
	public class AuthUnitTests
	{
		[SetUp]
		public void Setup()
		{
			Auth.Init(new AuthForUnitTests(true, 1, "user1", AuthConstants.Roles.Admin, (name) => new User() { Display = "user1", TimeZoneId = "Eastern Standard Time" }));
		}


		[Test]
		public void IsAdmin()
		{
			//Auth.Init(new AuthForUnitTests(true, 1, "user3", "moderator", (name) => new User() { Display = "user1", TimeZoneId = "Eastern Standard Time" }));
			//Assert.IsTrue(Auth.Service.IsAdmin());
		}


		[Test]
		public void IsUserInRole()
		{
			Assert.IsTrue(Auth.IsUserInRole(AuthConstants.Roles.Admin));
		}


		[Test]
		public void CanUseExtensionMethodsForRoleChecks()
		{
			Auth.Init(new AuthForUnitTests(true, 1, "user3", AuthConstants.Roles.AgencyPartner, (name) => new User() { Display = "user1", TimeZoneId = "Eastern Standard Time" }));
			Auth.Service.IsAgencyPartner();
		}
	}
}
