using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class BlueprintImportModelDefinition
	{
		public int Id { get; set; }
		public Dictionary<string, BlueprintImportModelField> RawFieldsDictionary { get; set; }
		public Dictionary<string, CmsField> CmsFieldsDictionary { get; set; }
		public CmsModelDefinition CmsModelDefinition { get; set; }
	}
}
