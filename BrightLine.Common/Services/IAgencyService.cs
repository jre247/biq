using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IAgencyService : ICrudService<Agency>
	{
		AgencyViewModel GetViewModel(Agency Agency);
		AgencyViewModel GetViewModel();
		Agency Save(AgencyViewModel model);
	}
}
