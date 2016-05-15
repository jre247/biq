using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
    public class CampaignContentModel : EntityBase, IEntity
    {
        public string ModelName { get; set; }
        public string InstancesData { get; set; }
        public string InstancesDataJson { get; set; }
        public virtual CampaignContentModelBaseType BaseType { get; set; }

        [Required]
        public virtual CampaignContentSchema Schema { get; set; }


        public virtual ICollection<CampaignContentModelInstance> Instances { get; set; }


        public string SqlTemplateQuery { get; set; }

        public string SqlTemplateFields { get; set; }
    }
}
