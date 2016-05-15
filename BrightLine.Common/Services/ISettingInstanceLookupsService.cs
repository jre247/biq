using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface ISettingInstanceLookupsService
	{
		SettingInstanceLookups CreateSettingInstanceLookups(CmsSettingInstance SettingInstance);
		SettingInstanceLookups GetLookupsForSettingInstance(int SettingInstanceId);
		void SaveLookupsForSettingInstance(SettingInstanceLookups SettingInstanceLookups);
	}
}
