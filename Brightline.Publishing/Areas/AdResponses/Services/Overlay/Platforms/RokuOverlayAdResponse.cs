using BrightLine.Common.Models;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.Roku;
using BrightLine.Publishing.Areas.AdResponses.AdTypes.PlatformAdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Enums;

namespace BrightLine.Publishing.Areas.AdResponses.Services.Overlay.Platforms
{
	public class RokuOverlayAdResponse : IPlatformAdResponseService
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get; set; }

		#endregion

		#region Init

		public RokuOverlayAdResponse(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Response for Overlay and Roku combination
		/// </summary>
		/// <returns></returns>
		public AdResponseViewModel GetAdResponse()
		{
			if (Ad == null)
				throw new ArgumentException("Ad is null inside RokuOverlayAdResponse");

			if(Ad.AdTag == null)
				return null;

			var adResponseKey = AdResponseHelper.GetAdResponseKey(TargetEnv, Ad);

			var raf = GetAdResponseForRAF();
			var di = GetAdResponseForDI();

			var adResponseBodyViewModel = new AdResponseBodyViewModel
			{
				RAF = raf,
				DI = di
			};

			var adResponse = AdResponseHelper.FormatAdResponse(Ad, adResponseBodyViewModel, AdResponseType.Json, adResponseKey);

			return adResponse;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Get Ad Response for Roku Ad Framework
		/// </summary>
		/// <param name="adResponseKeys"></param>
		/// <param name="adResponsesDictionary"></param>
		private RokuOverlayViewModel GetAdResponseForRAF()
		{
			return new RokuOverlayViewModel(Ad, RokuSdk.RokuAdFramework);
		}

		/// <summary>
		/// Get Ad Response for Direct Integration
		/// </summary>
		/// <param name="adResponseKeys"></param>
		/// <param name="adResponsesDictionary"></param>
		private RokuOverlayViewModel GetAdResponseForDI()
		{
			return new RokuOverlayViewModel(Ad, RokuSdk.DirectIntegration);
		}

		#endregion
	}
}
