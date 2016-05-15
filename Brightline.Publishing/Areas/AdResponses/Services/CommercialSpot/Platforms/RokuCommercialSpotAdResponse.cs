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
using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.VAST;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Roku;
using Brightline.Publishing.Areas.AdResponses.ViewModels.VAST;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Roku.CompanionAd;
using BrightLine.Publishing.Areas.AdResponses.Enums;

namespace BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot.Platforms
{
	public class RokuCommercialSpotAdResponse : IPlatformAdResponseService
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get; set; }

		#endregion

		#region Init

		public RokuCommercialSpotAdResponse(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Response for Commercial Spot and Roku combination
		/// </summary>
		/// <returns></returns>
		public AdResponseViewModel GetAdResponse()
		{
			if (Ad == null)
				throw new ArgumentException("Ad is null inside RokuDestinationAdResponse");

			if (Ad.AdTag == null)
				return null;

			var adResponseKey = AdResponseHelper.GetAdResponseKey(TargetEnv, Ad);

			var raf = GetAdResponseForRAF();
			var di = GetAdResponseForDI();

			var adResponseBodyViewModel = new AdResponseBodyViewModel
			{
				RAF = raf,
				DI = di
			};

			var trackingUrls = VASTAdResponseHelper.GetBrightlineTrackingUrls(Ad);

			var adResponse = AdResponseHelper.FormatAdResponse(Ad, adResponseBodyViewModel, AdResponseType.Xml, adResponseKey, trackingUrls);

			return adResponse;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Get an Ad Response for Roku Ad Framework
		/// </summary>
		/// <param name="adResponseKeys"></param>
		/// <param name="adResponsesDictionary"></param>
		private string GetAdResponseForRAF()
		{
			return GetAdResponseForVAST(VASTPlatform.RokuAdFramework);
		}

		/// <summary>
		/// Get an Ad Response for Direct Integration
		/// </summary>
		/// <param name="adResponseKeys"></param>
		/// <param name="adResponsesDictionary"></param>
		private string GetAdResponseForDI()
		{
			return GetAdResponseForVAST(VASTPlatform.RokuDirectIntegration);
		}

		/// <summary>
		/// Build a VAST that will be serialized to xml
		/// </summary>
		/// <param name="rokuSdk"></param>
		/// <returns></returns>
		private string GetAdResponseForVAST(VASTPlatform vastPlatform)
		{
			// Get the initial VAST that will have nodes that are shared across all platforms, minus the filled in CompanionAds array node
			var vast = new VAST(Ad, vastPlatform);

			// Serialize the VAST to xml
			var xml = VAST.ToXml(vast);

			// Format the Brightline Tracking Events macro
			xml = VAST.ReplaceBrightlineTrackingEventsNode(xml);

			return xml;
		}

		#endregion		
	}
}
