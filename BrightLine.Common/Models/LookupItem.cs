using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	/// <summary>
	/// This is a single lookup record that will be inserted into a lookup dictionary
	/// </summary>
	public class LookupItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string TableName { get; set; }
	}
}
