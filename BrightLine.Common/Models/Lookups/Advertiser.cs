using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Advertiser : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[DataMember]
		public string Name { get; set; }

		public virtual ICollection<Brand> Brands { get; set; }
		public virtual ICollection<User> Users { get; set; }
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

