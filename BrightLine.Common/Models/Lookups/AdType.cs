using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class AdType : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string Name { get; set; }

		[StringLength(255)]
		public string ManifestName { get; set; }

		public bool IsPromo { get; set; }
		public bool IsDVRProof { get; set; }
		public ICollection<Ad> Ads { get; set; }

		//public virtual AdTypeGroup AdTypeGroup { get; set; }
        public virtual ICollection<AdTypeGroup> AdTypeGroups { get; set; } 
	}
}
