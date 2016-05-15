using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IAdTagsExportService
	{
		void ExportAdTags(Stream stream, Campaign campaign);
		List<AdTagExportViewModel> GetCampaignAds(Campaign campaign);
		List<AdTagExportViewModel> SetAdTagAndUrls(List<AdTagExportViewModel> ads, Campaign campaign);
	}
}
