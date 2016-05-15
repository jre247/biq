using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class SettingInstanceSaveViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public int settingId { get; set; }
		public List<FieldSaveViewModel> fields { get; set; }
	}

	
}
