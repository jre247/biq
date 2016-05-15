using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Platform;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.Factories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using Newtonsoft.Json;
using BrightLine.Service.Redis.Interfaces;
using Brightline.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Common.Utility.AdType;
using Brightline.Publishing.Areas.AdResponses.Helpers;

namespace BrightLine.Publishing.Areas.AdResponses.Services
{
	public class AdResponsesService : IAdResponsesService
	{
		#region Public Methods

		public void Publish(Campaign campaign, string targetEnv, Guid publishId)
		{
			var redisService = IoC.Resolve<IRedisService>();

			var adResponseDictionary = GetAdResponsesForCampaign(campaign, targetEnv, publishId);

			ClearRemoteCache(adResponseDictionary);

			redisService.Add(adResponseDictionary);		
		}

		/// <summary>
		/// Build up a dictionary of all Ad Responses for a specific Campaign where the key is the Ad Tag Id and the value is a C# object that represents metadata and the actual Ad Response data.
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns> 
		///		A dictionary where the key is the Ad Tag Id and the value is a C# object that represents:
		///			1) Metadata about the Ad, such as PlatformId, AdId, AdTagId
		///			2) The actual Ad Response as a string. This Ad Response will be either json or VAST Xml
		///	</returns>
		public Dictionary<string, string> GetAdResponsesForCampaign(Campaign campaign, string targetEnv, Guid publishId)
		{
			var adResponseDictionary = new Dictionary<string, AdResponseViewModel>();
			var adResponseSerializedDictionary = new Dictionary<string, string>();

			// Get a hash of allowed Platforms
			var platformsAllowed = PlatformHelper.GetAllowedPlatformsHash();

			// Get a hash of allowed Ad Types
			var adTypesAllowed = AdTypeHelper.GetAllowedAdTypesHash();

			// Get Ads
			var ads = campaign.Ads.Where(a => 
					platformsAllowed.Contains(a.Platform.Id) &&
					adTypesAllowed.Contains(a.AdType.Id)
				).ToList();

			BuildAdResponsesCollection(targetEnv, publishId, adResponseDictionary, ads);

			// Serialize Ad Responses
			foreach (var adResponse in adResponseDictionary)
				adResponseSerializedDictionary.Add(adResponse.Key, JsonConvert.SerializeObject(adResponse.Value));

			return adResponseSerializedDictionary;
		}

		/// <summary>
		/// Build up a dictionary of all Ad Responses for a specific Ad where the key is the Ad Tag Id and the value is a C# object that represents metadata and the actual Ad Response data.
		/// </summary>
		/// <param name="campaign"></param>
		/// <returns> 
		///		A dictionary where the key is the Ad Tag Id and the value is a C# object that represents:
		///			1) Metadata about the Ad, such as PlatformId, AdId, AdTagId
		///			2) The actual Ad Response as a string. This Ad Response will be either json or VAST Xml
		///	</returns>
		public Dictionary<string, Dictionary<string, AdResponseViewModel>> GetAdResponse(Ad ad, string targetEnv, Guid publishId)
		{
			var adsRepo = IoC.Resolve<IAdService>();
			var adResponseDictionary = new Dictionary<string, AdResponseViewModel>();
			var ads = new List<Ad>();
			var adResponsesCategoryDictionary = new Dictionary<string, Dictionary<string, AdResponseViewModel>>();

			// Build a list that contains the current ad and its Destination Ad
			//	*Note: This way we can get the Promotional and Destination Ad AdResponses for the current Ad
			ads.Add(ad);

			// *Note: Commercial Spot Ads will not have Destination Ads
			if (ad.DestinationAd != null)
				ads.Add(ad.DestinationAd);

			BuildAdResponsesCollection(targetEnv, publishId, adResponseDictionary, ads);	

			var adResponsesPromotional = adResponseDictionary.Where(a => a.Value.Metadata.ad.id == ad.Id).ToDictionary(a => a.Key, a => a.Value);

			// *Note: Commercial Spot Ads will not have Destination Ads, so if that's the case then don't add Destination Ad Responses to the Ad Response Preview
			if (ad.DestinationAd != null)
			{
				var adResponsesDestination = adResponseDictionary.Where(a => a.Value.Metadata.ad.id == ad.DestinationAd.Id).ToDictionary(a => a.Key, a => a.Value);
				adResponsesCategoryDictionary.Add(PublishConstants.AdResponseDictionaryKeys.Destination, adResponsesDestination);
			}

			adResponsesCategoryDictionary.Add(PublishConstants.AdResponseDictionaryKeys.Promotional, adResponsesPromotional);

			return adResponsesCategoryDictionary;
		}

		#endregion

		#region Private Methods

		private void BuildAdResponsesCollection(string targetEnv, Guid publishId, Dictionary<string, AdResponseViewModel> adResponseDictionary, List<Ad> ads)
		{
			foreach (var ad in ads)
			{
				var factory = new AdTypeAdResponses();
				var service = factory.GetService(ad, targetEnv, publishId);

				// Get Ad Responses for a specific Ad Type
				var adResponses = service.GetAdResponses();

				if (adResponses == null)
					continue;

				foreach (var adResponse in adResponses)
					adResponseDictionary.Add(adResponse.Key, adResponse);
			}
		}

		/// <summary>
		/// Clear the remote cache for all Ad Responses. 
		/// </summary>
		/// <param name="adResponseDictionary"></param>
		private void ClearRemoteCache(Dictionary<string, string> adResponseDictionary)
		{
			// Build up a comma separated list of all Ad Response Keys
			var adTags = string.Join(",", adResponseDictionary.Select(a => a.Key).ToList());

			// Create JObject for keys
			var publishViewModel = new RedisPublishViewModel{keys = adTags};
			var adTagsJObject = JObject.FromObject(publishViewModel);

			// Publish message to Redis Pub/Sub
			var redisService = IoC.Resolve<IRedisService>();

			//var redisChannels = new RedisPubSubConstants.Channels();
			var cacheClear = RedisPubSubConstants.Channels.CacheClear;

			redisService.Publish(cacheClear, adTagsJObject.ToString());
		}

		#endregion
	}
}
