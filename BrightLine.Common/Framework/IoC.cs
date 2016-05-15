using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using SimpleInjector;
using Common.Logging;
using BrightLine.Utility;
using BrightLine.Common.Utility;

namespace BrightLine.Common.Framework
{
	public static partial class IoC
	{
		private static readonly object LockObj = new object();

		private static Container _container = new Container();

		public static Container Container
		{
			get { return _container; }

			set
			{
				lock (LockObj)
				{
					_container = value;
				}
			}
		}

		public static T Resolve<T>() where T : class
		{
			return _container.GetInstance<T>();
		}

		public static object Resolve(Type type)
		{
			var serviceType = _serviceMap[type];
			return _container.GetInstance(serviceType);
		}

		public static ILog Log { get { return IoC.Resolve<ILogHelper>().GetLogger(); } }

		public static ICache Cache { get { return _container.GetInstance<ICache>(); } }

		private static Dictionary<Type, Type> _serviceMap = new Dictionary<Type, Type> { 
				{ typeof(BrightLine.Common.Models.AccountInvitation), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AccountInvitation>) },
				{ typeof(BrightLine.Common.Models.AccountRetrievalRequest), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Ad>) },
				{ typeof(BrightLine.Common.Models.Ad), typeof(BrightLine.Common.Services.IAdService) },
				{ typeof(BrightLine.Common.Models.AdTrackingEvent), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdTrackingEvent>) },
				{ typeof(BrightLine.Common.Models.AdFormat), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdFormat>) },
				{ typeof(BrightLine.Common.Models.AdFunction), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdFunction>) },
				{ typeof(BrightLine.Common.Models.AdTag), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdTag>) },
				{ typeof(BrightLine.Common.Models.AdType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdType>) },
				{ typeof(BrightLine.Common.Models.AdTypeGroup), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AdTypeGroup>) },
				{ typeof(BrightLine.Common.Models.Advertiser), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Advertiser>) },
				{ typeof(BrightLine.Common.Models.App), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.App>) },
				{ typeof(BrightLine.Common.Models.AppPlatform), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.AppPlatform>) },
				{ typeof(BrightLine.Common.Models.AuditEvent), typeof(BrightLine.Common.Services.IAuditEventService) },
				{ typeof(BrightLine.Common.Models.Blueprint), typeof(BrightLine.Common.Services.IBlueprintService) },
				{ typeof(BrightLine.Common.Models.CmsModelDefinition), typeof(BrightLine.Common.Services.ICmsModelDefinitionService) },
				{ typeof(BrightLine.Common.Models.CmsField), typeof(BrightLine.Common.Services.ICmsFieldService) },
				{ typeof(BrightLine.Common.Models.Validation), typeof(BrightLine.Common.Services.IValidationService) },
				{ typeof(BrightLine.Common.Models.ValidationType), typeof(BrightLine.Common.Services.IValidationTypeService) },
				{ typeof(BrightLine.Common.Models.FileTypeValidation), typeof(BrightLine.Common.Services.IFileTypeValidationService) },
				{ typeof(BrightLine.Common.Models.BlueprintPlatform), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.BlueprintPlatform>) },
				{ typeof(BrightLine.Common.Models.Brand), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Brand>) },
				{ typeof(BrightLine.Common.Models.Campaign), typeof(BrightLine.Common.Services.ICampaignService) },
				{ typeof(BrightLine.Common.Models.CampaignContentModel), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModel>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelBaseType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelBaseType>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelInstance), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelInstance>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelProperty), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelProperty>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelPropertyType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyType>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelPropertyValue), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyValue>) },
				{ typeof(BrightLine.Common.Models.CampaignContentModelType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentModelType>) },
				{ typeof(BrightLine.Common.Models.CampaignContentSchema), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentSchema>) },
				{ typeof(BrightLine.Common.Models.CampaignContentSettings), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CampaignContentSettings>) },
				{ typeof(BrightLine.Common.Models.Category), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Category>) },
				{ typeof(BrightLine.Common.Models.CmsModel), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CmsModel>) },
				{ typeof(BrightLine.Common.Models.Creative), typeof(BrightLine.Common.Services.ICreativeService) },
				{ typeof(BrightLine.Common.Models.CmsModelInstance), typeof(BrightLine.Common.Services.ICmsModelInstanceService) },
				{ typeof(BrightLine.Common.Models.Lookups.Expose), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Lookups.Expose>) },
				{ typeof(BrightLine.Common.Models.DeliveryGroup), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.DeliveryGroup>) },
				{ typeof(BrightLine.Common.Models.Feature), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Feature>) },
				{ typeof(BrightLine.Common.Models.FeatureCategory), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.FeatureCategory>) },
				{ typeof(BrightLine.Common.Models.FeatureType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.FeatureType>) },
				{ typeof(BrightLine.Common.Models.FeatureTypeGroup), typeof(BrightLine.Core.IRepository<BrightLine.Common.Models.FeatureTypeGroup>) },
				{ typeof(BrightLine.Common.Models.LogEntry), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.LogEntry>) },
				{ typeof(BrightLine.Common.Models.Metric), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Metric>) },
				{ typeof(BrightLine.Common.Models.Page), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Page>) },
				{ typeof(BrightLine.Common.Models.PageDefinition), typeof(BrightLine.Common.Services.IPageDefinitionService) },    
				{ typeof(BrightLine.Common.Models.PasswordHashHistory), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.PasswordHashHistory>) },
				{ typeof(BrightLine.Common.Models.Placement), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Placement>) },
				{ typeof(BrightLine.Common.Models.Platform), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Platform>) },
				{ typeof(BrightLine.Common.Models.PlatformGroup), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.PlatformGroup>) },
				{ typeof(BrightLine.Common.Models.Product), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Product>) },
				{ typeof(BrightLine.Common.Models.Resource), typeof(BrightLine.Common.Services.IResourceService) },
				{ typeof(BrightLine.Common.Models.ResourceType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.ResourceType>) },
				{ typeof(BrightLine.Common.Models.Role), typeof(BrightLine.Common.Services.IRoleService) },
				{ typeof(BrightLine.Common.Models.Segment), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Segment>) },
				{ typeof(BrightLine.Common.Models.Setting), typeof(BrightLine.Common.Services.ISettingsService) },
				{ typeof(BrightLine.Common.Models.StorageSource), typeof(BrightLine.Common.Services.IStorageSourceService) },
				{ typeof(BrightLine.Common.Models.SubSegment), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.SubSegment>) },
				{ typeof(BrightLine.Common.Models.User), typeof(BrightLine.Common.Services.IUserService) },
				{ typeof(BrightLine.Common.Models.Vertical), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Vertical>) },
				{ typeof(BrightLine.Common.Models.Agency), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Agency>) },
				{ typeof(BrightLine.Common.Models.MediaPartner), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.MediaPartner>) },
				{ typeof(BrightLine.Common.Models.CmsRef), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CmsRef>) },
				{ typeof(BrightLine.Common.Models.CmsRefType), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.CmsRefType>) },
				{ typeof(BrightLine.Common.Models.NightwatchTransaction), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.NightwatchTransaction>) },
				{ typeof(BrightLine.Common.Models.CmsPublishStatus), typeof(BrightLine.Core.IRepository<BrightLine.Common.Models.CmsPublishStatus>) },
				{ typeof(BrightLine.Common.Models.CmsPublish),  typeof(BrightLine.Common.Services.IPublishService) },
				{ typeof(BrightLine.Common.Models.Lookups.TrackingEvent), typeof(BrightLine.Core.ICrudService<BrightLine.Common.Models.Lookups.TrackingEvent>) },

		};
	}

	public static class IoCHelpers
	{
		public static string AddServiceProperty(Type modelType, IList<Type> serviceInterfaces)
		{
			var name = GetName(modelType.Name);
			var modelName = modelType.Name;
			if (GetRawServiceNames(serviceInterfaces).Any(x => x.Equals(modelType.Name, StringComparison.InvariantCultureIgnoreCase)))
				return "public static I" + modelName + "Service " + name + " { get { return _container.GetInstance<I" + modelName + "Service>(); } }";

			return "public static ICrudService<" + modelName + "> " + name + " { get { return _container.GetInstance<ICrudService<" + modelName + ">>(); } }";
		}

		public static string GetName(string name)
		{
			var ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
			return ps.IsPlural(name) ? name : ps.Pluralize(name);
		}

		public static List<string> GetRawServiceNames(ICollection<Type> serviceInterfaces)
		{
			// strip Service and leading I
			return serviceInterfaces.Select(o => o.Name.Replace("Service", "").Substring(1)).ToList();
		}
	}

}
