using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class AdTrackingEvent : EntityBase, IEntity
	{
		[ForeignKey("TrackingEvent_Id")]
		public virtual TrackingEvent TrackingEvent { get; set; }
		public int? TrackingEvent_Id { get;set;}

		[ForeignKey("Ad_Id")]
		public virtual Ad Ad { get; set; }
		public int? Ad_Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(1028)]
		[Required]
		[DataMember]
		public string TrackingUrl { get;set;}
	}
}
