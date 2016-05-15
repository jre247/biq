using System.Linq;
using System.Reflection;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Auto-generated extension methods from roles in the database. ( Used for commonly used roles )
	/// </summary>
	public static class AuthExtensions
	{
		public static bool IsDeveloper(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.Developer); }

		public static bool IsAppDeveloper(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.AppDeveloper); }

		public static bool IsAdmin(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.Admin); }

		public static bool IsEmployee(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.Employee); }
		
		public static bool IsImplementationAssistantAdmin(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.ImplementationAssistantAdmin); }

		public static bool IsCmsAdmin(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.CMSAdmin); }

		public static bool IsCmsEditor(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.CMSEditor); }

		public static bool IsClient(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.Client); }

		public static bool IsAgencyPartner(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.AgencyPartner); }

		public static bool IsMediaPartner(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.MediaPartner); }

		public static bool IsCampaignAdmin(this IAuth auth) { return auth.IsUserInRole(AuthConstants.Roles.CampaignAdmin); }

		public static bool IsUser(this IAuth auth)
		{
			// get all the roles defined as constants in AuthConstants
			var fields = typeof(AuthConstants.Roles).GetFields(BindingFlags.Static | BindingFlags.Public);
			return auth.IsUserInAnyRole((from fi in fields select fi.GetValue(null).ToString()).ToArray());
		}

		/// <summary>
		/// Checks whether the current user is an admin and/or user has his admin role overriden to mimick another role.
		/// </summary>
		/// <param name="auth"></param>
		/// <param name="checkOverride">Whether or not if the user is really an admin with his role overriden.</param>
		/// <returns></returns>
		public static bool IsAdmin(this IAuth auth, bool checkOverride)
		{
			if (!checkOverride)
				return auth.IsUserInRole(AuthConstants.Roles.Admin);

			// Could be overriden, check authentication first.
			if (!auth.IsAuthenticated())
				return false;

			// CASE 1. Admin role not overriden ( really an admin )
			if (auth.IsUserInRole(AuthConstants.Roles.Admin))
				return true;

			// CASE 2. User is really an admin but role is overriden temporarily.
			var @override = auth.User as UserPrincipalWithOverride;
			if (@override != null && @override.Actual.IsInRole(AuthConstants.Roles.Admin))
				return true;

			return false;
		}
	}
}
