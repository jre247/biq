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
	public class PlacementService: CrudService<Placement>, IPlacementService
	{
		public PlacementService(IRepository<Placement> repo)
			: base(repo)
		{
			Repository = repo;
		}

		public PlacementViewModel GetViewModel()
		{
			var vm = new PlacementViewModel();
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public PlacementViewModel GetViewModel(Placement placement)
		{
			if (placement == null)
				return null;

			var vm = new PlacementViewModel(placement);
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public Placement Save(PlacementViewModel model)
		{
			var adTypeGroupsRepo = IoC.Resolve<IRepository<AdTypeGroup>>();
			var categoriesRepo = IoC.Resolve<IRepository<Category>>();
			var mediaPartnersRepo = IoC.Resolve<IMediaPartnerService>();
			var appsRepo = IoC.Resolve<IAppService>();

			Placement placement = null;

			if (model.Id == 0)
				placement = new Placement();
			else
				placement = Get(model.Id);

			placement.Name = model.Name;
			placement.AdTypeGroup = adTypeGroupsRepo.Get(model.SelectedAdTypeGroup.Id);
			placement.Category = categoriesRepo.Get(model.SelectedCategory.Id);
			placement.MediaPartner = mediaPartnersRepo.Get(model.SelectedMediaPartner.Id);
			placement.Height = model.Height;
			placement.Width = model.Width;
			placement.LocationDetails = model.LocationDetails;

			if (model.SelectedApp != null && model.SelectedApp.Id > 0)
				placement.App = appsRepo.Get(model.SelectedApp.Id);
			else
				placement.App_Id = null; //setting App navigation property to null. Entity Framework's change tracker doesn't pick up on the change. Instead setting App_Id to null works.

			placement = Upsert(placement);

			return placement;
		}

		public void FillSelectListsForViewModel(PlacementViewModel vm)
		{
			var categoriesRepo = IoC.Resolve<IRepository<Category>>();
			var mediaPartnersRepo = IoC.Resolve<IMediaPartnerService>();
			var appsRepo = IoC.Resolve<IAppService>();
			var adTypeGroupsRepo = IoC.Resolve<IRepository<AdTypeGroup>>();

			vm.Categories = categoriesRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);

			vm.AdTypeGroups = adTypeGroupsRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);

			vm.MediaPartners = mediaPartnersRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);

			vm.Apps = appsRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);
		}

		
	}
}
