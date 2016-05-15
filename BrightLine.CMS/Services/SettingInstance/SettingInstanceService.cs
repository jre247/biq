using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility;
using BrightLine.CMS.Factories;
using BrightLine.CMS.Services.Publish;

namespace BrightLine.CMS.Services
{
	public class SettingInstanceService : ISettingInstanceService
	{
		SettingInstanceLookups SettingInstanceLookups { get;set;}

		public SettingInstanceService()
		{ 
			SettingInstanceLookups = new SettingInstanceLookups();
		}

		public JObject Get(int settingInstanceId)
		{
			var settingInstanceRetrievalService = IoC.Resolve<ISettingInstanceRetrievalService>();

			return settingInstanceRetrievalService.Get(settingInstanceId);
		}

		public Dictionary<int, ModelInstanceListViewModel> GetSettingInstancesForSetting(int settingId)
		{
			var settingInstanceRetrievalService = IoC.Resolve<ISettingInstanceRetrievalService>();

			return settingInstanceRetrievalService.GetSettingInstancesForSetting(settingId);

		}

		public BoolMessageItem Save(ModelInstanceSaveViewModel viewModel)
		{
			var settingInstanceSaveService = IoC.Resolve<ISettingInstanceSaveService>();

			return settingInstanceSaveService.Save(viewModel);
		}
	}
}
