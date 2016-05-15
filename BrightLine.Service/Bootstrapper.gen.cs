using BrightLine.Core;
using SimpleInjector; 

namespace BrightLine.Service 
{
    public static partial class Bootstrapper
    {
        public static void InitializeContainer(Container container)
        {

			#region OLTP CrudService Registrations

			container.Register<ICrudService<BrightLine.Core.EntityBase>, CrudService<BrightLine.Core.EntityBase>>();
			container.Register<ICrudService<BrightLine.Common.Models.AccountInvitation>, CrudService<BrightLine.Common.Models.AccountInvitation>>();
			container.Register<ICrudService<BrightLine.Common.Models.AccountRetrievalRequest>, CrudService<BrightLine.Common.Models.AccountRetrievalRequest>>();
			container.Register<ICrudService<BrightLine.Common.Models.Ad>, CrudService<BrightLine.Common.Models.Ad>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdTag>, CrudService<BrightLine.Common.Models.AdTag>>();
			container.Register<ICrudService<BrightLine.Common.Models.Agency>, CrudService<BrightLine.Common.Models.Agency>>();
			container.Register<ICrudService<BrightLine.Common.Models.App>, CrudService<BrightLine.Common.Models.App>>();
			container.Register<ICrudService<BrightLine.Common.Models.AppPlatform>, CrudService<BrightLine.Common.Models.AppPlatform>>();
			container.Register<ICrudService<BrightLine.Common.Models.AuditEvent>, CrudService<BrightLine.Common.Models.AuditEvent>>();
			container.Register<ICrudService<BrightLine.Common.Models.Blueprint>, CrudService<BrightLine.Common.Models.Blueprint>>();
			container.Register<ICrudService<BrightLine.Common.Models.BlueprintPlatform>, CrudService<BrightLine.Common.Models.BlueprintPlatform>>();
			container.Register<ICrudService<BrightLine.Common.Models.Campaign>, CrudService<BrightLine.Common.Models.Campaign>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModel>, CrudService<BrightLine.Common.Models.CampaignContentModel>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelInstance>, CrudService<BrightLine.Common.Models.CampaignContentModelInstance>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelProperty>, CrudService<BrightLine.Common.Models.CampaignContentModelProperty>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyValue>, CrudService<BrightLine.Common.Models.CampaignContentModelPropertyValue>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentSchema>, CrudService<BrightLine.Common.Models.CampaignContentSchema>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentSettings>, CrudService<BrightLine.Common.Models.CampaignContentSettings>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsField>, CrudService<BrightLine.Common.Models.CmsField>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModel>, CrudService<BrightLine.Common.Models.CmsModel>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelDefinition>, CrudService<BrightLine.Common.Models.CmsModelDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelInstance>, CrudService<BrightLine.Common.Models.CmsModelInstance>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelInstanceField>, CrudService<BrightLine.Common.Models.CmsModelInstanceField>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelInstanceFieldValue>, CrudService<BrightLine.Common.Models.CmsModelInstanceFieldValue>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsPublish>, CrudService<BrightLine.Common.Models.CmsPublish>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsRef>, CrudService<BrightLine.Common.Models.CmsRef>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsSetting>, CrudService<BrightLine.Common.Models.CmsSetting>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsSettingDefinition>, CrudService<BrightLine.Common.Models.CmsSettingDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsSettingInstance>, CrudService<BrightLine.Common.Models.CmsSettingInstance>>();
			container.Register<ICrudService<BrightLine.Common.Models.Contract>, CrudService<BrightLine.Common.Models.Contract>>();
			container.Register<ICrudService<BrightLine.Common.Models.Creative>, CrudService<BrightLine.Common.Models.Creative>>();
			container.Register<ICrudService<BrightLine.Common.Models.DeliveryGroup>, CrudService<BrightLine.Common.Models.DeliveryGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Feature>, CrudService<BrightLine.Common.Models.Feature>>();
			container.Register<ICrudService<BrightLine.Common.Models.FileItem>, CrudService<BrightLine.Common.Models.FileItem>>();
			container.Register<ICrudService<BrightLine.Common.Models.FileTypeValidation>, CrudService<BrightLine.Common.Models.FileTypeValidation>>();
			container.Register<ICrudService<BrightLine.Common.Models.LogEntry>, CrudService<BrightLine.Common.Models.LogEntry>>();
			container.Register<ICrudService<BrightLine.Common.Models.MediaPartner>, CrudService<BrightLine.Common.Models.MediaPartner>>();
			container.Register<ICrudService<BrightLine.Common.Models.NightwatchTransaction>, CrudService<BrightLine.Common.Models.NightwatchTransaction>>();
			container.Register<ICrudService<BrightLine.Common.Models.Page>, CrudService<BrightLine.Common.Models.Page>>();
			container.Register<ICrudService<BrightLine.Common.Models.PasswordHashHistory>, CrudService<BrightLine.Common.Models.PasswordHashHistory>>();
			container.Register<ICrudService<BrightLine.Common.Models.Placement>, CrudService<BrightLine.Common.Models.Placement>>();
			container.Register<ICrudService<BrightLine.Common.Models.RateCard>, CrudService<BrightLine.Common.Models.RateCard>>();
			container.Register<ICrudService<BrightLine.Common.Models.Resource>, CrudService<BrightLine.Common.Models.Resource>>();
			container.Register<ICrudService<BrightLine.Common.Models.Setting>, CrudService<BrightLine.Common.Models.Setting>>();
			container.Register<ICrudService<BrightLine.Common.Models.User>, CrudService<BrightLine.Common.Models.User>>();
			container.Register<ICrudService<BrightLine.Common.Models.Validation>, CrudService<BrightLine.Common.Models.Validation>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdFormat>, CrudService<BrightLine.Common.Models.AdFormat>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdFunction>, CrudService<BrightLine.Common.Models.AdFunction>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdType>, CrudService<BrightLine.Common.Models.AdType>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdTypeGroup>, CrudService<BrightLine.Common.Models.AdTypeGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Advertiser>, CrudService<BrightLine.Common.Models.Advertiser>>();
			container.Register<ICrudService<BrightLine.Common.Models.Brand>, CrudService<BrightLine.Common.Models.Brand>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelBaseType>, CrudService<BrightLine.Common.Models.CampaignContentModelBaseType>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyType>, CrudService<BrightLine.Common.Models.CampaignContentModelPropertyType>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelType>, CrudService<BrightLine.Common.Models.CampaignContentModelType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Category>, CrudService<BrightLine.Common.Models.Category>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsPublishStatus>, CrudService<BrightLine.Common.Models.CmsPublishStatus>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsRefType>, CrudService<BrightLine.Common.Models.CmsRefType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Lookups.Expose>, CrudService<BrightLine.Common.Models.Lookups.Expose>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureCategory>, CrudService<BrightLine.Common.Models.FeatureCategory>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureType>, CrudService<BrightLine.Common.Models.FeatureType>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureTypeGroup>, CrudService<BrightLine.Common.Models.FeatureTypeGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Lookups.FieldType>, CrudService<BrightLine.Common.Models.Lookups.FieldType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Lookups.FileType>, CrudService<BrightLine.Common.Models.Lookups.FileType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Metric>, CrudService<BrightLine.Common.Models.Metric>>();
			container.Register<ICrudService<BrightLine.Common.Models.PageDefinition>, CrudService<BrightLine.Common.Models.PageDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.Platform>, CrudService<BrightLine.Common.Models.Platform>>();
			container.Register<ICrudService<BrightLine.Common.Models.PlatformGroup>, CrudService<BrightLine.Common.Models.PlatformGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Product>, CrudService<BrightLine.Common.Models.Product>>();
			container.Register<ICrudService<BrightLine.Common.Models.RateType>, CrudService<BrightLine.Common.Models.RateType>>();
			container.Register<ICrudService<BrightLine.Common.Models.ResourceType>, CrudService<BrightLine.Common.Models.ResourceType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Role>, CrudService<BrightLine.Common.Models.Role>>();
			container.Register<ICrudService<BrightLine.Common.Models.Segment>, CrudService<BrightLine.Common.Models.Segment>>();
			container.Register<ICrudService<BrightLine.Common.Models.StorageSource>, CrudService<BrightLine.Common.Models.StorageSource>>();
			container.Register<ICrudService<BrightLine.Common.Models.SubSegment>, CrudService<BrightLine.Common.Models.SubSegment>>();
			container.Register<ICrudService<BrightLine.Common.Models.ValidationType>, CrudService<BrightLine.Common.Models.ValidationType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Vertical>, CrudService<BrightLine.Common.Models.Vertical>>();
			#endregion

			#region OLAP AggregationService Registrations
			#endregion
		}
    }
}