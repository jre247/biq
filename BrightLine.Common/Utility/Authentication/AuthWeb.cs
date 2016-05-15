using BrightLine.Common.Models;
using System;
using System.Security.Principal;
using System.Web;


namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// <see cref="IAuth"/> implementation to provide Authentication service using the web based User(principal) object exposed in the context.Current.User object.
	/// </summary>
	public class AuthWeb : AuthBase, IAuth
	{
		private readonly Func<string, User> _userFunction;
		private TimeZoneInfo _userTimeZoneInfo;

		/// <summary>
		/// Initialize with the admin role name.
		/// </summary>
		/// <param name="userFunction"></param>
		public AuthWeb(Func<string, User> userFunction)
		{
			_userFunction = userFunction;
		}

		/// <summary>
		/// Get the current user.
		/// </summary>
		public IPrincipal User { get { return HttpContext.Current.User; } }

		/// <summary>
		/// Gets the user object associated w/ the user name.
		/// </summary>
		public User UserModel { get { return _userFunction(this.UserName); } }

		/// <summary>
		/// Gets the timezone associated w/ the user.
		/// </summary>
		public TimeZoneInfo UserTimeZoneInfo { get { { return _userTimeZoneInfo ?? (_userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(this.UserModel.TimeZoneId)); } } }

		/// <summary>
		/// The name of the current user.
		/// </summary>
		public override string UserName { get { return !IsAuthenticated() ? "" : HttpContext.Current.User.Identity.Name; } }

		/// <summary>
		/// Determine if the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		public override bool IsAuthenticated()
		{
			if (HttpContext.Current == null)
				return false;
			if (HttpContext.Current.User == null)
				return false;

			return HttpContext.Current.User.Identity.IsAuthenticated;
		}

		/// <summary>
		/// Return whether or not the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		public bool IsGuest()
		{
			return !IsAuthenticated();
		}

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public override bool IsUserInRole(string role)
		{
			return IsUserInAnyRole(role);
		}

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		public override bool IsUserInAnyRole(params string[] roles)
		{
			if (!IsAuthenticated())
				return false;

			if (roles == null || roles.Length == 0)
				return false;

			foreach (var role in roles)
			{
				if (HttpContext.Current.User.IsInRole(role))
					return true;
			}

			return false;
		}
	}
}
