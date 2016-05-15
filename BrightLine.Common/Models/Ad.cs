using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Ad : EntityBase, IEntity
	{
		[DataMember]
		[StringLength(255)]
		public virtual string Name { get; set; }

		[Required]
		[DataMember]
		[ForeignKey("AdType_Id")]
		public virtual AdType AdType { get; set; }
		public int? AdType_Id { get; set; }

		[DataMember]
		[ForeignKey("Creative_Id")]
		public virtual Creative Creative { get; set; }
		public int? Creative_Id { get; set; }

		[DataMember]
		public virtual ICollection<Feature> Features { get; set; }

		[DataMember]
		[ForeignKey("AdTypeGroup_Id")]
		public virtual AdTypeGroup AdTypeGroup { get; set; }
		public int? AdTypeGroup_Id { get;set;}

		[DataMember]
		[ForeignKey("AdFunction_Id")]
		public virtual AdFunction AdFunction { get; set; }
		public int? AdFunction_Id { get; set; }

		[DataMember]
		[ForeignKey("AdFormat_Id")]
		public virtual AdFormat AdFormat { get; set; }
		public int? AdFormat_Id { get; set; }

		[DataMember]
		[ForeignKey("DestinationAd_Id")]
		public virtual Ad DestinationAd { get; set; }
		public int? DestinationAd_Id { get; set; }

		[DataMember]
		[ForeignKey("AdTag_Id")]
		public virtual AdTag AdTag { get; set; }
		public int? AdTag_Id { get; set; }

		[ForeignKey("Campaign_Id")]
		public virtual Campaign Campaign { get; set; }
		public int? Campaign_Id { get; set; }
		
		[DataMember]
		public virtual DateTime BeginDate { get; set; }
		
		[DataMember]
		public virtual DateTime? EndDate { get; set; }
		
		[DataMember]
		[ForeignKey("Placement_Id")]
		public virtual Placement Placement { get; set; }
		public int? Placement_Id { get; set; }

		[DataMember]
		[ForeignKey("Platform_Id")]
		public virtual Platform Platform { get; set; }
		public int? Platform_Id { get; set; }

		[DataMember]
		[ForeignKey("DeliveryGroup_Id")]
		public virtual DeliveryGroup DeliveryGroup { get; set; }
		public int? DeliveryGroup_Id { get; set; }

		[DataMember]
		public virtual bool IsReported { get; set; }

		[DataMember]
		[ForeignKey("CompanionAd_Id")]
		public virtual Ad CompanionAd { get; set; }
		public int? CompanionAd_Id { get; set; }

		[NotMapped]
		public int? DestinationCreative { get; set; }

		[DataMember]
		public int? XCoordinateSd { get;set;}

		[DataMember]
		public int? XCoordinateHd { get; set; }

		[DataMember]
		public int? YCoordinateSd { get; set; }

		[DataMember]
		public int? YCoordinateHd { get; set; }

		[DataMember]
		public virtual ICollection<AdTrackingEvent> AdTrackingEvents { get; set; }

		[DataMember]
		public string RepoName { get; set; }
	}
}
