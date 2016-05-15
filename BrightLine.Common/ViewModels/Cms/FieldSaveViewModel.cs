using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class FieldSaveViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public List<string> value { get; set; }
	}
}
