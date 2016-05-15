using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class CampaignPermissionsService : ICampaignPermissionsService
	{
		#region Public Methods

		public bool IsAccessible(Campaign campaign)
		{
			// Check if Campaign is null or deleted
			if (campaign == null || campaign.IsDeleted)
				return false;

			// Check if User is Admin or Employee
			if (Auth.Service.IsAdmin() || Auth.Service.IsEmployee())
				return true;

			// If User is Client, check if Campaign belongs to that Advertiser
			if (Auth.Service.IsClient())
				return IsCampaignAdvertiser(campaign);

			// If User is Agency Partner, check if Campaign belongs to that Media Agency
			if (Auth.Service.IsAgencyPartner())
				return IsCampaignMediaAgency(campaign);

			// If User is Agency Partner, check if Campaign belongs to that Media Agency
			if (Auth.Service.IsMediaPartner())
				return IsCampaignMediaPartner(campaign);

			return false;
		}

		/// <summary>
		/// Determines whether or not the current User is from the Media Partner of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignMediaPartner(Campaign campaign)
		{
			var deliveryGroupsRepo = IoC.Resolve<IRepository<DeliveryGroup>>();
			var mediaPartnersRepo = IoC.Resolve<IMediaPartnerService>();

			// Check if Campaign is null or deleted
			if (campaign == null || campaign.IsDeleted || Auth.UserModel.MediaPartner == null)
				return false;


			var mediaPartnerId = Auth.UserModel.MediaPartner.Id;
			var deliveryGroups = deliveryGroupsRepo.Where(d => d.Campaign.Id == campaign.Id).ToList();

			// Find all Delivery Groups for the current Media Partner for this Campaign
			var deliveryGroupsForMediaPartner = deliveryGroups.Where(d => d.MediaPartner != null && d.MediaPartner.Id == mediaPartnerId).ToList();

			// Find all Delivery Groups that are assigned a Media Partner whose Parent is the current Media Partner 
			var mediaPartnerChildren = mediaPartnersRepo.Where(m => m.Parent_Id == mediaPartnerId).Select(m => m.Id).ToList();
			var deliveryGroupsForMediaPartnerChildren = deliveryGroups.Where(d => d.MediaPartner != null && mediaPartnerChildren.Contains(d.MediaPartner.Id)).ToList();

			// campaign is accessible for Media Partner if there are any Delivery Groups for either the current Media Partner or any Delivery Groups whose Media Partner's Parent is the current Media Partner
			var isMediaPartner = deliveryGroupsForMediaPartner.Count() > 0 || deliveryGroupsForMediaPartnerChildren.Count() > 0;

			return isMediaPartner;
		}

		/// <summary>
		/// Determines whether or not the current User is from the Media Agency of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignMediaAgency(Campaign campaign)
		{
			// Check if Campaign is null or deleted
			if (campaign == null || campaign.IsDeleted || Auth.UserModel.MediaAgency == null)
				return false;

			// Campaign is accessible if the Campaign's Agency is either the current Media Agency or if the Campaign's Agency's Parent references the current Media Agency
			var isAgency = campaign.MediaAgency.Id == Auth.UserModel.MediaAgency.Id || campaign.MediaAgency.Parent_Id == Auth.UserModel.MediaAgency.Id;
			return isAgency;
		}

		/// <summary>
		/// Determines whether or not the current User is from the Advertiser of the Campaign
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns></returns>
		public bool IsCampaignAdvertiser(Campaign campaign)
		{
			// Check if Campaign is null or deleted
			if (campaign == null || campaign.IsDeleted || Auth.UserModel.Advertiser == null)
				return false;

			// Compare Advertiser Id
			var isAdvertiser = campaign.Product.Brand.Advertiser.Id == Auth.UserModel.Advertiser.Id;
			return isAdvertiser;
		}

		#endregion
	}
}
