using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BrightLine.Common.Utility.Authentication
{
	public class AuthWebAdminHelper
	{
		public static string OverriddenRole { get; private set; }

		/// <summary>
		/// Checks whether admin roles are overriden temporarily and updates the http user identity.
		/// </summary>
		/// <param name="user"></param>
		public static UserPrincipalWithOverride HandleOverride(IPrincipal user)
		{
			var cookies = IoC.Resolve<ICookieService>();
			var roles = IoC.Resolve<IRoleService>();

			// FEATURE : IQ-283: Allow Admins to view application as different role
			// Check 1: Only admins can temporarily alter role.
			var overriddenUser = user as UserPrincipalWithOverride;
			if (overriddenUser != null)
				return (UserPrincipalWithOverride)user;

			var isAdmin = user.IsInRole(AuthConstants.Roles.Admin);
			// Only allow admins to override role.
			if (!isAdmin)
				return null;

			// Get override role cookie.
			var overriddenRole = cookies.Get(AuthConstants.Cookies.AdminRoleOverride);
			if (string.IsNullOrWhiteSpace(overriddenRole))
				return null;

			// Can not set the override role to admin itself.
			if (AuthConstants.Roles.Admin == overriddenRole)
				return null;

			// Override the identity.
			var userOverride = new UserPrincipalWithOverride(user, overriddenRole);
			HttpContext.Current.User = userOverride;
			OverriddenRole = roles.GetAll().FirstOrDefault(r => r.Name == overriddenRole).DisplayName;
			return userOverride;
		}

		/// <summary>
		/// Changes role to role id supplied. ( drops an override role cookie for admins only ).
		/// </summary>
		/// <param name="roleId"></param>
		public static void ChangeRole(int roleId)
		{
			var cookies = IoC.Resolve<ICookieService>();
			var roles = IoC.Resolve<IRoleService>();

			if (!Auth.Service.IsAdmin(true))
				return;

			var role = roles.Get(roleId);
			if (role == null)
				return;

			cookies.Set(AuthConstants.Cookies.AdminRoleOverride, role.Name);
		}

		/// <summary>
		/// Changes role to role id supplied. ( drops an override role cookie for admins only ).
		/// </summary>
		public static void ResetRole()
		{
			var cookies = IoC.Resolve<ICookieService>();

			cookies.Remove(AuthConstants.Cookies.AdminRoleOverride);
			OverriddenRole = null;
		}
	}
}
