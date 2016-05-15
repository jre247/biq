using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class StorageSource : EntityBase, ILookup, IEntity
	{
		[DataMember]
		[Required]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public virtual string Name { get; set; }

	}

	
}
