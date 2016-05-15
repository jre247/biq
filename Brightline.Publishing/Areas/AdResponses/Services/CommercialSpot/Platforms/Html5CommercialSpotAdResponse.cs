using BrightLine.Common.Models;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels;
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.AdTypes.PlatformAdResponses.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using Brightline.Publishing.Areas.AdResponses.ViewModels.VAST;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.VAST;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Html5.CompanionAd;
using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Enums;

namespace BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot.Platforms
{
	public class Html5CommercialSpotAdResponse : IPlatformAdResponseService
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get; set; }

		#endregion

		#region Init

		public Html5CommercialSpotAdResponse(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Response for Commercial Spot and Html5 (Samsung or FireTv) combination
		/// </summary>
		/// <returns></returns>
		public AdResponseViewModel GetAdResponse()
		{
			if (Ad == null)
				throw new ArgumentException("Ad is null inside Html5CommercialSpotAdResponse");

			if (Ad.AdTag == null)
				return null;

			var adResponsesDictionary = new Dictionary<string, AdResponseViewModel>();

			// Get Ad Response Body
			var adResponseBody = new AdResponseBodyViewModel();
			adResponseBody.Default = GenerateAdResponseForVAST();

			var trackingUrls = VASTAdResponseHelper.GetBrightlineTrackingUrls(Ad);

			// Get Ad Response Key
			var adResponseKey = AdResponseHelper.GetAdResponseKey(TargetEnv, Ad);

			// Build whole Ad Response, including Metadata AdResponse Body
			var adResponse = AdResponseHelper.FormatAdResponse(Ad, adResponseBody, AdResponseType.Xml, adResponseKey, trackingUrls);

			return adResponse;
		}
		
		#endregion	
		
		#region Private Methods

		private string GenerateAdResponseForVAST()
		{
			// Generate the initial VAST that will have nodes that are shared across all platforms, minus the filled in CompanionAds array node
			var vast = new VAST(Ad, VASTPlatform.Html5);

			// Serialize the VAST to xml
			var xml = VAST.ToXml(vast);

			// Format the Brightline Tracking Events macro
			xml = VAST.ReplaceBrightlineTrackingEventsNode(xml);

			return xml;
		}

		#endregion	
	}
}
