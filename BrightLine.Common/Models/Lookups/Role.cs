using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Role : EntityBase, ILookup, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string Name { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string DisplayName { get; set; }
		public virtual List<User> Users { get; set; }
	}
}
