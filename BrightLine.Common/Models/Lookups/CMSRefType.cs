﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightLine.Common.Models
{
    [DataContract]
    public class CmsRefType : EntityBase, ILookup, IEntity
    {
        [DataMember]
		[StringLength(255)]
        public string Name { get; set; }
    }
}
