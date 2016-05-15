using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;

namespace BrightLine.Common.Services
{
	public interface IResourceService : ICrudService<Resource>
	{
		ResourceViewModel Register(ResourceViewModel model);
		void Upload(int resourceId, int campaignId, byte[] contents, bool isCms = false);
	}
}
