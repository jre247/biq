using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using System.ComponentModel.DataAnnotations.Schema;
using BrightLine.Common.Framework;
using System.Linq.Expressions;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class DeliveryGroup : EntityBase, IEntity
	{
		
		[DataMember]
		public virtual Campaign Campaign { get; set; }

		[DataMember]
		[Required]
		[MaxLength(255)]
		public virtual string Name { get; set; }

		[DataMember]
		public int? ImpressionGoal { get; set; }

		[DataMember]
		public double? MediaSpend { get; set; }

		[DataMember]
		[Required]
		public virtual MediaPartner MediaPartner { get; set; }

		[DataMember]
		public virtual ICollection<Ad> Ads { get; set; }
	}
}
