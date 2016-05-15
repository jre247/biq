using System;
using System.ComponentModel.DataAnnotations;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
	public class AuditEvent : EntityBase, IEntity
	{
		/// <summary>
		/// Audit group ( e.g. can be a certain module / area / feature )
		/// </summary>
		[StringLength(50)]		
		public string Group { get; set; }


		/// <summary>
		/// Action being audited ( e.g. create/delete/get etc )
		/// </summary>
		[StringLength(50)]
		public string ActionName { get; set; }


		/// <summary>
		/// Date of the action
		/// </summary>
		public DateTime ActionDate { get; set ;}


		/// <summary>
		/// User who performed the action
		/// </summary>
		[StringLength(50)]
		public string User { get; set; }


		/// <summary>
		/// IP address of the user.
		/// </summary>
		[StringLength(50)]
		public string IPAddress { get; set; }


		/// <summary>
		/// Source of the action.
		/// </summary>
		[StringLength(50)]
		public string Source { get; set; }


		/// <summary>
		/// Source of the action.
		/// </summary>
		[StringLength(255)]
		public string RequestUrl { get; set; }
	}
}