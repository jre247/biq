using BrightLine.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class CampaignContentModelType : EntityBase, ILookup, IEntity
    {
        [Required]
		[StringLength(50)]
		[DataMember]
        public string Name { get; set; }
    }
}
