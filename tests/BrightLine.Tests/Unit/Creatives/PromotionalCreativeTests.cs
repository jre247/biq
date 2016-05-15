using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BrightLine.Tests.Common;
using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.CMS.Services;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using BrightLine.Service;
using BrightLine.Common.Services;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Utility;
using BrightLine.Common.Framework;
namespace BrightLine.Tests.Unit.Creatives
{
	[TestFixture]
	public class PromotionalCreativeTests
	{
		IocRegistration Container;
		int CreativesCount;
		int CreativeId;
		ICreativeService Creatives { get;set;}
		IResourceService Resources { get; set; }

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupIoCContainer(Container);

			Creatives = IoC.Resolve<ICreativeService>();
			Resources = IoC.Resolve<IResourceService>();

			CreativesCount = Creatives.GetAll().Count();
			CreativeId = CreativesCount + 1;
		}

		[Test(Description = "Promotional Creative save has correct creatives count.")]
		public void Promotional_Creative_Has_Correct_Creatives_Count()
		{
			var creative = MockEntities.BuildCreative(0, "test1", true, 10, "1,2,3");

			var newCreative = Creatives.Create(creative);

			Assert.IsTrue(Creatives.GetAll().Count() == CreativesCount + 1, "Creatives count is not correct.");
		}

		[Test(Description = "Promotional Creative save assigns CreativeId to appropriate resources.")]
		public void Promotional_Creative_Assigns_Creative_To_New_Resources()
		{
			var creative = MockEntities.BuildCreative(0, "test1", true, 10, "3,4");

			var newCreative = Creatives.Create(creative);

			var resource1 = Resources.Get(3);
			var resource2 = Resources.Get(4);
			Assert.IsTrue(resource1.Creative.Id == CreativeId, "Resource 1 not assigned correct creative.");
			Assert.IsTrue(resource2.Creative.Id == CreativeId, "Resource 2 not assigned correct creative.");
		}

		[Test(Description = "Promotional Creative resource initializes to activated.")]
		public void Promotional_Creative_Resource_Initializes_To_Activated()
		{
			var creative = MockEntities.BuildCreative(CreativeId, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = CreativeId } };
			Resources.Create(resource);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == false, "Creative Resource is not activated.");
		}

		[Test(Description = "Promotional Creative deactivates old resources for Create.")]
		public void Promotional_Creative_Deactivates_Old_Resources_For_Create()
		{
			var creative = MockEntities.BuildCreative(0, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = CreativeId } };
			Resources.Create(resource);

			var newCreative = Creatives.Create(creative);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == true, "Creative Resource is not deactivated.");
		}

		[Test(Description = "Promotional Creative deactivates old resources for Update.")]
		public void Promotional_Creative_Deactivates_Old_Resources_For_Update()
		{
			var creative = MockEntities.BuildCreative(4, "test1", true, 10, "3,4");
			var resource = new Resource { Id = 10, IsDeleted = false, Creative = new Creative { Id = 4 } };
			Resources.Create(resource);

			var newCreative = Creatives.Update(creative);

			var resourceDeactivated = Resources.Get(10);
			Assert.IsTrue(resourceDeactivated.IsDeleted == true, "Creative Resource is not deactivated.");
		}
	}
}
