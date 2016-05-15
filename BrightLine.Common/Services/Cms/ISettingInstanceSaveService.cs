using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public interface ISettingInstanceSaveService
	{
		BoolMessageItem Save(ModelInstanceSaveViewModel viewModel);
		BoolMessageItem SaveSettingInstance(ModelInstanceSaveViewModel viewModel, CmsSettingInstance settingInstance);
	}
}
