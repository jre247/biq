using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using System.Collections.Generic;

namespace BrightLine.Common.Services
{
	public interface ICreativeService:ICrudService<Creative>
	{
		Dictionary<int, PageViewModel> GetPagesForCreative(int creativeId);

		PromotionalCreativeViewModel GetPromotionalCreative(int id);

		DestinationCreativeViewModel GetDestinationCreative(int creativeId);

		IEnumerable<CampaignCreativeViewModel> GetCreativesForCampaign(int campaignId, bool isPromo);

		Creative Save(DestinationCreativeSaveViewModel viewModel);
	}
}
