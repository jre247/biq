using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.Blueprints;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using BrightLine.Service.BlueprintImport;
using BrightLine.Tests.Common;
using BrightLine.Utility;
using BrightLine.Web.Controllers;
using Common.Logging;
using Moq;
using NUnit.Framework;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Tests.Unit.Cms.Blueprints
{
	[TestFixture]
	public class BlueprintCreateEditTests
	{
		IocRegistration _container;
		private static Mock<HttpFileCollectionBase> postedfilesKeyCollection { get; set; }
		private Mock<HttpPostedFileBase> PostedPreviewFile { get; set; }
		private Mock<HttpContextBase> Context { get; set; }
		private Mock<IFlashMessageExtensions> _mockFlashMessageExtensions { get; set; }
		private string _blueprintTestRepo = "blueprint-test";
		private static Mock<HttpRequestBase> request { get; set; }
		private static Mock<HttpRequestBase> requestForBlueprintCreate { get; set; }
		private IBlueprintService BlueprintService { get; set; }
		private IBlueprintImportService BlueprintImportService { get; set; }

		[SetUp]
		public void Setup()
		{
			_container = MockUtilities.SetupIoCContainer(_container);

			_container.Register<IBlueprintImportLookupsService, BlueprintImportLookupsService>();

			Context = new Mock<HttpContextBase>();
			SetupMockFiles(Context);

			var mockLogHelper = GetMockLogerHelper();
			_container.Register<ILogHelper>(() => mockLogHelper.Object);
			_mockFlashMessageExtensions = new Mock<IFlashMessageExtensions>();
			_container.Register<IFlashMessageExtensions>(() => _mockFlashMessageExtensions.Object);
			var mockFileHelper = SetupMockFileHelper();
			_container.Register<IFileHelper>(() => mockFileHelper.Object);

			var blueprintHelper = new Mock<IBlueprintHelper>();
			_container.Register<IBlueprintHelper>(() => blueprintHelper.Object);
			
			var mockOltpContextHelper = new Mock<IOLTPContextHelper>();
			_container.Register<IOLTPContextHelper>(() => mockOltpContextHelper.Object);

			var blueprints = IoC.Resolve<IBlueprintService>();

			var blueprintsRepo = blueprints.ConvertToInMemoryRepository<Blueprint, ICrudService<Blueprint>>();
			BlueprintService = new BlueprintService(blueprintsRepo);	
		}

		[Test(Description = "Test creating blueprint with files works.")]
		public void Blueprint_Create_With_Files()
		{
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = new BlueprintViewModel
			{
				SelectedFeatureType = new EntityLookup
				{
					Id = 1,
					Name = "Test Feature Type"
				},
				ManifestName = "blueprint-test",
				Name = "Test 1"
			};

			var result = blueprintsController.Save(vm);
			result.Wait();
			var actionResult = result.Result;

			Assert.IsNotNull(actionResult);
		}

		[Test(Description = "Test editing blueprint without Id.")]
		public void Blueprint_Edit_With_Invalid_Id()
		{
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = new BlueprintViewModel
			{
				Id = 10000,
				ManifestName = _blueprintTestRepo
			};

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as ViewResult;
			var errorMessage = viewResult.ViewData.ModelState.ElementAt(0).Value.Errors.ElementAt(0).ErrorMessage;

			Assert.AreEqual("Blueprint does not exist.", errorMessage);
		}

		[Test(Description = "Test editing blueprint without Preview Image.")]
		public void Blueprint_Edit_Without_Preview_Image()
		{
			var blueprintsController = SetupBlueprintsControllerInstanceForBlueprintCreate();
			var vm = new BlueprintViewModel
			{
				ManifestName = _blueprintTestRepo
			};

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as ViewResult;
			var errorMessage = viewResult.ViewData.ModelState.ElementAt(0).Value.Errors.ElementAt(0).ErrorMessage;

			Assert.AreEqual("Preview Image is required.", errorMessage);
		}

		[Test(Description = "Test creating blueprint without ManifestName causes BlueprintImportException exception.")]
		public void Blueprint_Create_Without_ManifestName()
		{
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = new BlueprintViewModel();

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as ViewResult;
			var errorMessage = viewResult.ViewData.ModelState.ElementAt(0).Value.Errors.ElementAt(0).ErrorMessage;

			Assert.AreEqual("There was an unexpected error that occured during the Blueprint import process.", errorMessage);
		}

		//[Test(Description = "Test creating blueprint with incorrect ManifestName causes NotFoundException exception.")]
		//public void Blueprint_Create_With_Incorrect_ManifestName()
		//{
		//	var blueprintsController = SetupBlueprintsControllerInstance();
		//	var vm = new BlueprintViewModel
		//	{
		//		ManifestName = "blah123"
		//	};

		//	var result = blueprintsController.Save(vm);
		//	result.Wait();
		//	var viewResult = result.Result as ViewResult;
		//	var errorMessage = viewResult.ViewData.ModelState.ElementAt(0).Value.Errors.ElementAt(0).ErrorMessage;

		//	Assert.IsNotNull("Repository does not exist.", errorMessage);
		//}

		[Test(Description = "Test creating blueprint with errors has correct model data.")]
		public void Blueprint_Create_With_Errors_Has_Correct_Model_Data()
		{
			var manifestName = "blah123";
			var name = "test1";
			var featureTypeName = "Test Feature Type";
			var featureTypeGroupName = "Test Feature Type";
			var featureTypeId = 1;
			var featureTypeGroupId = 1;
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = GetBlueprintViewModel(name, featureTypeName, featureTypeGroupName, featureTypeId, featureTypeGroupId, manifestName);

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as ViewResult;
			var model = viewResult.Model as BlueprintViewModel;

			Assert.AreEqual(model.Id, 0, "Model Id is incorrect.");
			Assert.AreEqual(model.ManifestName, manifestName, "Model ManifestName is incorrect.");
			Assert.AreEqual(model.Name, name, "Model Name is incorrect.");
			Assert.AreEqual(model.SelectedFeatureTypeGroup.Id, featureTypeGroupId, "Model SelectedFeatureTypeGroup Id is incorrect.");
			Assert.AreEqual(model.SelectedFeatureTypeGroup.Name, featureTypeGroupName, "Model SelectedFeatureTypeGroup Name is incorrect.");
			Assert.AreEqual(model.SelectedFeatureType.Id, featureTypeId, "Model SelectedFeatureType Id is incorrect.");
			Assert.AreEqual(model.SelectedFeatureType.Name, featureTypeName, "Model SelectedFeatureType Name is incorrect.");
			Assert.AreEqual(model.FeatureTypes.Count(), 2, "Model FeatureTypes Count is incorrect.");
			Assert.AreEqual(model.FeatureTypeGroups.Count(), 2, "Model FeatureTypeGroups Count is incorrect.");
		}

		[Test(Description = "Test creating blueprint redirects.")]
		public void Blueprint_Create_Correctly_Redirects()
		{
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = new BlueprintViewModel
			{
				Id = 1,
				ManifestName = _blueprintTestRepo
			};

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as RedirectToRouteResult;
			var view = viewResult.RouteValues["action"];
			var blueprintId = (int)viewResult.RouteValues["id"];

			Assert.AreEqual(view, "Edit");
			Assert.AreEqual(blueprintId, 1);
		}

		[Test(Description = "Test creat returns correct blueprint.")]
		public void Blueprint_Create_Returns_Correct_Blueprint()
		{
			var blueprints = IoC.Resolve<IBlueprintService>();

			var name = "test1";
			var featureTypeName = "Test Feature Type";
			var featureTypeGroupName = "Test Feature Type";
			var featureTypeId = 1;
			var featureTypeGroupId = 1;
			var blueprintId = blueprints.GetAll().Count() + 1;
			var mockFileHelper = SetupMockFileHelper();
			var vm = GetBlueprintViewModel(name, featureTypeName, featureTypeGroupName, featureTypeId, featureTypeGroupId);

			var result = BlueprintService.Save(vm, request.Object);
			result.Wait();

			var blueprint = result.Result as Blueprint;
			Assert.AreEqual(blueprint.Id, blueprintId, "Blueprint Id is incorrect.");
			Assert.AreEqual(blueprint.ManifestName, _blueprintTestRepo, "Blueprint ManifestName is incorrect.");
			Assert.AreEqual(blueprint.MinorVersion, 0, "Blueprint MinorVersion is incorrect.");
			Assert.AreEqual(blueprint.MajorVersion, 1, "Blueprint MajorVersion is incorrect.");
			Assert.AreEqual(blueprint.Patch, 0, "Blueprint Patch is incorrect.");
			Assert.AreEqual(blueprint.FeatureType.Id, featureTypeId, "Blueprint FeatureType is incorrect.");
		}

		[Test(Description = "Test editing blueprint with validation error still returns the correct viewmodel.")]
		public void Blueprint_Edit_With_Error_Returns_Correct_Viewmodel()
		{
			var name = "test1";
			var featureTypeName = "Test Feature Type";
			var featureTypeGroupName = "Test Feature Type";
			var featureTypeId = 1;
			var featureTypeGroupId = 1;
			var blueprintId = 123432; //use incorrect Blueprint Id
			var mockFileHelper = SetupMockFileHelper();
			var blueprintsController = SetupBlueprintsControllerInstance();
			var vm = GetBlueprintViewModel(name, featureTypeName, featureTypeGroupName, featureTypeId, featureTypeGroupId, _blueprintTestRepo, blueprintId);

			var result = blueprintsController.Save(vm);
			result.Wait();
			var viewResult = result.Result as ViewResult;
			var model = viewResult.Model as BlueprintViewModel;

			Assert.AreEqual(model.ManifestName, _blueprintTestRepo, "Model ManifestName is incorrect.");
			Assert.AreEqual(model.Name, name, "Model Name is incorrect.");
			Assert.AreEqual(model.SelectedFeatureTypeGroup.Id, featureTypeGroupId, "Model SelectedFeatureTypeGroup Id is incorrect.");
			Assert.AreEqual(model.SelectedFeatureTypeGroup.Name, featureTypeGroupName, "Model SelectedFeatureTypeGroup Name is incorrect.");
			Assert.AreEqual(model.SelectedFeatureType.Id, featureTypeId, "Model SelectedFeatureType Id is incorrect.");
			Assert.AreEqual(model.SelectedFeatureType.Name, featureTypeName, "Model SelectedFeatureType Name is incorrect.");
			Assert.AreEqual(model.FeatureTypes.Count(), 2, "Model FeatureTypes Count is incorrect.");
			Assert.AreEqual(model.FeatureTypeGroups.Count(), 2, "Model FeatureTypeGroups Count is incorrect.");
		}



		#region Private Methods

		private BlueprintsController SetupBlueprintsControllerInstance()
		{
			var blueprintsController = new BlueprintsController();
			blueprintsController.ControllerContext = new ControllerContext(Context.Object, new RouteData(), blueprintsController);
			return blueprintsController;
		}

		private BlueprintsController SetupBlueprintsControllerInstanceForBlueprintCreate()
		{
			SetupMockFilesForBlueprintCreate(Context);			

			var blueprintsController = new BlueprintsController();
			blueprintsController.ControllerContext = new ControllerContext(Context.Object, new RouteData(), blueprintsController);
			return blueprintsController;
		}

		private Mock<ILogHelper> GetMockLogerHelper()
		{
			var mockLogHelper = new Mock<ILogHelper>();
			var mockLogger = new Mock<ILog>();
			mockLogHelper.Setup(c => c.GetLogger()).Returns(mockLogger.Object);
			return mockLogHelper;
		}

		private void SetupMockFiles(Mock<HttpContextBase> context)
		{
			request = new Mock<HttpRequestBase>();

			PostedPreviewFile = new Mock<HttpPostedFileBase>();
			var postedConnectedTVCreativeFile = new Mock<HttpPostedFileBase>();
			var postedConnectedTVSupportFile = new Mock<HttpPostedFileBase>();

			SetupMockFilesKeys();
			var fakeFileKeys = new List<string>() { "PreviewFile", "ConnectedTVCreativeFile", "ConnectedTVSupportFile" };

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);

			//if someone starts foreach'ing their way over .Files, give them the fake strings instead
			postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());

			SetupRequestFileForPreview();

			SetupRequestFileForConnectedTVSupport(postedConnectedTVSupportFile);

			SetupRequestFileForConnectedTVCreative(postedConnectedTVCreativeFile);
		}


		private void SetupMockFilesForBlueprintCreate(Mock<HttpContextBase> context)
		{
			requestForBlueprintCreate = new Mock<HttpRequestBase>();

			var postedConnectedTVCreativeFile = new Mock<HttpPostedFileBase>();
			var postedConnectedTVSupportFile = new Mock<HttpPostedFileBase>();

			//Someone is going to ask for Request.File and we'll need a mock (fake) of that.
			SetupMockFilesKeys();
			var fakeFileKeys = new List<string>() { "ConnectedTVCreativeFile", "ConnectedTVSupportFile" };

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			request.Setup(req => req.Files).Returns(postedfilesKeyCollection.Object);

			//if someone starts foreach'ing their way over .Files, give them the fake strings instead
			postedfilesKeyCollection.Setup(keys => keys.GetEnumerator()).Returns(fakeFileKeys.GetEnumerator());

			SetupRequestFileForConnectedTVCreative(postedConnectedTVCreativeFile);

			SetupRequestFileForConnectedTVSupport(postedConnectedTVSupportFile);
		}


		private void SetupRequestFileForPreview()
		{
			postedfilesKeyCollection.Setup(keys => keys["PreviewFile"]).Returns(PostedPreviewFile.Object);
			PostedPreviewFile.Setup(f => f.ContentLength).Returns(8192).Verifiable();
			PostedPreviewFile.Setup(f => f.FileName).Returns("foo.doc").Verifiable();
			PostedPreviewFile.Setup(f => f.SaveAs(It.IsAny<string>())).AtMostOnce().Verifiable();
		}


		private void SetupRequestFileForConnectedTVSupport(Mock<HttpPostedFileBase> postedConnectedTVSupportFile)
		{
			postedfilesKeyCollection.Setup(keys => keys["ConnectedTVSupportFile"]).Returns(postedConnectedTVSupportFile.Object);
			postedConnectedTVSupportFile.Setup(f => f.ContentLength).Returns(8192).Verifiable();
			postedConnectedTVSupportFile.Setup(f => f.FileName).Returns("foo.doc").Verifiable();
			postedConnectedTVSupportFile.Setup(f => f.SaveAs(It.IsAny<string>())).AtMostOnce().Verifiable();
		}

		private void SetupRequestFileForConnectedTVCreative(Mock<HttpPostedFileBase> postedConnectedTVCreativeFile)
		{
			postedfilesKeyCollection.Setup(keys => keys["ConnectedTVCreativeFile"]).Returns(postedConnectedTVCreativeFile.Object);
			postedConnectedTVCreativeFile.Setup(f => f.ContentLength).Returns(8192).Verifiable();
			postedConnectedTVCreativeFile.Setup(f => f.FileName).Returns("foo.doc").Verifiable();
			postedConnectedTVCreativeFile.Setup(f => f.SaveAs(It.IsAny<string>())).AtMostOnce().Verifiable();
		}

		private void SetupMockFilesKeys()
		{
			postedfilesKeyCollection = new Mock<HttpFileCollectionBase>();
		}


		private Mock<IFileHelper> SetupMockFileHelper(int resourceId = 1, string resourceName = "Test Resource")
		{
			var mockFileHelper = new Mock<IFileHelper>();
			mockFileHelper.Setup(c => c.CreateFile(postedfilesKeyCollection.Object)).Returns(new Resource { Id = resourceId, Name = resourceName });
			mockFileHelper.Setup(c => c.IsFilePresent(PostedPreviewFile.Object)).Returns(true);
			return mockFileHelper;
		}

		private BlueprintViewModel GetBlueprintViewModel(string name, string featureTypeName, string featureTypeGroupName, int featureTypeId, int featureTypeGroupId, string manifestName = "blueprint-test", int blueprintId = 0)
		{
			var vm = new BlueprintViewModel
			{
				Id = blueprintId,
				ManifestName = manifestName,
				SelectedFeatureType = new EntityLookup
				{
					Id = featureTypeId,
					Name = featureTypeName
				},
				SelectedFeatureTypeGroup = new EntityLookup
				{
					Id = featureTypeGroupId,
					Name = featureTypeGroupName
				},
				Name = name
			};
			return vm;
		}


		#endregion


	}
}
