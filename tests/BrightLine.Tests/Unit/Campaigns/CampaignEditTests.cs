using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Service;
using BrightLine.Tests.Common;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Web.Areas.Campaigns.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Tests.Unit.Campaigns
{
	[TestFixture]
	public class CampaignEditTests
	{
		IocRegistration Container;
		private Mock<IFlashMessageExtensions> MockFlashMessageExtensions { get; set; }
		private static Mock<HttpFileCollectionBase> PostedfilesKeyCollection { get; set; }
		private Mock<HttpPostedFileBase> PostedPreviewFile { get; set; }
		private Mock<HttpContextBase> Context { get; set; }
		private Mock<HttpRequestBase> Request { get;set;}
		

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth();

			Container = MockUtilities.SetupIoCContainer(Container);

			PostedPreviewFile = new Mock<HttpPostedFileBase>();
			Context = new Mock<HttpContextBase>();
			SetupMockFiles(Context);

			var mockLogHelper = new MockHelper().GetMockLogerHelper();
			Container.Register<ILogHelper>(() => mockLogHelper.Object);
			MockFlashMessageExtensions = new Mock<IFlashMessageExtensions>();
			Container.Register<IFlashMessageExtensions>(() => MockFlashMessageExtensions.Object);
			var mockFileHelper = SetupMockFileHelper();
			Container.Register<IFileHelper>(() => mockFileHelper.Object);

			var dateHelperService = IoC.Resolve<IDateHelperService>();

			dateHelperService.CurrentDate = new DateTime(2015, 5, 1);
		}

		[Test]
		public void Campaign_Create()
        {
			var name = "Test Name";
			var googleAnalyticsIds = "123";
			var description = "Test Description";
			var mediaAgencyId = Lookups.Agencies.HashByName[AgencyConstants.AgencyNames.BBDO];
			var creativeAgencyId = Lookups.Agencies.HashByName[AgencyConstants.AgencyNames.CaratNY];
			var productId = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo];
			var salesForceId = "Sales Test";
			var campaignType = CampaignTypes.Express;

			var viewModel = GetCampaignViewModel(name, googleAnalyticsIds, description, mediaAgencyId, creativeAgencyId, productId, salesForceId, campaignType);

			var campaignsController = SetupCampaignControllerInstance();
			campaignsController.Save(viewModel);

			AssertCampaign(name, googleAnalyticsIds, description, mediaAgencyId, creativeAgencyId, productId, salesForceId, campaignType);
        }

		#region Private Methods

		private CampaignsController SetupCampaignControllerInstance()
		{
			var controller = new CampaignsController();
			controller.ControllerContext = new ControllerContext(Context.Object, new RouteData(), controller);
			return controller;
		}

		private static void AssertCampaign(string name, string googleAnalyticsIds, string description, int mediaAgencyId, int creativeAgencyId, int productId, string salesForceId, CampaignTypes campaignType)
		{
			Campaign campaignCreated = null;
			campaignCreated = GetCampaign(name, campaignCreated);
			Assert.AreEqual(campaignCreated.Name, name, "Campaign Name is not correct.");
			Assert.AreEqual(campaignCreated.GoogleAnalyticsIds, googleAnalyticsIds, "Campaign GoogleAnalyticsIds is not correct.");
			Assert.AreEqual(campaignCreated.Description, description, "Campaign Description is not correct.");
			Assert.AreEqual(campaignCreated.MediaAgency_Id, mediaAgencyId, "Campaign Media Agency is not correct.");
			Assert.AreEqual(campaignCreated.CreativeAgency_Id, creativeAgencyId, "Campaign Creative Agency is not correct.");
			Assert.AreEqual(campaignCreated.Product.Id, productId, "Campaign Product is not correct.");
			Assert.AreEqual(campaignCreated.SalesForceId, salesForceId, "Campaign SalesForceId is not correct.");
			Assert.AreEqual(campaignCreated.CampaignType, campaignType, "Campaign CampaignType is not correct.");
			Assert.AreEqual(campaignCreated.Generation, 1, "Campaign Generation is not correct.");
		}

		private static CampaignViewModel GetCampaignViewModel(string name, string googleAnalyticsIds, string description, int mediaAgencyId, int creativeAgencyId, int productId, string salesForceId, CampaignTypes campaignType)
		{
			var viewModel = new CampaignViewModel
			{
				Name = name,
				GoogleAnalyticsIds = googleAnalyticsIds,
				Description = description,
				MediaAgency = new Agency { Id = mediaAgencyId },
				CreativeAgency = new Agency { Id = creativeAgencyId },
				Product = new Product { Id = productId },
				SalesForceId = salesForceId,
				CampaignType = campaignType,
				Internal = true
			};
			return viewModel;
		}

		private static Campaign GetCampaign(string name, Campaign campaignCreated)
		{
			var campaignsRepo = IoC.Resolve<ICampaignService>();

			// I can't think of a good way yet to just get the campaign that was created without looping through all campaigns to find the created campaign 
			var campaigns = campaignsRepo.GetAll();
			foreach (Campaign campaign in campaigns)
			{
				if (campaign.Name == name)
				{
					campaignCreated = campaign;
					break;
				}
			}
			return campaignCreated;
		}

		
		private void SetupMockFiles(Mock<HttpContextBase> context)
		{
			Request = new Mock<HttpRequestBase>();

			PostedPreviewFile = new Mock<HttpPostedFileBase>();
			var postedConnectedTVCreativeFile = new Mock<HttpPostedFileBase>();
			var postedConnectedTVSupportFile = new Mock<HttpPostedFileBase>();

			SetupMockFilesKeys();
			var fakeFileKeys = new List<string>() { "PreviewFile" };

			context.Setup(ctx => ctx.Request).Returns(Request.Object);
			Request.Setup(req => req.Files).Returns(PostedfilesKeyCollection.Object);

			//if someone starts foreach'ing their way over .Files, give them the fake strings instead
			PostedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());

			SetupRequestFileForPreview();
		}

		private void SetupMockFilesKeys()
		{
			PostedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
		}


		private void SetupRequestFileForPreview()
		{
			PostedfilesKeyCollection.Setup(keys => keys["PreviewFile"]).Returns(PostedPreviewFile.Object);
			PostedPreviewFile.Setup(f => f.ContentLength).Returns(8192).Verifiable();
			PostedPreviewFile.Setup(f => f.FileName).Returns("foo.jpg").Verifiable();
			PostedPreviewFile.Setup(f => f.SaveAs(It.IsAny<string>())).AtMostOnce().Verifiable();
		}

		private Mock<IFileHelper> SetupMockFileHelper(int resourceId = 1, string resourceName = "Test Resource")
		{
			var mockFileHelper = new Mock<IFileHelper>();
			mockFileHelper.Setup(c => c.CreateFile(PostedfilesKeyCollection.Object)).Returns(new Resource { Id = resourceId, Name = resourceName });
			mockFileHelper.Setup(c => c.IsFilePresent(PostedPreviewFile.Object)).Returns(true);
			return mockFileHelper;
		}

		#endregion

	}
}
