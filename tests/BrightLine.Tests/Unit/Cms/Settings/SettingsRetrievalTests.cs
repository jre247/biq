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
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Common.Utility.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrightLine.Core;
using BrightLine.Common.Framework;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class SettingsRetrievalTests
	{
		IocRegistration Container;
		ISettingService SettingService { get;set;}

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupAuth();
			MockUtilities.SetupIoCContainer(Container);
			SettingService = IoC.Resolve<ISettingService>();
		}


		[Test(Description = "Retrieving list of Settings is null.")]
		public void Cms_Settings_Retrieval_Is_Null()
		{
			var settings = SettingService.GetSettingsForCreative(134);

			Assert.IsNull(settings);
		}

		[Test(Description = "Retrieving list of Settings is not null.")]
		public void Cms_Settings_Retrieval_Is_Not_Null()
		{
			var settings = SettingService.GetSettingsForCreative(1);

			Assert.IsNotNull(settings);
		}

		[Test(Description = "Retrieving list of Settings has correct features count.")]
		public void Cms_Settings_Retrieval_Has_Correct_Features_Count()
		{
			var settings = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(settings.features.Count() == 1);
		}

		[Test(Description = "Retrieving list of Settings has correct settings count.")]
		public void Cms_Settings_Retrieval_Has_Correct_Settings_Count()
		{
			var creative = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(creative.features[1].settings.Count() == 2);
		}

		[Test(Description = "Retrieving list of Settings has correct feature hash keys.")]
		public void Cms_Settings_Retrieval_Has_Correct_Feature_Hash_Keys()
		{
			var creative = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(creative.features.ContainsKey(1), "Creative does not contain feature with key '1'");
		}

		[Test(Description = "Retrieving list of Settings has correct setting hash keys.")]
		public void Cms_Settings_Retrieval_Has_Correct_setting_Hash_Keys()
		{
			var creative = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(creative.features[1].settings.ContainsKey(1), "Feature does not contain setting with key '1'");
			
		}

		[Test(Description = "Retrieving list of Settings has correct feature ids.")]
		public void Cms_Settings_Retrieval_Has_Correct_Feature_Ids()
		{
			var creative1 = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(creative1.features[1].id == 1, "Feature does not have id equal to 1.");
			
		}

		[Test(Description = "Retrieving list of Settings has correct setting names.")]
		public void Cms_Settings_Retrieval_Has_Correct_setting_Ids()
		{
			var creative = SettingService.GetSettingsForCreative(1);

			Assert.IsTrue(creative.features[1].settings[1].name == "Setting 1", "Setting does not have name equal to 'Setting 1'.");
		
		}

	}
}
