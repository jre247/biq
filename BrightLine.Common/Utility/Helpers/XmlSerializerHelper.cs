using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BrightLine.Common.Utility
{
	/// <summary>
	/// Serializes an object to xml.
	/// </summary>
	public class XmlSerializerHelper
	{
		/// <summary>
		/// Serialize the object to xml.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="item">Object to serialize.</param>
		/// <returns>XML contents representing the serialized object.</returns>
		public static string XmlSerialize<T>(T item, string root)
		{
			var rootAttribute = new XmlRootAttribute(root);
			var serializer = new XmlSerializer(typeof(T), rootAttribute);
			var stringBuilder = new StringBuilder();
			using (var writer = new StringWriter(stringBuilder))
			{
				serializer.Serialize(writer, item);
			}

			return stringBuilder.ToString();
		}


		/// <summary>
		/// Serialize the object to xml.
		/// </summary>
		/// <param name="item">Object to serialize.</param>
		/// <returns>XML contents representing the serialized object.</returns>
		public static string XmlSerialize(object item, string root)
		{
			var type = item.GetType();
			var rootAttribute = new XmlRootAttribute(root);
			var serializer = new XmlSerializer(type, rootAttribute);
			var stringBuilder = new StringBuilder();
			using (var writer = new StringWriter(stringBuilder))
			{
				serializer.Serialize(writer, item);
			}
			return stringBuilder.ToString();
		}


		/// <summary>
		/// Deserialize from xml to the appropriate typed object.
		/// </summary>
		/// <typeparam name="T">Type of object to deserialize.</typeparam>
		/// <param name="xmlData">XML contents with serialized object.</param>
		/// <returns>Deserialized object.</returns>
		public static T XmlDeserialize<T>(string xmlData, string root)
		{
			T entity;
			var element = new XmlRootAttribute(root);
			var serializer = new XmlSerializer(typeof(T), element);
			using (var reader = new StringReader(xmlData))
			{
				entity = (T)serializer.Deserialize(reader);
			}

			return entity;
		}
	}
}
