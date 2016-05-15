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
	public class MediaPartnerService : CrudService<MediaPartner>, IMediaPartnerService
	{
		public MediaPartnerService(IRepository<MediaPartner> repo)
			: base(repo)
		{

		}

		public MediaPartnerViewModel GetViewModel()
		{
			var vm = new MediaPartnerViewModel();
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public MediaPartnerViewModel GetViewModel(MediaPartner mediaPartner)
		{
			if(mediaPartner == null)
				return null;

			var vm = new MediaPartnerViewModel(mediaPartner);
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public MediaPartner Save(MediaPartnerViewModel model)
		{
			var categoriesRepo = IoC.Resolve<IRepository<Category>>();
			MediaPartner mediaPartner = null;

			if (model.Id == 0)
				mediaPartner = new MediaPartner();
			else
				mediaPartner = Get(model.Id);
	
			mediaPartner.IsNetwork = model.IsNetwork;
			mediaPartner.Name = model.Name;
			mediaPartner.ManifestName = model.ManifestName;

			if (model.SelectedParent != null && model.SelectedParent.Id > 0)
				mediaPartner.Parent = Get(model.SelectedParent.Id);
			else
				mediaPartner.Parent_Id = null; //setting Parent navigation property to null. Entity Framework's change tracker doesn't pick up on the change. Instead setting Parent_Id to null works.

			mediaPartner.Category = categoriesRepo.Get(model.SelectedCategory.Id);
			mediaPartner = Upsert(mediaPartner);
			
			return mediaPartner;
		}

		public void FillSelectListsForViewModel(MediaPartnerViewModel vm)
		{
			var categoriesRepo = IoC.Resolve<IRepository<Category>>();

			vm.Categories = categoriesRepo.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);

			vm.Parents = GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);
		}

		
	}
}
