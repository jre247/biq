using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Blueprint : EntityBase, IEntity
	{
		public Blueprint()
		{
			MajorVersion = 1;
			MinorVersion = 1;
			Patch = 1;
			GroupId = Guid.NewGuid();
		}

		[DataMember]
		[Required]
		[StringLength(255)]
		public virtual string Name { get; set; }

		[DataMember]
		[StringLength(255)]
		public virtual string ManifestName { get; set; }

		[DataMember]
		public virtual FeatureType FeatureType { get; set; }

		[DataMember]
		public virtual ICollection<FeatureCategory> FeatureCategories { get; set; }

		[DataMember]
		public virtual Resource Preview { get; set; }

		[DataMember]
		public virtual Resource ConnectedTVCreative { get; set; }

		[DataMember]
		public virtual Resource ConnectedTVSupport { get; set; }

		[DataMember]
		[Required]
		public virtual Guid GroupId { get; set; }

		[DataMember]
		[Required]
		public virtual int MajorVersion { get; set; }

		[DataMember]
		[Required]
		public virtual int MinorVersion { get; set; }

		[DataMember]
		[Required]
		public virtual int Patch { get; set; }

		[DataMember]
		public virtual ICollection<BlueprintPlatform> BlueprintPlatforms { get; set; }

        [DataMember]
        public virtual ICollection<PageDefinition> PageDefinitions { get; set; }

		[DataMember]
		public virtual ICollection<CmsModelDefinition> ModelDefinitions { get; set; }

		[DataMember]
		public virtual ICollection<CmsSettingDefinition> SettingDefinitions { get; set; }
	}
}
