using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface ICampaignPermissionsService
	{
		bool IsAccessible(Campaign campaign);
		bool IsCampaignMediaPartner(Campaign campaign);
		bool IsCampaignMediaAgency(Campaign campaign);
		bool IsCampaignAdvertiser(Campaign campaign);
	}
}
