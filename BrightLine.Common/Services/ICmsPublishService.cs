using BrightLine.Common.Campaigns.ViewModels;
using BrightLine.Common.Models;
using BrightLine.Common.ViewModels;
using BrightLine.Core;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IPublishService : ICrudService<CmsPublish>
	{
		CmsPublishViewModel GetViewModel(int campaignId);
		BoolMessageItem Publish(int campaignId, string env);
		void PurgeCampaignAdResponses(int campaignId, string targetEnv);
	}
}
