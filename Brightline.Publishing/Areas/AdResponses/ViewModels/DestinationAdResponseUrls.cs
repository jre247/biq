using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightline.Publishing.Areas.AdResponses.ViewModels
{
	public class DestinationAdResponseUrls
	{
		#region Private Members

		private string Images { get; set; }
		private string Fonts { get; set; }
		private string Videos { get; set; }

		#endregion

		#region Public Members

		public string images {get{ return Images; }}
		public string fonts { get { return Fonts; } }
		public string videos { get { return Videos; } }	

		#endregion

		#region Init

		public DestinationAdResponseUrls(Ad ad)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			if (ad.Campaign.Generation == 1)
			{
				Images = string.Format("{0}/ads/{1}/1.0/assets/images/", settings.MediaG1CDNBaseUrl, ad.RepoName);
				Fonts = string.Format("{0}/ads/{1}/1.0/assets/fonts/", settings.MediaG1CDNBaseUrl, ad.RepoName);
				Videos = string.Format("{0}/videos/ads/{1}/", settings.MediaG1CDNBaseUrl, ad.RepoName);
			}
			else
			{
				Images = resourceHelper.GetBaseUrlForMediaResourceType(ad.Campaign.Id, MediaResourceType.Images);
				Fonts = resourceHelper.GetBaseUrlForMediaResourceType(ad.Campaign.Id, MediaResourceType.Fonts);
				Videos = resourceHelper.GetBaseUrlForMediaResourceType(ad.Campaign.Id, MediaResourceType.Videos);
			}
		}

		#endregion
	}
}
