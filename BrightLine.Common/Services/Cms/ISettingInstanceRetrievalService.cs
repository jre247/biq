using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public interface ISettingInstanceRetrievalService
	{
		JObject Get(int settingInstanceId);
		Dictionary<int, ModelInstanceListViewModel> GetSettingInstancesForSetting(int settingId);
	}
}
