using AutoMapper;
using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BrightLine.Common.ViewModels.Users
{
	[DataContract]
	public class SaveUserViewModel
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
		public IEnumerable<EntityLookup> Roles { get; set; }

		[DataMember]
		public int? Advertiser { get; set; }

		[DataMember]
		public int? MediaAgency { get; set; }

		[DataMember]
		public int? MediaPartner { get; set; }

		[DataMember]
		public DateTime? LastLoginDate { get; set; }

		[DataMember]
		public DateTime? LastActivityDate { get; set; }

		
	}
}