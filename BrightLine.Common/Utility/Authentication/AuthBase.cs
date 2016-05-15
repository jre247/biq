
namespace BrightLine.Common.Utility.Authentication
{
	public abstract class AuthBase
	{
		/// <summary>
		/// Determine if the current user is authenticated.
		/// </summary>
		/// <returns></returns>
		public abstract bool IsAuthenticated();

		/// <summary>
		/// Is User in the role.
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public abstract bool IsUserInRole(string role);

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		public abstract bool IsUserInAnyRole(params string[] roles);

		/// <summary>
		/// The name of the current user.
		/// </summary>
		public abstract string UserName { get; }

		/// <summary>
		/// Determine if the logged in user is the same as the username supplied.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool IsLoggedInUser(string username)
		{
			// Check for empty usershort name.
			if (string.IsNullOrEmpty(UserName))
				return false;

			return username == UserName;
		}
	}
}
