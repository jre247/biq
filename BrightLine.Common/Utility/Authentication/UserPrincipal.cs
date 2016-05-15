using System.Linq;
using System.Security.Principal;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Custom prinical class with additional propertes to identity user.  ( Can be used in app, but currently used for automated tests )
	/// </summary>
	public class UserPrincipal : IPrincipal
	{
		/// <summary>
		/// Create a new default instance.
		/// </summary>
		public UserPrincipal() { }

		/// <summary>
		/// Create new instance using supplied user information.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="userRolesDelimitedByComma"></param>
		/// <param name="identity"></param>
		public UserPrincipal(int userId, string userName, string userRolesDelimitedByComma, IIdentity identity)
		{
			string[] roles = userRolesDelimitedByComma.Split(new char[] { ',' });
			Init(userId, userName, roles, identity);
		}

		/// <summary>
		/// Create new instance using supplied user information.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="userRolesDelimitedByComma"></param>
		/// <param name="authType"></param>
		/// <param name="isAuthenicated"></param>
		public UserPrincipal(int userId, string userName, string userRolesDelimitedByComma, string authType, bool isAuthenicated)
		{
			string[] roles = userRolesDelimitedByComma.Split(new char[] { ',' });
			IIdentity identity = new UserIdentity(userId, userName, authType, isAuthenicated);
			Init(userId, userName, roles, identity);
		}

		/// <summary>
		/// Create new instance using supplied user information.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="roles"></param>
		/// <param name="identity"></param>
		public UserPrincipal(int userId, string userName, string[] roles, IIdentity identity)
		{
			Init(userId, userName, roles, identity);
		}

		/// <summary>
		/// Identity of the principal. 
		/// </summary>
		public IIdentity Identity { get; private set; }

		/// <summary>
		/// Id of the user.
		/// </summary>
		public int UserId { get; private set; }

		/// <summary>
		/// Username.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Comma delimited string of users roles.
		/// </summary>
		public string[] Roles { get; private set; }

		/// <summary>
		/// Initializes this instance with the specified parameters.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="userName"></param>
		/// <param name="roles"></param>
		/// <param name="identity"></param>
		public void Init(int userId, string userName, string[] roles, IIdentity identity)
		{
			Roles = roles;
			Identity = identity;
			UserId = userId;
			Name = userName;
		}

		/// <summary>
		/// Determines if this user is in the role supplied.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			if (Roles == null || !Roles.Any())
				return false;

			foreach (string userRole in Roles)
			{
				if (userRole == role)
					return true;
			}

			return false;
		}
	}
}
