using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Provides static access to the all the <see cref="IAuth"/> methods in the current <see cref="IAuth"/> provider being used.
	/// <para>
	/// Auth.IsAdmin();
	/// Auth.IsGuest();
	/// Auth.UserName;
	/// etc.
	/// </para>
	/// </summary>
	public class Auth
	{
		private static IAuth _provider = new AuthWeb(email => { return IoC.Resolve<IUserService>().GetUserByEmail(email); });

		/// <summary>
		/// Access to the impelementation.
		/// </summary>
		public static IAuth Service { get { return _provider; } }

		/// <summary>
		/// The name of the current user.
		/// </summary>
		public static string UserName { get { return _provider.UserName; } }

		/// <summary>
		/// The name of the current user.
		/// </summary>
		public static string UserEmail { get { return (_provider.UserModel != null) ? _provider.UserModel.Email : ""; } }

		/// <summary>
		/// Get the current user.
		/// </summary>
		public static IPrincipal User { get { return _provider.User; } }

		/// <summary>
		/// Get the BrightLine.Common.Models.User 
		/// </summary>
		public static Models.User UserModel { get { return _provider.UserModel; } }

		/// <summary>
		/// Get the BrightLine.Common.Models.User 
		/// </summary>
		public static TimeZoneInfo UserTimeZoneInfo { get { return _provider.UserTimeZoneInfo; } }

		public static IEnumerable<string> Roles
		{
			get
			{
				// FEATURE : IQ-283: Allow Admins to view application as different role
				var overriddenUser = AuthWebAdminHelper.HandleOverride(User);
				if (overriddenUser != null)
					return new[] { overriddenUser.OverrideRole };

				return UserModel.Roles.Select(r => r.Name);
			}
		}

		/// <summary>
		/// Initialize the current <see cref="IAuth"/> provider.
		/// </summary>
		/// <param name="provider"></param>
		public static void Init(IAuth provider)
		{
			_provider = provider;
		}

		/// <summary>
		/// Return whether or not the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		public static bool IsAuthenticated()
		{
			return _provider.IsAuthenticated();
		}

		/// <summary>
		/// Return whether or not the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		public static bool IsGuest()
		{
			return _provider.IsGuest();
		}

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public static bool IsUserInRole(string role)
		{
			return _provider.IsUserInAnyRole(role);
		}

		/// <summary>
		/// Determine if the logged in user is the same as the username supplied.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public static bool IsLoggedInUser(string username)
		{
			return _provider.IsLoggedInUser(username);
		}

		/// <summary>
		/// Checks if user is in any of the roles supplied.
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		public static bool IsUserInAnyRole(params string[] roles)
		{
			return _provider.IsUserInAnyRole(roles);
		}
	}
}
