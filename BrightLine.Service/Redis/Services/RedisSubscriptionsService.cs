using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels.RedisPubSub;
using BrightLine.Service.Redis.Interfaces;
using BrightLine.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.Redis.Services
{
	/// <summary>
	/// This class sets up various subscriptions to Redis's Pub/Sub
	/// </summary>
	public class RedisSubscriptionsService : IRedisSubscriptionsService
	{
		#region Public Methods

		/// <summary>
		/// Setup all subscriptions to Redis Pub/Sub
		/// </summary>
		public void Setup()
		{
			SetupRefreshBrsSubscription();
		}

		/// <summary>
		/// Refresh the brs files by downloading them and storing in Redis
		/// </summary>
		/// <param name="redisService"></param>
		public void RefreshBrsFiles()
		{
			var redisService = IoC.Resolve<IRedisService>();

			// Download Brs Files
			var brsFiles = DownloadBrsFiles();

			// Serialize Brs Files
			var brsFilesSerialized = new Dictionary<string, string>();
			foreach (var brs in brsFiles)
			{
				var brsJObject = JObject.FromObject(brs.Value);
				brsFilesSerialized.Add(brs.Key, brsJObject.ToString());
			}

			// Save brs files to Redis
			redisService.Add(brsFilesSerialized);

			// Create a message that says the brs files are refreshed
			var viewModel = new RefreshBrsMessageViewModel { isRefreshed = true };
			var viewModelJObject = JObject.FromObject(viewModel);

			// Now publish to the channel to say that the brs files are now in Redis
			var refreshBrs = RedisPubSubConstants.Channels.RefreshBrs;
			redisService.Publish(refreshBrs, viewModelJObject.ToString());
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Setup a subscription to the Refresh Brs channel in Redis Pub/Sub
		/// </summary>
		private void SetupRefreshBrsSubscription()
		{
			var redisService = IoC.Resolve<IRedisService>();

			// Setup callback for Subscription
			Action<RedisChannel, RedisValue> callback = (channelOut, message) =>
			{
				var messageDeserialized = JsonConvert.DeserializeObject<RefreshBrsMessageViewModel>(message);
				if (messageDeserialized.refresh)
				{
					RefreshBrsFiles();
				}
			};

			// Subscribe to the Refresh brs channel
			var refreshBrs = RedisPubSubConstants.Channels.RefreshBrs;
			redisService.Subscribe(refreshBrs, callback);
		}

		/// <summary>
		/// Download Main.brs and RokuDirectIntegration.brs 
		/// </summary>
		private Dictionary<string, BrsFileViewModel> DownloadBrsFiles()
		{
			var settings = IoC.Resolve<ISettingsService>();
			var brsFiles = new Dictionary<string, BrsFileViewModel>();

			// Download Main.brs
			using (var client = new WebClient())
			{
				var url = settings.MainBrsFileUrl;
				var mainBrs = client.DownloadString(url);
				var viewModel = new BrsFileViewModel { data = mainBrs };
				brsFiles.Add(RedisPubSubConstants.BrsFiles.Names.Main, viewModel);
			}

			// Download RokuDirectIntegration.brs
			using (var client = new WebClient())
			{
				var url = settings.RokuDirectIntegrationBrsFileUrl;
				var rokuDirectIntegration = client.DownloadString(url);
				var viewModel = new BrsFileViewModel { data = rokuDirectIntegration };
				brsFiles.Add(RedisPubSubConstants.BrsFiles.Names.RokuDirectIntegration, viewModel);
			}

			return brsFiles;
		}

		#endregion
	}
}
