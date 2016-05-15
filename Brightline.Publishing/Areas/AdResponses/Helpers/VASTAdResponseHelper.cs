using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.AdTrackingUrl;
using BrightLine.Common.ViewModels.AdTrackingUrl.AdServer;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Constants;
using BrightLine.Service;
using BrightLine.Service.AdTrackingUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Helpers
{
	public class VASTAdResponseHelper
	{
		#region Public Methods

		public static string GetMediaFileTypeForVASTResponse(Resource resource)
		{
			var mp4 = Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4];

			if (resource.Extension.Id == mp4)
				return VASTConstants.MediaFileTypes.mp4;
			else
				throw new ArgumentException("There is no matching VAST MediaFile Type for resource.");
		}

		public static Dictionary<string, string> GetBrightlineTrackingUrls(Ad ad)
		{
			var trackingUrls = new Dictionary<string, string>();		

			// Start Tracking Url
			var startEventViewModel = new StartEventAdTrackingUrlViewModel(ad.Id);
			GenerateTrackingUrlForStartEvent(trackingUrls, startEventViewModel, VASTConstants.TrackingUrlKeys.Start);

			// First Quartile Tracking Url
			var firstQuartileViewModel = new QuartileEventAdTrackingUrlViewModel(AdTagUrlConstants.AdServer.PercentComplete.FirstQuartile);
			GenerateTrackingUrlForQuartileEvent(trackingUrls, firstQuartileViewModel, VASTConstants.TrackingUrlKeys.FirstQuartile);

			// Midpoint Event Tracking Url
			var midpointViewModel = new QuartileEventAdTrackingUrlViewModel(AdTagUrlConstants.AdServer.PercentComplete.Midpoint);
			GenerateTrackingUrlForQuartileEvent(trackingUrls, midpointViewModel, VASTConstants.TrackingUrlKeys.Midpoint);

			// Third Quartile Tracking Url
			var thirdQuartileViewModel = new QuartileEventAdTrackingUrlViewModel(AdTagUrlConstants.AdServer.PercentComplete.ThirdQuartile);
			GenerateTrackingUrlForQuartileEvent(trackingUrls, thirdQuartileViewModel, VASTConstants.TrackingUrlKeys.ThirdQuartile);

			// Complete Tracking Url
			var completeViewModel = new QuartileEventAdTrackingUrlViewModel(AdTagUrlConstants.AdServer.PercentComplete.Complete);
			GenerateTrackingUrlForQuartileEvent(trackingUrls, completeViewModel, VASTConstants.TrackingUrlKeys.Complete);

			return trackingUrls;
		}

		/// <summary>
		/// Build an Ad Tag url for Roku Overlay
		/// </summary>
		/// <param name="ad"></param>
		/// <returns></returns>
		public static string BuildAdTagUrlForOverlay(Ad ad)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var adTagId = ad.CompanionAd.AdTag.Id.ToString();

			var adTagUrlGenerator = new AdTagUrlGenerator();
			var adTagUrl = adTagUrlGenerator.Generate(adTagId, roku);

			// Tack on the mblist query param to the Ad Tag url
			adTagUrl = string.Format("{0}&{1}={2}", adTagUrl, AdTagUrlConstants.QueryParams.MBList, settings.MBList);
			
			return adTagUrl;
		}

		#endregion

		#region Private Methods

		private static void GenerateTrackingUrlForStartEvent(Dictionary<string, string> trackingUrls, StartEventAdTrackingUrlViewModel viewModel, string trackingUrlKey)
		{
			var adTagUrlGeneratorService = new AdServerAdTrackingUrlGenerator();
			var trackingUrl = adTagUrlGeneratorService.Generate(viewModel);

			trackingUrls.Add(trackingUrlKey, trackingUrl);
		}

		private static void GenerateTrackingUrlForQuartileEvent(Dictionary<string, string> trackingUrls, QuartileEventAdTrackingUrlViewModel viewModel, string trackingUrlKey)
		{
			var adTagUrlGeneratorService = new AdServerAdTrackingUrlGenerator();
			var trackingUrl = adTagUrlGeneratorService.Generate(viewModel);

			trackingUrls.Add(trackingUrlKey, trackingUrl);
		}

		#endregion
	}
}
