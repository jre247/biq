using System;

namespace BrightLine.Core.Attributes
{
	[AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class EntityAttribute : Attribute
	{
		public string Editor { get; set; }
		public string Details { get; set; }
		public bool AllowCopy { get; set; }
		public string ExportUrl { get; set; }
		public bool HasFile { get; set; }
	}
}