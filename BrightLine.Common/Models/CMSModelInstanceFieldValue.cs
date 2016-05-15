using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
    public class CmsModelInstanceFieldValue : EntityBase, IEntity
    {
        public double? NumberValue { get; set; }

		[StringLength(255)]
        public string StringValue { get; set; }

        public bool? BoolValue { get; set; }

        public DateTime? DateValue { get; set; }

		public virtual CmsModelInstanceField CmsModelInstanceField { get; set; }

		
    }
}
