using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class CreativeFeaturesViewModel
	{
		[DataMember]
		public Dictionary<int, FeatureViewModel> features { get; set; }

		public static CreativeFeaturesViewModel ToCreativeFeaturesModelsViewModel(List<IGrouping<int, ModelViewModel>> featureModels)
		{
			var vm = new CreativeFeaturesViewModel();
			var featuresDictionary = new Dictionary<int, FeatureViewModel>();

			foreach (var model in featureModels)
			{
				var featureVm = FeatureViewModel.ToFeatureModelsViewModel(model);
				featuresDictionary.Add(model.Key, featureVm);
			}

			vm.features = featuresDictionary;

			return vm;
		}

		public static CreativeFeaturesViewModel ToCreativeFeaturesSettingsViewModel(List<IGrouping<int, SettingViewModel>> featureSettings)
		{
			var vm = new CreativeFeaturesViewModel();
			var featuresDictionary = new Dictionary<int, FeatureViewModel>();

			foreach (var setting in featureSettings)
			{
				var featureVm = FeatureViewModel.ToFeatureSettingsViewModel(setting);
				featuresDictionary.Add(setting.Key, featureVm);
			}

			vm.features = featuresDictionary;

			return vm;
		}


		[DataContract]
		public class FeatureViewModel
		{
			[DataMember]
			public int id { get; set; }
			[DataMember]
			public Dictionary<int, ModelViewModel> models { get; set; }
			[DataMember]
			public Dictionary<int, SettingViewModel> settings { get; set; }

			public static FeatureViewModel ToFeatureModelsViewModel(IGrouping<int, ModelViewModel> featureModels)
			{
				return new FeatureViewModel { id = featureModels.Key, models = featureModels.ToList().ToDictionary(c => c.id, c => c) };
			}

			public static FeatureViewModel ToFeatureSettingsViewModel(IGrouping<int, SettingViewModel> featureSettings)
			{
				return new FeatureViewModel { id = featureSettings.Key, settings = featureSettings.ToList().ToDictionary(c => c.id, c => c) };
			}
		}

		[DataContract]
		public class ModelViewModel
		{
			[DataMember]
			public int id { get; set; }
			[DataMember]
			public string name { get; set; }
			[DataMember]
			public string display { get; set; }
			[DataMember]
			public string displayNamePlural { get; set; }
			[DataMember]
			public int modelDefinitionId { get; set; }
			[DataMember]
			public int instancesCount { get; set; }
			public int featureId { get; set; }

		}

		[DataContract]
		public class SettingViewModel
		{
			[DataMember]
			public int id { get; set; }
			[DataMember]
			public string name { get; set; }
			[DataMember]
			public int settingDefinitionId { get; set; }
			[DataMember]
			public int instancesCount { get; set; }
			[DataMember]
			public int settingInstanceId { get; set; }

			public int featureId { get; set; }

		}
	}

}
