using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class PageRefFieldValue
	{
		public int pageId { get; set; }

		public PageRefFieldValue() { }

		public PageRefFieldValue(int pageIdIn)
		{
			pageId = pageIdIn;
		}
	}
}
