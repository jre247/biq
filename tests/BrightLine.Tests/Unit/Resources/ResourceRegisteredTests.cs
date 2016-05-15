using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Utility.StorageSource;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Service;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using NUnit.Framework;
using System.Linq;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class ResourceTests
	{
		IocRegistration Container;
		IResourceService Resources { get; set; }

		[SetUp]
		public void SetUp()
		{
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			Resources = IoC.Resolve<IResourceService>();
		}

		[Test]
		public void Registered_External_Resource_Exists_In_Repository()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "http://vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			var resource = Resources.Get(resourceVm.id);
			Assert.IsNotNull(resource);
		}	

		[Test]
		public void Registered_Media_Resource_Exists_In_Repository()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			var resource = Resources.Get(resourceVm.id);
			Assert.IsNotNull(resource);
		}

		[Test]
		public void Registered_External_Resource_Has_Correct_Storage_Source()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "http://vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			var resource = Resources.Get(resourceVm.id);
			Assert.IsTrue(resource.StorageSource.Id == Lookups.StorageSources.HashByName[StorageSourceConstants.StorageSourceNames.External]);
		}

		[Test]
		public void Registered_Media_Resource_Has_Correct_Storage_Source()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			var resource = Resources.Get(resourceVm.id);
			Assert.IsTrue(resource.StorageSource.Id == Lookups.StorageSources.HashByName[StorageSourceConstants.StorageSourceNames.Media]);
		}

		[Test]
		public void Registered_External_Resource_Filename_Omits_Protocol()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "http://vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			Assert.IsFalse(resourceVm.filename.Contains(ResourceConstants.PROTOCOL_HTTP));
			Assert.IsFalse(resourceVm.filename.Contains(ResourceConstants.PROTOCOL_HTTPS));
		}

		[Test]
		public void Registered_External_Resource_Filename_Has_Prepended_Slashes()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "http://vivo-video.jpeg", 1);

			var resourceVm = Resources.Register(viewModel);

			Assert.IsTrue(resourceVm.filename.ElementAt(0) == '/');
			Assert.IsTrue(resourceVm.filename.ElementAt(1) == '/');
		}


		[Test]
		public void Registered_Resource_Capitalized_Extension_Works()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123few", "http://vivo-video.PNG", 1);

			var resourceVm = Resources.Register(viewModel);
		}

		[Test]
		public void Registered_Resource_Has_Correct_Name()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123few", "vivo-video.PNG", 1);

			var resourceVm = Resources.Register(viewModel);

			Assert.AreEqual(resourceVm.name, "vivo-video");
			Assert.AreNotEqual(resourceVm.filename, "vivo-video"); //filename should have a guid tacked on to the resource's name, so the filename and name for the resource should not be the same
		}

		[Test]
		public void Registered_Media_Resource_Name_Does_Not_Have_Special_Characters()
		{
			var viewModel = MockEntities.CreateResourceViewModel("abc123fjoiwefj", "vivo-%video%20v1.png", 1);

			var resourceVm = Resources.Register(viewModel);

			var resource = Resources.Get(resourceVm.id);

			// Remove Guid and extension from resource
			//	*Note: It is assumed that the guid comes after the last underscore in the Resource's filename
			var indexOfGuid = resource.Filename.LastIndexOf("_");
			var filename = resource.Filename.Substring(0, indexOfGuid);

			Assert.AreEqual(filename, "vivo-video_v1", "Resource filename is not correct.");
		}


	}
}
