using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class AdFormat : EntityBase, ILookup, IEntity
	{
		[EntityEditor(ShowInListing = true, AllowEdit = true)]
        [Required]
		[StringLength(255)]
		[DataMember]
        public string Name { get; set; }
	}
}
