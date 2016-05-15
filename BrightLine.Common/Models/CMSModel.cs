using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using BrightLine.Common.Models.Lookups;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class CmsModel : EntityBase, IEntity
	{
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		[NotMapped]
		public string DisplayName
		{
			get
			{
				return base.Display;
			}
			set
			{
				base.Display = value;
			}
		}

		[DataMember]
		public virtual CmsModelDefinition CmsModelDefinition { get; set; }

		[DataMember]
		public virtual Feature Feature { get; set; }

		[DataMember]
		public virtual ICollection<CmsModelInstance> ModelInstances { get; set; }

	}
}
