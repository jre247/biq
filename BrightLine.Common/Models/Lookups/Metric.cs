using BrightLine.Common.Models.Enums;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Metric : EntityBase, ILookup, IEntity
	{
		[DataMember]
		[Required]
		[EntityEditor(ShowInListing = true)]
		public virtual string Name { get; set; }

		[DataMember]
		[Required]
		[EntityEditor(ShowInListing = true)]
		public virtual MetricFormattingTypes Type { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public override string Display { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public override string ShortDisplay { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public virtual string HexColor { get; set; }
	}
}
