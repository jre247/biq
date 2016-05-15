using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.AdTypes.PlatformAdResponses.Interfaces
{
	public interface IPlatformAdResponseService
	{
		AdResponseViewModel GetAdResponse();
	}
}
