using BrightLine.Common.Framework;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.ViewModels.Resources;
using System;
using System.Runtime.Serialization;
using System.Web;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class CampaignsListingViewModel
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public bool? Internal { get; set; }

		[DataMember]
		public string BrandName { get; set; }

		[DataMember]
		public string AdvertiserName { get; set; }

		[DataMember]
		public string VerticalName { get; set; }

		[DataMember]
		public int? AgencyId { get;set;}

		[DataMember]
		public int? MediaPartnerId { get; set; }

		[DataMember]
		public int? AdvertiserId { get; set; }

		[DataMember]
		public string ProductName { get; set; }

		public DateTime? beginDateRaw { get; set; }
		public DateTime? endDateRaw { get; set; }

		[DataMember]
		public DateTime? BeginDate { get { return DateHelper.ToUserTimezone(beginDateRaw); } }

		[DataMember]
		public DateTime? EndDate { get { return DateHelper.ToUserTimezone(endDateRaw); } }

		public DateTime? LastModifiedRaw;
		[DataMember]
		public DateTime? LastModified
		{
			get { return DateHelper.ToUserTimezone(LastModifiedRaw); }
			set { LastModifiedRaw = value; }
		}

		[DataMember]
		public bool IsFavorite { get; set; }

		[DataMember]
		public ResourceViewModel Resource
		{
			get
			{
				if (ThumbnailId.HasValue && ThumbnailResourceType.HasValue)
				{
					var resourceHelper = IoC.Resolve<IResourceHelper>();

					return resourceHelper.GetResourceViewModel(ThumbnailId.Value, ThumbnailFileName, ThumbnailName, ThumbnailResourceType.Value, Id);
				}

				return null;
			}
		}

		[DataMember]
		public string Status
		{
			get
			{
				return CampaignHelper.GetCampaignStatus(BeginDate, EndDate);
			}
		}

		[DataMember]
		public string ThumbnailName { get; set; }

		[DataMember]
		public string ThumbnailFileName { get; set; }

		[DataMember]
		public int? ThumbnailId { get; set; }

		[DataMember]
		public int? ThumbnailResourceType { get; set; }

		[DataMember]
		public bool HasAnalytics { get; set; }

		[DataMember]
		public bool HasCms { get; set; }
	}
}