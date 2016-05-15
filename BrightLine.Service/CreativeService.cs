using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Framework;
using Newtonsoft.Json;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Common.ViewModels.Campaigns;

namespace BrightLine.Service
{
	public class CreativeService : CrudService<Creative>, ICreativeService
	{
		public CreativeService(IRepository<Creative> repo)
			: base(repo)
		{ 
		}

		/// <summary>
		/// Save a Creative
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		public Creative Save(DestinationCreativeSaveViewModel viewModel)
		{	
			var creative = Get(viewModel.id);
			if (creative == null)
				creative = new Creative();

			SaveCreativePrimaryFields(viewModel, creative);

			SaveFeatures(viewModel, creative);

			SaveRepoNameForAds(viewModel);

			var isEdit = viewModel.id > 0;
			if (isEdit)
				creative = Update(creative);
			else
				creative = Create(creative);

			return creative;
		}

		public override Creative Create(Creative creative)
		{
			var exists = base.Where(c => c.Campaign.Id == creative.Campaign.Id && string.Equals(c.Name, creative.Name)).Any();
			if (exists)
				throw new ValidationException("Creative with the same name exists for this campaign.");

			var newCreative = base.Create(creative);

			AssignCreativeToResources(creative);

			if (!creative.AdType.IsPromo)
			{
				//only create models and settings for destination creatives
				CreateModelsAndSettings(newCreative);	
			}
				
			return newCreative;
		}
	
		public override List<Creative> Create(IEnumerable<Creative> creatives)
		{
			if (creatives == null)
				return new List<Creative>();

			foreach (var creative in creatives)
			{
				var exists = base.Where(c => c.Campaign.Id == creative.Campaign.Id && string.Equals(c.Name, creative.Name)).Any();
				if (exists)
					throw new ValidationException("Creative with the same name exists for this campaign.");

				Repository.Insert(creative);
			}

			this.Save();
			return creatives.ToList();
		}

		public override Creative Update(Creative creative)
		{
			var exists = base.Where(c => creative.Id != c.Id && c.Campaign.Id == creative.Campaign.Id && string.Equals(c.Name, creative.Name)).Any();
			if (exists)
				throw new ValidationException("Creative with the same name exists for this campaign.");

			AssignCreativeToResources(creative);

			return base.Update(creative);
		}

		public override List<Creative> Update(IEnumerable<Creative> creatives)
		{
			if (creatives == null)
				return new List<Creative>();

			foreach (var creative in creatives)
			{
				var exists = base.Where(c => creative.Id != c.Id && c.Campaign.Id == creative.Campaign.Id && string.Equals(c.Name, creative.Name)).Any();
				if (exists)
					throw new ValidationException("Creative with the same name exists for this campaign.");

				Repository.Update(creative);
			}

			this.Save();
			return creatives.ToList();
		}

		public Dictionary<int, PageViewModel> GetPagesForCreative(int creativeId)
		{
			var pagesRepo = IoC.Resolve<IRepository<Page>>();

			var pages = pagesRepo.Where(p => p.Feature.Creative.Id == creativeId);
			if (pages == null)
				return null;

			var pagesDictionary = pages.Select(m => new PageViewModel
			{
				id = m.Id,
				name = m.Name
			}).ToList().ToDictionary(c => c.id, c => c);

			return pagesDictionary;
		}

		public IEnumerable<CampaignCreativeViewModel> GetCreativesForCampaign(int campaignId, bool isPromo)
		{
			var campaignsRepo = IoC.Resolve<ICampaignService>();
			var adTypesRepo = IoC.Resolve<IRepository<AdType>>();
			var adFunctionsRepo = IoC.Resolve<IRepository<AdFunction>>();
			var resourcesRepo = IoC.Resolve<IResourceService>();
			var featuresRepo = IoC.Resolve<IRepository<Feature>>();

			var creatives = GetAll();
			var adTypes = adTypesRepo.GetAll();
			var adFunctions = adFunctionsRepo.GetAll();
			var resources = resourcesRepo.GetAll();
			var campaigns = campaignsRepo.GetAll();
			var features = featuresRepo.GetAll();

			var creativesViewModel = (from cr in creatives
									  join c in campaigns on cr.Campaign.Id equals c.Id
									  join at in adTypes on (cr != null ? cr.AdType.Id : (int?)null) equals at.Id into atcr
									  from at in atcr.DefaultIfEmpty()
									  join af in adFunctions on (cr != null ? cr.AdFunction.Id : (int?)null) equals af.Id into afcr
									  from af in afcr.DefaultIfEmpty()
									  where c.Id == campaignId && at != null && at.IsPromo == isPromo
									  let resource = resources.Where(r => r.Creative.Id == cr.Id).OrderByDescending(r => r.DateCreated).FirstOrDefault()
									  select new CampaignCreativeViewModel
									  {
										  id = cr.Id,
										  name = cr.Name,
										  lastModifiedRaw = cr.DateUpdated ?? cr.DateCreated,
										  isDeleted = cr.IsDeleted,
										  campaignId = c.Id,
										  adTypeId = (at != null ? at.Id : (int?)null),
										  adTypeName = (at != null ? at.Name : null),
										  adFunctionId = (af != null ? af.Id : (int?)null),
										  adFunctionName = (af != null ? af.Name : null),
										  isPromo = (at != null ? at.IsPromo : false),
										  resourceId = resource.Id,
										  resourceName = resource.Name,
										  resourceFilename = resource.Filename,
										  resourceType = resource.ResourceType.Id,
										  features = (from f in features
													  join crr in creatives on f.Creative.Id equals crr.Id
													  where f.Campaign.Id == campaignId
													  select f.Id).Distinct()
									  }
					).ToList();
			return creativesViewModel.Distinct();
		}

