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
	public interface IAppService : ICrudService<App>
	{
		AppViewModel GetViewModel(App app);
		AppViewModel GetViewModel();
		App Save(AppViewModel model);
		void FillSelectListsForViewModel(AppViewModel vm);
	}
}
