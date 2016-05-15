using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class AppPlatform : EntityBase, IEntity
	{
		[DataMember]
		[Required]
		[EntityEditor(IsFromCollection = true)]
		public virtual App App { get; set; }

		[DataMember]
		[Required]
		[EntityEditor(IsFromCollection = true)]
		public virtual Platform Platform { get; set; }

		[DataMember]
		[StringLength(12)]
		[Index("IX_AppPlatform_Identifier", 1, IsUnique = true)]
		public string Identifier { get; set; }

		[DataMember]
		public virtual Category Category { get; set; }

		[DataMember]
		[NotMapped]
		public override string Display { get { return string.Format("{0} - {1}", Platform.Display, App.Display); } set { } }

		[DataMember]
		[NotMapped]
		public override string ShortDisplay { get { return string.Format("{0} - {1}", Platform.ShortDisplay, App.ShortDisplay); } set { } }
	}
}
