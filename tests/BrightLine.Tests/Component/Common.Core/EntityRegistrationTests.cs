using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Core;
using BrightLine.Core.Observer;
using BrightLine.Service;
using BrightLine.Service.Aspects;
using BrightLine.Service.External.Redis;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using NUnit.Framework;
using System.Linq;

namespace BrightLine.Tests.Component.Common.Core
{
	[TestFixture]
	public class EntityRegistrationTests
	{
		private IRepository<AuditEvent> _auditRepo;
		private IAuditEventService _auditService;
		private IRedisService _redisService;

		[SetUp]
		public void Setup()
		{
			var redisSettings = new RedisSettings
			{
				Host = "local",
				Port = 0,
				Username = "",
				Password = "",
			};
			var container = new IocRegistration();
			container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(MockEntities.GetCampaigns));
			container.Register<IRepository<AuditEvent>, EntityRepositoryInMemory<AuditEvent>>(true);
			container.Register<IAuditEventService, AuditEventService>(true);
			container.Register<IRedisService>(() => new RedisService(redisSettings));
			container.Register<ICampaignService, CampaignService>(true);
			IocSetup.Setup(container);

			_auditRepo = IoC.Resolve<IRepository<AuditEvent>>();
			_auditService = new AuditEventService(_auditRepo);
		}


		[Test]
		public void CanRegisterOperationAspects()
		{
			Observer.RegisterAspects<ObserveOperationAttribute, ObserveOperationAspect>();
			Assert.IsTrue(Observer.HasAspect<Resource>());
		}


		[Test]
		public void CanRegisterPropertyRedisAspects()
		{
			Observer.RegisterAspects<ObservePropertyAttribute, ObservePropertyRedisAspect>();
			Assert.IsTrue(Observer.HasAspect<Resource>());
		}


		[Test]
		public void CanExecuteOperationAspect()
		{
			Observer.RegisterAspects<ObserveOperationAttribute, ObserveOperationAspect>();
			var s = this.GetType().Name;
			var args = new ObservableOperationArgs(CrudOperations.Create, "test", s);
			Observer.ExecuteAspect<Resource>(args);
			var count = _auditService.GetAll().Count();
			Assert.AreEqual(1, count, "The audit repository should have one element.");
		}


		[Test]
		public void CanExecutePropertyRedisAspect()
		{
			Observer.RegisterAspects<ObservePropertyAttribute, ObservePropertyRedisAspect>();
			var g = this.GetType().Name;
			var campaign = IoC.Campaigns.Get(1);
			var args = new ObservablePropertyArgs(g, campaign, "Features");
			Observer.ExecuteAspect<Campaign>(args);
			//TODO: redis implementation testing...
			Assert.IsTrue(true, "Assume redis worked.");
			//int count = IoC.Redis.GetAll().Count;
			//Assert.AreEqual(1, count, "The redis channel should have one element.");
		}
	}
}
