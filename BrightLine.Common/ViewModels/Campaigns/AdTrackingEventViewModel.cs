using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Campaigns
{
	public class AdTrackingEventViewModel
	{
		public int id { get;set;}
		public int adId { get; set; }
		public int trackingEventId { get; set; }
		public string trackingUrl { get; set; }
	}
}
