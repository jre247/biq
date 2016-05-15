using System.Runtime.Serialization;
using BrightLine.Core;
using BrightLine.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BrightLine.Core.Attributes;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Campaign : EntityBase, IEntity
	{
		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[Required]
		[DataMember]
		public string Name { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		[DataMember]
		public string SalesForceId { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(1000)]
		public string Description { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(255)]
		public string GoogleAnalyticsIds { get; set; }

		/// <summary>
		/// Name that is used in CMS API URIs
		/// </summary>
		public string CmsKey { get; set; }

		[DataMember]
		public bool? Internal { get; set; }
		
		[Required]
		[DataMember]
		public virtual Product Product { get; set; }
		
		public virtual ICollection<User> UsersFavorite { get; set; }

		public virtual ICollection<CampaignContentSchema> CampaignContentSchemas { get; set; }

		[DataMember]
		public virtual CampaignTypes CampaignType { get; set; }

		[DataMember]
		public virtual ICollection<Feature> Features { get; set; }

		[DataMember]
		public virtual ICollection<Creative> Creatives { get; set; }

		[DataMember]
		public virtual Resource Thumbnail { get; set; }

		[DataMember]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? BeginDate { get; private set; }
		[DataMember]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? EndDate { get; private set; }

		public virtual ICollection<Ad> Ads { get; set; }

		[DataMember]
		[Required]
		[ForeignKey("MediaAgency_Id")]
		public virtual Agency MediaAgency { get; set; }
		public int? MediaAgency_Id { get; set; }

		[DataMember]
		[Required]
		[ForeignKey("CreativeAgency_Id")]
		public virtual Agency CreativeAgency { get; set; }
		public int? CreativeAgency_Id { get; set; }

		[DataMember]
		public int? Generation { get; set; }
	}
}