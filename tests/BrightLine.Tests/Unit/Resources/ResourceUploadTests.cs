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
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.Services.External;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class ResourceUploadTests
	{
		IocRegistration Container;
		private static Mock<HttpFileCollectionBase> PostedfilesKeyCollection { get; set; }
		private static Mock<HttpRequestBase> Request { get; set; }
		private Mock<IResourceHelper> ResourceHelper { get; set; }
		private IResourceService ResourceService { get; set; }
		private const int NewResourceId = 1;
		private const int CampaignId = 1;
		private const string Md5Hash = "3aee30c5995fe044dd18547637507ab5";
		private EntityRepositoryInMemory<Resource> ResourceRepo { get;set;}
		private Mock<HttpContextBase> Context { get;set;}
		private Stream FileStream;
		private byte[] Contents { get;set;}
		private bool isSuiteInitialized = false;

		[SetUp]
		public void SetUp()
		{
			Container = MockUtilities.SetupIoCContainer(Container);

			var mockSftpService = new Mock<ISftpService>();
			Container.Register<ISftpService>(() => mockSftpService.Object);

			var mockCloudFileService = new Mock<ICloudFileService>();
			Container.Register<ICloudFileService>(() => mockCloudFileService.Object);

			Context = new Mock<HttpContextBase>();

			SetupStreamForFile();

			//var ResourceHelper = new Mock<IResourceHelper>();
			//var stream = new Mock<MemoryStream>();
			//ResourceHelper.Setup(c => c.GenerateMd5HashForFile(stream.Object)).Returns(Md5Hash);
			//Container.Register<IResourceHelper>(() => ResourceHelper.Object);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			var resources = IoC.Resolve<IResourceService>();

			ResourceService = IoC.Resolve<IResourceService>();

			// Only set file Contents byte array once
			if (!isSuiteInitialized)
			{
				// Convert stream to byte array
				using (var reader = new BinaryReader(FileStream))
				{
					FileStream.Position = 0;
					Contents = reader.ReadBytes((int)FileStream.Length);
				}
			}
			
			isSuiteInitialized = true;
		}


		[TearDown]
		public void TearDown()
		{
			FileStream.Close();
			FileStream.Dispose();
		}

		[Test]
		[ExpectedException(typeof(ResourceUploadException))]
		public void Resource_Upload_Without_File_Throws_ResourceUploadException()
		{
			var RequestError = new Mock<HttpRequestBase>();
			ResourceService.Upload(NewResourceId, CampaignId, null);
		}


		[Test]
		public void Resource_Upload_Resource_HasMd5Hash()
		{
			SetupMockFiles(Context);

			ResourceService.Upload(NewResourceId, CampaignId, Contents);

			var resourceDb = ResourceService.Get(NewResourceId);
			Assert.IsNotNull(resourceDb.MD5Hash);
			Assert.AreEqual(resourceDb.MD5Hash, Md5Hash);
		}

		[Test]
		public void Resource_Upload_Resource_Has_Bitrate()
		{
			var bitrateCompare = 10;

			SetupMockFiles(Context);

			var resourceId = 100;
			var filename = "apple.mp4";
			var fileTypeMp4Id = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];
			var fileTypeMp4Name = Lookups.FileTypes.HashById[fileTypeMp4Id];
			var resourceTypeHdVideoId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo];
			var resourceTypeHdVideoName = Lookups.ResourceTypes.HashById[resourceTypeHdVideoId];
			var creativeId = 1;
			var campaignId = 1;
			var resourceName = "apple";
			var size = 1000000;
			var duration = 100;
			var width = 300;
			var height = 200;

			var resource = MockEntities.CreateResource(resourceId, filename, fileTypeMp4Id, fileTypeMp4Name, resourceTypeHdVideoId, resourceTypeHdVideoName, duration, size, width, height, creativeId, campaignId, resourceName);
			ResourceService.Create(resource);

			ResourceService.Upload(resourceId, CampaignId, Contents);

			var resourceDb = ResourceService.Get(resourceId);
			Assert.AreEqual(resourceDb.Bitrate, bitrateCompare);
		}

		[Test]
		public void Resource_Upload_Resource_Has_IsUploaded_Set_To_True_()
		{
			SetupMockFiles(Context);

			ResourceService.Upload(NewResourceId, CampaignId, Contents);

			var resourceDb = ResourceService.Get(NewResourceId);
			Assert.IsTrue(resourceDb.IsUploaded);
		}

		[Test]
		public void Resource_Upload_Uploaded_Media_Resource_Does_Not_Reference_Parent_Resource()
		{
			SetupMockFiles(Context);

			ResourceService.Upload(NewResourceId, CampaignId, Contents);

			var resourceDb = ResourceService.Get(NewResourceId);
			Assert.IsNull(resourceDb.Parent);
		}

		[Test]
		public void Resource_Upload_Uploaded_Media_Resource_References_Parent_Resource()
		{
			var resourceCompareId = 2;
			SetupMockFiles(Context);

			ResourceService.Upload(resourceCompareId, CampaignId, Contents);
			ResourceService.Upload(NewResourceId, CampaignId, Contents);

			var resourceDb = ResourceService.Get(NewResourceId);
			Assert.IsNotNull(resourceDb.Parent_Id);
			Assert.AreEqual(resourceDb.Parent_Id, resourceCompareId);
		}

		private void SetupMockFiles(Mock<HttpContextBase> context)
		{
			//We'll need mocks (fake) of Context, Request and a fake PostedFile
			Request = new Mock<HttpRequestBase>();

			var postedPreviewFile = new Mock<HttpPostedFileBase>();

			//mock fake data for when Request.File is called
			SetupMockFilesKeys();
			var fakeFileKeys = new List<string>() { "UploadedResource" };

			context.Setup(ctx => ctx.Request).Returns(Request.Object);

			// return the Mock with fake keys when someone asks for Files
			Request.Setup(req => req.Files).Returns(PostedfilesKeyCollection.Object);

			//if someone starts foreach'ing their way over .Files, give them the fake strings instead
			PostedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());

			//if someone asks for file then give them the fake
			PostedfilesKeyCollection.Setup(keys => keys["UploadedResource"]).Returns(postedPreviewFile.Object);

			postedPreviewFile.Setup(x => x.InputStream).Returns(FileStream);
			postedPreviewFile.Setup(f => f.ContentLength).Returns(8192).Verifiable();
			postedPreviewFile.Setup(f => f.FileName).Returns("foo.png").Verifiable();

			//Make sure someone only calls SaveAs only once
			postedPreviewFile.Setup(f => f.SaveAs(It.IsAny<string>())).AtMostOnce().Verifiable();
		}

		private void SetupStreamForFile()
		{
			if (FileStream != null)
				return;

			FileStream = ResourceLoader.BuildStream("CMS.testResource.png");
		}

		private static void SetupMockFilesKeys()
		{
			PostedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
		}


		
	}
}
