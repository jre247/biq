﻿using BrightLine.Common.ViewModels.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface ICampaignsListingService
	{
		List<CampaignsListingViewModel> GetCampaignsListing();
	}
}
