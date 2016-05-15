using BrightLine.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace BrightLine.Web
{
	public class PropertyContractResolver : DefaultContractResolver
	{
		private Property _objectProperties;
		private readonly List<Property> _properties;
		private readonly bool _isCamel;
		private readonly bool _useDefault;

		public PropertyContractResolver(bool isCamel, IEnumerable<string> properties = null)
		{
			_isCamel = isCamel;
			if (properties == null || !properties.Any())
				_useDefault = true;
			else
			{
				var props = properties.Select(ps => new Property(ps));
				_properties = new List<Property>(props);
				_objectProperties = new Property(from ps in _properties select ps.Name);
			}
		}

		protected override List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			var isEnumerable = objectType != typeof(string) && (objectType.IsArray || ReflectionHelper.HasInterface<IEnumerable>(objectType));
			if (_useDefault || isEnumerable)
				return base.GetSerializableMembers(objectType);

			if (_objectProperties != null)
			{
				foreach (var p in _properties)
				{
					var mi = ReflectionHelper.TryGetMemberInfo(objectType, p.Name);
					if (mi == null)
						continue;
					if (mi.MemberType == MemberTypes.Property)
						p.Type = ((PropertyInfo)mi).PropertyType;
					if (mi.MemberType == MemberTypes.Field)
						p.Type = ((FieldInfo)mi).FieldType;
				}

				var ms = GetMembers(objectType, _objectProperties);
				_objectProperties = null;
				return ms;
			}

			var property = _properties.FirstOrDefault(p => p.IsOfType(objectType));
			if (property == null)
			{
				var ms = GetBasicMembers(objectType);
				return ms;
			}

			if (!property.Nested.Any())
				return base.GetSerializableMembers(objectType);

			var members = GetMembers(objectType, property);
			return members;
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var serializableMembers = this.GetSerializableMembers(type);
			if (serializableMembers == null)
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			var propertyCollection = new JsonPropertyCollection(type);
			foreach (var member in serializableMembers)
			{
				var property = this.CreateProperty(member, memberSerialization);
				if (property != null)
					propertyCollection.AddProperty(property);
			}

			return propertyCollection.OrderBy(p => p.Order ?? -1).ToList();
		}

		protected override string ResolvePropertyName(string propertyName)
		{
			return (_isCamel) ? ToCamelCase(propertyName) : propertyName;
		}

		//protected override JsonObjectContract CreateObjectContract(Type objectType)
		//{
		//	return base.CreateObjectContract(objectType);
		//}

		protected override JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			if (DateTimeJsonConverter.Default.CanConvert(objectType))
				return DateTimeJsonPrimitiveContract.Primitive;

			return base.CreatePrimitiveContract(objectType);
		}

		private static List<MemberInfo> GetMembers(Type objectType, Property property)
		{
			var members = new List<MemberInfo>();
			for (var index = 0; index < property.Nested.Count(); index++)
			{	//TODO: check access for properties for json serialization?
				var p = property.Nested[index];
				var mi = ReflectionHelper.TryGetMemberInfo(objectType, p);
				if (mi == null)
					continue;

				members.Add(mi);
			}

			return members;
		}

		private static List<MemberInfo> GetBasicMembers(Type objectType)
		{
			var members = new List<MemberInfo>();
			var properties = new[] { "id", "name", "display", "shortdisplay" };
			foreach (var property in properties)
			{	//TODO: check access for properties for json serialization?
				var mi = ReflectionHelper.TryGetProperty(objectType, property);
				if (mi == null)
					continue;

				members.Add(mi);
			}

			return members;
		}

		private static string ToCamelCase(string name)
		{
			if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
				return name;
			var str = char.ToLower(name[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
			if (name.Length > 1)
				str = str + name.Substring(1);
			return str;
		}

		private class Property
		{
			public Property(string ps)
			{
				var full = ps.Trim().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				Name = full[0];
				Nested = full.Skip(1).ToArray();
			}

			public Property(IEnumerable<string> properties)
			{
				Name = null;
				Nested = properties.ToArray();
			}

			public string Name { get; private set; }
			public string[] Nested { get; private set; }
			public Type Type { get; set; }

			internal bool IsOfType(Type objectType)
			{
				if (objectType == null)
					return false;

				var thisType = this.Type;

				if (thisType == null)
					return false;

				if (objectType == thisType)
					return true;

				var objectIsEnumerable = objectType != typeof(string) && (objectType.IsArray || ReflectionHelper.HasInterface<IEnumerable>(objectType));
				if (objectIsEnumerable)
					objectType = objectType.GetGenericArguments()[0];

				var thisIsEnumerable = thisType != typeof(string) && (thisType.IsArray || ReflectionHelper.HasInterface<IEnumerable>(thisType));
				if (thisIsEnumerable)
					thisType = thisType.GetGenericArguments()[0];

				if (thisType == null || objectType == null)
					return false;

				if (thisType.FullName.Contains("System.Data.Entity.DynamicProxies"))
					thisType = thisType.BaseType;

				if (objectType.FullName.Contains("System.Data.Entity.DynamicProxies"))
					objectType = objectType.BaseType;

				return objectType == thisType;
			}
		}

		private class DateTimeJsonPrimitiveContract : JsonPrimitiveContract
		{
			private static DateTimeJsonPrimitiveContract _primitive;
			private static DateTimeJsonPrimitiveContract _nullable;
			private static object _primitiveLock = new object();
			private static object _nullableLock = new object();

			private DateTimeJsonPrimitiveContract(Type type)
				: base(type)
			{
				Converter = DateTimeJsonConverter.Default;
			}

			public static DateTimeJsonPrimitiveContract Primitive
			{
				get
				{
					if (_primitive == null)
						lock (_primitiveLock)
							if (_primitive == null)
								_primitive = new DateTimeJsonPrimitiveContract(typeof(DateTime));

					return _primitive;
				}
			}
			public static DateTimeJsonPrimitiveContract Nullable
			{
				get
				{
					if (_nullable == null)
						lock (_nullableLock)
							if (_nullable == null)
								_nullable = new DateTimeJsonPrimitiveContract(typeof(DateTime?));

					return _nullable;
				}
			}
		}

		public class DateTimeJsonConverter : JsonConverter
		{
			private readonly Type[] _types = new Type[] { typeof(DateTime), typeof(DateTime?) };
			private static DateTimeJsonConverter _default;
			private static object _lock = new object();

			private DateTimeJsonConverter()
			{ }

			public static DateTimeJsonConverter Default
			{
				get
				{
					if (_default == null)
						lock (_lock)
							if (_default == null)
								_default = new DateTimeJsonConverter();

					return _default;
				}
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				var formatted = DateHelper.ToString((DateTime?)value);
				serializer.Serialize(writer, formatted);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				if (reader.TokenType == JsonToken.None)
					return null;

				if (!reader.Read())
					return null;
				if (!reader.Read())
					return null;

				var dt = (DateTime)serializer.Deserialize(reader, typeof(DateTime));
				return dt;
			}

			public override bool CanConvert(Type objectType)
			{
				return _types.Any(t => t == objectType);
			}
		}
	}
}
