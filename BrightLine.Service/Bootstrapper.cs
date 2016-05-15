using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.SqlServer;
using BrightLine.Core;
using BrightLine.Service.External.AmazonStorage;
using BrightLine.Service.RabbitMQ.Interfaces;
using BrightLine.Service.RabbitMQ.Services;
using BrightLine.Service.Redis.Interfaces;
using BrightLine.Service.Redis.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;

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
			container.Register<ICrudService<BrightLine.Common.Models.AdFormat>, CrudService<BrightLine.Common.Models.AdFormat>>();
			container.Register<ICrudService<BrightLine.Common.Models.Agency>, CrudService<BrightLine.Common.Models.Agency>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdFunction>, CrudService<BrightLine.Common.Models.AdFunction>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdTag>, CrudService<BrightLine.Common.Models.AdTag>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdType>, CrudService<BrightLine.Common.Models.AdType>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdTypeGroup>, CrudService<BrightLine.Common.Models.AdTypeGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Advertiser>, CrudService<BrightLine.Common.Models.Advertiser>>();
			container.Register<ICrudService<BrightLine.Common.Models.App>, CrudService<BrightLine.Common.Models.App>>();
			container.Register<ICrudService<BrightLine.Common.Models.AppPlatform>, CrudService<BrightLine.Common.Models.AppPlatform>>();
			container.Register<ICrudService<BrightLine.Common.Models.AuditEvent>, CrudService<BrightLine.Common.Models.AuditEvent>>();
			container.Register<ICrudService<BrightLine.Common.Models.Blueprint>, CrudService<BrightLine.Common.Models.Blueprint>>();
			container.Register<ICrudService<BrightLine.Common.Models.BlueprintPlatform>, CrudService<BrightLine.Common.Models.BlueprintPlatform>>();
			container.Register<ICrudService<BrightLine.Common.Models.Brand>, CrudService<BrightLine.Common.Models.Brand>>();
			container.Register<ICrudService<BrightLine.Common.Models.Campaign>, CrudService<BrightLine.Common.Models.Campaign>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModel>, CrudService<BrightLine.Common.Models.CampaignContentModel>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelBaseType>, CrudService<BrightLine.Common.Models.CampaignContentModelBaseType>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelInstance>, CrudService<BrightLine.Common.Models.CampaignContentModelInstance>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelProperty>, CrudService<BrightLine.Common.Models.CampaignContentModelProperty>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyType>, CrudService<BrightLine.Common.Models.CampaignContentModelPropertyType>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelPropertyValue>, CrudService<BrightLine.Common.Models.CampaignContentModelPropertyValue>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentModelType>, CrudService<BrightLine.Common.Models.CampaignContentModelType>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentSchema>, CrudService<BrightLine.Common.Models.CampaignContentSchema>>();
			container.Register<ICrudService<BrightLine.Common.Models.CampaignContentSettings>, CrudService<BrightLine.Common.Models.CampaignContentSettings>>();
			container.Register<ICrudService<BrightLine.Common.Models.Category>, CrudService<BrightLine.Common.Models.Category>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsField>, CrudService<BrightLine.Common.Models.CmsField>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModel>, CrudService<BrightLine.Common.Models.CmsModel>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelDefinition>, CrudService<BrightLine.Common.Models.CmsModelDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelInstanceField>, CrudService<BrightLine.Common.Models.CmsModelInstanceField>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsSetting>, CrudService<BrightLine.Common.Models.CmsSetting>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsSettingDefinition>, CrudService<BrightLine.Common.Models.CmsSettingDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.MediaPartner>, CrudService<BrightLine.Common.Models.MediaPartner>>();
			container.Register<ICrudService<BrightLine.Common.Models.Creative>, CrudService<BrightLine.Common.Models.Creative>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsModelInstance>, CrudService<BrightLine.Common.Models.CmsModelInstance>>();
			container.Register<ICrudService<BrightLine.Common.Models.DeliveryGroup>, CrudService<BrightLine.Common.Models.DeliveryGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Feature>, CrudService<BrightLine.Common.Models.Feature>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureCategory>, CrudService<BrightLine.Common.Models.FeatureCategory>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureType>, CrudService<BrightLine.Common.Models.FeatureType>>();
			container.Register<ICrudService<BrightLine.Common.Models.FeatureTypeGroup>, CrudService<BrightLine.Common.Models.FeatureTypeGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.FileTypeValidation>, CrudService<BrightLine.Common.Models.FileTypeValidation>>();
			container.Register<ICrudService<BrightLine.Common.Models.LogEntry>, CrudService<BrightLine.Common.Models.LogEntry>>();
			container.Register<ICrudService<BrightLine.Common.Models.Metric>, CrudService<BrightLine.Common.Models.Metric>>();
			container.Register<ICrudService<BrightLine.Common.Models.PasswordHashHistory>, CrudService<BrightLine.Common.Models.PasswordHashHistory>>();
			container.Register<ICrudService<BrightLine.Common.Models.Page>, CrudService<BrightLine.Common.Models.Page>>();
			container.Register<ICrudService<BrightLine.Common.Models.PageDefinition>, CrudService<BrightLine.Common.Models.PageDefinition>>();
			container.Register<ICrudService<BrightLine.Common.Models.Placement>, CrudService<BrightLine.Common.Models.Placement>>();
			container.Register<ICrudService<BrightLine.Common.Models.Platform>, CrudService<BrightLine.Common.Models.Platform>>();
			container.Register<ICrudService<BrightLine.Common.Models.PlatformGroup>, CrudService<BrightLine.Common.Models.PlatformGroup>>();
			container.Register<ICrudService<BrightLine.Common.Models.Product>, CrudService<BrightLine.Common.Models.Product>>();
			container.Register<ICrudService<BrightLine.Common.Models.Resource>, CrudService<BrightLine.Common.Models.Resource>>();
			container.Register<ICrudService<BrightLine.Common.Models.ResourceType>, CrudService<BrightLine.Common.Models.ResourceType>>();
			container.Register<ICrudService<BrightLine.Common.Models.Role>, CrudService<BrightLine.Common.Models.Role>>();
			container.Register<ICrudService<BrightLine.Common.Models.Segment>, CrudService<BrightLine.Common.Models.Segment>>();
			container.Register<ICrudService<BrightLine.Common.Models.SubSegment>, CrudService<BrightLine.Common.Models.SubSegment>>();
			container.Register<ICrudService<BrightLine.Common.Models.User>, CrudService<BrightLine.Common.Models.User>>();
			container.Register<ICrudService<BrightLine.Common.Models.Vertical>, CrudService<BrightLine.Common.Models.Vertical>>();
			container.Register<ICrudService<BrightLine.Common.Models.Validation>, CrudService<BrightLine.Common.Models.Validation>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsRef>, CrudService<BrightLine.Common.Models.CmsRef>>();
			container.Register<ICrudService<BrightLine.Common.Models.NightwatchTransaction>, CrudService<BrightLine.Common.Models.NightwatchTransaction>>();
			container.Register<ICrudService<BrightLine.Common.Models.CmsPublish>, CrudService<BrightLine.Common.Models.CmsPublish>>();
			container.Register<ICrudService<BrightLine.Common.Models.Lookups.TrackingEvent>, CrudService<BrightLine.Common.Models.Lookups.TrackingEvent>>();
			container.Register<ICrudService<BrightLine.Common.Models.AdTrackingEvent>, CrudService<BrightLine.Common.Models.AdTrackingEvent>>();

			container.Register<BrightLine.Common.Services.IAccountService, BrightLine.Service.AccountService>();
			container.Register<BrightLine.Common.Services.IAdService, BrightLine.Service.AdService>();
			container.Register<BrightLine.Common.Services.IAdTagService, BrightLine.Service.AdTagService>();
			container.Register<BrightLine.Common.Services.IAgencyService, BrightLine.Service.AgencyService>();
			container.Register<BrightLine.Common.Services.IAuditEventService, BrightLine.Service.AuditEventService>();
			container.Register<BrightLine.Common.Services.IBlueprintService, BrightLine.Service.BlueprintService>();
			container.Register<BrightLine.Common.Services.ICmsModelDefinitionService, BrightLine.Service.CmsModelDefinitionService>();
			container.Register<BrightLine.Common.Services.IFileTypeValidationService, BrightLine.Service.FileTypeValidationService>();
			container.Register<BrightLine.Common.Services.ICmsFieldService, BrightLine.Service.CmsFieldService>();
			container.Register<BrightLine.Common.Services.IPageDefinitionService, BrightLine.Service.PageDefinitionService>();
			container.Register<BrightLine.Common.Services.IValidationTypeService, BrightLine.Service.ValidationTypeService>();
			container.Register<BrightLine.Common.Services.IValidationService, BrightLine.Service.ValidationService>();
			container.Register<BrightLine.Common.Services.ICampaignService, BrightLine.Service.CampaignService>();
			container.Register<BrightLine.Common.Services.IResourceService, BrightLine.Service.ResourceService>();
			container.Register<BrightLine.Common.Services.ICookieService, BrightLine.Service.CookieService>();
			container.Register<BrightLine.Common.Services.ICreativeService, BrightLine.Service.CreativeService>();
			container.Register<BrightLine.Common.Services.ICmsModelInstanceService, BrightLine.Service.CmsModelInstanceService>();
			container.Register<BrightLine.Common.Services.IEmailService, BrightLine.Service.EmailService>();
			container.Register<BrightLine.Common.Services.IRoleService, BrightLine.Service.RoleService>();
			container.Register<BrightLine.Common.Services.IStorageSourceService, BrightLine.Service.StorageSourceService>();
			container.Register<BrightLine.Common.Services.ISettingsService, BrightLine.Service.SettingsService>();
			container.Register<BrightLine.Common.Services.ISecurableService, BrightLine.Service.SecurableService>();
			container.Register<BrightLine.Common.Services.IUserService, BrightLine.Service.UserService>();
			container.Register<BrightLine.Common.Services.IDateHelperService, BrightLine.Service.DateHelperService>();
			container.Register<BrightLine.Common.Services.IMediaPartnerService, BrightLine.Service.MediaPartnerService>();
			container.Register<BrightLine.Common.Services.IPlacementService, BrightLine.Service.PlacementService>();
			container.Register<BrightLine.Common.Services.IAppService, BrightLine.Service.AppService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportService, BrightLine.Service.BlueprintImportService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportFieldsService, BrightLine.Service.BlueprintImportFieldsService>();
			container.Register<BrightLine.Common.Services.IFieldTypeService, BrightLine.Service.FieldTypeService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportValidationService, BrightLine.Service.BlueprintImportValidationService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportModelsService, BrightLine.Service.BlueprintImportModelsService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportSettingsService, BrightLine.Service.BlueprintImportSettingsService>();
			container.Register<BrightLine.Common.Services.IBlueprintImportPagesService, BrightLine.Service.BlueprintImportPagesService>();
			container.Register<BrightLine.Common.Services.IFileHelper, BrightLine.Common.Utility.FileHelper>();
			container.Register<BrightLine.Common.Services.IFlashMessageExtensions, BrightLine.Common.Utility.FlashMessageExtensions>();
			container.Register<BrightLine.Common.Services.IResourceHelper, BrightLine.Common.Utility.ResourceHelper>();
			container.Register<BrightLine.Common.Services.IEnvironmentHelper, BrightLine.Common.Utility.EnvironmentHelper>();
			container.Register<BrightLine.Common.Services.IBlueprintHelper, BrightLine.Common.Utility.BlueprintHelper>();
			container.Register<BrightLine.Common.Utility.ILogHelper, BrightLine.Common.Utility.LogHelper>();
			container.Register<BrightLine.Data.IOLTPContextHelper, BrightLine.Data.OLTPContextHelper>();
			container.Register<BrightLine.Common.Services.IAdTagsExportService, BrightLine.Service.AdTagsExportService>();
			container.Register<BrightLine.Common.Services.ICmsFieldHelper, BrightLine.Common.Utility.Helpers.CmsFieldHelper>();
			container.Register<BrightLine.Common.Services.IAdValidationService, BrightLine.Service.AdValidationService>();
			container.Register<BrightLine.Common.Services.IHttpContextHelper, BrightLine.Common.Utility.Helpers.HttpContextHelper>();
			container.Register<BrightLine.Common.Services.IBlueprintImportLookupsService, BrightLine.Service.BlueprintImport.BlueprintImportLookupsService>();
			container.Register<BrightLine.Common.Services.ICampaignSummaryService, BrightLine.Service.CampaignSummaryService>();
			container.Register<BrightLine.Common.Services.ICampaignLookupsService, BrightLine.Service.CampaignLookupsService>();
			container.Register<BrightLine.Common.Services.ICampaignsListingService, BrightLine.Service.CampaignsListingService>();
			container.Register<BrightLine.Common.Services.ICampaignPermissionsService, BrightLine.Service.CampaignPermissionsService>();
			container.Register<BrightLine.Common.Services.IAdTrackingEventService, BrightLine.Service.AdTrackingEventService>();
			container.Register<IRabbitMQService, RabbitMQService>();
			container.Register<IRedisService, RedisService>();
			container.Register<IRedisSubscriptionsService, RedisSubscriptionsService>();
			

			#endregion

			#region OLAP AggregationService Registrations
			#endregion
		}

	}
}