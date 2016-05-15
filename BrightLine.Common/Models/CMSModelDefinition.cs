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
	public class CmsModelDefinition : EntityBase, IEntity
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
		[StringLength(255)]
		public string DisplayFieldName { get; set; }

		[ForeignKey("DisplayFieldType_Id")]
		public virtual FieldType DisplayFieldType { get; set; }

		public int? DisplayFieldType_Id { get;set;}

		[DataMember]
		public virtual ICollection<CmsField> Fields { get; set; }

		public int? Blueprint_Id { get; set; }

		[ForeignKey("Blueprint_Id")]
		public virtual Blueprint Blueprint { get; set; }

	}
}
