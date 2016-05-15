using BrightLine.Common.Core;
using BrightLine.Core;
using BrightLine.Core.Attributes;
using BrightLine.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace BrightLine.Common.Utility
{
	public class ReflectionHelper
	{
		private const string COMMON_NAMESPACE = "BrightLine.Common.Models.";
		private const string COMMON_DLL = "BrightLine.Common";

		/// <summary>
		/// Attempts to set all properties on the object instance with values from a json object.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="data"></param>
		public static bool TryCast<T>(object data, out T instance) where T : class, new()
		{
			instance = default(T);
			if (data == null)
				return false;

			try
			{
				instance = new T();
				var type = instance.GetType();
				var pis = type.GetProperties();
				foreach (var pi in pis)
				{
					var value = ReflectionHelper.TryGetValue(data, pi.Name);
					ReflectionHelper.TrySetValue(instance, pi, value);
				}
			}
			catch { return false; }

			return true;
		}

		/// <summary>
		/// Attempts to set all properties on the object instance with values from a json object.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="data"></param>
		public static void TrySetProperties(object instance, JObject data)
		{
			if (instance == null || data == null)
				return;

			var type = instance.GetType();
			var pis = type.GetProperties();
			foreach (var kv in data)
			{
				var key = kv.Key;
				var pi = pis.FirstOrDefault(p => p.Name.ToLower() == key.ToLower());
				ReflectionHelper.TrySetValue(instance, pi, kv.Value);
			}

			return;
		}

		/// <summary>
		/// Attempts to set all properties on the object instance with name/value pairs from the collection.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="nvc"></param>
		public static void TrySetProperties(object instance, NameValueCollection nvc)
		{
			if (instance == null || nvc == null)
				return;

			var type = instance.GetType();
			var pis = type.GetProperties();
			foreach (string key in nvc.Keys)
			{
				if (string.IsNullOrWhiteSpace(key))
					continue;

				var pi = pis.FirstOrDefault(p => p.Name.ToLower() == key.ToLower());

				ReflectionHelper.TrySetValue(instance, pi, nvc[key]);
			}

			return;
		}

		/// <summary>
		/// Tries to set the value of the property.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TrySetValue(object instance, string property, object value)
		{
			if (instance == null || string.IsNullOrWhiteSpace(property))
				return false;

			var type = instance.GetType();
			var pi = TryGetProperty(type, property);
			var set = false;
			if (pi != null)
				set = TrySetValue(instance, pi, value);
			else
			{
				var fi = TryGetField(type, property);
				if (fi != null)
					set = TrySetValue(instance, fi, value);
			}

			return set;
		}

		/// <summary>
		/// Tries to set the value of the field.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TrySetValue(object instance, FieldInfo field, object value)
		{
			var set = false;
			if (instance == null || field == null)
				return set;

			try
			{
				var currentValue = field.GetValue(instance);
				var fieldType = field.FieldType;
				var castValue = TryCastValue(value, fieldType, currentValue);
				field.SetValue(instance, castValue);
				set = true;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return set;
		}

		/// <summary>
		/// Tries to set the value of the property.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool TrySetValue(object instance, PropertyInfo property, object value)
		{
			var set = false;
			if (instance == null || property == null || property.SetMethod == null)
				return set;

			try
			{
				object currentValue = null;
				var getMethod = property.GetGetMethod(true);
				if (getMethod != null)
					currentValue = getMethod.Invoke(instance, new object[] { });
				var propertyType = property.PropertyType;
				var castValue = TryCastValue(value, propertyType, currentValue);
				var setMethod = property.GetSetMethod(true);
				if (setMethod != null)
				{
					setMethod.Invoke(instance, new[] { castValue });
					set = true;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return set;
		}

		/// <summary>
		/// Attempts to cast the value provided to the target type.
		/// </summary>
		/// <param name="value">The value to cast.</param>
		/// <param name="targetType">The type the value should be coerced into.</param>
		/// <param name="currentValue">The current value of the property. Needed for EF and lists to be change tracked.</param>
		/// <returns>An object of type targetType if cast successful or null if unsuccessful.</returns>
		public static object TryCastValue(object value, Type targetType, object currentValue = null)
		{
			if (value == null || targetType == null)
				return value;

			object castValue = null;
			try
			{
				var stringValue = value.ToString();
				if (targetType == typeof(string))
					castValue = stringValue;
				else if (string.IsNullOrEmpty(stringValue) == false)
				{
					if (targetType.IsEnum)
						castValue = Enum.Parse(targetType, stringValue, true);
					else if (targetType == typeof(Guid))
						castValue = new Guid(stringValue);
					else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						var genericType = targetType.GetGenericArguments()[0];
						castValue = TryCastValue(value, genericType);
					}
					else if (targetType == typeof(bool))
						castValue = (stringValue == "1" || stringValue.ToLower() == "true");
					else if (targetType.GetInterface("IEntity") != null)
					{
						if (value is IEntity)
							castValue = value;
						else
						{
							int id;
							if (value.GetType() == typeof(JObject))
								id = (int)(((JObject)value)["Id"] ?? ((JObject)value)["id"]);
							else
								int.TryParse(stringValue, out id);

							var em = EntityManager.GetManager(targetType);
							castValue = em.GetOrNew(id);
						}
					}
					else if (targetType == typeof(ILookup))
					{
						int id;
						if (value.GetType() == typeof(JObject))
							id = (int)(((JObject)value)["Id"] ?? ((JObject)value)["id"]);
						else
							int.TryParse(stringValue, out id);

						castValue = new EntityLookup(id, null);
					}
					else if (targetType.GetInterface("IEnumerable") != null)
					{
						var listType = targetType.GetGenericArguments()[0];
						var add = targetType.GetMethod("Add");
						var clear = targetType.GetMethod("Clear");
						if (currentValue == null)
						{
							var enumerableType = typeof(List<>).MakeGenericType(new Type[] { listType });
							currentValue = Activator.CreateInstance(enumerableType);
						}

						clear.Invoke(currentValue, new object[] { });
						if (value.GetType() == typeof(JArray))
						{
							var items = (JArray)value;
							var em = EntityManager.GetManager(listType);
							for (var index = 0; index < items.Count; index++)
							{
								var item = items[index];
								int id = 0;
								if (item is JObject)
								{
									var jo = (JObject)item;
									id = (int)(jo["Id"] ?? jo["id"]);
									var jObject = em.GetOrNew(id);
									ReflectionHelper.TrySetProperties(jObject, jo);
									if (jObject != null)
										add.Invoke(currentValue, new[] { jObject });
									continue;
								}

								if (item is JToken)
									id = (int)((JValue)item);

								var v = em.GetOrNew(id);
								if (v != null)
									add.Invoke(currentValue, new[] { v });
							}
						}
						else
						{
							foreach (var item in stringValue.Split(',')) // if the value to be set is a list, assume comma-separated ids.
							{
								var itemValue = TryCastValue(item, listType);
								if (itemValue != null)
									add.Invoke(currentValue, new[] { itemValue });
							}
						}
						castValue = currentValue;
					}
					else
						castValue = Convert.ChangeType(stringValue, targetType);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return castValue;
		}

		/// <summary>
		/// Attempts to cast the value provided to the target type.
		/// </summary>
		/// <param name="value">The value to cast.</param>
		/// <param name="currentValue">The current value of the property. Needed for EF and lists to be change tracked.</param>
		/// <returns>An object of type T if cast successful or default(T) if unsuccessful.</returns>
		public static T TryCastValue<T>(object value, object currentValue = null)
		{
			var v = ReflectionHelper.TryCastValue(value, typeof(T), currentValue);
			try { return (T)v; }
			catch { return default(T); }
		}

		/// <summary>
		/// Attempts to get the value of the property.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static object TryGetValue(object instance, string name)
		{
			object value = null;
			if (instance == null || string.IsNullOrWhiteSpace(name))
				return value;

			var type = instance.GetType();
			var pi = TryGetProperty(type, name);
			if (pi != null)
				value = TryGetValue(instance, pi);
			else
			{
				var fi = TryGetField(type, name);
				if (fi != null)
					value = TryGetValue(instance, fi);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the value of the field.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		public static object TryGetValue(object item, FieldInfo field)
		{
			object value = null;
			if (item == null || field == null)
				return value;

			try
			{
				value = field.GetValue(item);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the value of the property.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static object TryGetValue(object item, PropertyInfo property)
		{
			object value = null;
			if (item == null || property == null || property.GetMethod == null)
				return value;

			try
			{
				value = property.GetValue(item);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the value of the property.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T TryGetValue<T>(object instance, string name)
		{
			T value = default(T);
			if (instance == null || string.IsNullOrWhiteSpace(name))
				return value;

			var type = instance.GetType();
			var pi = TryGetProperty(type, name);
			if (pi != null)
				value = TryGetValue<T>(instance, pi);
			else
			{
				var fi = TryGetField(type, name);
				if (fi != null)
					value = TryGetValue<T>(instance, fi);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the value of the field.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		public static T TryGetValue<T>(object item, FieldInfo field)
		{
			T value = default(T);
			if (item == null || field == null)
				return value;

			try
			{
				value = (T)field.GetValue(item);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the value of the property.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static T TryGetValue<T>(object item, PropertyInfo property)
		{
			T value = default(T);
			if (item == null || property == null || property.GetMethod == null)
				return value;

			try
			{
				value = (T)property.GetValue(item);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			return value;
		}

		/// <summary>
		/// Attempts to get the string value of the property.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string TryGetStringValue(object item, string name)
		{
			var value = ReflectionHelper.TryGetValue(item, name);
			return (value == null) ? null : value.ToString();
		}

		/// <summary>
		/// Attempts to get the string value of the property.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="field"></param>
		/// <returns></returns>
		public static string TryGetStringValue(object item, FieldInfo field)
		{
			var value = ReflectionHelper.TryGetValue(item, field);
			return (value == null) ? null : value.ToString();
		}

		/// <summary>
		/// Attempts to get the string value of the property.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static string TryGetStringValue(object item, PropertyInfo property)
		{
			var value = ReflectionHelper.TryGetValue(item, property);
			return (value == null) ? null : value.ToString();
		}


		public static MemberInfo TryGetMemberInfo(Type type, string memberName)
		{
			var mi = ReflectionHelper.TryGetProperty(type, memberName) as MemberInfo ??
					ReflectionHelper.TryGetField(type, memberName) as MemberInfo;
			return mi;
		}


		public static List<string> GetPropertyTypes(object p, List<string> properties)
		{
			var types = new List<string>();
			foreach (var property in properties)
			{
				var type = p.GetType();
				var prop = TryGetProperty(type, property);
				if (prop == null)
					continue;

				types.Add(prop.PropertyType.Name.ToLower());
			}
			return types;
		}


		public static Type GetPropertyOrGenericType(object p, string property)
		{
			if (string.IsNullOrWhiteSpace(property))
				return null;

			var pType = p.GetType();
			var prop = TryGetProperty(pType, property);
			if (prop == null)
				return null;

			var type = prop.PropertyType;
			if (type.GetInterface("IEnumerable", true) != null)
				type = type.GetGenericArguments()[0];

			return type;
		}


		public static PropertyInfo TryGetProperty(Type type, string property)
		{
			if (type == null || string.IsNullOrWhiteSpace(property))
				return null;

			var pis = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			var pi = pis.FirstOrDefault(p => p.Name.ToLower() == property.ToLower());
			return pi;
		}


		public static bool HasProperty(object data, string property)
		{
			if (data == null)
				return false;

			var type = data.GetType();
			return (TryGetProperty(type, property) != null);
		}


		/// <summary>
		/// Gets a list of property info objects for each property name in the list supplied
		/// </summary>
		/// <param name="type"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		public static List<PropertyInfo> GetProperties(Type type, List<string> properties)
		{
			var types = new List<PropertyInfo>();
			foreach (var property in properties)
			{
				var prop = TryGetProperty(type, property);
				types.Add(prop);
			}
			return types;
		}


		public static FieldInfo TryGetField(Type type, string field)
		{
			if (type == null || string.IsNullOrWhiteSpace(field))
				return null;

			var fis = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			var fi = fis.FirstOrDefault(p => p.Name.ToLower() == field.ToLower());
			return fi;
		}


		public static bool HasField(object data, string property)
		{
			if (data == null)
				return false;

			var type = data.GetType();
			return (TryGetField(type, property) != null);
		}


		public static Type TryGetType(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			// check for the common model assembly
			var fqt = string.Format("{0}{1}, {2}", COMMON_NAMESPACE, name, COMMON_DLL);
			var type = Type.GetType(fqt, throwOnError: false, ignoreCase: true);
			if (type == null)
			{
				var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.ToLower().Contains("brightline"));
				var types = (from assembly in assemblies
							 let ts = assembly.GetTypes()
							 from t in ts
							 where t.FullName.Contains(name)
							 select t).ToList();

				if (types.Count > 1)
					throw new AmbiguousMatchException("Ambiguous match on type name: " + name + ".");

				type = types.FirstOrDefault();

				if (type == null)
					throw new TypeLoadException("Type: " + name + " could not be loaded.");
			}

			return type;
		}


		public static Assembly TryGetAssembly(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.ToLower().Contains(name));
			return assembly;
		}


		public static Assembly TryGetAssemblyFromType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetTypes().Contains(type));
			return assembly;
		}


		public static List<Type> GetLoadedTypes(string assembly = null, bool excludeSystemTypes = true)
		{
			var assemblies = excludeSystemTypes
								 ? AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName.Contains("System"))
								 : AppDomain.CurrentDomain.GetAssemblies();
			if (!string.IsNullOrWhiteSpace(assembly))
				assemblies = assemblies.Where(a => a.FullName.ToLower().Contains(assembly.ToLower())).ToArray();

			var types = (from a in assemblies
						 let ts = a.GetTypes()
						 from t in ts
						 select t).ToList();

			return types;
		}


		public static Attribute TryGetAttribute(Type type, Type attributeType)
		{
			if (type == null || attributeType == null)
				return null;

			var cas = type.GetCustomAttributes(attributeType, true);
			if (cas.Length == 0)
				return null;

			return (Attribute)cas[0];
		}


		public static Attribute TryGetAttribute(PropertyInfo propertyInfo, Type attributeType)
		{
			if (propertyInfo == null || attributeType == null)
				return null;

			var cas = propertyInfo.GetCustomAttributes(attributeType, true);
			if (cas.Length == 0)
				return null;

			return (Attribute)cas[0];
		}


		public static TAttribute TryGetAttribute<T, TAttribute>() where TAttribute : Attribute
		{
			var cas = typeof(T).GetCustomAttributes(typeof(TAttribute), true);
			if (cas == null || cas.Length == 0)
				return null;

			return (TAttribute)cas[0];
		}


		public static TAttribute TryGetAttribute<TAttribute>(Type type) where TAttribute : Attribute
		{
			if (type == null)
				return null;

			var cas = type.GetCustomAttributes(typeof(TAttribute), true);
			if (cas.Length == 0)
				return null;

			return (TAttribute)cas[0];
		}


		public static TAttribute TryGetAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
		{
			if (memberInfo == null)
				return null;

			var cas = memberInfo.GetCustomAttributes(typeof(TAttribute), true);
			if (cas.Length == 0)
				return null;

			return (TAttribute)cas[0];
		}


		public static bool HasAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
		{
			if (memberInfo == null)
				return false;

			var att = memberInfo.GetCustomAttributes<TAttribute>();
			return (att != null && att.Any());
		}


		public static bool HasInterface<TInterface>(Type type)
		{
			if (type == null)
				return false;

			if (typeof(TInterface) == type)
				return true;

			var @interface = type.GetInterface(typeof(TInterface).Name, true);
			return (@interface != null);
		}


		/// <summary>
		/// Deep copy from the original entity.
		/// Only entities with EntityAttribute.AllowCopy set are valid.
		/// Only properties marked EntityEditorAttribute.CopyProperty set are copied.
		/// </summary>
		/// <param name="original">The entity to copy.</param>
		/// <returns>A deep copy of the original entity.</returns>
		public static IEntity Copy(IEntity original)
		{
			if (original == null)
				return null;

			//var clone = Clone(original);
			var copy = MakeNew(original);
			return copy;
		}


		#region Private methods

		private static IEntity Clone(IEntity original)
		{
			var serialized = Serialize(original);
			var clone = Deserialize(serialized);
			return clone;
		}


		private static IEntity Deserialize(byte[] array)
		{
			if (array == null)
				return null;

			object obj;
			using (var ms = new MemoryStream(array))
			{
				var bf = new BinaryFormatter();
				obj = bf.Deserialize(ms);
			}

			return (IEntity)obj;
		}


		private static byte[] Serialize(IEntity graph)
		{
			if (graph == null)
				return null;

			byte[] array;
			using (var ms = new MemoryStream())
			{
				var bf = new BinaryFormatter();
				bf.Serialize(ms, graph);
				array = ms.ToArray();
			}

			return array;
		}


		private static IEntity MakeNew(IEntity old)
		{
			if (old == null)
				return null;

			var newer = old;
			newer.Id = 0;
			newer.DateCreated = DateTime.UtcNow;
			newer.DateUpdated = null;
			newer.DateDeleted = null;
			newer.IsDeleted = false;

			var type = newer.GetType();
			var pis = type.GetProperties().Where(p =>
			{
				var eea = p.GetCustomAttribute<EntityEditorAttribute>();
				return (eea != null && eea.CopyProperty);
			});

			foreach (var pi in pis)
			{
				var pType = pi.PropertyType;
				var value = pi.GetValue(old);
				if (pType.GetInterface("IEntity", true) != null)
					value = MakeNew(value as IEntity);
				else if (pType.IsArray || pType.GetInterface("ICollection") != null)
					value = ReflectionHelper.MakeNewCollection(value as ICollection);

				ReflectionHelper.TrySetValue(newer, pi, value);
			}
			return newer;
		}


		private static IEnumerable MakeNewCollection(ICollection collection)
		{
			if (collection == null)
				return null;

			var type = collection.GetType();
			var listType = type.GetGenericArguments()[0];
			var isEntity = (listType.GetInterface("IEntity") != null);

			var array = new object[collection.Count];
			for (var index = 0; index < collection.Count; index++)
			{
				var item = array[index];
				array[index] = isEntity ? MakeNew(item as IEntity) : TryCastValue(item, listType);
			}

			return collection;
		}

		#endregion
	}
}
