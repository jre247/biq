using BrightLine.Common.Models.Lookups;
using BrightLine.Common.ViewModels.Cms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class SettingInstancePublishedJsonViewModel
	{
		public Dictionary<string, string> Properties { get;set;}

		public SettingInstancePublishedJsonViewModel()
		{
			Properties = new Dictionary<string,string>();
		}

	}
}
