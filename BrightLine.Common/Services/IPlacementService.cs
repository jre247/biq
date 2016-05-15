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
	public interface IPlacementService : ICrudService<Placement>
	{
		PlacementViewModel GetViewModel(Placement placement);
		PlacementViewModel GetViewModel();
		Placement Save(PlacementViewModel model);
		void FillSelectListsForViewModel(PlacementViewModel vm);
	}
}
