using System;
using System.Collections.Generic;
using System.Reflection;

namespace BrightLine.Common.Utility
{
	public static class AttributeHelper
	{
		/// <summary>
		/// Gets all types in the assembly supplied with the specified attribute type on its class
		/// </summary>
		/// <typeparam name="T">The attribute type ot check for</typeparam>
		/// <param name="assemblyName">The assembly to check for the attributes</param>
		/// <returns></returns>
		public static Dictionary<Type, T> GetTypesWithClassAttribute<T>(string assemblyName)
		{
			var assembly = Assembly.Load(assemblyName);
			return GetTypesWithClassAttribute<T>(assembly);
		}


		/// <summary>
		/// Gets all types in the assembly supplied with the specified attribute type on its class
		/// </summary>
		/// <typeparam name="T">The attribute type ot check for</typeparam>
		/// <param name="assembly">The assembly to check for the attributes</param>
		/// <returns></returns>
		public static Dictionary<Type, T> GetTypesWithClassAttribute<T>(Assembly assembly)
		{
			var allTypes = assembly.GetTypes();
			var matches = new Dictionary<Type, T>();
			foreach (var type in allTypes)
			{
				var attributes = type.GetCustomAttributes(typeof(T), false);
				if (attributes.Length <= 0)
					continue;

				matches.Add(type, (T)attributes[0]);
			}

			return matches;
		}


		/// <summary>
		/// Gets all types in the assembly supplied with the specified attribute type on its class
		/// </summary>
		/// <param name="assembly">The assembly to check for the attributes</param>
		/// <param name="typeAttribute">The attribute type ot check for</param>
		/// <returns></returns>
		public static Dictionary<Type, object> GetTypesWithClassAttribute(Assembly assembly, Type typeAttribute)
		{
			var allTypes = assembly.GetTypes();
			var matches = new Dictionary<Type, object>();
			foreach (var type in allTypes)
			{
				var attributes = type.GetCustomAttributes(typeAttribute, false);
				if (attributes.Length <= 0)
					continue;

				matches.Add(type, attributes[0]);
			}

			return matches;
		}


		public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
		{
			if (type == null)
				throw new ArgumentNullException("type");

			var a = type.GetCustomAttribute<TAttribute>();
			return (a != null);
		}

		public static bool HasAttribute(this Type type, Type attribute, bool inherit = true)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (attribute == null)
				throw new ArgumentNullException("attribute");
			//if (!(attribute == typeof(Attribute)))
			//	throw new ArgumentException("attribute must be derived from Attribute", "attribute");

			var a = type.GetCustomAttribute(attribute, inherit);
			return (a != null);
		}
	}
}
