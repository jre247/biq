using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.ViewModels.AdTrackingUrl;
using BrightLine.Common.ViewModels.AdTrackingUrl.IQ;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service.AdTrackingUrl
{
	public class IQAdTrackingUrlGenerator : BaseAdTrackingUrlGenerator<IQAdTrackingUrlViewModel>
	{
		#region Public Methods

		/// <summary>
		/// Build the Ad Tracking Url 
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		public override string Generate(IQAdTrackingUrlViewModel viewModel)
		{
			var samsungAdHubFirstTile = Lookups.Placements.HashByName[PlacementConstants.Names.SamsungAdHubFirstTile];

			var adTagUrl = base.BuildFullUrl(viewModel);

			// Add Redirect macro to end of Ad Tag Url for Samsung Ad Hub First Tile
			if (viewModel.placement_id == samsungAdHubFirstTile)
				adTagUrl += string.Format("&{0}", AdTagUrlConstants.Macros.Redirect);

			return adTagUrl;
		}

		#endregion
	}
}
