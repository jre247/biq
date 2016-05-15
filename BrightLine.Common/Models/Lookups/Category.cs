using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Category : EntityBase, ILookup, IEntity
	{
		[DataMember]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public virtual string Name { get; set; }

		[DataMember]
		[NotMapped]
		public override string Display { get { return Name; } set { } }

		[DataMember]
		[NotMapped]
		public override string ShortDisplay { get { return Name; } set { } }
	}
}