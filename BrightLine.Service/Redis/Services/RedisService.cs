using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Utility;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using BrightLine.Service.Redis.Interfaces;
using ServiceStack.Redis;
using BrightLine.Common.ViewModels.RedisPubSub;
using BrightLine.Common.Utility.Constants;

namespace BrightLine.Service.Redis.Services
{
	public class RedisService : IRedisService
	{
		#region Members

		private static readonly Lazy<ConnectionMultiplexer> lazyConnection;
		private static readonly int Db;

		public static ConnectionMultiplexer Connection
		{
			get
			{
				return lazyConnection.Value;
			}
		}

		#endregion

		#region Init

		static RedisService()
		{
			var settings = IoC.Resolve<ISettingsService>();
			var redisConnectionString = settings.RedisClusterConnectionString;

			lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
			{
				return ConnectionMultiplexer.Connect(redisConnectionString);
			});

			Db = settings.RedisClusterDb;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Add multiple items to Redis
		/// </summary>
		/// <param name="data"></param>
		public void Add(Dictionary<string, string> data)
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				foreach (var item in data)
					cache.StringSet(item.Key, item.Value);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the Add method of Redis Service.", ex);
			}
		}

		/// <summary>
		/// Add a single item to Redis 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		public void Add(string key, string data)
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				cache.StringSet(key, data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the Add method of Redis Service.", ex);
			}
		}

		/// <summary>
		/// Purge all items in Redis environment
		/// </summary>
		public void PurgeAll()
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				var endpoints = Connection.GetEndPoints();
				var server = Connection.GetServer(endpoints.First());

				var allKeys = server.Keys().Select(k => k.ToString()).ToList();

				PurgeForKeys(cache, allKeys);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the PurgeAll method of Redis Service.", ex);
			}
		}

		/// <summary>
		/// Purge for a list of keys
		/// </summary>
		/// <param name="keys"></param>
		public void Purge(List<string> keys)
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				PurgeForKeys(cache, keys);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the Purge method of Redis Service.", ex);
			}
		}

		/// <summary>
		/// Publish data to a specific Pub/Sub Channel
		/// </summary>
		/// <param name="keys"></param>
		public void Publish(string channel, string data)
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				ISubscriber sub = Connection.GetSubscriber();
				sub.Publish(channel, data);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the Publish method of Redis Service.", ex);
			}
		}

		/// <summary>
		/// Subscribe to a specific Pub/Sub Channel
		/// </summary>
		/// <param name="channel"></param>
		public void Subscribe(string channel, Action<RedisChannel, RedisValue> callback)
		{
			try
			{
				var cache = Connection.GetDatabase(Db);

				ISubscriber sub = Connection.GetSubscriber();
				sub.Subscribe(channel, callback);
			}
			catch (Exception ex)
			{
				IoC.Log.Error("An unexpected error occurred in the Subscribe method of Redis Service.", ex);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Purge Redis for a list of keys
		/// </summary>
		/// <param name="cache"></param>
		/// <param name="allKeys"></param>
		private void PurgeForKeys(IDatabase cache, List<string> allKeys)
		{
			foreach (var key in allKeys)
				cache.KeyDelete(key);
		}

		#endregion
	}
}
