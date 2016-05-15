using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class Setting : EntityBase, IEntity
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public string Type { get; set; }
	}
}
