using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;


namespace BrightLine.Utility
{

	/// <summary>
	/// Thin wrapper around the System.Runtime.MemoryCache
	/// This wrapper serves a few purposes:
	/// 
	/// 1. More easily track/obtain metadata ( expiration time, easily get keys ) 
	/// 2. Provide convenient reporting of cache data
	/// 3. Provide an abstraction over caching.
	/// 
	/// </summary>
	public class Cache : ICache
	{
		private IDictionary<string, CacheMetadata> _cacheMetadata;
		private object _syncRoot = new object();

		/// <summary>
		/// Initialize
		/// </summary>
		public Cache()
		{
			_cacheMetadata = new Dictionary<string, CacheMetadata>();
		}

		/// <summary>
		/// Adds the item to the cache.
		/// </summary>
		/// <param name="key">The key to associated the cache item with</param>
		/// <param name="cacheValue">The initial value ( can be empty )</param>
		/// <param name="minutes">The time in minutes to keep the cache.</param>
		public void Add(string key, object cacheValue, TimeSpan minutes)
		{
			lock (_syncRoot)
			{
				InternalAdd(key, cacheValue, null, minutes);
			}
		}

		/// <summary>
		/// Adds the item to the cache using an initial cache object, and an automatic fetecher for reloadeding.
		/// </summary>
		/// <param name="key">The key to associated the cache item with</param>
		/// <param name="cacheValue">The initial value ( can be empty )</param>
		/// <param name="fetcher">The callback to fetch the data.</param>
		/// <param name="minutes">The time in minutes to keep the cache.</param>
		public void Add(string key, object cacheValue, Func<object> fetcher, TimeSpan minutes)
		{
			lock (_syncRoot)
			{
				InternalAdd(key, cacheValue, fetcher, minutes);
			}
		}

		/// <summary>
		/// Adds the item to the cache using if not present, gets existing otherwise.
		/// </summary>
		/// <param name="key">The key to associated the cache item with</param>
		/// <param name="cacheValue">The initial value ( can be empty )</param>
		/// <param name="minutes">The time in minutes to keep the cache.</param>
		public object AddOrGetExisting(string key, object cacheValue, TimeSpan minutes)
		{
			lock (_syncRoot)
			{
				if (InternalContains(key))
					return InternalGet(key);

				InternalAdd(key, cacheValue, null, minutes);
				return cacheValue;
			}
		}

		/// <summary>
		/// Sets the cache key to the value supplied.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="minutes"></param>
		public void Set(string key, object val, TimeSpan minutes)
		{
			// Whether add or update existing, the audit and expiration will get updated.
			// So it's ok to just add as this is not an auto-refreshable item.
			lock (_syncRoot)
			{
				InternalAdd(key, val, null, minutes);
			}
		}

		/// <summary>
		/// Get the cache item using the keyname supplied.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Get<T>(string key)
		{
			lock (_syncRoot)
			{
				return (T)InternalGet(key);
			}
		}

		/// <summary>
		/// Whether or not the cache contains the key supplied.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			lock (_syncRoot)
			{
				return InternalContains(key);
			}
		}

