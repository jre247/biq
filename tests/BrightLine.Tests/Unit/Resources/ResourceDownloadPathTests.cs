using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Unit.Resources
{
	[TestFixture]
	public class ResourceDownloadPathTests
	{
		IocRegistration Container;
		private const string RootDownloadPath = "//cdn-local-m.brightline.tv/campaigns";
		[SetUp]
		public void SetUp()
		{
			MockUtilities.SetupAuth();

			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);
		}

		[Test]
		public void ResourceHelper_Image_Resource_Download_Path_Is_Correct()
		{
			var resourceId = 1;
			var resourceName = "test1.txt";
			var campaignId = 20198;
			var resourceTypeId = 2;
			var mediaServerResourceDirectory = ResourceConstants.MediaResourceTypes.Images;
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			var resourceDownloadPath = resourceHelper.GetResourceDownloadPath(resourceId, resourceName, resourceTypeId, campaignId);

			var expectedResourceDownloadPath = string.Format("{0}/{1}/{2}/{3}", RootDownloadPath, campaignId, mediaServerResourceDirectory, resourceName);
			Assert.AreEqual(resourceDownloadPath, expectedResourceDownloadPath);
		}

		[Test]
		public void ResourceHelper_Video_Resource_Download_Path_Is_Correct()
		{
			var resourceId = 1;
			var resourceName = "test1.txt";
			var campaignId = 20198;
			var resourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo];
			var mediaServerResourceDirectory = ResourceConstants.MediaResourceTypes.Videos;
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			var resourceDownloadPath = resourceHelper.GetResourceDownloadPath(resourceId, resourceName, resourceTypeId, campaignId);

			var expectedResourceDownloadPath = string.Format("{0}/{1}/{2}/{3}", RootDownloadPath, campaignId, mediaServerResourceDirectory, resourceName);
			Assert.AreEqual(resourceDownloadPath, expectedResourceDownloadPath);
		}

		[Test]
		public void ResourceHelper_Campaign_Resource_Download_Path_Is_Correct()
		{
			var resourceName = "test1.txt";
			var campaignId = 20198;
			var resourceTypeId = 2;
			var resourceTypes = IoC.Resolve<IRepository<ResourceType>>();
			var mediaServerResourceDirectory = ResourceConstants.MediaResourceTypes.Images;

			var campaign = new Campaign
			{
				Id = campaignId, 
				Thumbnail = new Resource{
					Id = 2,
					ResourceType = resourceTypes.Get(resourceTypeId),
					Filename = resourceName
				}
			};

			var viewModel = CampaignViewModel.FromCampaign(campaign);

			var resourceDownloadPath = viewModel.ResourceDownloadUrl;
			var expectedResourceDownloadPath = string.Format("{0}/{1}/{2}/{3}", RootDownloadPath, campaignId, mediaServerResourceDirectory, resourceName);
			Assert.AreEqual(resourceDownloadPath, expectedResourceDownloadPath);
		}

		[Test]
		public void ResourceHelper_PromotionalCreative_Resource_Download_Path_Is_Correct()
		{
			var resourceName = "test1.txt";
			var campaignId = 20198;
			var mediaServerResourceDirectory = ResourceConstants.MediaResourceTypes.Images;
			var resourceTypeId = 2;
			var creative = MockEntities.BuildCreative(1, "abc", false, campaignId, "1", resourceTypeId, resourceName);

			var viewModel = PromotionalCreativeViewModel.FromCreative(creative);

			var resourceDownloadPath = viewModel.Resources[0].url;
			var expectedResourceDownloadPath = string.Format("{0}/{1}/{2}/{3}", RootDownloadPath, campaignId, mediaServerResourceDirectory, resourceName);
			Assert.AreEqual(resourceDownloadPath, expectedResourceDownloadPath);
		}

		[Test]
		public void ResourceHelper_DestinationCreative_Resource_Download_Path_Is_Correct()
		{
			var resources = IoC.Resolve<IResourceService>();

			var mediaServerResourceDirectory = ResourceConstants.MediaResourceTypes.Images;
			var resourceTypeId = 2;
			var resourceId = 1;
			var resource = resources.Get(resourceId);
			var resourceName = resource.Filename;
			var campaignId = resource.Creative.Campaign.Id;
			var creative = MockEntities.BuildCreative(1, "abc", false, campaignId, "1", resourceTypeId, resourceName, resourceId);

			var viewModel = DestinationCreativeViewModel.FromCreative(creative);

			var resourceDownloadPath = viewModel.resource.url;
			var expectedResourceDownloadPath = string.Format("{0}/{1}/{2}/{3}", RootDownloadPath, campaignId, mediaServerResourceDirectory, resourceName);
			Assert.AreEqual(resourceDownloadPath, expectedResourceDownloadPath);
		}

	
	}
}
