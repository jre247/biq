using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public class ModelDefinitionViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public string displayName { get; set; }
		public List<FieldViewModel> fields { get; set; }

		public ModelDefinitionViewModel()
		{

		}

		public ModelDefinitionViewModel(CmsModelDefinition cmsModelDefinition)
		{
			id = cmsModelDefinition.Id;
			name = cmsModelDefinition.Name;
			displayName = cmsModelDefinition.DisplayName;
			fields = cmsModelDefinition.Fields.Select(f => new FieldViewModel(f)).ToList();
		}

		//public static JObject ToJObject(CmsModelDefinition cmsModelDefinition)
		//{
		//	if (cmsModelDefinition == null)
		//		return null;

		//	var json = JObject.FromObject(cmsModelDefinition);
		//	return json;
		//}


	}
}
