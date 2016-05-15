using BrightLine.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class CampaignContentModelPropertyType : EntityBase, ILookup, IEntity
    {
		[Required]
		[DataMember]
        public string Name { get; set; }
    }
}
