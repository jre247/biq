using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class CmsSettingDefinition : EntityBase, IEntity
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
		public virtual ICollection<CmsField> Fields { get; set;}

		public int? Blueprint_Id { get; set; }

		[ForeignKey("Blueprint_Id")]
		public virtual Blueprint Blueprint { get; set; }

	}
}
