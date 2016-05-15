using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Serialization
{
	public class AppDataModelJSON
	{
		/// <summary>
		/// All instances serialized as JSON.
		/// </summary>
		public string All { get; set; }


		/// <summary>
		/// Raw data
		/// </summary>
		public List<string> InstancesRaw { get; set; }


		/// <summary>
		/// Data in published format.
		/// </summary>
		public List<string> InstancesPublished { get; set; }
	}
}
