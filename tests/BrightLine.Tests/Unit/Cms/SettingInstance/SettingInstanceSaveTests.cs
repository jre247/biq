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
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Utility;
using BrightLine.Common.ViewModels;
using Newtonsoft.Json;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using Moq;
using System.Web;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ValidationType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class SettingInstanceSaveTests
	{
		#region Private Variables

		IocRegistration Container;
		private ICmsService CmsService { get;set;}
		private int NewSettingInstanceId { get;set;}
		private int SettingInstancesInitialCount { get;set;}
		private CmsSetting CmsSetting { get;set;}
		private CmsSettingInstance CmsSettingInstance { get;set;}
		private int TestEmployeeId = 1;
		IRepository<CmsSettingInstance> CmsSettingInstances { get;set;}
		ISettingInstanceService SettingInstanceService { get;set;}

		#endregion //Private Variables

		#region Setup

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth(1, AuthConstants.Roles.Employee, TestEmployeeId);
			Container = MockUtilities.SetupIoCContainer(Container);

			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			CmsService = IoC.Resolve<ICmsService>();
			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();
			CmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();
			SettingInstanceService = IoC.Resolve<ISettingInstanceService>();
			CmsSetting = cmsSettings.Get(1);

			CmsSettingInstance = CmsSettingInstances.Get(1);

			NewSettingInstanceId = CmsSettingInstances.GetAll().Count() + 1;
			SettingInstancesInitialCount = CmsSettingInstances.GetAll().Count();

			var campaigns = IoC.Resolve<ICampaignService>();
			campaigns.Upsert(MockEntities.CreateCampaign(1, "test", TestEmployeeId));
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current.Items.Remove(SettingInstanceConstants.SETTING_INSTANCE_LOOKUPS_KEY);
		}

		#endregion //Setup

		#region Tests

		[Test(Description = "Cms Setting Instance saving creates new instance if no instance exists for viewmodel's instance id.")]
		public void Cms_Setting_Instance_Saving_Creates_New_Instance()
		{
			var viewModel = CreateViewModelForFieldValue("1432", 1, 0);

			CmsService.SaveSettingInstance(viewModel);

			var settingInstances = CmsSettingInstances.GetAll();
			Assert.IsTrue(settingInstances.Count() == SettingInstancesInitialCount + 1);
		}

		[Test(Description = "Cms Setting Instance saving creates new instance with returned instance id.")]
		public void Cms_Setting_Instance_Saving_Creates_New_Instance_With_Returned_Id()
		{
			var viewModel = CreateViewModelForFieldValue("2", 1, 0);

			var boolMessage = CmsService.SaveSettingInstance(viewModel);

			var newSettingInstance = CmsSettingInstances.Get(NewSettingInstanceId);
			Assert.IsTrue(newSettingInstance.Id == boolMessage.EntityId);
		}


		[Test(Description = "Cms Setting Instance saving does not create new instance if viewmodel references existing instance id.")]
		public void Cms_Setting_Instance_Saving_Does_Not_Create_New_Instance()
		{
			var viewModel = CreateViewModelForFieldValue("1432", 2, 1);
		
			CmsService.SaveModelInstance(viewModel);

			var settingInstances = CmsSettingInstances.GetAll();
			Assert.IsTrue(settingInstances.Count() == SettingInstancesInitialCount);
		}

		[Test(Description = "Cms Setting Instance with unique validator returns invalid.")]
		public void Cms_Setting_Instance_Save_With_Unique_Validator_Is_False()
		{
			var modelInstanceSaveVm = CreateViewModelForUniqueFieldValue("abc", 4, 2);
			SetupCmsSettingDefinitionForUnique();

			var boolMessage = SettingInstanceService.Save(modelInstanceSaveVm);

			Assert.IsFalse(boolMessage.Success);
			Assert.IsTrue(boolMessage.Message == "String validation failed");
			
		}

		[Test(Description = "Cms Setting Instance with unique validator returns invalid.")]
		public void Cms_Setting_Instance_Save_With_Unique_Validator_Is_True()
		{
			var modelInstanceSaveVm = CreateViewModelForUniqueFieldValue("abcd123", 4, 1);
			SetupCmsSettingDefinitionForUnique();

			var boolMessage = SettingInstanceService.Save(modelInstanceSaveVm);

			Assert.IsTrue(boolMessage.Success);
		}


		#endregion //Tests

		#region Private Methods

		private void SetupCmsSettingDefinitionForUnique()
		{
			var cmsField = MockEntities.GetFieldWithoutRequiredValidator(2, "test", "test", "test", Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String], ValidationTypeSystemTypeConstants.STRING, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique], ValidationTypeSystemTypeConstants.BOOL, ValidationTypeNameConstants.UNIQUE, "true");
			CmsSetting.CmsSettingDefinition.Fields.Add(cmsField);
		}


		private static ModelInstanceSaveViewModel CreateViewModelForFieldValue(string fieldValue, int fieldId = 1, int instanceId = 1)
		{
			var viewModel = MockEntities.GetSettingInstanceSaveViewModel(instanceId, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(fieldId, fieldValue));
			return viewModel;
		}

		private static ModelInstanceSaveViewModel CreateViewModelForUniqueFieldValue(string fieldValue, int instanceId = 1, int fieldId = 1)
		{
			var viewModel = MockEntities.GetSettingInstanceSaveViewModel(instanceId, "Question");
			viewModel.fields.Add(MockEntities.GetFieldSaveViewModelWithInput(fieldId, fieldValue));
			return viewModel;
		}

		#endregion //Private Methods


	}
}
