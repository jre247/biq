using System.Runtime.Serialization;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightLine.Common.Models
{
	public class Segment : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[DataMember]
		public string Name { get; set; }

		[EntityEditor(IsFromCollection = true, ShowInListing = true)]
		public virtual Vertical Vertical { get; set; }
		public virtual ICollection<SubSegment> SubSegments { get; set; }

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
