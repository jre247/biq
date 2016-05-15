using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using BrightLine.Tests.Common;
using BrightLine.Tests.Samples;

using BrightLine.Core;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Framework;


namespace BrightLine.Tests.Unit
{
    [TestFixture]
    public class _SampleUnitTestsWithMocksAndIoc
    {
        [SetUp]
        public void Setup()
        {
            //IocSetup.SetupDefaults();
        }


        [TearDown]
        public void TearDown()
        {
        }


        [Test]
        public void Can_Use_Ioc_With_InMemory_Repositories()
        {
            var container = new IocRegistration();
            container.Register<IRepository<Campaign>, EntityRepositoryInMemory<Campaign>>(true);
            container.Register<IRepository<BLAccount>, EntityRepositoryInMemory<BLAccount>>(true);
            container.Register<ICrudService<BLAccount>, BLAccountService>(true);
            IocSetup.Setup(container);

            var svc       = IoC.Resolve<ICrudService<BLAccount>>() as BLAccountService;
            var groups    = IoC.Resolve<IRepository<BLAccount>>();
            var campaigns = IoC.Resolve<IRepository<Campaign>>();

            // Setup Fake data for campaign groups
			SampleBuilder.BuildTestData(groups, campaigns);

            // This method internally gets repos out of the Ioc Container.
            var avg = svc.Avg_With_IOC();
            Assert.AreEqual(0, avg);
        }


        [Test]
        public void Can_Use_Ioc_With_Moq_Fake_Repository()
        {
            // Using Moq
            var container = new IocRegistration();
			container.Register<IRepository<BLAccount>>(() => SampleBuilder.GetAccountRepository());
            IocSetup.Setup(container);

            var repo = IoC.Resolve<IRepository<BLAccount>>();

            // Can call get
            var account = repo.Get(1);
            Assert.AreEqual("acc1", account.Name);
            Assert.AreEqual(1, account.Id);

            // Result: Call the GetAll
            var records = repo.GetAll();
            Assert.AreEqual(3, records.Count());
            Assert.AreEqual("acc1", records.First().Name);
        }  
    }
}
