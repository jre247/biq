using BrightLine.Common.Core;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace BrightLine.Common.Framework
{
	public static class Extensions
	{
		#region NameValueCollection extensions

		public static bool GetBoolean(this NameValueCollection settings, string key, bool @default = false)
		{
			if (settings == null)
				return @default;

			bool value;
			if (!bool.TryParse(settings[key], out value))
				value = @default;

			return value;
		}

		public static int GetInt(this NameValueCollection settings, string key, int @default = 0)
		{
			if (settings == null || string.IsNullOrWhiteSpace(key))
				return @default;

			var value = settings[key];
			if (string.IsNullOrWhiteSpace(value))
				return @default;

			int v;
			if (!int.TryParse(value.Trim(), out v))
				v = @default;
			return v;
		}

		public static string GetString(this NameValueCollection settings, string key, string @default = "")
		{
			if (settings == null || string.IsNullOrWhiteSpace(key))
				return @default;

			var value = settings[key];
			if (string.IsNullOrWhiteSpace(value))
				return @default;

			return value.Trim();
		}

		public static ushort GetUShort(this NameValueCollection settings, string key, ushort @default = 0)
		{
			if (settings == null || string.IsNullOrWhiteSpace(key))
				return @default;

			var value = settings[key];
			if (string.IsNullOrWhiteSpace(value))
				return @default;

			ushort v;
			if (!ushort.TryParse(value.Trim(), out v))
				v = @default;
			return v;
		}

		public static short GetShort(this NameValueCollection settings, string key, short @default = 0)
		{
			if (settings == null || string.IsNullOrWhiteSpace(key))
				return @default;

			var value = settings[key];
			if (string.IsNullOrWhiteSpace(value))
				return @default;

			short v;
			if (!short.TryParse(value.Trim(), out v))
				v = @default;
			return v;
		}

		public static byte GetByte(this NameValueCollection settings, string key, byte @default = 0)
		{
			if (settings == null || string.IsNullOrWhiteSpace(key))
				return @default;

			var value = settings[key];
			if (string.IsNullOrWhiteSpace(value))
				return @default;

			byte v;
			if (!byte.TryParse(value.Trim(), out v))
				v = @default;
			return v;
		}

		#endregion

		#region ICollection methods

		public static ICollection<T> ClearAndAddRange<T>(this ICollection<T> collection, IEnumerable<T> source) where T : class, IEntity
		{
			if (collection == null || source == null)
				return collection;

			collection.Clear();
			return collection.AddRange(source);
		}

		public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> source, bool allowDuplicates = false) where T : class
		{
			if (collection == null || source == null)
				return collection;

			foreach (var s in source)
			{
				if (!(allowDuplicates && collection.Contains(s)))
					collection.Add(s);
			}

			return collection;
		}

		public static ICollection<T> ClearAndAdd<T>(this ICollection<T> collection, T source) where T : class, IEntity
		{
			if (collection == null)
				return collection;

			collection.Add(source);
			return collection;
		}

		/// <summary>
		/// Merge the source into the collection list. The collection returned will contain the original collection without matched elements in source,
		/// plus additional elements from source.
		/// </summary>
		/// <typeparam name="T">The IEntity derived class.</typeparam>
		/// <param name="collection">The collection to return.</param>
		/// <param name="source">The source collection for the merge.</param>
		/// <param name="comparer">An optional comparer for the two the collection items. Useful for comparisons not on Ids.</param>
		/// <param name="fullRemove">Specifies whether to fully remove the items from the collection. Default is false.</param>
		/// <param name="setAsDeleted">Specifies whether to set or clear the IsDeleted flag on the collection to true. Default is true.</param>
		/// <returns>The original collection without matched element in source plus additional elements in source.</returns>
		/// <remarks>The general use for this method is to work around EF need for existing collections with foreign key relationships to be maintained when saving.</remarks>
		public static ICollection<T> Merge<T>(this ICollection<T> collection, ICollection<T> source, Func<T, T, bool> comparer = null, bool fullRemove = false, bool setAsDeleted = true)
			where T : class, IEntity
		{
			if (collection == null && source == null)
				return collection;
			if (collection == null)
				return source;
			if (source == null)
				return collection;

			if (comparer == null) // if comparer is null, use the default entity Equals
				comparer = (c, s) => ((c != null) && c.Equals(s));

			var ec = new EqualityComparer<T>(comparer, (t) => t.GetHashCode());
			var remove = new List<T>(collection.Except(source, ec));
			foreach (var r in remove)
			{
				r.IsDeleted = setAsDeleted;
			}

			var add = source.Except(collection, ec);
			collection.AddRange(add);

			if (fullRemove)
			{
				foreach (var r in remove)
					collection.Remove(r);
			}

			return collection;
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "Collection cannot be null");
			if (action == null)
				throw new ArgumentNullException("action", "Action cannot be null");

			foreach (var item in collection)
			{
				action(item);
			}
		}

		#endregion

		#region IEnumerable methods

		public static IEnumerable<TReturn> Combine<T1, T2, TReturn>(this IEnumerable<T1> source, IEnumerable<T2> second, Func<T1, T2, TReturn> func)
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source cannot be null.");
			if (second == null)
				yield break;

			using (var e1 = source.GetEnumerator())
			using (var e2 = second.GetEnumerator())
			{
				while (e1.MoveNext() && e2.MoveNext())
				{
					var result = func(e1.Current, e2.Current);
					yield return result;
				}
			}
		}

		public static int SumProduct(this IEnumerable<int> source, params IEnumerable<int>[] multipliers)
		{
			if (multipliers == null)
				return default(int);

			Func<int, int, int> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static int? SumProduct(this IEnumerable<int?> source, params IEnumerable<int?>[] multipliers)
		{
			if (multipliers == null)
				return default(int?);

			Func<int?, int?, int?> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static long SumProduct(this IEnumerable<long> source, params IEnumerable<long>[] multipliers)
		{
			if (multipliers == null)
				return default(long);

			Func<long, long, long> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static long? SumProduct(this IEnumerable<long?> source, params IEnumerable<long?>[] multipliers)
		{
			if (multipliers == null)
				return default(long?);

			Func<long?, long?, long?> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static float SumProduct(this IEnumerable<float> source, params IEnumerable<float>[] multipliers)
		{
			if (multipliers == null)
				return default(float);

			Func<float, float, float> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static float? SumProduct(this IEnumerable<float?> source, params IEnumerable<float?>[] multipliers)
		{
			if (multipliers == null)
				return default(float?);

			Func<float?, float?, float?> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static double SumProduct(this IEnumerable<double> source, params IEnumerable<double>[] multipliers)
		{
			if (multipliers == null)
				return default(double);

			Func<double, double, double> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static double? SumProduct(this IEnumerable<double?> source, params IEnumerable<double?>[] multipliers)
		{
			if (multipliers == null)
				return default(double?);

			Func<double?, double?, double?> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static decimal SumProduct(this IEnumerable<decimal> source, params IEnumerable<decimal>[] multipliers)
		{
			if (multipliers == null)
				return default(decimal);

			Func<decimal, decimal, decimal> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		public static decimal? SumProduct(this IEnumerable<decimal?> source, params IEnumerable<decimal?>[] multipliers)
		{
			if (multipliers == null)
				return default(decimal?);

			Func<decimal?, decimal?, decimal?> func = ((one, two) => one * two);
			var products = source;
			foreach (var multiplier in multipliers)
			{
				products = Combine(products, multiplier, func);
			}

			var sum = products.Sum();
			return sum;
		}

		#endregion

		#region Entity methods

		public static List<IEntity> ToEntities<T>(this IEnumerable<T> collection) where T : class, IEntity
		{
			if (collection == null)
				return new List<IEntity>();

			return collection.Cast<IEntity>().ToList();
		}

		public static List<ILookup> ToLookups<T>(this IEnumerable<T> collection) where T : class, ILookup
		{
			if (collection == null)
				return new List<ILookup>();

			return collection.Cast<ILookup>().ToList();
		}

		public static List<ILookup> ToLookups<T>(this IEnumerable<T> collection, string property) where T : class, IEntity
		{
			var casting = new List<ILookup>();
			if (collection == null)
				return casting;

			property = property ?? "Name";
			casting.AddRange(collection.Select(entity => EntityLookup.ToLookup(entity, property)));
			return casting.ToList();
		}

		public static ILookup ToLookup<T>(this T entity) where T : class, IEntity
		{
			if (entity == null)
				return null;

			var lookup = EntityLookup.ToLookup(entity, "Name");
			return lookup;
		}

		public static ILookup ToLookup<T>(this T entity, string property) where T : class, IEntity
		{
			if (entity == null)
				return null;

			property = property ?? "Name";
			var lookup = EntityLookup.ToLookup(entity, property);
			return lookup;
		}

		public static List<ILookup> InsertLookups(this IEnumerable<ILookup> collection, int index, params ILookup[] lookups)
		{
			var inserted = new List<ILookup>();
			if (collection == null)
				collection = new List<ILookup>();

			inserted.AddRange(collection);
			inserted.InsertRange(index, lookups);
			return inserted;
		}

		#endregion

		#region String Methods
		public static string Truncate(this string text, int limit)		
        {		
            if (String.IsNullOrEmpty(text))		
                return text;		
            if (text.Length < limit)		
                return text;		
            return text.Substring(0, limit - 3) + "...";		
        }

		#endregion

		#region Private methods

		private class EqualityComparer<T> : IEqualityComparer<T>
		{
			//http://stackoverflow.com/questions/98033/wrap-a-delegate-in-an-iequalitycomparer
			private readonly Func<T, T, bool> _comparer;
			private readonly Func<T, int> _hash;

			public EqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
			{
				_comparer = comparer;
				_hash = hash;
			}

			public bool Equals(T x, T y)
			{
				return _comparer(x, y);
			}

			public int GetHashCode(T obj)
			{
				return _hash(obj);
			}
		}

		#endregion
	}
}
