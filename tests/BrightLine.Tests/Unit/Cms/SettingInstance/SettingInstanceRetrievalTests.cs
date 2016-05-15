using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BrightLine.Tests.Common;


using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.CMS.Services;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using Newtonsoft.Json;
using BrightLine.Common.Framework;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Utility;
using Moq;
using BrightLine.Common.Utility.FieldType;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class SettingInstanceRetrievalTests
	{
		IocRegistration Container;
		ISettingInstanceService SettingInstanceService { get;set;}

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth();
			Container = MockUtilities.SetupIoCContainer(Container);
			Container.Register<IResourceHelper, ResourceHelper>();

			var environmentHelper = new Mock<IEnvironmentHelper>();
			environmentHelper.Setup(c => c.IsLocal).Returns(false);
			Container.Register<IEnvironmentHelper>(() => environmentHelper.Object);

			SettingInstanceService = IoC.Resolve<ISettingInstanceService>();
		}

		[Test(Description = "Retrieving an existing Setting Instance is null.")]
		public void Cms_Setting_Instance_Retrieval_Against_Existing_Instance_Is_Null()
		{
			var settingInstanceJson = SettingInstanceService.Get(25454555);

			Assert.IsNull(settingInstanceJson);
		}

		[Test(Description = "Retrieving an existing Setting Instance is not null.")]
		public void Cms_Setting_Instance_Retrieval_Against_Existing_Instance_Is_Not_Null()
		{
			var cmsSettingInstances = IoC.Resolve<IRepository<CmsSettingInstance>>();

			var settingInstanceJson = SettingInstanceService.Get(2);
			var settingInstanceCompare = cmsSettingInstances.Get(2);

			Assert.IsNotNull(settingInstanceJson);
			Assert.IsNotNull(settingInstanceCompare);
		}

		[Test(Description = "Retrieving an existing Setting Instance has correct base level properties.")]
		public void Cms_Setting_Instance_Retrieval_Against_Existing_Instance_Has_Correct_Base_Level_Properties()
		{
			var settingInstanceJson = SettingInstanceService.Get(2);

			var settingInstance = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(settingInstanceJson.ToString());
			Assert.AreEqual(settingInstance.id, 2);
			Assert.AreEqual(settingInstance.name, "Setting 2");
		}

		[Test(Description = "Retrieving an existing Setting Instance has correct fields.")]
		public void Cms_Setting_Instance_Retrieval_Against_Existing_Instance_Has_Correct_Fields()
		{
			var settingInstanceJson = SettingInstanceService.Get(2);

			var settingInstance = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(settingInstanceJson.ToString());
			var settingInstanceFieldsJson = JsonConvert.SerializeObject(settingInstance.fields);
			var settingFieldsCompareJson = JsonConvert.SerializeObject(new List<FieldViewModel>{MockEntities.GetFieldViewModel(1, "test", FieldTypeConstants.FieldTypeNames.String, "bce", "bcef")});

			Assert.AreEqual(settingInstanceFieldsJson, settingFieldsCompareJson);
		}

	}
}
