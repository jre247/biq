using BrightLine.Common.Models;
using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Settings
{
	public class SettingsViewModel
	{
		public List<SettingViewModel> Settings { get; set; }

		public SettingsViewModel()
		{ }

		public SettingsViewModel(Dictionary<string, SettingValue> settings)
		{
			this.Settings = settings.Where(s => s.Value.SettingOrigin != SettingOrigin.Config).Select(s => new SettingViewModel
			{
				Key = s.Key,
				Value = s.Value.Value.ToString()
			}).ToList();
		}
	}

	public class SettingViewModel
	{
		public string Key { get;set;}
		public string Value { get; set; }
	}
}
