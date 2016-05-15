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
	public class ModelInstancePublishedJsonViewModel
	{
		public Dictionary<string, string> Properties { get;set;}

		public ModelInstancePublishedJsonViewModel()
		{
			Properties = new Dictionary<string,string>();
		}

	}
}
