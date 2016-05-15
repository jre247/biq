using System;
using System.Linq;
using System.Security.Principal;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Decorator patter for supporting role override ONLY for Admins!
	/// FEATURE : IQ-283: Allow Admins to view application as different role
	/// </summary>
	public class UserPrincipalWithOverride : IPrincipal
	{
		/// <summary>
		/// Gets the actual user
		/// </summary>
		public IPrincipal Actual { get; private set; }

		/// <summary>
		/// Get the actual identity.
		/// </summary>
		public IIdentity Identity { get { return Actual.Identity; } }

		/// <summary>
		/// Get the overriden role.
		/// </summary>
		public string OverrideRole { get; private set; }

		/// <summary>
		/// Override role for admin.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="overrideRole"></param>
		public UserPrincipalWithOverride(IPrincipal user, string overrideRole)
		{
			if (user == null)
				throw new ArgumentException("User principal must be supplied.");

			// Check - don't allow overrides unless admin.
			if (!user.IsInRole(AuthConstants.Roles.Admin))
				throw new ArgumentException("User role override only available for administrators.");

			Actual = user;
			OverrideRole = overrideRole;
		}

		/// <summary>
		/// Check whether or not the user in the role.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			if (string.IsNullOrEmpty(role))
				return false;

			if (OverrideRole == role)
				return true;

			// Check the override role first.
			var isInRole = OverrideRole.Split(',').Any(s => String.CompareOrdinal(role, s) == 0);
			return isInRole;
		}
	}
}
