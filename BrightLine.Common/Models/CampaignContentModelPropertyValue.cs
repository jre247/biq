using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
    public class CampaignContentModelPropertyValue : EntityBase, IEntity
    {
        public Campaign Campaign { get; set; }
		
        public CampaignContentModel Model { get; set; }

        public CampaignContentModelInstance Instance { get; set; }
        
        public CampaignContentModelProperty Property { get; set; }

        public CampaignContentModelPropertyType PropertyType { get; set; }

        public double? NumberValue { get; set; }

        public string StringValue { get; set; }

        public bool? BoolValue { get; set; }

        public DateTime? DateValue { get; set; }
    }
}
