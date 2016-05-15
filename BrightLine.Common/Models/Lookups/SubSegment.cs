using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class SubSegment : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[DataMember]
		public string Name { get; set; }

		[Required]
		[EntityEditor(IsFromCollection = true, ShowInListing = true)]
		public virtual Segment Segment { get; set; }
		public virtual ICollection<Product> Brands { get; set; }

		public override string Display
		{
			get { return Name; }
			set { }
		}
		public override string ShortDisplay
		{
			get { return Name; }
			set { }
		}
	}
}