		public PromotionalCreativeViewModel GetPromotionalCreative(int creativeId)
		{
			var creative = Where(c => c.Id == creativeId && c.AdType.IsPromo).SingleOrDefault();
			if (creative == null)
				return null;

			var viewModel = PromotionalCreativeViewModel.FromCreative(creative);

			return viewModel;
		}

		public DestinationCreativeViewModel GetDestinationCreative(int creativeId)
		{
			var creative = Where(c => c.Id == creativeId && !c.AdType.IsPromo).SingleOrDefault();
			if (creative == null)
				return null;

			var viewModel = DestinationCreativeViewModel.FromCreative(creative);

			return viewModel;
		}


		#region Private Methods

		/// <summary>
		/// Save the primary fields for a Creative
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="creative"></param>
		private void SaveCreativePrimaryFields(DestinationCreativeSaveViewModel viewModel, Creative creative)
		{
			var campaigns = IoC.Resolve<ICampaignService>();
			var adFunctions = IoC.Resolve<IRepository<AdFunction>>();
			var adTypes = IoC.Resolve<IRepository<AdType>>();

			creative.Name = viewModel.name;
			creative.Description = viewModel.description;
			creative.Campaign = campaigns.Get(viewModel.campaign);
			creative.Thumbnail_Id = viewModel.Thumbnail_Id;
			creative.AdFunction = adFunctions.Get(viewModel.adFunction);
			creative.InactivityThreshold = viewModel.inactivityThreshold;
			creative.AdType = adTypes.Get(viewModel.adType);
			creative.ResourceIds = viewModel.ResourceIds; // This property will be referenced when setting a Creative for an uploaded Resource in the method AssignCreativeToResources
		}

		/// <summary>
		/// Save Features for Creative
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="creative"></param>
		private void SaveFeatures(DestinationCreativeSaveViewModel viewModel, Creative creative)
		{
			if (viewModel.features == null)
				return;

			var blueprints = IoC.Resolve<IBlueprintService>();
			var campaigns = IoC.Resolve<ICampaignService>();
			var featureTypes = IoC.Resolve<IRepository<FeatureType>>();
			
			if (creative.Features == null)
				creative.Features = new List<Feature>();

			// Save each Feature
			foreach (var featureVm in viewModel.features)
			{	
				Feature feature = null;
				var isFeatureEdit = featureVm.id > 0;

				if (isFeatureEdit)
				{
					feature = creative.Features.Where(f => f.Id == featureVm.id).SingleOrDefault();
				}
				else
				{
					feature = new Feature();
					creative.Features.Add(feature);
				}

				// Save Feature primary fields
				feature.Blueprint = blueprints.Get(featureVm.blueprint);
				feature.Campaign = campaigns.Get(featureVm.campaign);
				feature.FeatureType = featureTypes.Get(featureVm.featureType);
				feature.Name = featureVm.name;
				feature.Creative = creative;

				SavePagesForFeature(featureVm, feature);
			}
		}

		/// <summary>
		/// Save Pages for a given Feature
		/// </summary>
		/// <param name="featureVm"></param>
		/// <param name="feature"></param>
		private void SavePagesForFeature(DestinationCreativeSaveViewModel.FeatureViewModel featureVm, Feature feature)
		{
			var pageDefinitions = IoC.Resolve<IPageDefinitionService>();

			// Save Feature's Pages
			foreach (var pageVm in featureVm.pages)
			{
				Page page = null;
				var isPageEdit = pageVm.id > 0;

				if (feature.Pages == null)
					feature.Pages = new List<Page>();

				if (isPageEdit)
				{
					page = feature.Pages.Where(p => p.Id == pageVm.id).SingleOrDefault();
				}
				else
				{
					page = new Page();
					feature.Pages.Add(page);
				}

				// Save Page's primary fields
				page.Name = pageVm.name;
				page.PageDefinition = pageDefinitions.Get(pageVm.pageDefinition);
				page.Feature = feature;
			}
		}
		
