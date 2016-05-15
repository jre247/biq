using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility
{
    /// <summary>
    /// System.Runtim.Cache does not support an interface and the System.Runtime.Cache.Default is not easily unit-testable.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Adds the item to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheValue"></param>
        /// <param name="minutes"></param>
        void Add(string key, object cacheValue, TimeSpan minutes);
		
        /// <summary>
        /// Adds the item to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="minutes"></param>
        void Add(string key, object cacheValue, Func<object> dataFetcher, TimeSpan minutes);
		
        /// <summary>
        /// Adds the item to the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cacheValue"></param>
        /// <param name="minutes"></param>
        object AddOrGetExisting(string key, object cacheValue, TimeSpan minutes);
		
        /// <summary>
        /// Get the cache item using the keyname supplied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Whether or not the cache contains the key supplied.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);

		/// <summary>
		/// Removes the item from the cache.
		/// </summary>
		/// <param name="key"></param>
		void Remove(string key);

		/// <summary>
		/// Removes items from the cache that satisfy the predicate.
		/// </summary>
		/// <param name="predicate">A filter for the cache.</param>
		void Remove(Func<string, bool> predicate);

		/// <summary>
		/// Removes all items from the cache.
		/// </summary>
		void Purge();
		
        /// <summary>
        /// Sets the cache key to the value supplied.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        void Set(string key, object val, TimeSpan minutes);

        /// <summary>
        /// Get the cache item using the keyname supplied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// NOTE: This could potentially fail as this is not thread safe.
        /// 1. In order for this to be threadsafe ( the add/set/clear ) needs to be threadsafe.
        /// 2. Locking on the add/set may be ok now ( due to volume ), but if higher could be an issue.
        /// 3. Allow for the time being to let this potentially fail.
        /// </summary>
        /// <returns></returns>
        List<CacheMetadata> GetCacheEntries();

        /// <summary>
        /// Refreshes a cache item ( the cache item must be auto-refreshable via initializing it via a delegate)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Refresh(string key);
		
        /// <summary>
        /// Total items in cache.
        /// </summary>
        /// <returns></returns>
        int Total();
    }
}