		/// <summary>
		/// Removes the item to the cache.
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			lock (_syncRoot)
			{
				// 1. Remove from underlying store.
				MemoryCache.Default.Remove(key);

				// 2. Remove from metadata ( if not auto-refreshable )
				if (_cacheMetadata.ContainsKey(key))
				{
					var item = _cacheMetadata[key];
					if (!item.IsAutoRefreshed())
					{
						_cacheMetadata.Remove(key);
					}
				}
			}
		}

		/// <summary>
		/// Removes items from the cache that satisfy the predicate
		/// </summary>
		/// <param name="predicate">The cache filter.</param>
		public void Remove(Func<string, bool> predicate)
		{
			var cacheItems = GetCacheEntries();
			lock (_syncRoot)
			{
				foreach (var cacheItem in cacheItems)
				{
					var key = cacheItem.Key;
					if (!predicate(key))
						continue;

					MemoryCache.Default.Remove(key);
					var item = _cacheMetadata[key];
					_cacheMetadata.Remove(key);
				}
			}
		}

		/// <summary>
		/// Removes the item to the cache.
		/// </summary>
		/// <param name="key"></param>
		public void Purge()
		{
			lock (_syncRoot)
			{
				foreach (var pair in _cacheMetadata)
				{
					MemoryCache.Default.Remove(pair.Key);
				}
			}
		}

		/// <summary>
		/// Get the cache item using the keyname supplied.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key)
		{
			lock (_syncRoot)
			{
				return InternalGet(key);
			}
		}

		/// <summary>
		/// Refreshes a cache item ( the cache item must be auto-refreshable via initializing it via a delegate)
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Refresh(string key)
		{
			lock (_syncRoot)
			{
				if (!_cacheMetadata.ContainsKey(key))
					return null;

				var meta = _cacheMetadata[key];
				return InternalRefresh(key, meta);
			}
		}

		/// <summary>
		/// Total items in cache ( includes items that are auto-cached ).
		/// </summary>
		/// <returns></returns>
		public int Total()
		{
			lock (_syncRoot)
			{
				return _cacheMetadata.Count;
			}
		}

		/// <summary>
		/// NOTE: This could potentially fail as this is not thread safe.
		/// 1. In order for this to be threadsafe ( the add/set/clear ) needs to be threadsafe.
		/// 2. Locking on the add/set may be ok now ( due to volume ), but if higher could be an issue.
		/// 3. Allow for the time being to let this potentially fail.
		/// </summary>
		/// <returns></returns>
		public List<CacheMetadata> GetCacheEntries()
		{
			lock (_syncRoot)
			{
				var items = new List<CacheMetadata>();
				foreach (var pair in _cacheMetadata)
				{
					var isInCache = MemoryCache.Default.Contains(pair.Key);

					// CASE 1: Static item in cache.
					if (isInCache)
					{
						var copy = pair.Value.Clone() as CacheMetadata;
						var val = MemoryCache.Default.Get(pair.Key);
						copy.StringVal = GetCacheValue(val);
						items.Add(copy);
					}
					// CASE 2: auto-refreshable not in cache.
					else if (pair.Value.IsAutoRefreshed())
					{
						var copy = pair.Value.Clone() as CacheMetadata;
						copy.StringVal = "not loaded in cache";
						items.Add(copy);
					}
				}
				return items;
			}
		}

		#region Internal non-locked methods

		/// <summary>
		/// Get the cache item using the keyname supplied.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		private object InternalGet(string key)
		{
			CacheMetadata meta = null;
			object obj = null;
			try
			{
				// Not in cache ?
				if (!InternalContains(key))
				{
					// CASE 1: Not in underlying cache and metadata -> removed.
					if (!_cacheMetadata.ContainsKey(key))
					{
						return null;
					}

					// CASE 2: Auto-Refreshable.
					meta = _cacheMetadata[key];
					if (meta.IsAutoRefreshed())
					{
						return InternalRefresh(key, meta);
					}

					// CASE 3: Expired, so remove
					_cacheMetadata.Remove(key);
					return null;
				}
				// Audit
				if (_cacheMetadata.ContainsKey(key))
				{
					meta = _cacheMetadata[key];
					Audit(meta, false);
				}
				obj = MemoryCache.Default.Get(key);
			}
			catch (Exception)
			{
			}
			return obj;
		}

		/// <summary>
		/// Internal check to see if key is in cache. (internal methods not-locked )
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private bool InternalContains(string key)
		{
			return MemoryCache.Default.Contains(key);
		}

		/// <summary>
		/// Adds the item to the cache using an initial cache object, and an automatic fetecher for reloadeding. (internal methods not-locked )
		/// </summary>
		/// <param name="key"></param>
		/// <param name="cacheValue"></param>
		/// <param name="minutes"></param>
		private void InternalAdd(string key, object cacheValue, Func<object> fetcher, TimeSpan minutes)
		{
			// 1. Build up metadata.
			CacheMetadata meta = null;
			var expires = DateTime.UtcNow.Add(minutes);
			var changeUpdateFlag = cacheValue != null;

			if (_cacheMetadata.ContainsKey(key))
			{
				meta = _cacheMetadata[key];
				meta.Expires = expires;
				meta.ExpirationMinutes = minutes.TotalMinutes;
				Audit(meta, changeUpdateFlag);
			}
			else
			{
				meta = CreateMetadata(key, cacheValue, expires, minutes.TotalMinutes, fetcher);
				Audit(meta, changeUpdateFlag);
			}

			// 2. Build up offset
			var offset = new DateTimeOffset(meta.Expires);

			// 3. Store in cache ( Use builtin .NET cache. )
			// NOTE: Don't get the value from fetcher ( auto-refresh ) immediately, make it lazy-loaded.
			if (cacheValue != null)
			{
				MemoryCache.Default.Add(key, cacheValue, offset);
			}

			// 4. Store the metadata ( for the admin UI. )
			_cacheMetadata[key] = meta;
		}

		private object InternalRefresh(string key, CacheMetadata meta)
		{
			if (!meta.IsAutoRefreshed())
				return null;

			var val = meta.Fetcher();
			meta.Expires = DateTime.UtcNow.AddMinutes(meta.ExpirationMinutes);
			Audit(meta, true);
			var offset = new DateTimeOffset(meta.Expires);
			MemoryCache.Default.Set(key, val, offset);
			return val;
		}

		private CacheMetadata CreateMetadata(string key, object cacheValue, DateTime expires, double expirationMinutes,
											 Func<object> fetcher)
		{
			var meta = new CacheMetadata();
			meta.Key = key;
			if (cacheValue != null)
				meta.Type = cacheValue.GetType().FullName;
			else if (fetcher != null)
			{
				meta.Type = "( auto loaded )";
				meta.Fetcher = fetcher;
			}
			meta.Expires = expires;
			meta.ExpirationMinutes = expirationMinutes;
			return meta;
		}

		private void Audit(CacheMetadata meta, bool isUpdated)
		{
			meta.LastAccessTime = DateTime.UtcNow;

			// Avoid overflow
			if (meta.AccessCount > (int.MaxValue - 10))
			{
				meta.AccessCount = 0;
			}
			meta.AccessCount++;
			if (isUpdated)
				meta.LastUpdated = DateTime.UtcNow;
		}

		private string GetCacheValue(object obj)
		{
			if (obj == null)
				return "";
			var val = obj.ToString();
			if (val.Length < 40)
				return val;
			return val.Substring(0, 40) + "...";
		}

		#endregion
	}
}
