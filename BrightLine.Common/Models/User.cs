using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class User : EntityBase, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string Email { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string FirstName { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string LastName { get; set; }

		public string FullName
		{
			get { return string.Format("{0} {1}", FirstName, LastName); }
		}
		public override string Display
		{
			get { return string.Format("{0} {1}", FirstName, LastName); }
			set { base.Display = value; }
		}
		public override string ShortDisplay
		{
			get { return string.Format("{0} {1}", FirstName, LastName); }
			set { base.Display = value; }
		}

		public string Password { get; set; }
		public string Salt { get; set; }
		public byte PasswordFormat { get; set; }
		[DataMember]
		public bool IsApproved { get; set; }
		[DataMember]
		public bool IsActive { get; set; }
		[DataMember]
		public DateTime? LastActivityDate { get; set; }
		[DataMember]
		public DateTime? LastLoginDate { get; set; }
		[DataMember]
		public DateTime? LastPasswordChangedDate { get; set; }
		[DataMember]
		public byte FailedPasswordAttemptCount { get; set; }
		[DataMember]
		public DateTime? FailedPasswordAttemptWindowStart { get; set; }
		[DataMember]
		public DateTime? LockOutWindowStart { get; set; }
		[DataMember]
		public string TimeZoneId { get; set; }
		[DataMember]
		public bool Internal { get; set; }

		[DataMember]
		public virtual Advertiser Advertiser { get; set; }

		[DataMember]
		public virtual Agency MediaAgency { get; set; }

		[DataMember]
		public virtual MediaPartner MediaPartner { get; set; }

		[DataMember]
		public virtual ICollection<Role> Roles { get; set; }
		public virtual ICollection<PasswordHashHistory> PasswordHashes { get; set; }
		[DataMember]
		public virtual ICollection<AccountInvitation> AccountInvitations { get; set; }
		public virtual ICollection<AccountRetrievalRequest> AccountRetrievalRequests { get; set; }
		public virtual ICollection<Campaign> CampaignFavorites { get; set; }
	}
}