		/// <summary>
		/// Save the RepoName field for all of the Ads that implement a specific Creative
		/// </summary>
		/// <param name="viewModel"></param>
		private void SaveRepoNameForAds(DestinationCreativeSaveViewModel viewModel)
		{
			var adsService = IoC.Resolve<IAdService>();

			foreach (var ad in viewModel.ads)
			{
				var adDb = adsService.Get(ad.id);
				if (adDb == null)
				{
					IoC.Log.Error("There was an attempt to save an Ad's RepoName for an Ad that does not exist. Ad Id: " + ad.id);
					continue;
				}

				adDb.RepoName = ad.repoName;
				adsService.Update(adDb);
			}
		}

		/// <summary>
		/// Assign a Creative to a list of Resources
		/// </summary>
		/// <param name="creative"></param>
		private void AssignCreativeToResources(Creative creative)
		{
			if (string.IsNullOrEmpty(creative.ResourceIds))
				return;

			var resources = IoC.Resolve<IResourceService>();

			var resourceIdList = creative.ResourceIds.Split(',').Select(int.Parse).ToList();

			//deactivate resources that are currently assigned to this creative
			var resourcesToDeactivate = resources.Where(r => !r.IsDeleted && r.Creative != null && r.Creative.Id == creative.Id && !resourceIdList.Contains(r.Id)).ToList();
			foreach(var resourceToDeactivate in resourcesToDeactivate){
				resourceToDeactivate.IsDeleted = true;
			}

			foreach (var resourceId in resourceIdList)
			{
				var resource = resources.Get(resourceId);
				resource.Creative = creative;
			}	

			resources.Save();
		}


		private void CreateModelsAndSettings(Creative newCreative)
		{
			if (newCreative.Features == null)
				return;

			foreach (var feature in newCreative.Features)
			{
				CreateModels(feature);
				CreateSettings(feature);
			}
		}

		private void CreateSettings(Feature feature)
		{
			var cmsSettingInstancesRepo = IoC.Resolve<IRepository<CmsSettingInstance>>();

			if (feature.Blueprint.SettingDefinitions == null || feature.Blueprint.SettingDefinitions.Count() == 0)
				return;

			var cmsSettings = IoC.Resolve<IRepository<CmsSetting>>();

			//note: there should only be one setting definition per blueprint
			var settingDefinition = feature.Blueprint.SettingDefinitions.ElementAt(0);

			var setting = new CmsSetting
			{
				Feature = feature,
				Name = settingDefinition.Name,
				CmsSettingDefinition = settingDefinition
			};

			//TODO: note that it is more efficient to follow Entity Framework's 1:1 mapping between CmsSetting and CmsSettingInstance, however there will need to be refactoring changes to allow for this
			cmsSettings.Insert(setting);
			cmsSettings.Save();
						
			//create setting instance 
			var settingInstance = new CmsSettingInstance
			{
				Name = setting.Name,
				Display = setting.Display,
				Setting_Id = setting.Id
			};

			cmsSettingInstancesRepo.Insert(settingInstance);
			cmsSettingInstancesRepo.Save();

			setting.CmsSettingInstance = settingInstance;
			cmsSettingInstancesRepo.Save();

			//save instance json
			var settingInstanceJsonVm = new ModelInstanceJsonViewModel
			{
				id = settingInstance.Id,
				name = setting.Name,
				fields = new List<FieldViewModel>()
			};
			foreach(var field in setting.CmsSettingDefinition.Fields)
			{
				var instanceField = new FieldViewModel(field);
				settingInstanceJsonVm.fields.Add(instanceField);
			}

			settingInstance.Json = JsonConvert.SerializeObject(settingInstanceJsonVm);
			cmsSettingInstancesRepo.Save();
		}

		private void CreateModels(Feature feature)
		{
			if (feature.Blueprint.ModelDefinitions == null)
				return;

			var cmsModels = IoC.Resolve<IRepository<CmsModel>>();

			foreach (var modelDefinition in feature.Blueprint.ModelDefinitions)
			{
				var model = new CmsModel
				{
					Feature = feature,
					Name = modelDefinition.Name,
					CmsModelDefinition = modelDefinition,
				};

				cmsModels.Insert(model);
				cmsModels.Save();
			}
		}

		

		#endregion Private Methods
	}
}
