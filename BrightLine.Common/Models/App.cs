using BrightLine.Core;
using BrightLine.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class App : EntityBase, IEntity, ILookup
	{
		public Guid AppId { get; set; }

		[DataMember]
		[EntityEditor]
		[StringLength(255)]
		public string Name { get; set; }

		[DataMember]
		[EntityEditor(IsFromCollection = true)]
		public virtual MediaPartner MediaPartner { get; set; }

		public int? Advertiser_Id { get; set; }

		[DataMember]
		[EntityEditor(IsFromCollection = true)]
		[ForeignKey("Advertiser_Id")]
		public virtual Advertiser Advertiser { get; set; }

		[DataMember]
		[EntityEditor(IsFromCollection = true)]
		public virtual Category Category { get; set; }

		[DataMember]
		[NotMapped]
		public override string Display { get { return Name; } set { } }

		[DataMember]
		[NotMapped]
		public override string ShortDisplay { get { return Name; } set { } }
	}
}