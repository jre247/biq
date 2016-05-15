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
	public interface IMediaPartnerService : ICrudService<MediaPartner>
	{
		MediaPartnerViewModel GetViewModel(MediaPartner MediaPartner);
		MediaPartnerViewModel GetViewModel();
		MediaPartner Save(MediaPartnerViewModel model);
		void FillSelectListsForViewModel(MediaPartnerViewModel vm);
	}
}
