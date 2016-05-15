using System.ComponentModel.DataAnnotations;
using BrightLine.Core;
using System;

namespace BrightLine.Common.Models
{
	public class AccountRetrievalRequest : EntityBase, IEntity
	{
		/// <summary>
		/// Hash of retrieval request token. Prevents recreation of request URL
		/// </summary>
		[Required]
		public string TokenHash { get; set; }

		/// <summary>
		/// Public invitation token
		/// </summary>
		[Required]
		public Guid SecondaryToken { get; set; }
		
		/// <summary>
		/// Salt used in TokenHash
		/// </summary>
		[Required]
		public string Salt { get; set; }

		[Required]
		public DateTime DateIssued { get; set; }

		[Required]
		public DateTime DateExpired { get; set; }

		public DateTime? DateRetrieved { get; set; }

		/// <summary>
		/// User attempting to retrieve account
		/// </summary>
		[Required]
		public virtual User User { get; set; }
	}
}
