using BrightLine.Common.Utility;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
    [DataContract(IsReference = true)]
    public class Creative : EntityBase, IEntity
    {
        [DataMember]
        [Required]
        public virtual Campaign Campaign { get; set; }

        [DataMember]
        [Required, Column(Order = 2, TypeName = "VARCHAR"), StringLength(255)]
        public string Name { get; set; }

		//TODO: this property will be deprecated after Resources are backfilled
        [DataMember]
        public int? Resource_Id { get; set; }

		[DataMember]
		public int? Thumbnail_Id { get; set; }

		[DataMember]
		public virtual ICollection<Resource> Resources { get; set; }

        [DataMember]
        public virtual ICollection<Ad> Ads { get; set; }

        [DataMember]
		[StringLength(1000)]
        public string Description { get; set; }

        [DataMember]
        public int? Height { get; set; }

        [DataMember]
        public int? Width { get; set; }

        [DataMember]
		[Required]
		[ForeignKey("AdType_Id")]
        public virtual AdType AdType { get; set; }
		public int? AdType_Id { get; set; }

        [DataMember]
		[ForeignKey("AdFunction_Id")]
        public virtual AdFunction AdFunction { get; set; }
		public int? AdFunction_Id { get; set; }

        [DataMember]
        public virtual ICollection<Feature> Features { get; set; }

		[NotMapped]
		public string ResourceIds { get;set;}

		[DataMember]
		[ForeignKey("AdFormat_Id")]
		public virtual AdFormat AdFormat { get; set; }
		public int? AdFormat_Id { get;set;}

		[DataMember]
		public int? InactivityThreshold { get; set; }
		
        public override bool IsValid()
        {
            var isValid = true;

			// Validate Features
			if (Features != null)
			{
				var featuresList = new List<Feature>(Features);

				isValid = ValidateResources();
				if (!isValid)
					return isValid;

				for (var featureIndex = 0; featureIndex < featuresList.Count; featureIndex++)
				{
					var feature = featuresList[featureIndex];

					isValid = ValidateUniqueFeatureName(featureIndex, featuresList);
					if (!isValid)
						break;

					if (!feature.IsValid())
					{
						isValid = false;
						break;
					}
				}
			}

			// Validate Inactivity Threshold for Destination Creative
			if (!AdType.IsPromo)
				if (InactivityThreshold.HasValue)
					if (InactivityThreshold > int.MaxValue)
						isValid = false;

            return isValid;
        }

		/// <summary>
		/// There cannot be more than 1 SD Image, and there cannot be more than 1 HD Image, on the creative level
		/// </summary>
		/// <returns></returns>
		private bool ValidateResources()
		{
			if(this.Resources == null)
				return true;

			var isValid = true;
			var resources = new List<Resource>(this.Resources);

			var sdResourceCount = 0;
			var hdResourceCount = 0;
			foreach(var resource in this.Resources)
			{
				if (resource.IsDeleted)
					continue;

				if (sdResourceCount > 1 || hdResourceCount > 1)
				{
					isValid = false;
					break;
				}
					
				if(resource.ResourceType.Id == BrightLine.Common.Utility.Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage])
					sdResourceCount++;

				if (resource.ResourceType.Id == BrightLine.Common.Utility.Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage])
					hdResourceCount++;
			}

			return isValid;

		}

		private bool ValidateUniqueFeatureName(int featureIndex, List<Feature> featuresList)
		{
			var isValid = true;
			var feature = featuresList[featureIndex];

			for (var featureCompareIndex = 0; featureCompareIndex < featuresList.Count; featureCompareIndex++)
			{
				if (featureIndex == featureCompareIndex)
					continue;

				var featureCompare = featuresList[featureCompareIndex];

				if (feature.Name.ToLowerInvariant() == featureCompare.Name.ToLowerInvariant())
				{
					isValid = false;

					break;
				}
				
			}
			return isValid;
		}
    }
}
