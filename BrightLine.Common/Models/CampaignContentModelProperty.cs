using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
    public class CampaignContentModelProperty : EntityBase, IEntity
    {
        public CampaignContentModel Model { get; set; }

        [Required]
        public string Name { get; set; }

        public CampaignContentModelPropertyType PropertyType { get; set; }

        public bool IsRequired { get; set; }

        public string Metatype { get; set; }
    }
}
