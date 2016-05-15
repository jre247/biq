using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public class SettingDefinitionViewModel
	{
		public string name { get; set; }
		public string displayName { get; set; }
		public List<FieldViewModel> fields { get; set; }

		public SettingDefinitionViewModel()
		{

		}

		public SettingDefinitionViewModel(CmsSettingDefinition cmsSettingDefinition)
		{
			name = cmsSettingDefinition.Name;
			displayName = cmsSettingDefinition.DisplayName;
			fields = cmsSettingDefinition.Fields.Select(f => new FieldViewModel(f)).ToList();
		}
	}
}
