using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Spreadsheets;
using BrightLine.Service.External.AmazonStorage;
using BrightLine.Utility;
using BrightLine.Utility.Commands;
using BrightLine.Utility.Menus;
using BrightLine.Web.Areas.Admin.Controllers;
using BrightLine.Web.Areas.Developer.Controllers;
using BrightLine.Web.Controllers;
using Common.Logging;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace BrightLine.Web.App_Start
{
	public partial class Bootstrapper
	{
		public static void Bootstrap()
		{
			var container = IoC.Container;
			container.Options.AllowOverridingRegistrations = true;

			// initialize concrete implementations from Data and Service projects
			Data.Bootstrapper.InitializeContainer(container);
			Service.Bootstrapper.InitializeContainer(container);
			Publishing.Bootstrapper.InitializeContainer(container);
			Cms.Bootstrapper.InitializeContainer(container);
			EnvironmentBootStrapper.InitializeContainer(container);
			AspectBootStrapper.InitializeContainer(container);
			MenuBootstrapper.InitializeContainer(container);
			
			// Register the cache ( thin wrapper aorund memory cache to get callbacks on updates/deletes for listing of cache items in admin UI ).
			container.Register<ICache>(() => new Cache(), Lifestyle.Singleton);


			// Verify all the registrations are correct.
			container.Verify();

			var users = IoC.Resolve<IUserService>();
			var auditEvents = IoC.Resolve<IAuditEventService>();
			
			// Initialize the Auth lookup
			Auth.Init(new AuthWeb(username =>
				{
					var svc = users;
					return svc.GetUserByEmail(username);
				}));


			// Configure the Command auditing
			Command.Auditor = (action, group, source) => auditEvents.Audit(action, group, source);

			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

			AsposeRegistration.RegisterLicense();

			Lookups.InitializeLookupDictionaries();
		}
	}


	public class MenuBootstrapper
	{
		/// <summary>
		/// Initialize the menu.
		/// </summary>
		/// <param name="container"></param>
		public static void InitializeContainer(Container container)
		{
			var menu = new Menu()
				{
					Items = new List<MenuItem>()
						{
							new MenuItem()
								{
									CssId = "campaigns-tab",
									Text = "Campaigns",
									Action = "Spa",
									Controller = typeof (BaseController),
									Roles =
										new string[]
											{
												AuthConstants.Roles.Admin, AuthConstants.Roles.AgencyPartner, AuthConstants.Roles.Client,
												AuthConstants.Roles.Employee, AuthConstants.Roles.AdOps, AuthConstants.Roles.AccountManager
											},
									RouteValues = new RouteValueDictionary() {{"Area", null}}
								},
							new MenuItem()
								{
									CssId = "cms-tab",
									Text = "CMS",
									Action = "Index",
									Controller = typeof (CMSController),
									Roles =
										new string[]
											{
												AuthConstants.Roles.Admin, AuthConstants.Roles.Developer, AuthConstants.Roles.CMSAdmin,
												AuthConstants.Roles.CMSEditor, AuthConstants.Roles.AppDeveloper
											},
									RouteValues = new RouteValueDictionary() {{"Area", null}}
								},
					
							new MenuItem()
								{
									CssId = "blueprints-tab",
									Text = "Blueprints",
									Action = "Index",
									Controller = typeof (BlueprintsController),
									Roles =
										new string[]
											{
												AuthConstants.Roles.Admin, AuthConstants.Roles.Developer, AuthConstants.Roles.BlueprintAdmin
											},
									RouteValues = new RouteValueDictionary() {{"Area", null}}
								},
							new MenuItem()
								{
									CssId = "admin-tab",
									Text = "Admin",
									Roles = new[] {AuthConstants.Roles.Admin},
									Children = new List<MenuItem>()
										{
											new MenuItem()
												{
													Text = "Resource Types",
													Action = "ResourceTypes",
													Controller = typeof (AdminController),
													RouteValues = new RouteValueDictionary() {{"Area", "admin"}}
												},
											new MenuItem()
												{
													Text = "Settings",
													Action = "Index",
													Controller = typeof (SettingsController),
													RouteValues = new RouteValueDictionary() {{"Area", null}}
												},
											new MenuItem()
												{
													Text = "Users",
													Action = "Index",
													Controller = typeof (UsersController),
													RouteValues = new RouteValueDictionary() {{"Area", "admin"}}
												},
											new MenuItem()
												{
													Text = "Entities",
													Children = new List<MenuItem>()
														{
															new MenuItem()
															{
																Text = "Advertisers",
																Action = "List",
																Controller = typeof (EntityController),
																RouteValues = new RouteValueDictionary() {{"model", "advertiser"}, {"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
															{
																Text = "Agencies",
																Action = "List",
																Controller = typeof (AgenciesController),
																RouteValues = new RouteValueDictionary() {{"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
															{
																Text = "Apps",
																Action = "List",
																Controller = typeof (AppsController),
																RouteValues = new RouteValueDictionary() {{"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
																											
															new MenuItem()
															{
																Text = "Brands",
																Action = "List",
																Controller = typeof (EntityController),
																RouteValues = new RouteValueDictionary() {{"model", "brand"}, {"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
															{
																Text = "Media Partners",
																Action = "List",
																Controller = typeof (MediaPartnersController),
																RouteValues = new RouteValueDictionary() {{"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
															{
																Text = "Placements",
																Action = "List",
																Controller = typeof (PlacementsController),
																RouteValues = new RouteValueDictionary() {{"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
															{
																Text = "Products",
																Action = "List",
																Controller = typeof (EntityController),
																RouteValues = new RouteValueDictionary() {{"model", "product"}, {"Area", "admin"}},
																Roles = new string[] {AuthConstants.Roles.Developer}
															},
															new MenuItem()
																{
																	Text = "Segments",
																	Action = "List",
																	Controller = typeof (EntityController),
																	RouteValues = new RouteValueDictionary() {{"model", "segment"}, {"Area", "admin"}},
																	Roles = new string[] {AuthConstants.Roles.Developer}
																},
															new MenuItem()
																{
																	Text = "Subsegments",
																	Action = "List",
																	Controller = typeof (EntityController),
																	RouteValues = new RouteValueDictionary() {{"model", "subsegment"}, {"Area", "admin"}},
																	Roles = new string[] {AuthConstants.Roles.Developer}
																},
															new MenuItem()
																{
																	Text = "Verticals",
																	Action = "List",
																	Controller = typeof (EntityController),
																	RouteValues = new RouteValueDictionary() {{"model", "vertical"}, {"Area", "admin"}},
																	Roles = new string[] {AuthConstants.Roles.Developer}
																},
														}
												},
										}
								},

							new MenuItem()
								{
									CssId = "developer-tab",
									Text = "Developer",
									Roles = new[] {AuthConstants.Roles.Developer},
									Children = new List<MenuItem>()
										{
											new MenuItem()
												{
													Text = "Overview",
													Action = "Index",
													Controller = typeof (OverviewController),
													RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
												},
											new MenuItem()
												{
													Text = "ELMAH",
													Action = "Elmah",
													Controller = typeof (LogController),
													RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
												},
											new MenuItem()
												{
													Text = "Cache",
													Action = "Index",
													Controller = typeof (CacheController),
													RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
												},
											new MenuItem()
												{
													Text = "Log",
													Action = "Index",
													Controller = typeof (LogController),
													RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
												},
											new MenuItem()
											{
												Text = "Nightwatch Tests",
												Action = "Index",
												Controller = typeof (NightwatchTestsController),
												RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
											},
											new MenuItem()
											{
												Text = "Media Buckets Sync",
												Action = "Index",
												Controller = typeof (MediaBucketsController),
												RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
											},
											new MenuItem()
											{
												Text = "Lookups",
												Action = "Index",
												Controller = typeof (LookupsController),
												RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
											},
											new MenuItem()
											{
												Text = "Cleanup Ads",
												Action = "Index",
												Controller = typeof (AdsController),
												RouteValues = new RouteValueDictionary() {{"Area", "developer"}}
											}
										}
								}
						}
				};

			container.Register<Menu>(() => menu, Lifestyle.Singleton);


			//// NOTE: Query the fluetnscript roles for the pages/actions.
			//// 1. Get all the policies
			//var policies = FluentSecurity.SecurityConfiguration.Current.PolicyContainers.ToList();
			//
			//// 2. Createa lookup based on controller/action.
			//var policyLookup = new Dictionary<string, IPolicyContainer>();
			//foreach (var policy in policies)
			//{
			//    var key = policy.ControllerName + "-" + policy.ActionName;
			//    policyLookup[key] = policy;
			//}
			//// 3. Now update the menu items w/ the roles.
		}
	}



	/// <summary>
	/// Environment specific bootstrapper.
	/// </summary>
	public class EnvironmentBootStrapper
	{
		private static string _rootAppDataDir;
		private static string _resourceLocalDir;


		/// <summary>
		/// Configure the ioc container based on environment.
		/// </summary>
		/// <param name="container"></param>
		public static void InitializeContainer(Container container)
		{
			// Keep track of app data dir for services that may need it.
			_rootAppDataDir = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/");
			_resourceLocalDir = _rootAppDataDir + "ResourceDocs";

			// All environments ( this is done here if Ioc setup
			// can not easily be put in the automatic build service / ioc t4 templates.
			ConfiguringAll(container);

			// Environment specific
			if (Env.IsDev)
				ConfigureDev(container);

			else if (Env.IsCI)
				ConfigureCI(container);

			else if (Env.IsUat)
				ConfigureUAT(container);

			else if (Env.IsProd)
				ConfigureProd(container);

			// Hook for after configuration
			ConfiguredAll(container);
		}


		private static void ConfiguringAll(Container container)
		{	
			var s3Settings = new AmazonS3Settings()
				{
					AccessId = ConfigurationManager.AppSettings["amazons3.AccessId"],
					AccessKey = ConfigurationManager.AppSettings["amazons3.AccessKey"],
					Bucket = ConfigurationManager.AppSettings["amazons3.Bucket"]
				};
			container.Register<ICloudFileService>(() => new AmazonS3Service(s3Settings));
			container.Register<ILog>(() => LogManager.GetLogger("BL.common.logger"));
		}


		private static void ConfigureDev(Container container)
		{
			// container.Register<IFileItemRepository>(() => new FileItemLocalRepository(_resourceLocalDir));			
			// container.Register<IFileItemService>(() => new FileItemService(IoC.Resolve<IRepository<FileItem>>()) { StorageRepo = IoC.Resolve<IFileItemRepository>() });
		}


		private static void ConfigureCI(Container container)
		{

		}


		private static void ConfigureUAT(Container container)
		{

		}


		private static void ConfigureProd(Container container)
		{

		}


		private static void ConfiguredAll(Container container)
		{
		}
	}


	/// <summary>
	/// Entity specific bootstrapper.
	/// </summary>
	public class AspectBootStrapper
	{
		/// <summary>
		/// Configure the ioc container based on environment.
		/// </summary>
		/// <param name="container"></param>
		public static void InitializeContainer(Container container)
		{
			//// Register the attribute with the hook implementation.
			//Observer.RegisterAspects<ObserveOperationAttribute, ObserveOperationAspect>();
			//Observer.RegisterAspects<ObservePropertyAttribute, ObservePropertyRedisAspect>();
		}
	}
}