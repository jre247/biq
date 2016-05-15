using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface ISettingInstancePublishedJsonService
	{
		void SaveSettingInstancePublishedJson(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance);
		object DeserializeSettingnstancePropertyValue(KeyValuePair<string, object> item);
	}
}
