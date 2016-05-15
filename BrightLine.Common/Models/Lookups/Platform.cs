using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Platform : EntityBase, ILookup, IEntity
	{
		[Required]
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[DataMember]
		public virtual PlatformGroup PlatformGroup { get; set; }

		[DataMember]
		public virtual ICollection<MediaPartner> MediaPartners { get; set; }

		public int? Reach { get; set; }

		[NotMapped]
		public override string Display { get { return Name; } set { } }
		[NotMapped]
		public override string ShortDisplay { get { return Name; } set { } }

		/// <summary>
		/// Name used when generating manifest
		/// </summary>
		[DataMember]
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		public string ManifestName { get; set; }
	}
}
