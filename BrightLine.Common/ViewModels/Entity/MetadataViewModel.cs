using BrightLine.Common.Utility;
using BrightLine.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Entity
{
	public class MetadataViewModel
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public static JObject ToJObject(IEnumerable<MetadataViewModel> meta)
		{
			var json = new JObject();
			foreach (var m in meta)
			{
				var key = m.Name;
				var content = JObject.FromObject(m);
				var jo = new JProperty(key, content);
				json.Add(jo);
			}

			return json;
		}

		public static IEnumerable<MetadataViewModel> GetModels()
		{
			var types = ReflectionHelper.GetLoadedTypes("BrightLine.Common").Where(t => !t.FullName.StartsWith("System.Data.Entity.DynamicProxies"));
			var models = types.Where(t => ReflectionHelper.HasInterface<IEntity>(t) && !ReflectionHelper.HasInterface<ILookup>(t));
			var meta = models.Select(m => new MetadataViewModel()
			{
				Name = m.Name,
				Description = ReflectionHelper.HasAttribute<DescriptionAttribute>(m) ?
					ReflectionHelper.TryGetAttribute<DescriptionAttribute>(m).Description :
					m.Name
			});

			return meta;
		}

		public static IEnumerable<MetadataViewModel> GetLookups()
		{
			var types = ReflectionHelper.GetLoadedTypes("BrightLine.Common").Where(t => !t.FullName.StartsWith("System.Data.Entity.DynamicProxies"));
			var models = types.Where(t => ReflectionHelper.HasInterface<ILookup>(t));
			var meta = models.Select(m => new MetadataViewModel()
			{
				Name = m.Name,
				Description = ReflectionHelper.HasAttribute<DescriptionAttribute>(m) ?
					ReflectionHelper.TryGetAttribute<DescriptionAttribute>(m).Description :
					m.Name
			});

			return meta;
		}
	}
}
