using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Tests.Common;
using NUnit.Framework;

using BrightLine.Tests.Samples;

using BrightLine.Core;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Framework;


namespace BrightLine.Tests.Unit
{
    [TestFixture]
    public class _SampleUnitTestsWithMocks
    {

        [Test]
        public void Can_Use_Moq_With_Fake_Service()
        {
            // Using Moq
			IBLAccountService svc = SampleBuilder.GetBLAccountService1();

            // Result: Call the Get 
            var roles = svc.GetAllRoles();
            Assert.AreEqual("admin", roles[0]);
            Assert.AreEqual("moderator", roles[1]);

            var name = svc.GetFullName("johndoe");
            Assert.AreEqual("user : johndoe", name);
        }


        [Test]
        public void Can_Use_Moq_With_Fake_Repository()
        {
            // Using Moq
			IRepository<BLAccount> repo = SampleBuilder.GetAccountRepository();

            // Can call get
            var account = repo.Get(1);
            Assert.AreEqual("acc1", account.Name);
            Assert.AreEqual(1, account.Id);

            // Result: Call the GetAll
            var records = repo.GetAll();
            Assert.AreEqual(3, records.Count());
            Assert.AreEqual("acc1", records.First().Name);
        }


        /*
        [Test]
        public void Can_Use_RhinoMocks_With_Fake_Service()
        {
            // Using rhino mocks
            IAccountService svc = MockAccountBuilder.GetAccountService2();

            // Result: Call the Get 
            var roles = svc.GetAllRoles();
            Assert.AreEqual("admin", roles[0]);
            Assert.AreEqual("moderator", roles[1]);

            var name = svc.GetFullName("johndoe");
            Assert.AreEqual("user : johndoe", name);
        } 
        */


        [Test]
        public void Repository_Example_1_InMemory_For_Unit_Tests()
        {
            // Using Moq
            IRepository<BLAccount> repo = new EntityRepositoryInMemory<BLAccount>();
            repo.Insert(new BLAccount("acc1", true) );
            repo.Insert(new BLAccount("acc2", false));
            repo.Insert(new BLAccount("acc3", true) );

            // Can call get
            var account = repo.Get(1);
            Assert.AreEqual("acc1", account.Name);
            Assert.AreEqual(1, account.Id);

            // Result: Call the GetAll
            var records = repo.GetAll();
            Assert.AreEqual(3, records.Count());
            Assert.AreEqual("acc1", records.First().Name);
        }


        [Test]
        public void Repository_Example_2_Service_With_Multiple_Repos_Without_IOC()
        {
            // Example 1: Manually setup the repositorys
            IRepository<BLAccount> accounts = new EntityRepositoryInMemory<BLAccount>();
            IRepository<Campaign> campaigns = new EntityRepositoryInMemory<Campaign>();

            // Setup Fake data for campaign groups
			SampleBuilder.BuildTestData(accounts, campaigns);

            // Pass the service the repositories.
            var svc = new BLAccountService();
            svc.SetupRepo(accounts);
            svc.CampaignRepo = campaigns;
            var avg = svc.Avg_Without_IOC();
            Assert.AreEqual(0, avg);
        }
    }
}
