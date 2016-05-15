using System.Security.Principal;

namespace BrightLine.Common.Utility.Authentication
{
	/// <summary>
	/// Custom Identity class ( Can be used in app, but currently used for automated tests )
	/// </summary>
	public class UserIdentity : IIdentity
	{
		/// <summary>
		/// Create new instance using default initialization,
		/// authenticated = false, username = string.Empty
		/// </summary>
		public UserIdentity() { }

		/// <summary>
		/// Create new instance using supplied values.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="userName"></param>
		/// <param name="authenticationType"></param>
		/// <param name="isAuthenticated"></param>
		public UserIdentity(int id, string userName, string authenticationType, bool isAuthenticated)
		{
			UserId = id;
			IsAuthenticated = isAuthenticated;
			Name = userName;
			AuthenticationType = authenticationType;
		}

		#region IIdentity Members

		/// <summary>
		/// Get the authentication type.
		/// </summary>
		public string AuthenticationType { get; private set; }

		/// <summary>
		/// Indicates if user is authenticated.
		/// </summary>
		public bool IsAuthenticated { get; private set; }

		/// <summary>
		/// Return the username.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get the user id.
		/// </summary>
		public int UserId { get; private set; }

		#endregion
	}
}
