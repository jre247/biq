using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class SettingInstanceRetrievalService : ISettingInstanceRetrievalService
	{
		public JObject Get(int settingInstanceId)
		{
			JObject settingInstanceJson = null;
			var cmsSettingInstancesRepo = IoC.Resolve<IRepository<CmsSettingInstance>>();

			var cmsSettingInstance = cmsSettingInstancesRepo.Get(settingInstanceId);
			if (cmsSettingInstance != null)
			{
				var settingInstance = JsonConvert.DeserializeObject<ModelInstanceJsonViewModel>(cmsSettingInstance.Json);
				settingInstanceJson = JObject.FromObject(settingInstance);
			}

			return settingInstanceJson;
		}

		public Dictionary<int, ModelInstanceListViewModel> GetSettingInstancesForSetting(int settingId)
		{
			var settingInstances = new Dictionary<int, ModelInstanceListViewModel>();
			var cmsSettingInstancesRepo = IoC.Resolve<IRepository<CmsSettingInstance>>();
			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();

			var setting = cmsSettings.Get(settingId);
			if (setting == null)
				return null;

			var cmsSettingInstances = cmsSettingInstancesRepo.Where(m => m.Setting_Id == settingId);
			if (cmsSettingInstances != null)
			{
				settingInstances = cmsSettingInstances.Select(m => new ModelInstanceListViewModel
				{
					id = m.Id,
					display = m.Display,
					name = m.Name
				}).ToList().ToDictionary(c => c.id, c => c);
			}

			return settingInstances;
		}
	}
}
