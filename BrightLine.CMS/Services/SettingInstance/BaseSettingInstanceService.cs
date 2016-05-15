using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services.SettingInstance
{
	public class BaseSettingInstanceService
	{
		protected SettingInstanceLookups GetSettingInstanceLookups(CmsSettingInstance settingInstance)
		{
			var settingInstanceLookupsService = IoC.Resolve<ISettingInstanceLookupsService>();

			var settingInstanceLookups = settingInstanceLookupsService.GetLookupsForSettingInstance(settingInstance.Setting_Id);
			return settingInstanceLookups;
		}

	}
}
