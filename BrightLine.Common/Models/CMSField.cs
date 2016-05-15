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
	public class CmsField : EntityBase, IEntity
	{
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		[NotMapped]
		public string DisplayName { 
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
		[StringLength(1000)]
		public string Description { get; set; }

		public int? Type_Id { get; set; }

		[ForeignKey("Type_Id")]
		public virtual FieldType Type { get; set; }

		[DataMember]
		public virtual CmsModelDefinition CmsModelDefinition { get; set; }

		[DataMember]
		public virtual CmsSettingDefinition CmsSettingDefinition { get; set; }

		[DataMember]
		public bool List { get; set; }

		[DataMember]
		public virtual ICollection<Validation> Validations { get; set; }

		public int? CmsRef_Id { get; set; }

		[ForeignKey("CmsRef_Id")]
		public virtual CmsRef CmsRef { get; set; }	

		public int? Expose_Id { get; set; }

		[ForeignKey("Expose_Id")]
		public virtual Expose Expose { get; set; }

		[DataMember]
		[StringLength(255)]
		public string DefaultValueString { get; set; }

		[DataMember]
		public int? DefaultValueNumber { get; set; }

		[DataMember]
		public float? DefaultValueFloat { get; set; }

		[DataMember]
		public bool? DefaultValueBool { get; set; }

		[DataMember]
		public DateTime? DefaultValueDateTime { get; set; }
	}
}
