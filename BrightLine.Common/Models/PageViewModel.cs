using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class PageViewModel
	{
		public int id { get; set; }

		[StringLength(255)]
		public string name { get; set; }
	}
}
