using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class CmsSetting : EntityBase, IEntity
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
		public virtual Feature Feature { get; set; }

		[DataMember]
		public virtual CmsSettingInstance CmsSettingInstance { get; set; }

		[DataMember]
		public virtual CmsSettingDefinition CmsSettingDefinition { get; set; }

	}
}
