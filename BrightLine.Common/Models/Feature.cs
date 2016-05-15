using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class Feature : EntityBase, IEntity
	{
		[Required]
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		[CascadeSoftDelete]
		public virtual ICollection<Ad> Ads { get; set; }

		[Required]
		public virtual Campaign Campaign { get; set; }

		[Required]
		[DataMember]
		public virtual FeatureType FeatureType { get; set; }

		[DataMember]
		public virtual FeatureCategory FeatureCategory { get; set; }

		[DataMember]
		[CascadeSoftDelete]
		public virtual ICollection<Page> Pages { get; set; }

		[DataMember]
		[CascadeSoftDelete]
		public virtual ICollection<CmsModel> Models { get; set; }

		public double? EngagementRate { get; set; }

		[DataMember]
		public virtual Blueprint Blueprint { get; set; }

		[DataMember]
		public virtual int OrderIndex { get; set; }

		[DataMember]
		[StringLength(255)]
		public virtual string ButtonName { get; set; }
		
		[DataMember]
		public DateTime? DateAdded { get; set; }

		[DataMember]
		public DateTime? DateArchived { get; set; }

		[DataMember]
		public virtual Creative Creative { get; set; }

        public override bool IsValid()
        {
            var isValid = true;

			if(Pages == null)
				return isValid;
			
			foreach (var page in Pages)
			{
				foreach (var pageCompare in Pages)
				{
					if (page.PageDefinition.Id != pageCompare.PageDefinition.Id)
					{
						if (!string.IsNullOrWhiteSpace(page.Name))
						{
							if (page.Name.ToLowerInvariant() == pageCompare.Name.ToLowerInvariant())
							{
								isValid = false;

								break;
							}
						}
					}
				}
			}
			
			//TODO: validate models in addition to pages

            return isValid;
        }
	}
}
