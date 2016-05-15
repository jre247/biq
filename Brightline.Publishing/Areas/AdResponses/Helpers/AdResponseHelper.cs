using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Platform;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Helpers
{
	public class AdResponseHelper
	{
		#region Public Methods

		/// <summary>
		/// Get the formatted Ad Response.
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="adResponseData"></param>
		/// <param name="adResponseType"></param>
		/// <returns>A AdResponseViewModel object that represents the Ad Response metadata and actual Ad Response data</returns>
		public static AdResponseViewModel FormatAdResponse(Ad ad, AdResponseBodyViewModel adResponseBody, AdResponseType adResponseType, string key, Dictionary<string, string> trackingUrls = null)
		{
			var responseType = adResponseType == AdResponseType.Xml ? PublishConstants.ResponseTypes.Xml : PublishConstants.ResponseTypes.Json;

			return new AdResponseViewModel
			{
				Key = key,
				Metadata = MetaDataViewModel.Parse(ad, responseType, trackingUrls),
				AdResponseBody = adResponseBody
			};
		}

		/// <summary>
		/// Get the key that will be used for an Ad Response
		/// </summary>
		/// <param name="targetEnv"></param>
		/// <param name="adTagId"></param>
		/// <returns></returns>
		public static string GetAdResponseKey(string targetEnv, Ad ad)
		{
			var adTagId = ad.AdTag.Id;
			var adResponseKey = string.Format("{0}_{1}", targetEnv, adTagId);
			return adResponseKey;
		}

		/// <summary>
		/// Get the key that will be used for a Commercial Spot Shim Ad Response
		///		*Note: This Shim Ad Response Key is temporary and will be removed per Jira item BL-727
		/// </summary>
		/// <param name="targetEnv"></param>
		/// <param name="ad"></param>
		/// <returns></returns>
		public static string GetAdResponseKeyForShim(string targetEnv, Ad ad)
		{
			var adTagId = ad.AdTag.Id;
			var adResponseKey = string.Format("{0}_{1}", targetEnv, "VAST_" + ad.CompanionAd.AdTag.Id);
			return adResponseKey;
		}

		/// <summary>
		/// Build a Html5 Ad Response that has Metadata, Key, and an empty Body
		/// </summary>
		/// <param name="ad"></param>
		/// <param name="targetEnv"></param>
		/// <returns></returns>
		public static AdResponseViewModel BuildGenericHtml5AdResponse(Ad ad, string targetEnv)
		{
			if (ad == null)
				throw new ArgumentException("Ad is null inside Html5CommercialSpotAdResponse");

			if (ad.AdTag == null)
				return null;

			var adResponsesDictionary = new Dictionary<string, AdResponseViewModel>();
			var adResponseBody = new AdResponseBodyViewModel();

			// Get Ad Response Key
			var adResponseKey = GetAdResponseKey(targetEnv, ad);

			// Build whole Ad Response, including Metadata and null for AdResponse Body
			var adResponse = FormatAdResponse(ad, adResponseBody, AdResponseType.Json, adResponseKey);

			return adResponse;
		}

		#endregion
	}
}
