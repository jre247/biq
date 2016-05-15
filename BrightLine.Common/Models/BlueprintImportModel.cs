using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class BlueprintImportModel
	{
		public string name { get; set; }
		public string displayName { get; set; }
		public string displayField { get; set; }
		public List<BlueprintImportModelField> fields { get; set; }
	}

	public class BlueprintImportModelField
	{
		public string name { get; set; }
		public string displayName { get; set; }
		public string description { get; set; }
		public string type { get; set; }
		public string expose { get; set; }
		public string list { get; set; }
		public BlueprintImportRef @ref { get; set; }
		public BlueprintImportModelValidation validation { get; set; }
	}

	public class BlueprintImportModelValidation
	{
		public List<string> extension { get; set; }
		public string required { get; set; }
		public string unique { get; set; }
		public string height { get; set; }
		public string minHeight { get; set; }
		public string maxHeight { get; set; }
		public string length { get; set; }
		public string minLength { get; set; }
		public string maxLength { get; set; }
		public string width { get; set; }
		public string minWidth { get; set; }
		public string maxWidth { get; set; }
		public string minFloat { get; set; }
		public string maxFloat { get; set; }
		public string minDatetime { get; set; }
		public string maxDatetime { get; set; }
		public string maxImageSize { get; set; }
		public string maxVideoSize { get; set; }
		public string maxVideoDuration { get; set; }
	}

	public class BlueprintImportRef
	{
		public string type { get; set; }
		public string model { get; set; }
		public string page { get; set; }		
	}

	

}
