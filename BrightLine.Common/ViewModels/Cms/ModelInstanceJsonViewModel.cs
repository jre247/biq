using BrightLine.Common.Models.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class ModelInstanceJsonViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
	
		public List<FieldViewModel> fields { get; set; }
	}
}
