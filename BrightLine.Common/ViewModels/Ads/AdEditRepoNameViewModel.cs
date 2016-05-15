using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Ads
{
	public class AdEditRepoNameViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public int platformId { get; set; }
		public string platformName { get; set; }
		public string repoName { get; set; }
	}
}
