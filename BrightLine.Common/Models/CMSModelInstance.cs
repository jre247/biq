using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using BrightLine.Common.Models.Lookups;
using System.Collections.ObjectModel;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class CmsModelInstance : EntityBase, IEntity
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
		public string PublishedJson { get; set; }

		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		public virtual CmsModel Model { get; set; }


		private ICollection<CmsModelInstanceField> _Fields { get; set; }
		public virtual ICollection<CmsModelInstanceField> Fields
		{
			get { return _Fields ?? (_Fields = new Collection<CmsModelInstanceField>()); }
			protected set { _Fields = value; }
		}

	}
}
