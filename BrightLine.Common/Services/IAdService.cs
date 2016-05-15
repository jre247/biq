using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using System.Collections.Generic;
using System.IO;

namespace BrightLine.Common.Services
{
	public interface IAdService : ICrudService<Ad>
	{
		Ad SaveAd(AdViewModel adViewModel);
		List<CampaignAdViewModel> GetAdsForCampaign(Campaign campaign);
		void CleanupDirtyAds();
	}
}
