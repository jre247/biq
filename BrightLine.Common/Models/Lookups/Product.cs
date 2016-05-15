using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class Product : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[DataMember]
		public string Name { get; set; }

		public int? OldIdentity { get; set; }

		//[Required]
		[EntityEditor(IsFromCollection = true, ShowInListing = true)]
		public virtual Brand Brand { get; set; }

		[EntityEditor(IsFromCollection = true, ShowInListing = true, Display = "Sub segment")]
		public virtual SubSegment SubSegment { get; set; }
		public virtual ICollection<Campaign> Campaigns { get; set; }
	}
}
