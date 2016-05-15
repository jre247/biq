using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service.Redis.Interfaces
{
	public interface IRedisService
	{
		void Add(Dictionary<string, string> data);
		void Add(string key, string data);
		void PurgeAll();
		void Purge(List<string> keys);
		void Publish(string channel, string data);
		void Subscribe(string channel, Action<RedisChannel, RedisValue> callback);
	}
}
