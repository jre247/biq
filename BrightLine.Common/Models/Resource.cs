using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using BrightLine.Common.Models.Lookups;

namespace BrightLine.Common.Models
{
	[DataContract(IsReference = false)]
	[Entity(HasFile = true)]
	public class Resource : EntityBase, IEntity
	{
		/// <summary>
		/// Friendly name
		/// </summary>
		[DataMember, Required(ErrorMessage = "Name must be at least 5 letters long."), MinLength(5)]
		[EntityEditor(ShowInListing = true)]
		[StringLength(255)]
		public virtual string Name { get; set; }

		/// <summary>
		/// File name.
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual string Filename { get; set; }


		/// <summary>
		/// The url to access the file, if determined statically.
		/// </summary>
		[DataMember]
		public virtual string Url { get; set; }


		/// <summary>
		/// The file size in bytes, if known
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual int? Size { get; set; }


		/// <summary>
		/// The height of the file, if applicable
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual int? Height { get; set; }


		/// <summary>
		/// The width of the file, if applicable. 
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual int? Width { get; set; }


		/// <summary>
		/// The user that uploaded the file.
		/// </summary>
		[DataMember]
		[EntityEditor(IsHidden = true, IsUserId = true)]
		public virtual User User { get; set; }

		/// <summary>
		/// The MD5 Hash for the file.
		/// </summary>
		[DataMember]
		[EntityEditor(IsHidden = true)]
		public virtual string MD5Hash { get; set; }

		/// <summary>
		/// The duration in milliseconds, if known
		/// </summary>
		[EntityEditor(ShowInListing = false)]
		public virtual int? Duration { get; set; }

		/// <summary>
		/// The resource parent, if known
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		[ForeignKey("Parent_Id")]
		public virtual Resource Parent { get; set; }

		public int? Parent_Id { get; set; }

		/// <summary>
		/// The file type (i.e. gif, jpeg)
		/// </summary>
		//[DataMember]
		//[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		//public virtual FileType? FileType { get; set; }

		/// <summary>
		/// The resource type (i.e. creative, campaign thumbnail)
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual ResourceType ResourceType { get; set; }

		/// <summary>
		/// The storage source (i.e. AWS)
		/// </summary>
		[DataMember]
		[EntityEditor(ShowInListing = true, DisplayOnly = true)]
		public virtual StorageSource StorageSource { get; set; }

		public int? Extension_Id { get; set; }

		public bool IsUploaded { get; set; }

		/// <summary>
		/// The File Type (i.e. png)
		/// </summary>
		[DataMember]
		[ForeignKey("Extension_Id")]
		public virtual FileType Extension { get; set; }

		/// <summary>
		/// The Resource's Creative
		public virtual Creative Creative { get; set; }

		public int? Bitrate { get; set; }

	}
}
