using BrightLine.Core;
using SimpleInjector;

namespace BrightLine.Data
{
	public static partial class Bootstrapper
	{
		public static void InitializeContainer(Container container)
		{

			#region CrudRepository Registrations

			container.Register<IRepository<BrightLine.Core.EntityBase>, CrudRepository<BrightLine.Core.EntityBase>>();
			container.Register<IRepository<BrightLine.Common.Models.AccountInvitation>, CrudRepository<BrightLine.Common.Models.AccountInvitation>>();
			container.Register<IRepository<BrightLine.Common.Models.AccountRetrievalRequest>, CrudRepository<BrightLine.Common.Models.AccountRetrievalRequest>>();
			container.Register<IRepository<BrightLine.Common.Models.Ad>, CrudRepository<BrightLine.Common.Models.Ad>>();
			container.Register<IRepository<BrightLine.Common.Models.AdTag>, CrudRepository<BrightLine.Common.Models.AdTag>>();
			container.Register<IRepository<BrightLine.Common.Models.Agency>, CrudRepository<BrightLine.Common.Models.Agency>>();
			container.Register<IRepository<BrightLine.Common.Models.App>, CrudRepository<BrightLine.Common.Models.App>>();
			container.Register<IRepository<BrightLine.Common.Models.AppPlatform>, CrudRepository<BrightLine.Common.Models.AppPlatform>>();
			container.Register<IRepository<BrightLine.Common.Models.AuditEvent>, CrudRepository<BrightLine.Common.Models.AuditEvent>>();
			container.Register<IRepository<BrightLine.Common.Models.Blueprint>, CrudRepository<BrightLine.Common.Models.Blueprint>>();
			container.Register<IRepository<BrightLine.Common.Models.BlueprintPlatform>, CrudRepository<BrightLine.Common.Models.BlueprintPlatform>>();
			container.Register<IRepository<BrightLine.Common.Models.Campaign>, CrudRepository<BrightLine.Common.Models.Campaign>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModel>, CrudRepository<BrightLine.Common.Models.CampaignContentModel>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelInstance>, CrudRepository<BrightLine.Common.Models.CampaignContentModelInstance>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelProperty>, CrudRepository<BrightLine.Common.Models.CampaignContentModelProperty>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelPropertyValue>, CrudRepository<BrightLine.Common.Models.CampaignContentModelPropertyValue>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentSchema>, CrudRepository<BrightLine.Common.Models.CampaignContentSchema>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentSettings>, CrudRepository<BrightLine.Common.Models.CampaignContentSettings>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsField>, CrudRepository<BrightLine.Common.Models.CmsField>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsModel>, CrudRepository<BrightLine.Common.Models.CmsModel>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsModelDefinition>, CrudRepository<BrightLine.Common.Models.CmsModelDefinition>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsModelInstance>, CrudRepository<BrightLine.Common.Models.CmsModelInstance>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsModelInstanceField>, CrudRepository<BrightLine.Common.Models.CmsModelInstanceField>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsModelInstanceFieldValue>, CrudRepository<BrightLine.Common.Models.CmsModelInstanceFieldValue>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsPublish>, CrudRepository<BrightLine.Common.Models.CmsPublish>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsRef>, CrudRepository<BrightLine.Common.Models.CmsRef>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsSetting>, CrudRepository<BrightLine.Common.Models.CmsSetting>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsSettingDefinition>, CrudRepository<BrightLine.Common.Models.CmsSettingDefinition>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsSettingInstance>, CrudRepository<BrightLine.Common.Models.CmsSettingInstance>>();
			container.Register<IRepository<BrightLine.Common.Models.Contract>, CrudRepository<BrightLine.Common.Models.Contract>>();
			container.Register<IRepository<BrightLine.Common.Models.Creative>, CrudRepository<BrightLine.Common.Models.Creative>>();
			container.Register<IRepository<BrightLine.Common.Models.DeliveryGroup>, CrudRepository<BrightLine.Common.Models.DeliveryGroup>>();
			container.Register<IRepository<BrightLine.Common.Models.Feature>, CrudRepository<BrightLine.Common.Models.Feature>>();
			container.Register<IRepository<BrightLine.Common.Models.FileItem>, CrudRepository<BrightLine.Common.Models.FileItem>>();
			container.Register<IRepository<BrightLine.Common.Models.FileTypeValidation>, CrudRepository<BrightLine.Common.Models.FileTypeValidation>>();
			container.Register<IRepository<BrightLine.Common.Models.LogEntry>, CrudRepository<BrightLine.Common.Models.LogEntry>>();
			container.Register<IRepository<BrightLine.Common.Models.MediaPartner>, CrudRepository<BrightLine.Common.Models.MediaPartner>>();
			container.Register<IRepository<BrightLine.Common.Models.NightwatchTransaction>, CrudRepository<BrightLine.Common.Models.NightwatchTransaction>>();
			container.Register<IRepository<BrightLine.Common.Models.Page>, CrudRepository<BrightLine.Common.Models.Page>>();
			container.Register<IRepository<BrightLine.Common.Models.PasswordHashHistory>, CrudRepository<BrightLine.Common.Models.PasswordHashHistory>>();
			container.Register<IRepository<BrightLine.Common.Models.Placement>, CrudRepository<BrightLine.Common.Models.Placement>>();
			container.Register<IRepository<BrightLine.Common.Models.RateCard>, CrudRepository<BrightLine.Common.Models.RateCard>>();
			container.Register<IRepository<BrightLine.Common.Models.Resource>, CrudRepository<BrightLine.Common.Models.Resource>>();
			container.Register<IRepository<BrightLine.Common.Models.Setting>, CrudRepository<BrightLine.Common.Models.Setting>>();
			container.Register<IRepository<BrightLine.Common.Models.User>, CrudRepository<BrightLine.Common.Models.User>>();
			container.Register<IRepository<BrightLine.Common.Models.Validation>, CrudRepository<BrightLine.Common.Models.Validation>>();
			container.Register<IRepository<BrightLine.Common.Models.AdFormat>, CrudRepository<BrightLine.Common.Models.AdFormat>>();
			container.Register<IRepository<BrightLine.Common.Models.AdFunction>, CrudRepository<BrightLine.Common.Models.AdFunction>>();
			container.Register<IRepository<BrightLine.Common.Models.AdType>, CrudRepository<BrightLine.Common.Models.AdType>>();
			container.Register<IRepository<BrightLine.Common.Models.AdTypeGroup>, CrudRepository<BrightLine.Common.Models.AdTypeGroup>>();
			container.Register<IRepository<BrightLine.Common.Models.Advertiser>, CrudRepository<BrightLine.Common.Models.Advertiser>>();
			container.Register<IRepository<BrightLine.Common.Models.Brand>, CrudRepository<BrightLine.Common.Models.Brand>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelBaseType>, CrudRepository<BrightLine.Common.Models.CampaignContentModelBaseType>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelPropertyType>, CrudRepository<BrightLine.Common.Models.CampaignContentModelPropertyType>>();
			container.Register<IRepository<BrightLine.Common.Models.CampaignContentModelType>, CrudRepository<BrightLine.Common.Models.CampaignContentModelType>>();
			container.Register<IRepository<BrightLine.Common.Models.Category>, CrudRepository<BrightLine.Common.Models.Category>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsPublishStatus>, CrudRepository<BrightLine.Common.Models.CmsPublishStatus>>();
			container.Register<IRepository<BrightLine.Common.Models.CmsRefType>, CrudRepository<BrightLine.Common.Models.CmsRefType>>();
			container.Register<IRepository<BrightLine.Common.Models.Lookups.Expose>, CrudRepository<BrightLine.Common.Models.Lookups.Expose>>();
			container.Register<IRepository<BrightLine.Common.Models.FeatureCategory>, CrudRepository<BrightLine.Common.Models.FeatureCategory>>();
			container.Register<IRepository<BrightLine.Common.Models.FeatureType>, CrudRepository<BrightLine.Common.Models.FeatureType>>();
			container.Register<IRepository<BrightLine.Common.Models.FeatureTypeGroup>, CrudRepository<BrightLine.Common.Models.FeatureTypeGroup>>();
			container.Register<IRepository<BrightLine.Common.Models.Lookups.FieldType>, CrudRepository<BrightLine.Common.Models.Lookups.FieldType>>();
			container.Register<IRepository<BrightLine.Common.Models.Lookups.FileType>, CrudRepository<BrightLine.Common.Models.Lookups.FileType>>();
			container.Register<IRepository<BrightLine.Common.Models.Metric>, CrudRepository<BrightLine.Common.Models.Metric>>();
			container.Register<IRepository<BrightLine.Common.Models.PageDefinition>, CrudRepository<BrightLine.Common.Models.PageDefinition>>();
			container.Register<IRepository<BrightLine.Common.Models.Platform>, CrudRepository<BrightLine.Common.Models.Platform>>();
			container.Register<IRepository<BrightLine.Common.Models.PlatformGroup>, CrudRepository<BrightLine.Common.Models.PlatformGroup>>();
			container.Register<IRepository<BrightLine.Common.Models.Product>, CrudRepository<BrightLine.Common.Models.Product>>();
			container.Register<IRepository<BrightLine.Common.Models.RateType>, CrudRepository<BrightLine.Common.Models.RateType>>();
			container.Register<IRepository<BrightLine.Common.Models.ResourceType>, CrudRepository<BrightLine.Common.Models.ResourceType>>();
			container.Register<IRepository<BrightLine.Common.Models.Role>, CrudRepository<BrightLine.Common.Models.Role>>();
			container.Register<IRepository<BrightLine.Common.Models.Segment>, CrudRepository<BrightLine.Common.Models.Segment>>();
			container.Register<IRepository<BrightLine.Common.Models.StorageSource>, CrudRepository<BrightLine.Common.Models.StorageSource>>();
			container.Register<IRepository<BrightLine.Common.Models.SubSegment>, CrudRepository<BrightLine.Common.Models.SubSegment>>();
			container.Register<IRepository<BrightLine.Common.Models.ValidationType>, CrudRepository<BrightLine.Common.Models.ValidationType>>();
			container.Register<IRepository<BrightLine.Common.Models.Vertical>, CrudRepository<BrightLine.Common.Models.Vertical>>();
			container.Register<IRepository<BrightLine.Common.Models.Lookups.TrackingEvent>, CrudRepository<BrightLine.Common.Models.Lookups.TrackingEvent>>();
			container.Register<IRepository<BrightLine.Common.Models.AdTrackingEvent>, CrudRepository<BrightLine.Common.Models.AdTrackingEvent>>();

			#endregion

		}
	}
}