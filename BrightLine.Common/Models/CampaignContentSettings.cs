using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
    public class CampaignContentSettings : EntityBase, IEntity
    {
        [Required]
        public virtual CampaignContentSchema Schema { get; set; }

        [StringLength(100)]
        public String Source { get; set; }


        public String Content { get; set; }
    }
}
