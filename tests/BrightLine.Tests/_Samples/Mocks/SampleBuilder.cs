using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
//using Rhino.Mocks;
using BrightLine.Core;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Framework;
using BrightLine.Tests.Samples;


namespace BrightLine.Tests.Samples
{

    /// <summary>
    /// Builds up mock services and hides underlying mocking framework.
    /// </summary>
    class SampleBuilder
    {
        /// <summary>
        /// Build up a fake account service
        /// </summary>
        /// <returns></returns>
        public static IBLAccountService GetBLAccountService1()
        {
            // Mock up the interfaces
            // 1. Set up the mock services
            var mock = new Mock<IBLAccountService>();

            // 2. Setup calls to "GetAllRoles" to return specific data.
            mock.Setup(svc => svc.GetAllRoles()).Returns(new List<string>() { "admin", "moderator" });

            // 3. Access invocation arguments
            mock.Setup(svc => svc.GetFullName("johndoe")).Returns((string s) => "user : " + s.ToLower());

            // 4. More examples listed at https://code.google.com/p/moq/wiki/QuickStart

            return mock.Object;
        }


        /// <summary>
        /// Build up a fake account service
        /// </summary>
        /// <returns></returns>
        public static IRepository<BLAccount> GetAccountRepository()
        {
            // Mock up the interfaces
            // 1. Set up the mock services
            var mock = new Mock<IRepository<BLAccount>>();

            // 2. Setup calls to "GetAll" to return fake data.
            mock.Setup(repo => repo.GetAll(false)).Returns(
                new List<BLAccount>()
                { 
                    new BLAccount("acc1", true) { Id = 1},
                    new BLAccount("acc2", true) { Id = 2},
                    new BLAccount("acc3", true) { Id = 3}
                }.AsQueryable());

            // 3. Access invocation arguments
            mock.Setup(repo => repo.Get(1)).Returns(new BLAccount("acc1", true) { Id = 1 });

            // 4. More examples listed at https://code.google.com/p/moq/wiki/QuickStart
            return mock.Object;
        }


        /*
        /// <summary>
        /// Build up a fake account service
        /// </summary>
        /// <returns></returns>
        public static IAccountService GetAccountService2()
        {
            // Mock up the interfaces
            // 1. Set up the mock services
            var mock = Rhino.Mocks.MockRepository.GenerateMock<IAccountService>();

            // 2. Setup calls to "GetAllRoles" to return specific data.
            mock.Stub(svc => svc.GetAllRoles()).Return(new List<string>() { "admin", "moderator" });

            // 3. Access invocation arguments
            mock.Stub(svc => svc.GetFullName("johndoe")).Repeat.Any().Do((Func<string, string>)(s => "user : " + s.ToLower()));

            // 4. More examples listed at http://www.wrightfully.com/using-rhino-mocks-quick-guide-to-generating-mocks-and-stubs/

            return mock;
        }
        */

		internal static void BuildTestData(IRepository<BLAccount> groups, IRepository<Campaign> campaigns)
		{
			groups.Insert(new BLAccount() { Name = "BLAccount 1" });
			groups.Insert(new BLAccount() { Name = "BLAccount 2" });
			groups.Insert(new BLAccount() { Name = "BLAccount 3" });
			groups.Insert(new BLAccount() { Name = "BLAccount 4" });

			campaigns.Insert(new Campaign() { Name = "Campaign 1" });
			campaigns.Insert(new Campaign() { Name = "Campaign 2" });
			//campaigns.Insert(new Campaign() { Name = "Campaign 3" });
			//campaigns.Insert(new Campaign() { Name = "Campaign 4" });
		}
	}
}
