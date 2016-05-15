using BrightLine.Common.Models;
using BrightLine.Common.Utility.Authentication;
using System;
using System.Security.Principal;


namespace BrightLine.Tests.Common
{
    public class AuthForUnitTests : AuthBase, IAuth
    {
        private readonly Func<string, User> _userFunction;
        private readonly IPrincipal _userPrincipal;
        private readonly IIdentity _userIdentity;
        private readonly bool _isAuthenticated;
        private User _user;
        private TimeZoneInfo _userTimeZoneInfo;


        /// <summary>
        /// Initialize with the admin role name.
        /// </summary>
        public AuthForUnitTests(Func<string, User> userFunction)
        {
            _userFunction = userFunction;
        }


        /// <summary>
        /// Initialize a user for unit-tests
        /// </summary>
        /// <param name="isAuthenticated"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="rolesDelimited"></param>
        public AuthForUnitTests(bool isAuthenticated, int userId, string userName, string rolesDelimited, Func<string, User> userFetcher)
        {
            _isAuthenticated = isAuthenticated;
            _userFunction = userFetcher;
            if (isAuthenticated)
            {
                _userIdentity = new UserIdentity(userId, userName, "unit-test", true);
                _userPrincipal = new UserPrincipal(userId, userName, rolesDelimited, _userIdentity);
            }
        }


        /// <summary>
        /// Get the current user.
        /// </summary>
        public IPrincipal User
        {
            get { return _userPrincipal; }
        }


        /// <summary>
        /// The user model
        /// </summary>
        public User UserModel
        {
            get { return _user ?? (_user = _userFunction(this.UserName)); }
        }


        /// <summary>
        /// Time zone info based on user.
        /// </summary>
        public TimeZoneInfo UserTimeZoneInfo
        {
            get
            {
                { return _userTimeZoneInfo ?? (_userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(this.UserModel.TimeZoneId)); }
            }
        }


        /// <summary>
        /// The name of the current user.
        /// </summary>
        public override string UserName
        {
            get
            {
                if (!IsAuthenticated())
                    return string.Empty;

                return _userPrincipal.Identity.Name;
            }
        }


        /// <summary>
        /// Determine if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public override bool IsAuthenticated()
        {
            return _isAuthenticated;
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
		/// <returns></returns>
		public override bool IsUserInRole(string role)
		{
			if (!IsAuthenticated())
				return false;
			return _userPrincipal.IsInRole(role);
		}

		/// <summary>
		/// Is User in the selected roles.
		/// </summary>
		/// <returns></returns>
		public override bool IsUserInAnyRole(params string[] roles)
		{
			if (!IsAuthenticated())
				return false;

			if (roles == null || roles.Length == 0)
				return false;

			foreach (var role in roles)
			{
				if (_userPrincipal.IsInRole(role))
					return true;
			}

			return false;
		}
    }
}
