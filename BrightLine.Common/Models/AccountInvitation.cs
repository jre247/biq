using BrightLine.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class AccountInvitation : EntityBase, IEntity
	{
		/// <summary>
		/// Hash of invitation token. Prevents recreation of invitation URL
		/// </summary>
		[Required]
		public string TokenHash { get; set; }

		/// <summary>
		/// Salt used in TokenHash
		/// </summary>
		[Required]
		public string Salt { get; set; }

		/// <summary>
		/// Public invitation token
		/// </summary>
		[Required]
		public Guid SecondaryToken { get; set; }

		[Required]
		[DataMember]
		public DateTime DateIssued { get; set; }

		[DataMember]
		public DateTime DateExpired { get; set; }

		[DataMember]
		public DateTime? DateActivated { get; set; }

		/// <summary>
		/// Id of User that was invited	to iQ
		/// </summary>
		public virtual User InvitedUser { get; set; }
		
		/// <summary>
		/// Id of User that that invited new User to iQ
		/// </summary>
		public virtual User CreatingUser { get; set; }
	}
}
