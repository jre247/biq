using BrightLine.Common.Framework;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class MediaPartner : EntityBase, IEntity, ILookup
	{
		[DataMember]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		[EntityEditor(Display = "Manifest Name")]
		[StringLength(255)]
		public string ManifestName { get; set; }

		public int? Parent_Id { get; set; }

		[DataMember]
		[ForeignKey("Parent_Id")]
		[EntityEditor(IsFromCollection = true)]
		public virtual MediaPartner Parent { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true, IsHidden = true)]
		public virtual Category Category { get; set; }

		public virtual ICollection<Platform> Platforms { get; set; }

		[DataMember]
		[EntityEditor(ShowInListing = true, Display = "Network")]
		public bool IsNetwork { get; set; }

		public virtual ICollection<Contract> Contracts { get; set; }

		public override string Display
		{
			get { return Name; }
			set { }
		}

	}
}