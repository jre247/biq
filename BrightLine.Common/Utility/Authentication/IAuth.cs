using System;
using System.Security.Principal;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Interface for Authentication.
	/// </summary>
	public interface IAuth
	{
		/// <summary>
		/// Determines whether the user is authenticted.
		/// </summary>
		/// <returns></returns>
		bool IsAuthenticated();
		
		/// <summary>
		/// Return whether or not the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		bool IsGuest();
		
		/// <summary>
		/// The name of the current user.
		/// </summary>
		string UserName { get; }
		
		/// <summary>
		/// Get the current user.
		/// </summary>
		IPrincipal User { get; }

		/// <summary>
		/// Get the current User model.
		/// </summary>
		Models.User UserModel { get; }
		
		/// <summary>
		/// Get the current User model.
		/// </summary>
		TimeZoneInfo UserTimeZoneInfo { get; }
		
		/// <summary>
		/// Is User in the selected role.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		bool IsUserInRole(string role);

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		bool IsUserInAnyRole(params string[] roles);

		/// <summary>
		/// Determine if the logged in user is the same as the username supplied.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		bool IsLoggedInUser(string username);
	}
}
