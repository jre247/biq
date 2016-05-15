using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Enums;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class CampaignViewModel
	{
		private IRepository<Agency> Agencies { get; set; }
		private IResourceService Resources { get; set; }
		private IRepository<Product> Products { get; set; }

		public CampaignViewModel()
		{
			Agencies = IoC.Resolve<IRepository<Agency>>();
			Resources = IoC.Resolve<IResourceService>();
			Products = IoC.Resolve<IRepository<Product>>();
		}

		[DataMember, Required, HiddenInput]
		public int Id { get; set; }

		public bool IsNewEntity { get { return this.Id == 0; } }

		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public bool Internal { get; set; }

		[DataMember, Display(Name = "Sales Force Id")]
		public string SalesForceId { get; set; }

		[DataMember, Display(Name = "Google Analytics Id")]
		public string GoogleAnalyticsIds { get; set; }

		[DataMember]
		public string CmsKey { get; set; }

		[DataMember, Display(Name = "Media Agency"), RequiredLookup(ErrorMessage = "Media Agency must be selected.")]
		public ILookup MediaAgency { get; set; }

		[DataMember, Display(Name = "Creative Agency"), RequiredLookup(ErrorMessage = "Creative Agency must be selected.")]
		public ILookup CreativeAgency { get; set; }

		[DataMember, RequiredLookup(ErrorMessage = "Product must be selected.")]
		public ILookup Product { get; set; }

		[DataMember, Display(Name = "Campaign Type"), Required(ErrorMessage = "Campaign type must be selected.")]
		public CampaignTypes? CampaignType { get; set; }

		[DataMember]
		public ILookup Thumbnail { get; set; }

		[DataMember]
		public DateTime DateUpdated { get; set; }

		[DataMember]
		public string ResourceDownloadUrl { get; set; }

		public int? ResourceId { get; set; }

		public static CampaignViewModel FromCampaign(Campaign campaign)
		{
			if (campaign == null)
				return new CampaignViewModel();

			var resourceHelper = IoC.Resolve<IResourceHelper>();

			string resourceDownloadUrl = null;

			var product = campaign.Product as ILookup;
			var mediaAgency = campaign.MediaAgency as ILookup;
			var creativeAgency = campaign.CreativeAgency as ILookup;
			var thumbnail = EntityLookup.ToLookup(campaign.Thumbnail, "name");
			if(thumbnail != null)
				resourceDownloadUrl = resourceHelper.GetResourceDownloadPath(thumbnail.Id, campaign.Thumbnail.Filename, campaign.Thumbnail.ResourceType.Id, campaign.Id);

			var lastModified = DateHelper.ToUserTimezone(campaign.DateUpdated ?? campaign.DateCreated);
			var cvm = new CampaignViewModel()
				{
					Id = campaign.Id,
					Name = campaign.Name,
					Description = campaign.Description,
					SalesForceId = campaign.SalesForceId,
					CmsKey = campaign.CmsKey,
					Product = product,
					GoogleAnalyticsIds = campaign.GoogleAnalyticsIds,
					CampaignType = campaign.CampaignType,
					Thumbnail = thumbnail,
					DateUpdated = lastModified,
					MediaAgency = mediaAgency,
					CreativeAgency = creativeAgency,
					Internal = campaign.Internal.HasValue ? campaign.Internal.Value : false,
					ResourceDownloadUrl = resourceDownloadUrl
				};

			return cvm;
		}

		public Campaign ToCampaign(Campaign campaign)
		{
			var mediaAgency = Agencies.Get(this.MediaAgency.Id);
			var creativeAgency = Agencies.Get(this.CreativeAgency.Id);

			campaign.Id = this.Id;
			campaign.Name = this.Name;
			campaign.MediaAgency_Id = mediaAgency.Id;
			campaign.CreativeAgency_Id = creativeAgency.Id;
			campaign.Description = this.Description;
			campaign.SalesForceId = this.SalesForceId;
			campaign.CmsKey = this.CmsKey;
			campaign.GoogleAnalyticsIds = this.GoogleAnalyticsIds;
			campaign.Thumbnail = (this.Thumbnail == null) ? null : Resources.Get(this.Thumbnail.Id);
			this.Thumbnail = EntityLookup.ToLookup(campaign.Thumbnail, "filename");
			var product = Products.Get(this.Product.Id);
			campaign.Product = product;
			campaign.Internal = this.Internal;
			if (this.CampaignType.HasValue)
				campaign.CampaignType = this.CampaignType.Value;

			return campaign;
		}

		
	}
}
