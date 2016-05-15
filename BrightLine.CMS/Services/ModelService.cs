using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace BrightLine.CMS.Services
{
	public class ModelService : CrudService<CmsModel>, IModelService
	{

		public ModelService(IRepository<CmsModel> repo)
			: base(repo)
		{	}

		/// <summary>
		/// Gets a dictionary of features for a creative, where each feature is a dictionary of that feature's models
		/// </summary>
		/// <param name="creativeId"></param>
		/// <returns></returns>
		public CreativeFeaturesViewModel GetModelsForCreative(int creativeId)
		{
			var creatives = IoC.Resolve<ICreativeService>();

			var creativeViewModel = new CreativeFeaturesViewModel();
			
			var creative = creatives.Get(creativeId);
			if (creative == null)
				return null;

			var featureModels = base.Where(m => m.Feature.Creative.Id == creativeId).Select(f => new CreativeFeaturesViewModel.ModelViewModel
			{
				id = f.Id,
				name = f.Name,
				display = f.Display ?? f.CmsModelDefinition.Display,
				featureId = f.Feature.Id,
				modelDefinitionId = f.CmsModelDefinition.Id
			}).GroupBy(c => c.featureId).ToList();

			//pluralize each model name in each feature 
			//*note: cannot immediately pluralize model name in linq statement above because linq to queries would bomb, since the pluralize method cannot be translated into a sql statement
			foreach (var feature in featureModels)
			{
				foreach (var model in feature)
				{
					if (model.display == null)
						continue;

					model.displayNamePlural = PluralizationService.CreateService(new CultureInfo("en-US")).Pluralize(model.display);
				};
			};

			if (featureModels != null)
				creativeViewModel = CreativeFeaturesViewModel.ToCreativeFeaturesModelsViewModel(featureModels.ToList());

			return creativeViewModel;
		}
	}
}
