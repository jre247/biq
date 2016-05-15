using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		/// <summary>
		/// Gets campaigns for testing.
		/// </summary>
		/// <returns></returns>
		public static List<Campaign> GetCampaigns()
		{
			var items = new List<Campaign>()
				{
					new Campaign() {Id = 1, Name = "Ford", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 2, Name = "Yugo", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 3, Name = "Ferrari", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 4, Name = "Buick", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 5, Name = "Jaquar", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 6, Name = "Scion", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 7, Name = "Nissan", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 8, Name = "Tesla", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
					new Campaign() {Id = 9, Name = "Bentley", UsersFavorite = new List<User>(), CampaignContentSchemas = new List<CampaignContentSchema>(), Features = new List<Feature>(),},
		
				};
			return items;
		}

		public static Campaign CreateCampaign(int campaignId, string campaignName, int advertiserId)
		{
			var campaign = new Campaign();
			campaign.Id = campaignId;
			campaign.Name = campaignName;
			campaign.Product = new Product();
			campaign.Product.Brand = new Brand();
			campaign.Product.Brand.Advertiser = new Advertiser();
			campaign.Product.Brand.Advertiser.Id = advertiserId;
			campaign.MediaAgency = new Agency { Id = 1 };

			return campaign;
		}

	}

}