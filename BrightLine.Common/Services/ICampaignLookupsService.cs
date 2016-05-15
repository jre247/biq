using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface ICampaignLookupsService
	{
		JObject GetCampaignLookups();
	}
}
