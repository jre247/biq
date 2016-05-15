using BrightLine.Utility;
using System.Collections.Generic;

namespace BrightLine.Web.Areas.Developer.Models
{
	public class CacheModel
	{
		public List<string> Keys { get; set; }

        public List<CacheMetadata> CacheMetaItems { get; set; } 
	}
}
