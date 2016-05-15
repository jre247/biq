using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System;
using BrightLine.Common.Models.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightLine.Common.Models
{
	public class Validation : EntityBase, IEntity
	{
		public int? ValidationType_Id { get; set; }

		[ForeignKey("ValidationType_Id")]
		public virtual ValidationType ValidationType { get; set; }

		public virtual CmsField CmsField { get; set; }

		public virtual FileType FileType { get; set; }

		public string Value { get; set; }

		public virtual ICollection<FileTypeValidation> FileTypeValidations { get; set; }
	}
}
