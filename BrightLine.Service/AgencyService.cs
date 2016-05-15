using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class AgencyService : CrudService<Agency>, IAgencyService
	{
		public AgencyService(IRepository<Agency> repo)
			: base(repo)
		{
		}

		public AgencyViewModel GetViewModel()
		{
			var vm = new AgencyViewModel();
			GetLookups(vm);

			return vm;
		}

		public AgencyViewModel GetViewModel(Agency agency)
		{
			if(agency == null)
				return null;

			var vm = new AgencyViewModel(agency);
			GetLookups(vm);

			return vm;
		}

		public Agency Save(AgencyViewModel model)
		{
			Agency agency = null;

			if (model.Id == 0)
				agency = new Agency();
			else
				agency = Get(model.Id);

			if (model.SelectedParent != null && model.SelectedParent.Id > 0)
				agency.Parent = Get(model.SelectedParent.Id);
			else
				agency.Parent_Id = null; //setting Parent navigation property to null. Entity Framework's change tracker doesn't pick up on the change. Instead setting Parent_Id to null works.

			agency.Name = model.Name;
			agency = Upsert(agency);
			
			return agency;
		}

		public void GetLookups(AgencyViewModel vm)
		{
			vm.Parents = GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);
		}

		
	}
}
