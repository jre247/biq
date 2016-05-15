using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Enums;
using BrightLine.Data;
using BrightLine.Web.Areas.Admin.Controllers;
using BrightLine.Web.Areas.Campaigns.Controllers;
using BrightLine.Web.Areas.Developer.Controllers;
using BrightLine.Web.Controllers;
using BrightLine.Web.Helpers;
using BrightLine.Web.Models.Security;
using Elmah.Mvc;
using FluentSecurity;
using FluentSecurity.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RouteDebug;
using BrightLine.Web.App_Start;
using BrightLine.Common.Services;
using BrightLine.Service.Redis.Interfaces;
using BrightLine.Publishing.Constants;

namespace BrightLine.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static string AppIdentifier { get; set; }

		#region Protected methods

		protected void Application_Start()
		{
			LoadConfiguration();
			SetupViewEngines();
			Bootstrap();
			ConfigureUserSecurity();
			GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			SetupRedisSubscriptions();

			this.EndRequest += Application_EndRequest;
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			var users = IoC.Resolve<IUserService>();
			var settings = IoC.Resolve<ISettingsService>();

			if ((HttpContext.Current.Request.IsSecureConnection == false) && (settings.CurrentEnvironment == EnvironmentType.PRO || settings.CurrentEnvironment == EnvironmentType.UAT))
			{
				Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl);
				// Log the request.
				IoC.Log.Error(HttpContext.Current.Request.Url.AbsoluteUri);
			}

			RequestTimer.Start(HttpContext.Current.Request.RawUrl);

			users.AuditActivity(Auth.UserModel);
		}

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			// Check for auth cookie.
			var cookies = IoC.Resolve<ICookieService>();

			var ticket = cookies.GetAuthenticationTicket();
			if (ticket == null)
				return;

			// Build up the user identity.
			var identity = new CustomIdentity(ticket);
			var principal = new CustomPrincipal(identity);
			HttpContext.Current.User = principal;

			// FEATURE : IQ-283: Allow Admins to view application as different role
			AuthWebAdminHelper.HandleOverride(principal);
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			var flashMessageExtensions = IoC.Resolve<IFlashMessageExtensions>();

			var exception = Server.GetLastError();
			flashMessageExtensions.Debug(exception);

			// Log the exception.
			IoC.Log.Error(exception);
		}

		/// <summary>
		/// Handler for end of request 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_EndRequest(object sender, EventArgs e)
		{
			OLTPContextSingleton.Dispose();
			RequestTimer.Dump();
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			var users = IoC.Resolve<IUserService>();

			users.AuditLogin(Auth.UserModel);
			
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Setup subscriptions to Redis's Pub/Sub
		/// </summary>
		private void SetupRedisSubscriptions()
		{
			var redisSubscriptionsService = IoC.Resolve<IRedisSubscriptionsService>();
			redisSubscriptionsService.Setup();
		}

		private static void LoadConfiguration()
		{
			// Set the environment first.
			// NOTE: The method "DefaultFromConfig" is made private for safety purposes
			// to prevent any code from publicly changing the environment.
			typeof(Env).GetMethod("LoadFromConfig", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
			AppIdentifier = Guid.NewGuid().ToString("N");
		}

		private static void SetupViewEngines()
		{
			// remove all view engines, then readd Razor (only one used currently)
			ViewEngines.Engines.Clear();
			var rve = new RazorViewEngine();
			ViewEngines.Engines.Add(rve);
		}

		private static void Bootstrap()
		{
			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			RewriteRoutesForTesting(RouteTable.Routes);
			App_Start.Bootstrapper.Bootstrap();
			SetDefaultSerializerSettings();
			SetModelBindingOverrides();
		}

		private static void ConfigureUserSecurity()
		{
			var roleService = IoC.Resolve<IRoleService>();

			SecurityConfigurator.Configure(c =>
			{
				c.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);
				c.GetRolesFrom(() =>
				{
					// FEATURE : IQ-283: Allow Admins to view application as different role
					var overriddenUser = AuthWebAdminHelper.HandleOverride(HttpContext.Current.User);
					if (overriddenUser != null)
						return new[] { overriddenUser.OverrideRole };

					var name = HttpContext.Current.User.Identity.Name;
					var roles = roleService.GetRoles(name);
					return roles;
				});
				c.ResolveServicesUsing(type =>
				{
					if (type != typeof(IPolicyViolationHandler))
						return Enumerable.Empty<object>();

					var types = Assembly
						.GetAssembly(typeof(MvcApplication))
						.GetTypes()
						.Where(x => typeof(IPolicyViolationHandler).IsAssignableFrom(x)).ToList();

					var handlers = types.Select(t => Activator.CreateInstance(t) as IPolicyViolationHandler).ToList();
					return handlers;
				});

				c.ForAllControllersInNamespaceContainingType<BaseController>().DenyAnonymousAccess();

				c.For<AccountController>().DenyAuthenticatedAccess();
				c.For<AccountController>(x => x.SignOut()).DenyAnonymousAccess();
				c.For<AccountController>(x => x.Redirect()).DenyAnonymousAccess();
				c.For<AccountController>(x => x.UpdatePassword()).DenyAnonymousAccess();
				c.For<AccountController>(x => x.Settings()).DenyAnonymousAccess();
				c.For<AccountController>(x => x.ChangeRole(0)).DenyAnonymousAccess();
				c.For<AccountController>(x => x.ResetRole()).DenyAnonymousAccess();

				c.ForAllControllersInNamespaceContainingType<DeveloperController>().RequireAnyRole(AuthConstants.Roles.Developer);
				c.For<ElmahController>().RequireAnyRole(AuthConstants.Roles.Developer);

				c.ForAllControllersInNamespaceContainingType<AdminController>().RequireAnyRole(AuthConstants.Roles.Admin);

				c.For<ErrorController>().Ignore();
				c.For<CampaignsController>().RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer, AuthConstants.Roles.AgencyPartner, AuthConstants.Roles.Client, AuthConstants.Roles.Employee);
				c.ForActionsMatching(info => info.ControllerType == typeof(CampaignsController) &&
				(new[] { "Create", "Edit" }).Contains(info.ActionName)).RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer);

				// only allow admins and developers to get list of entity models and lookups.
				c.ForActionsMatching(info => info.ControllerType == typeof(EntityApiController) &&
				(new[] { "GetLookups", "GetModels" }).Contains(info.ActionName)).RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer);

				c.For<CMSController>().RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer, AuthConstants.Roles.AppDeveloper, AuthConstants.Roles.CMSAdmin, AuthConstants.Roles.CMSEditor);
				c.For<BlueprintsController>().RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer);

				var modelFilter = new DelegatePolicy("GenericEmployeeCrud", (dsc =>
				{
					var request = HttpContext.Current.Request;
					var model = request.RequestContext.RouteData.Values["model"];
					if (model == null)
						return PolicyResult.CreateFailureResult(dsc.Policy, "Model cannot be empty.");

					var employeeModels = new[] { "campaign", "experience", "checklist" };
					var isEmployeeAccessible = employeeModels.Contains(model.ToString().ToLower());

					var isAdmin = dsc.CurrentUserRoles().Contains(AuthConstants.Roles.Admin);
					var isDeveloper = dsc.CurrentUserRoles().Contains(AuthConstants.Roles.Developer);

					if (!isEmployeeAccessible && !isAdmin && !isDeveloper)
						return PolicyResult.CreateFailureResult(dsc.Policy, "Access denied");

					return PolicyResult.CreateSuccessResult(dsc.Policy);
				}));

				c.For<EntityController>().RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer, AuthConstants.Roles.Employee).AddPolicy(modelFilter);
				c.For<UsersController>().RequireAnyRole(AuthConstants.Roles.Admin, AuthConstants.Roles.Developer);
			});
		}

		private static void RewriteRoutesForTesting(RouteCollection routes)
		{
			if (!ConfigurationManager.AppSettings.GetBoolean("RouteDebugger:Enabled"))
				return;

			using (routes.GetReadLock())
			{
				var foundDebugRoute = false;
				foreach (var routeBase in routes)
				{
					var route = routeBase as Route;
					if (route != null)
						route.RouteHandler = new DebugRouteHandler();

					if (route == DebugRoute.Singleton)
						foundDebugRoute = true;

				}
				if (!foundDebugRoute)
					routes.Add(DebugRoute.Singleton);
			}
		}

		private static void SetDefaultSerializerSettings()
		{
			var jsonMediaTypeFormatter = GlobalConfiguration.Configuration.Formatters.FirstOrDefault(f => f.GetType() == typeof(JsonMediaTypeFormatter));
			var xmlMediaTypeFormatter = GlobalConfiguration.Configuration.Formatters.FirstOrDefault(f => f.GetType() == typeof(XmlMediaTypeFormatter));
			GlobalConfiguration.Configuration.Formatters.Remove(jsonMediaTypeFormatter);
			GlobalConfiguration.Configuration.Formatters.Remove(xmlMediaTypeFormatter);
			var camelFormatter = new JsonMediaTypeFormatter
			{
				SerializerSettings =
				{
					ContractResolver = new PropertyContractResolver(isCamel: true),// CamelCasePropertyNamesContractResolver(),
					NullValueHandling = NullValueHandling.Include,
					PreserveReferencesHandling = PreserveReferencesHandling.None,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			};
			//add a camel case formatter for the default
			GlobalConfiguration.Configuration.Formatters.Insert(0, camelFormatter);
		}

		private static void SetModelBindingOverrides()
		{
			ModelBinders.Binders.DefaultBinder = new EntityModelBinder();
		}

		#endregion

		private static class RequestTimer
		{
			private static Stopwatch _stopWatch;
			private static string _url;

			[Conditional("LOG_REQUEST")]
			public static void Start(string url)
			{
				_url = url;
				_stopWatch = Stopwatch.StartNew();
			}

			[Conditional("LOG_REQUEST")]
			public static void Dump()
			{
				Debug.WriteLine("request: {0} - {1}ms", _url, _stopWatch == null ? 0 : _stopWatch.ElapsedMilliseconds);
			}
		}
	}
}