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
	public class CmsRef : EntityBase, IEntity
	{
		public int? CmsRefType_Id { get; set; }

		[ForeignKey("CmsRefType_Id")]
		public virtual CmsRefType CmsRefType { get; set; }

		[DataMember]
		public virtual CmsModelDefinition CmsModelDefinition { get; set; }

		public int? PageDefinition_Id { get; set; }

		[DataMember]
		[ForeignKey("PageDefinition_Id")]
		public virtual PageDefinition PageDefinition { get; set; }

	}
}
