
using System.Data.Entity;
//using BrightLine.Common.Models;
//using BrightLine.Common.Models.OLAP;
//using BrightLine.Common.Models.OLAP.Dimensions;
//using BrightLine.Common.Models.OLAP.Aggregations.Ad;
//using BrightLine.Common.Models.OLAP.Aggregations.App;
//using BrightLine.Common.Models.OLAP.Aggregations.Destination;

namespace BrightLine.Data
{
	public partial class OLTPContext
	{

		#region DbSets

		public DbSet<BrightLine.Common.Models.AccountInvitation> AccountInvitations { get; set; }
		public DbSet<BrightLine.Common.Models.AccountRetrievalRequest> AccountRetrievalRequests { get; set; }
		public DbSet<BrightLine.Common.Models.Ad> Ads { get; set; }
		public DbSet<BrightLine.Common.Models.AdTag> AdTags { get; set; }
		public DbSet<BrightLine.Common.Models.Agency> Agencies { get; set; }
		public DbSet<BrightLine.Common.Models.App> Apps { get; set; }
		public DbSet<BrightLine.Common.Models.AppPlatform> AppPlatforms { get; set; }
		public DbSet<BrightLine.Common.Models.AuditEvent> AuditEvents { get; set; }
		public DbSet<BrightLine.Common.Models.Blueprint> Blueprints { get; set; }
		public DbSet<BrightLine.Common.Models.BlueprintPlatform> BlueprintPlatforms { get; set; }
		public DbSet<BrightLine.Common.Models.Campaign> Campaigns { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModel> CampaignContentModels { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelInstance> CampaignContentModelInstances { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelProperty> CampaignContentModelProperties { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelPropertyValue> CampaignContentModelPropertyValues { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentSchema> CampaignContentSchemas { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentSettings> CampaignContentSettings { get; set; }
		public DbSet<BrightLine.Common.Models.CmsField> CmsFields { get; set; }
		public DbSet<BrightLine.Common.Models.CmsModel> CmsModels { get; set; }
		public DbSet<BrightLine.Common.Models.CmsModelDefinition> CmsModelDefinitions { get; set; }
		public DbSet<BrightLine.Common.Models.CmsModelInstance> CmsModelInstances { get; set; }
		public DbSet<BrightLine.Common.Models.CmsModelInstanceField> CmsModelInstanceFields { get; set; }
		public DbSet<BrightLine.Common.Models.CmsModelInstanceFieldValue> CmsModelInstanceFieldValues { get; set; }
		public DbSet<BrightLine.Common.Models.CmsPublish> CmsPublishes { get; set; }
		public DbSet<BrightLine.Common.Models.CmsRef> CmsRefs { get; set; }
		public DbSet<BrightLine.Common.Models.CmsSetting> CmsSettings { get; set; }
		public DbSet<BrightLine.Common.Models.CmsSettingDefinition> CmsSettingDefinitions { get; set; }
		public DbSet<BrightLine.Common.Models.CmsSettingInstance> CmsSettingInstances { get; set; }
		public DbSet<BrightLine.Common.Models.Contract> Contracts { get; set; }
		public DbSet<BrightLine.Common.Models.Creative> Creatives { get; set; }
		public DbSet<BrightLine.Common.Models.DeliveryGroup> DeliveryGroups { get; set; }
		public DbSet<BrightLine.Common.Models.Feature> Features { get; set; }
		public DbSet<BrightLine.Common.Models.FileItem> FileItems { get; set; }
		public DbSet<BrightLine.Common.Models.FileTypeValidation> FileTypeValidations { get; set; }
		public DbSet<BrightLine.Common.Models.LogEntry> LogEntries { get; set; }
		public DbSet<BrightLine.Common.Models.MediaPartner> MediaPartners { get; set; }
		public DbSet<BrightLine.Common.Models.NightwatchTransaction> NightwatchTransactions { get; set; }
		public DbSet<BrightLine.Common.Models.Page> Pages { get; set; }
		public DbSet<BrightLine.Common.Models.PasswordHashHistory> PasswordHashHistories { get; set; }
		public DbSet<BrightLine.Common.Models.Placement> Placements { get; set; }
		public DbSet<BrightLine.Common.Models.RateCard> RateCards { get; set; }
		public DbSet<BrightLine.Common.Models.Resource> Resources { get; set; }
		public DbSet<BrightLine.Common.Models.Setting> Settings { get; set; }
		public DbSet<BrightLine.Common.Models.User> Users { get; set; }
		public DbSet<BrightLine.Common.Models.Validation> Validations { get; set; }
		public DbSet<BrightLine.Common.Models.AdFormat> AdFormats { get; set; }
		public DbSet<BrightLine.Common.Models.AdFunction> AdFunctions { get; set; }
		public DbSet<BrightLine.Common.Models.AdType> AdTypes { get; set; }
		public DbSet<BrightLine.Common.Models.AdTypeGroup> AdTypeGroups { get; set; }
		public DbSet<BrightLine.Common.Models.Advertiser> Advertisers { get; set; }
		public DbSet<BrightLine.Common.Models.Brand> Brands { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelBaseType> CampaignContentModelBaseTypes { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelPropertyType> CampaignContentModelPropertyTypes { get; set; }
		public DbSet<BrightLine.Common.Models.CampaignContentModelType> CampaignContentModelTypes { get; set; }
		public DbSet<BrightLine.Common.Models.Category> Categories { get; set; }
		public DbSet<BrightLine.Common.Models.CmsPublishStatus> CmsPublishStatus { get; set; }
		public DbSet<BrightLine.Common.Models.CmsRefType> CmsRefTypes { get; set; }
		public DbSet<BrightLine.Common.Models.FeatureCategory> FeatureCategories { get; set; }
		public DbSet<BrightLine.Common.Models.FeatureType> FeatureTypes { get; set; }
		public DbSet<BrightLine.Common.Models.FeatureTypeGroup> FeatureTypeGroups { get; set; }
		public DbSet<BrightLine.Common.Models.Metric> Metrics { get; set; }
		public DbSet<BrightLine.Common.Models.PageDefinition> PageDefinitions { get; set; }
		public DbSet<BrightLine.Common.Models.Platform> Platforms { get; set; }
		public DbSet<BrightLine.Common.Models.PlatformGroup> PlatformGroups { get; set; }
		public DbSet<BrightLine.Common.Models.Product> Products { get; set; }
		public DbSet<BrightLine.Common.Models.RateType> RateTypes { get; set; }
		public DbSet<BrightLine.Common.Models.ResourceType> ResourceTypes { get; set; }
		public DbSet<BrightLine.Common.Models.Role> Roles { get; set; }
		public DbSet<BrightLine.Common.Models.Segment> Segments { get; set; }
		public DbSet<BrightLine.Common.Models.StorageSource> StorageSources { get; set; }
		public DbSet<BrightLine.Common.Models.SubSegment> SubSegments { get; set; }
		public DbSet<BrightLine.Common.Models.ValidationType> ValidationTypes { get; set; }
		public DbSet<BrightLine.Common.Models.Vertical> Verticals { get; set; }
		public DbSet<BrightLine.Common.Models.Lookups.Expose> Exposes { get; set; }
		public DbSet<BrightLine.Common.Models.Lookups.FieldType> FieldTypes { get; set; }
		public DbSet<BrightLine.Common.Models.Lookups.FileType> FileTypes { get; set; }
		public DbSet<BrightLine.Common.Models.Lookups.TrackingEvent> TrackingEvents { get; set; }

		#endregion
	}
}