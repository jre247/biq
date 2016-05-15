using System;
using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Utility;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.ViewModels.Users;

namespace BrightLine.Service
{
	public class UserService : CrudService<User>, IUserService
	{
		public UserService(IRepository<User> repo) : base(repo) { }

		public bool ToggleFavorite(User user, Campaign campaign)
		{
			if (user == null || user.IsDeleted || campaign == null || campaign.IsDeleted)
				return false;

			if (user.CampaignFavorites.Contains(campaign))
				user.CampaignFavorites.Remove(campaign);
			else
				user.CampaignFavorites.Add(campaign);

			this.Update(user);
			return true;
		}

		/// <summary>
		/// Gets a lookup of id to first/lastname.
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetUserFullNames()
		{
			var users = GetAll();
			var lookup = new Dictionary<int, string>();
			if (users == null)
				return lookup;

			foreach (var user in users)
			{
				lookup[user.Id] = user.FirstName + " " + user.LastName;
			}

			return lookup;
		}

		public User GetUserByEmail(string email)
		{
			return Where(o => string.Equals(o.Email, email)).SingleOrDefault();
		}

		public User SaveUser(int id, string jsonData)
		{
			var rolesService = IoC.Resolve<IRoleService>();
			var agencies = IoC.Resolve<IAgencyService>();
			var mediaPartners = IoC.Resolve<IMediaPartnerService>();
			var advertisers = IoC.Resolve<IRepository<Advertiser>>();
			var accounts = IoC.Resolve<IAccountService>();

			var user = Get(id) ?? new User();
			var userData = JsonConvert.DeserializeObject<SaveUserViewModel>(jsonData);
			user.FirstName = userData.FirstName;
			user.LastName = userData.LastName;
			user.IsActive = userData.IsActive;
			user.TimeZoneId = userData.TimeZoneId;
			user.Internal = userData.Internal;
			var rolesRaw = (userData.Roles).Select(r => (int)r.Id);
			var newRoles = rolesService.GetAll().Where(r => rolesRaw.Contains(r.Id)).ToList();
			var roles = user.Roles.Merge(newRoles, fullRemove: true, setAsDeleted: false);

			ValidateInternalAndExternalUserRoles(roles);

			user.Roles = roles;

			// Save Advertiser
			var advertiserId = userData.Advertiser;
			user.Advertiser = advertiserId > 0 ? advertisers.Get(advertiserId.Value) : null;
			if (userData.Advertiser > 0 && user.Advertiser == null)
				throw new ValidationException(AuthConstants.Errors.ADVERTISER_MUST_BE_SELECTED);

			// Save Media Agency
			var mediaAgencyId = userData.MediaAgency;
			user.MediaAgency = mediaAgencyId > 0 ? agencies.Get(mediaAgencyId.Value) : null;
			if (userData.MediaAgency > 0 && user.MediaAgency == null)
				throw new ValidationException(AuthConstants.Errors.MEDIA_AGENCY_MUST_BE_SELECTED);

			// Save Media Partner
			var mediaPartnerId = userData.MediaPartner;
			user.MediaPartner = mediaPartnerId > 0 ? mediaPartners.Get(mediaPartnerId.Value) : null;
			if (userData.MediaPartner > 0 && user.MediaPartner == null)
				throw new ValidationException(AuthConstants.Errors.MEDIA_PARTNER_MUST_BE_SELECTED);

			if (user.IsNewEntity)
			{
				user.Email = userData.Email;
				this.Create(user);
				accounts.CreateInvitation(user, Auth.UserModel);
			}
			else
			{
				rolesService.ClearUserRoles(user.Email);
				this.Save();
			}

			return user;
		}

		/// <summary>
		/// Validate the following logic for the user's roles:
		///		1) User cannot be in both an internal and external role
		///		2) If User has an internal role they can not simultanously be in an external role, and vice versa
		/// </summary>
		/// <param name="roles"></param>
		private static void ValidateInternalAndExternalUserRoles(ICollection<Role> roles)
		{
			var roleIdsHash = Lookups.Roles.HashByName;
			var clientRoleId = roleIdsHash[AuthConstants.Roles.Client];
			var agencyPartnerRoleId = roleIdsHash[AuthConstants.Roles.AgencyPartner];
			var mediaPartnerRoleId = roleIdsHash[AuthConstants.Roles.MediaPartner];
			var employeeRoleId = roleIdsHash[AuthConstants.Roles.Employee];
			var externalRoles = new HashSet<int>();

			externalRoles.Add(clientRoleId);
			externalRoles.Add(agencyPartnerRoleId);
			externalRoles.Add(mediaPartnerRoleId);

			var isExternalRolePresent = roles.Any(r => externalRoles.Contains(r.Id));
			var internalRolePresent = roles.Any(r => !externalRoles.Contains(r.Id));
			var isEmployeeRolePresent = roles.Any(r => r.Id == employeeRoleId);

			if (isExternalRolePresent && internalRolePresent)
				throw new ValidationException(AuthConstants.Errors.INVALID_ROLE_SELECTED);

			//if an Internal role selected then Employee role must also be selected
			if(internalRolePresent)
				if(!isEmployeeRolePresent)
					throw new ValidationException(AuthConstants.Errors.EMPLOYEE_ROLE_NOT_SELECTED);
			//if an External role selected then Employee role must not also be selected
			else if (isExternalRolePresent)
				if (isEmployeeRolePresent)
					throw new ValidationException(AuthConstants.Errors.EMPLOYEE_ROLE_MUST_NOT_BE_SELECTED);
		}

		/// <summary>
		/// Returns a JSON object of the current user (if authenticated)
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public JObject GetUserInfo(User user)
		{
			var jObject = new JObject();
			if (user == null)
			{
				// set authenticated to false so that the object/property is never null
				jObject["authenticated"] = false;
				return jObject;
			}

			jObject["authenticated"] = true;
			jObject["id"] = user.Id;
			jObject["fullName"] = user.FullName;
			jObject["email"] = user.Email;
			try
			{
				var tziId = user.TimeZoneId;
				var tzi = TimeZoneInfo.FindSystemTimeZoneById(tziId);
				jObject["timeOffSet"] = tzi.BaseUtcOffset.ToString();

			}
			catch (Exception ex)
			{
				// swallow
				// TODO: Log?
			}

			var roles = Auth.Roles;
			foreach (var role in roles)
			{
				jObject.Add("is" + role, true);
			}

			return jObject;
		}

		public void AuditLogin(User user)
		{
			if (user == null)
				return;

			user.LastLoginDate = DateTime.UtcNow;
			Update(user);
		}

		public void AuditActivity(User user)
		{
			if (user == null)
				return;

			user.LastActivityDate = DateTime.UtcNow;
			Update(user);
		}

		public bool IsPasswordExpired(User user)
		{
			if (user == null)
				return false;

			var settings = IoC.Resolve<ISettingsService>();

			// if not an agency partner or client, don't ever prompt to change password.
			if (!Auth.IsUserInAnyRole(AuthConstants.Roles.AgencyPartner, AuthConstants.Roles.Client))
				return false;

			var last = user.LastPasswordChangedDate;
			if (!last.HasValue)
			{
				user.LastPasswordChangedDate = last = DateTime.UtcNow;
				Update(user);
			}

			var daysSince = DateTime.UtcNow.Subtract(last.Value).Days;
			var expired = (daysSince >= settings.PasswordExpirationDayCount);
			return expired;
		}
	}
}
