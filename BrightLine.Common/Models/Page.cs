using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System;

namespace BrightLine.Common.Models
{
    [DataContract]
    public class Page : EntityBase, IEntity
    {
        [Required]
        [StringLength(255)]
        [DataMember]
        public string Name { get; set; }

		[DataMember]
		public virtual string RelativeUrl { get; set; }

        [DataMember]
        public virtual PageDefinition PageDefinition { get; set; }

        public virtual Feature Feature { get; set; }
    }


   
}
