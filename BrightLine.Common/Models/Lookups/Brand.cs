using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class Brand : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[DataMember]
		public string Name { get; set; }

		[EntityEditor(IsFromCollection = true)]
		public virtual Advertiser Advertiser { get; set; }

		public virtual ICollection<Product> Products { get; set; }
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
