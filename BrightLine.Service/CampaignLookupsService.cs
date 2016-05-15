using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class CampaignLookupsService : ICampaignLookupsService
	{
		#region Public Methods

		public JObject GetCampaignLookups()
		{
			var platformsRepo = IoC.Resolve<IRepository<Platform>>();
			var appsRepo = IoC.Resolve<IAppService>();
			var categoriesRepo = IoC.Resolve<IRepository<Category>>();
			var adFunctionsRepo = IoC.Resolve<IRepository<AdFunction>>();
			var adTypeGroupsRepo = IoC.Resolve<IRepository<AdTypeGroup>>();
			var adFormatsRepo = IoC.Resolve<IRepository<AdFormat>>();
			var pageDefinitionsRepo = IoC.Resolve<IPageDefinitionService>();
			var trackingEventsRepo = IoC.Resolve<IRepository<TrackingEvent>>();

			// general lookups
			var lookups = new[]
				{
					new EntityLookups { Name = LookupsConstants.Entities.APPS, Values = appsRepo.GetAll().ToLookups(LookupsConstants.Name) },
					new EntityLookups { Name = LookupsConstants.Entities.CATEGORIES, Values = categoriesRepo.GetAll() },
					new EntityLookups { Name = LookupsConstants.Entities.AD_FUNCTIONS, Values = adFunctionsRepo.GetAll() },
					new EntityLookups { Name = LookupsConstants.Entities.AD_TYPE_GROUPS, Values = adTypeGroupsRepo.GetAll() },
					new EntityLookups { Name = LookupsConstants.Entities.AD_FORMATS, Values = adFormatsRepo.GetAll() },
					new EntityLookups { Name = LookupsConstants.Entities.PAGE_DEFINITIONS, Values = pageDefinitionsRepo.GetAll() },
					new EntityLookups { Name = LookupsConstants.Entities.STATUSES, Values = CampaignHelper.GetStatusList() },
					new EntityLookups { Name = LookupsConstants.Entities.TRACKING_EVENTS, Values = trackingEventsRepo.GetAll().OrderBy(t => t.Name).ToLookups(LookupsConstants.Name) }
				};

			var json = EntityLookups.ToJObject(lookups);

			// special lookups
			GetPlatformLookups(json);
			GetAdTypeLookups(json);
			GetBlueprintLookups(json);
			GetFeatureTypeLookups(json);
			GetMetricsLookups(json);
			GetMediaPartnerLookups(json);

			return json;
		}

		#endregion

		#region Private Methods

		private void GetPlatformLookups(JObject json)
		{
			var platformsRepo = IoC.Resolve<IRepository<Platform>>();

			var platforms = platformsRepo.GetAll().OrderBy(m => m.Name);
			var platformsDictionary = new Dictionary<int, object>();
			foreach (var platform in platforms)
			{
				var p = new
				{
					id = platform.Id,
					name = platform.Name,
					manifestName = platform.ManifestName
				};
				platformsDictionary.Add(platform.Id, p);
			}
			json[LookupsConstants.Entities.PLATFORMS] = JObject.FromObject(platformsDictionary);
		}

		private void GetAdTypeLookups(JObject json)
		{
			var adTypesRepo = IoC.Resolve<IRepository<AdType>>();

			var adTypes = adTypesRepo.GetAll().OrderBy(at => at.Name);
			var filteredAdTypes = FilterAdTypes(adTypes);

			var ats = new Dictionary<int, object>();
			foreach (var adType in filteredAdTypes)
			{
				var adTypeGroups = adType.AdTypeGroups.Select(atg => atg.Id).ToArray();
				var at = new
				{
					id = adType.Id,
					name = adType.Name,
					isPromo = adType.IsPromo,
					adTypeGroupId = adTypeGroups,
				};
				ats.Add(adType.Id, at);
			}
			json[LookupsConstants.Entities.AD_TYPES] = JObject.FromObject(ats);
		}

		/// <summary>
		/// Filter out any specified Ad Types from the list of all Ad Types
		/// </summary>
		/// <param name="adTypes"></param>
		/// <returns></returns>
		private IEnumerable<AdType> FilterAdTypes(IQueryable<AdType> adTypes)
		{
			//filter out the Brand Destination Ad Type from list of Ad Types
			var brandDestinationAdTypeId = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			return adTypes.Where(a => a.Id != brandDestinationAdTypeId).ToList();
		}

		private void GetBlueprintLookups(JObject json)
		{
			var blueprintsRepo = IoC.Resolve<IBlueprintService>();
			var pageDefinitionsRepo = IoC.Resolve<IPageDefinitionService>();
			var fileHelper = IoC.Resolve<IFileHelper>();

			var blueprints = blueprintsRepo.GetAll().OrderBy(bp => bp.Name);

			var bps = new Dictionary<int, object>();
			foreach (var blueprint in blueprints)
			{
				var pageDefinitionIds = pageDefinitionsRepo.GetAll().Where(p => p.Blueprint.Id == blueprint.Id).Select(p => p.Id).ToList();

				var bp = new
				{
					id = blueprint.Id,
					name = blueprint.Name,
					manifestName = blueprint.ManifestName,
					//TODO: Add placeholder image for blueprint thumbnail
					thumbnail = ((blueprint.Preview != null) ? fileHelper.GetCloudFileDownloadUrl(blueprint.Preview) : ""),
					pageDefinitionIds = pageDefinitionIds,
				};
				bps.Add(blueprint.Id, bp);
			}
			json[LookupsConstants.Entities.BLUEPRINTS] = JObject.FromObject(bps);
		}

		private void GetFeatureTypeLookups(JObject json)
		{
			var featureTypesRepo = IoC.Resolve<IRepository<FeatureType>>();
			var fileHelper = IoC.Resolve<IFileHelper>();

			var featureTypes = featureTypesRepo.GetAll().OrderBy(ft => ft.Name);
			var fts = new Dictionary<int, object>();
			foreach (var featureType in featureTypes.Where(ft => ft.Blueprints.Any()))
			{
				var ftbps = featureType.Blueprints.ToArray();
				var thumbnail = "";
				var thumbnailPreview = ftbps.FirstOrDefault(ftbp => ftbp.Preview != null);
				if (thumbnailPreview != null)
					thumbnail = fileHelper.GetCloudFileDownloadUrl(thumbnailPreview.Preview);
				var ft = new
				{
					id = featureType.Id,
					name = featureType.Name,
					blueprints = ftbps.Select(bp => bp.Id),
					thumbnail = thumbnail
				};
				fts.Add(featureType.Id, ft);
			}
			json[LookupsConstants.Entities.FEATURE_TYPES] = JObject.FromObject(fts);
		}

		private void GetMetricsLookups(JObject json)
		{
			var metricsRepo = IoC.Resolve<IRepository<Metric>>();

			var metrics = metricsRepo.GetAll().OrderBy(m => m.Name);
			var mts = new Dictionary<int, object>();
			foreach (var metric in metrics)
			{
				var m = new
				{
					id = metric.Id,
					name = metric.Name,
					type = metric.Type.ToString(),
					metricType = (int)metric.Type,
					color = metric.HexColor,
				};
				mts.Add(metric.Id, m);
			}
			json[LookupsConstants.Entities.METRICS] = JObject.FromObject(mts);
		}

		private void GetMediaPartnerLookups(JObject json)
		{
			var mediaPartnersRepo = IoC.Resolve<IMediaPartnerService>();

			var mediaPartners = mediaPartnersRepo.GetAll();

			if (Auth.UserModel.MediaPartner != null)
			{
				var mediaPartnerId = Auth.UserModel.MediaPartner.Id;
				mediaPartners = mediaPartners.Where(d => (d.Id == mediaPartnerId) || d.Parent_Id == mediaPartnerId);
			}

			mediaPartners = mediaPartners.OrderBy(m => m.Name);

			var mediaPartnersHash = mediaPartners.Select(d => new
			{
				id = d.Id,
				name = d.Name
			}).ToList().ToDictionary(c => c.id, c => c);

			json[LookupsConstants.Entities.MEDIA_PARTNERS] = JObject.FromObject(mediaPartnersHash);

		}



		#endregion
	}
}
