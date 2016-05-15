using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.AdTrackingUrl.IQ
{
	public class IQAdTrackingUrlViewModel : AdTrackingUrlViewModel
	{
		public string type { get; set; }
		public bool valid { get; set; }
		public int ad_id { get; set; }

		[JsonIgnore]
		public int placement_id { get; set; }

		public IQAdTrackingUrlViewModel(string type, bool valid, int adId, int placementId)
		{
			this.type = type;
			this.valid = valid;
			this.ad_id = adId;
			this.placement_id = placementId;
		}
	}
}
