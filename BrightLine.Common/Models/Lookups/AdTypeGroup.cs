using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class AdTypeGroup : EntityBase, ILookup, IEntity
	{
		[DataMember]
		[Required]
		public virtual ICollection<AdType> AdTypes { get; set; }

		[DataMember]
		[Required]
		[StringLength(255)]
		public virtual string Name { get; set; }
	}
}
