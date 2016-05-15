using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Platform;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service
{
	public class AdTagUrlGenerator
	{
		#region Members

		private readonly string BaseUrl;

		#endregion

		#region Init

		public AdTagUrlGenerator()
		{
			var settings = IoC.Resolve<ISettingsService>();

			BaseUrl = settings.AdServerUrl;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Build the Ad Tag Url by concatenating the Base Tracking Url with a Json stringified object
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		public string Generate(string adTagId, int platformId)
		{
			string adTagUrl = null;
			var settings = IoC.Resolve<ISettingsService>();
			var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];

			if (platformId == roku)
				adTagUrl = string.Format("{0}/?id={1}&{2}&{3}", BaseUrl, adTagId, AdTagUrlConstants.RokuAdIdMacro, AdTagExportConstants.AdTagUrlPostfix);
			else
				adTagUrl = string.Format("{0}/?id={1}&{2}", BaseUrl, adTagId, AdTagExportConstants.AdTagUrlPostfix);

			return adTagUrl;
		}

		#endregion
	}
}
