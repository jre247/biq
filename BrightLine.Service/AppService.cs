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
	public class AppService : CrudService<App>, IAppService
	{
		private IMediaPartnerService MediaPartners { get;set;}
		private IRepository<Advertiser> Advertisers { get;set;}

		public AppService(IRepository<App> repo)
			: base(repo)
		{
			MediaPartners = IoC.Resolve<IMediaPartnerService>();
			Advertisers = IoC.Resolve<IRepository<Advertiser>>();
		}

		public AppViewModel GetViewModel()
		{
			var vm = new AppViewModel();
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public AppViewModel GetViewModel(App app)
		{
			if (app == null)
				return null;

			var vm = new AppViewModel(app);
			FillSelectListsForViewModel(vm);

			return vm;
		}

		public App Save(AppViewModel model)
		{
			App App = null;

			if (model.Id == 0)
				App = new App();
			else
				App = Get(model.Id);

			App.Name = model.Name;
			App.MediaPartner = MediaPartners.Get(model.SelectedMediaPartner.Id);

			if (model.SelectedAdvertiser != null && model.SelectedAdvertiser.Id > 0)
				App.Advertiser = Advertisers.Get(model.SelectedAdvertiser.Id);
			else
				App.Advertiser_Id = null; //setting Advertiser navigation property to null. Entity Framework's change tracker doesn't pick up on the change. Instead setting Advertiser_Id to null works.

			App = Upsert(App);

			return App;
		}

		public void FillSelectListsForViewModel(AppViewModel vm)
		{
			vm.MediaPartners = MediaPartners.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);

			vm.Advertisers = Advertisers.GetAll().
				OrderBy(p => p.Name).
				ToLookups().
				InsertLookups(0, EntityLookup.Select);
		}

		
	}
}
