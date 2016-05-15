using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using BrightLine.Core.Attributes;
using System.Collections.ObjectModel;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class CmsSettingInstance : EntityBase, IEntity
	{
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
		public string Json { get; set; }

		[DataMember]
		public string PublishedJson { get;set;}

		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		public int Setting_Id { get; set; }
		
	}
}
