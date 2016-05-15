using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdTagExport;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Common.Utility.Spreadsheets.Writer;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class CmsModelInstanceService : CrudService<CmsModelInstance>, ICmsModelInstanceService
	{
		public CmsModelInstanceService(IRepository<CmsModelInstance> repo)
			: base(repo)
		{ 

		}

		
	}
}
