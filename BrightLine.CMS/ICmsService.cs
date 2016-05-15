using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS
{
	public interface ICmsService
	{
		BoolMessageItem SaveModelInstance(ModelInstanceSaveViewModel viewModel);

		CreativeFeaturesViewModel GetModelsForCreative(int creativeId);

		Dictionary<int, ModelInstanceListViewModel> GetModelInstancesForModel(int modelId, bool verbose);

		JObject GetModelInstance(int modelInstanceId);

		JObject GetSettingInstance(int settingInstanceId);

		BoolMessageItem SaveSettingInstance(ModelInstanceSaveViewModel viewModel);

		Dictionary<int, ModelInstanceListViewModel> GetSettingInstancesForSetting(int settingId);

		CreativeFeaturesViewModel GetSettingsForCreative(int creativeId);
	}
}
