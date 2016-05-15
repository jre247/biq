using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightLine.Common.Models
{
    [DataContract]
    public class PageDefinition : EntityBase, ILookup, IEntity
    {
        [DataMember]
        public int Key { get; set; }

        [DataMember]
		[StringLength(255)]
        public string Name { get; set; }

		[ForeignKey("Blueprint_Id")]
        public virtual Blueprint Blueprint { get; set; }

		public int? Blueprint_Id { get; set; }
    }
}
