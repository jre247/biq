using BrightLine.Common.Utility.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.AdTrackingUrl.AdServer
{
	public class StartEventAdTrackingUrlViewModel : AdServerAdTrackingUrlViewModel
	{
		public string id { get; set; }
		public int ad_id { get; set; }
		public string type { get; set; }

		public StartEventAdTrackingUrlViewModel(int adId)
		{
			id = AdTagUrlConstants.AdServer.AdServerIdMacro;
			type = AdTagUrlConstants.AdServer.Types.Impression;
			ad_id = adId;
		}
	}
}
