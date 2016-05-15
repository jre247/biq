using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.ViewModels.AdTrackingUrl;
using BrightLine.Common.ViewModels.AdTrackingUrl.AdServer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service.AdTrackingUrl
{
	public class AdServerAdTrackingUrlGenerator : BaseAdTrackingUrlGenerator<AdServerAdTrackingUrlViewModel>
	{
		#region Public Methods

		/// <summary>
		/// Build the Ad Tracking Url for a Quartile or Start Event
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		public override string Generate(AdServerAdTrackingUrlViewModel viewModel)
		{
			return base.BuildFullUrl(viewModel);
		}

		#endregion
	}
}
