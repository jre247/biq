using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Interfaces
{
	public interface IAdResponsesService
	{
		void Publish(Campaign campaign, string targetEnv, Guid publishId);
		Dictionary<string, Dictionary<string, AdResponseViewModel>> GetAdResponse(Ad ad, string targetEnv, Guid guid);
		Dictionary<string, string> GetAdResponsesForCampaign(Campaign campaign, string targetEnv, Guid publishId);
	}
}
