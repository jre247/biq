using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.AdTrackingUrl.AdServer
{
	public interface AdServerAdTrackingUrlViewModel : AdTrackingUrlViewModel
	{
		string id { get; set; }
		string type { get; set; }
	}
}
