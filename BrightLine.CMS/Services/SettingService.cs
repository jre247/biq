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

namespace BrightLine.CMS.Services
{
	public class SettingService : CrudService<CmsSetting>, ISettingService
	{
		public SettingService(IRepository<CmsSetting> repo)
			: base(repo)
		{
			
		}

		/// <summary>
		/// Gets a dictionary of features for a creative, where each feature is a dictionary of that feature's models
		/// </summary>
		/// <param name="creativeId"></param>
		/// <returns></returns>
		public CreativeFeaturesViewModel GetSettingsForCreative(int creativeId)
		{
			var creatives = IoC.Resolve<ICreativeService>();
			var creativeViewModel = new CreativeFeaturesViewModel();
			
			var creative = creatives.Get(creativeId);
			if (creative == null)
				return null;

			var featureSettings = base.Where(m => m.Feature.Creative.Id == creativeId).Select(f => new CreativeFeaturesViewModel.SettingViewModel
			{
				id = f.Id,
				name = f.Name,
				featureId = f.Feature.Id,
				settingInstanceId = f.CmsSettingInstance.Id
			}).GroupBy(c => c.featureId);

			if (featureSettings != null)
				creativeViewModel = CreativeFeaturesViewModel.ToCreativeFeaturesSettingsViewModel(featureSettings.ToList());

			return creativeViewModel;
		}
	}
}
