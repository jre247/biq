using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class ModelInstanceListViewModel
	{
		public int id { get; set; }
		public string display { get; set; }
		public string displayName { get; set; }

		public string name { get; set; }

		public string displayField { get; set; }

		public string displayFieldType { get; set; }

		public List<FieldViewModel> fields { get; set; }
	}
}
