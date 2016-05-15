using BrightLine.Common.Utility.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.AdTrackingUrl.AdServer
{
	public class QuartileEventAdTrackingUrlViewModel : AdServerAdTrackingUrlViewModel
	{
		public string id { get; set; }
		public string type { get; set; }
		public string duration_type { get; set; }
		public int percent_complete { get; set; }

		public QuartileEventAdTrackingUrlViewModel(int percentComplete)
		{
			id = AdTagUrlConstants.AdServer.AdServerIdMacro;
			type = AdTagUrlConstants.AdServer.Types.Duration;
			duration_type = AdTagUrlConstants.AdServer.DurationType;
			percent_complete = percentComplete;
		}
	}
}
