﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BrightLine.Tests.Common;


using BrightLine.CMS;
using BrightLine.CMS.Serialization;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CmsAppDataSerializerTests
	{
		[Test]
		public void Can_Load_From_Json()
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get("CMS.Loreal_Data.js");
			var data = serializer.DeserializeData(content);
			data.LoadLookup();

			var schemaContent = ResourceLoader.Get("CMS.Loreal_Schema1.js");
			var schema = serializer.DeserializeSchema(schemaContent);
			schema.LoadLookups();

			var converter = new DataModelJSONConverter(schema, data);
			var json = converter.ConvertToDocument(data.GetModel("products"));
			Console.WriteLine(json);
		}
	}
}
