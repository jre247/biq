using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using BrightLine.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;

namespace BrightLine.Core
{
	/// <summary>
	/// Helper class to access entity data in the form of various lookup/dictionaries. e.g. id=>entity, id=>Name etc.
	/// </summary>
	/// TODO: Refactor the EntityLookups class out of existence
	public class EntityLookups
	{
		public string Name { get; set; }
		public IEnumerable<ILookup> Values { get; set; }

		public static JObject ToJObject(EntityLookups[] lookups)
		{
			if (lookups == null)
				return new JObject();

			var json = new JObject();
			foreach (var lookup in lookups)
			{
				json[lookup.Name] = JObject.FromObject(ToLookups(lookup));
			}

			return json;
		}

		#region Private Methods

		private static JObject ToLookups(EntityLookups entityLookups)
		{
			var collection = entityLookups.Values;

			if (collection == null || !collection.Any())
				return null;

			var lookups = from l in collection
						  let item = new { id = l.Id, name = l.Name }
						  let lookup = JObject.FromObject(item)
						  orderby l.Name ascending
						  select new JProperty(l.Id.ToString(CultureInfo.InvariantCulture), lookup);
			var json = new JObject(lookups.ToArray());
			return json;
		}

		
		#endregion
	}
}