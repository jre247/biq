using AutoMapper;
using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace BrightLine.Common.ViewModels.Users
{
	[DataContract]
	public class UserViewModel
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public bool IsLocked { get; set; }

		[DataMember]
		public bool Internal { get; set; }

		[DataMember]
		public bool IsActive { get; set; }

		[DataMember]
		public bool IsDeleted { get; set; }

		[DataMember]
		public string TimeZoneId { get; set; }

		[DataMember]
		public IEnumerable<ILookup> Roles { get; set; }

		[DataMember]
		public int? Advertiser { get; set; }

		[DataMember]
		[Required]
		public int? MediaAgency { get; set; }

		[DataMember]
		[Required]
		public int? MediaPartner { get; set; }

		[DataMember]
		public DateTime? LastLoginDate { get; set; }

		[DataMember]
		public DateTime? LastActivityDate { get; set; }

		[DataMember]
		public List<AccountInvitationViewModel> AccountInvitations { get; set; }

		public static List<UserViewModel> FromUsers(User[] users)
		{
			var userListViewModel = new List<UserViewModel>();
			foreach (var user in users)
			{
				var userViewModel = new UserViewModel();

				userViewModel.FirstName = user.FirstName;
				userViewModel.LastName = user.LastName;
				userViewModel.Email = user.Email;
				userViewModel.Internal = user.Internal;
				userViewModel.IsActive = user.IsActive;
	
				userViewModel.Id = user.Id;

				if (user.Advertiser != null)
					userViewModel.Advertiser = user.Advertiser.Id;

				if (user.MediaAgency != null)
					userViewModel.MediaAgency = user.MediaAgency.Id;

				if (user.MediaPartner != null)
					userViewModel.MediaPartner = user.MediaPartner.Id;

				userViewModel.Roles =  user.Roles.ToLookups("Name").ToArray();

				userViewModel.LastActivityDate = DateHelper.ToUserTimezone(user.LastActivityDate, user.TimeZoneId);
				userViewModel.LastLoginDate = DateHelper.ToUserTimezone(user.LastLoginDate, user.TimeZoneId);

				var tzi = user.TimeZoneId;
				var accountInvitationListViewModel = user.AccountInvitations.Select(a => new AccountInvitationViewModel
				{
					DateActivated = DateHelper.ToUserTimezone(a.DateActivated, tzi),
					DateExpired = DateHelper.ToUserTimezone(a.DateExpired, tzi)
				}).ToList();
				
				userListViewModel.Add(userViewModel);
			}

			return userListViewModel;
		}

		[DataContract]
		public class AccountInvitationViewModel
		{
			[DataMember]
			public DateTime? DateActivated { get; set; }

			[DataMember]
			public DateTime? DateExpired { get; set; }
		}
	}
}