using BrightLine.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace BrightLine.Common.Models
{
	public class PasswordHashHistory : EntityBase, IEntity
	{
		[Required]
		[MaxLength(1024)]
		public string PasswordHash { get; set; }
		
		[Required]
		public DateTime DateChanged { get; set; }

		public virtual User User { get; set; }
	}
}
