using BrightLine.CMS;
using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.CMS.Services.Publish;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdFunction;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.CmsRefType;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Expose;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Helpers;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.Utility.StorageSource;
using BrightLine.Common.Utility.ValidationType;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Publishing.Html5BrandDestination.Services;
using BrightLine.Service;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common
{
	public class MockUtilities
	{
		public static void SetupAuth(int userId = 12, string role = AuthConstants.Roles.AgencyPartner, int? agencyId = null, int? mediaPartnerId = null, int? advertiserId = 12)
		{
			var authUser = new User() { Id = userId, Display = "user1", TimeZoneId = "Eastern Standard Time" };
			authUser.Advertiser = new Advertiser();

			if (advertiserId.HasValue)
				authUser.Advertiser.Id = advertiserId.Value;

			if (agencyId.HasValue)
				authUser.MediaAgency = new Agency{Id = agencyId.Value, Name = "test1"};

			if (mediaPartnerId.HasValue)
				authUser.MediaPartner = new MediaPartner{Id = mediaPartnerId.Value};

			Auth.Init(new AuthForUnitTests(true, userId, "user1", role, (name) => authUser));
		}


		public static IocRegistration SetupIoCContainer(IocRegistration _container)
		{
			_container = new IocRegistration();
			_container.RegisterSingleton(() => MockBuilder<Role>.GetPopulatedRepository(MockEntities.GetRoles));
			_container.RegisterSingleton(() => MockBuilder<User>.GetPopulatedRepository(MockEntities.GetUsers));
			_container.RegisterSingleton(() => MockBuilder<Campaign>.GetPopulatedRepository(MockEntities.GetCampaigns));
			_container.RegisterSingleton(() => MockBuilder<CmsModelInstance>.GetPopulatedRepository(MockEntities.CreateModelInstances));
			_container.RegisterSingleton(() => MockBuilder<Resource>.GetPopulatedRepository(MockEntities.CreateResources));
			_container.RegisterSingleton(() => MockBuilder<Page>.GetPopulatedRepository(MockEntities.CreatePages));
			_container.RegisterSingleton(() => MockBuilder<Creative>.GetPopulatedRepository(MockEntities.CreateCreatives));
			_container.RegisterSingleton(() => MockBuilder<FileType>.GetPopulatedRepository(MockEntities.CreateFileTypes));
			_container.RegisterSingleton(() => MockBuilder<FieldType>.GetPopulatedRepository(MockEntities.CreateFieldTypes));
			_container.RegisterSingleton(() => MockBuilder<ResourceType>.GetPopulatedRepository(MockEntities.CreateResourceTypes));
			_container.RegisterSingleton(() => MockBuilder<Platform>.GetPopulatedRepository(MockEntities.GetAllPlatforms));
			_container.RegisterSingleton(() => MockBuilder<CmsSetting>.GetPopulatedRepository(MockEntities.CreateCmsSettings));
			_container.RegisterSingleton(() => MockBuilder<CmsSettingInstance>.GetPopulatedRepository(MockEntities.CreateSettingInstances));
			_container.RegisterSingleton(() => MockBuilder<CmsModel>.GetPopulatedRepository(MockEntities.CreateModels));
			_container.RegisterSingleton(() => MockBuilder<DeliveryGroup>.GetPopulatedRepository(MockEntities.CreateDeliveryGroups));
			_container.RegisterSingleton(() => MockBuilder<Ad>.GetPopulatedRepository(MockEntities.CreateAds));
			_container.RegisterSingleton(() => MockBuilder<Blueprint>.GetPopulatedRepository(MockEntities.CreateBlueprints));
			_container.RegisterSingleton(() => MockBuilder<Expose>.GetPopulatedRepository(MockEntities.CreateExposes));
			_container.RegisterSingleton(() => MockBuilder<CmsRefType>.GetPopulatedRepository(MockEntities.CreateCmsRefTypes));
			_container.RegisterSingleton(() => MockBuilder<CmsModelDefinition>.GetPopulatedRepository(MockEntities.CreateCmsModelDefinitions));
			_container.RegisterSingleton(() => MockBuilder<ValidationType>.GetPopulatedRepository(MockEntities.CreateValidationTypes));
			_container.RegisterSingleton(() => MockBuilder<FileTypeValidation>.GetPopulatedRepository(MockEntities.CreateFileTypeValidations));
			_container.RegisterSingleton(() => MockBuilder<PageDefinition>.GetPopulatedRepository(MockEntities.CreatePageDefinitions));
			_container.RegisterSingleton(() => MockBuilder<CmsField>.GetPopulatedRepository(MockEntities.CreateCmsFields));
			_container.RegisterSingleton(() => MockBuilder<Validation>.GetPopulatedRepository(MockEntities.CreateValidations));
			_container.RegisterSingleton(() => MockBuilder<CmsRef>.GetPopulatedRepository(MockEntities.CreateCmsRefs));
			_container.RegisterSingleton(() => MockBuilder<CmsSettingDefinition>.GetPopulatedRepository(MockEntities.CreateCmsSettingDefinitions));
			_container.RegisterSingleton(() => MockBuilder<FeatureType>.GetPopulatedRepository(MockEntities.CreateFeatureTypes));
			_container.RegisterSingleton(() => MockBuilder<FeatureTypeGroup>.GetPopulatedRepository(MockEntities.CreateFeatureTypeGroups));
			_container.RegisterSingleton(() => MockBuilder<StorageSource>.GetPopulatedRepository(MockEntities.CreateStorageSources));
			_container.RegisterSingleton(() => MockBuilder<MediaPartner>.GetPopulatedRepository(MockEntities.CreateMediaPartners));
			_container.RegisterSingleton(() => MockBuilder<Advertiser>.GetPopulatedRepository(MockEntities.GetAdvertisers));
			_container.RegisterSingleton(() => MockBuilder<AuditEvent>.GetPopulatedRepository(MockEntities.CreateAuditEvents));
			_container.RegisterSingleton(() => MockBuilder<AdType>.GetPopulatedRepository(MockEntities.CreateAdTypes));
			_container.RegisterSingleton(() => MockBuilder<Agency>.GetPopulatedRepository(MockEntities.CreateAgencies));
			_container.RegisterSingleton(() => MockBuilder<Product>.GetPopulatedRepository(MockEntities.CreateProducts));
			_container.RegisterSingleton(() => MockBuilder<Category>.GetPopulatedRepository(MockEntities.CreateCategories));
			_container.RegisterSingleton(() => MockBuilder<Placement>.GetPopulatedRepository(MockEntities.CreatePlacements));
			_container.RegisterSingleton(() => MockBuilder<AdTypeGroup>.GetPopulatedRepository(MockEntities.CreateAdTypeGroups));
			_container.RegisterSingleton(() => MockBuilder<AdTag>.GetPopulatedRepository(MockEntities.CreateAdTags));
			_container.RegisterSingleton(() => MockBuilder<AdFormat>.GetPopulatedRepository(MockEntities.CreateAdFormats));
			_container.RegisterSingleton(() => MockBuilder<AdTrackingEvent>.GetPopulatedRepository(MockEntities.CreateAdTrackingEvents));
			_container.RegisterSingleton(() => MockBuilder<TrackingEvent>.GetPopulatedRepository(MockEntities.CreateTrackingEvents));


			_container.Register<ICache, Cache>();
			_container.Register<IUserService, UserService>();
			_container.Register<IRepository<CampaignContentSchema>, EntityRepositoryInMemory<CampaignContentSchema>>();
			_container.Register<ICampaignService, CampaignService>();
			_container.Register<IRoleService, RoleService>();
			_container.Register<IModelInstanceService, ModelInstanceService>();
			_container.Register<IModelInstanceValidationService, ModelInstanceValidationService>();
			_container.Register<IModelInstanceSaveService, ModelInstanceSaveService>();
			_container.Register<IModelInstancePublishedJsonService, ModelInstancePublishedJsonService>();
			_container.Register<IModelInstanceRetrievalService, ModelInstanceRetrievalService>();
			_container.Register<IModelInstanceSaveServerPropertiesService, ModelInstanceSaveServerPropertiesService>();
			_container.Register<IModelService, ModelService>();
			_container.Register<ICreativeService, CreativeService>();
			_container.Register<IRepository<CmsModelInstanceField>, EntityRepositoryInMemory<CmsModelInstanceField>>();
			_container.Register<IRepository<CmsModelInstanceFieldValue>, EntityRepositoryInMemory<CmsModelInstanceFieldValue>>();
			_container.Register<ICmsService, CmsService>();
			_container.Register<IResourceService, ResourceService>();
			_container.Register<ISettingsService, MockSettingService>();
			_container.Register<IHttpContextHelper, MockHttpContextHelper>();
			_container.Register<ICmsModelInstanceService, CmsModelInstanceService>();
			_container.Register<ISettingService, SettingService>();
			_container.Register<IDateHelperService, MockDateHelperService>();
			_container.Register<IAdService, AdService>();
			_container.Register<IAdTagsExportService, AdTagsExportService>();
			_container.Register<IBlueprintService, BlueprintService>();
			_container.Register<IBlueprintImportService, BlueprintImportService>();
			_container.Register<ICmsModelDefinitionService, CmsModelDefinitionService>();
			_container.Register<IPageDefinitionService, PageDefinitionService>();
			_container.Register<IValidationTypeService, ValidationTypeService>();
			_container.Register<ICmsFieldService, CmsFieldService>();
			_container.Register<IValidationService, ValidationService>();
			_container.Register<IFileTypeValidationService, FileTypeValidationService>();
			_container.Register<IFieldTypeService, FieldTypeService>();
			_container.Register<IBlueprintImportValidationService, BlueprintImportValidationService>();
			_container.Register<IBlueprintImportFieldsService, BlueprintImportFieldsService>();
			_container.Register<IBlueprintImportModelsService, BlueprintImportModelsService>();
			_container.Register<IBlueprintImportSettingsService, BlueprintImportSettingsService>();
			_container.Register<IBlueprintImportPagesService, BlueprintImportPagesService>();
			_container.Register<IStorageSourceService, StorageSourceService>();
			_container.Register<IAuditEventService, AuditEventService>();		
			_container.Register<ICmsFieldHelper, CmsFieldHelper>();	
			_container.Register<ISettingInstancePublishedJsonService, SettingInstancePublishedJsonService>();
			_container.Register<ISettingInstanceService, SettingInstanceService>();
			_container.Register<ISettingInstanceValidationService, SettingInstanceValidationService>();
			_container.Register<ISettingInstanceSaveService, SettingInstanceSaveService>();
			_container.Register<ISettingInstanceRetrievalService, SettingInstanceRetrievalService>();
			_container.Register<IModelInstanceLookupsService, ModelInstanceLookupsService>();
			_container.Register<ISettingInstanceLookupsService, SettingInstanceLookupsService>();
			_container.Register<IAdValidationService, AdValidationService>();
			_container.Register<IAgencyService, AgencyService>();
			_container.Register<IMediaPartnerService, MediaPartnerService>();
			_container.Register<IPlacementService, PlacementService>();
			_container.Register<IAdTagService, AdTagService>();
			_container.Register<IAdTrackingEventService, AdTrackingEventService>();
			_container.Register<IHtml5BrandDestinationService, Html5BrandDestinationService>();
			_container.Register<ICampaignLookupsService, CampaignLookupsService>();
			_container.Register<ICampaignPermissionsService, CampaignPermissionsService>();
			_container.Register<ICampaignsListingService, CampaignsListingService>();
			_container.Register<ICampaignSummaryService, CampaignSummaryService>();

			IocSetup.Setup(_container);

			MockHelper.BuildMockLookups();

			MockHttpContext.Init("admin@bl-test.tv");

			return _container;
		}

		
		
	}
}
