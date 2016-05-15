using System.Linq;
using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BrightLine.Core.Attributes;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class FeatureType : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		[EntityEditor(ShowInListing = true)]
		public string Name { get; set; }

		public virtual ICollection<Feature> Features { get; set; }

		[EntityEditor(ShowInListing = true, IsFromCollection = true)]
		public virtual ICollection<FeatureCategory> FeatureCategories { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true, IsFromCollection = true)]
		public virtual FeatureTypeGroup FeatureTypeGroup { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true, IsFromCollection = true)]
		public virtual ICollection<FeatureCategory> Categories { get; set; }
		[DataMember]
		[EntityEditor(ShowInListing = true, IsFromCollection = true)]
		public virtual ICollection<Blueprint> Blueprints { get; set; }

		[DataMember]
		[NotMapped]
		public override string Display
		{
			get
			{
				return ((FeatureTypeGroup != null) ? FeatureTypeGroup.Name + ": " : "") + Name;
			}
			set { }
		}

		[DataMember]
		[NotMapped]
		public override string ShortDisplay { get { return Name; } set { } }
	}
}
