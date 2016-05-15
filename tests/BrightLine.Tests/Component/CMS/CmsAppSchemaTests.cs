using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BrightLine.Tests.Common;


using BrightLine.CMS;
using BrightLine.CMS.Validators;
using BrightLine.CMS.Serialization;


namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class CmsAppSchemaTests
	{
		[Test]
		public void Can_Check_Lookups()
		{
			var schema = new AppSchema();
			schema.Models.Add(new DataModelSchema() { Name = "products"} );
			schema.Lookups.Add(new DataLookupTable() { Name = "brand" });
			schema.LoadLookups();

			Assert.IsTrue(schema.HasModel("products"));
			Assert.IsTrue(schema.HasModel("PRODUCTS"));
			Assert.IsFalse(schema.HasModel("pages"));
			Assert.AreEqual(schema.GetModel("products").Name, "products");
			Assert.AreEqual(schema.GetModel("PRODUCTS").Name, "products");

			Assert.IsTrue(schema.HasLookup("brand"));
			Assert.IsTrue(schema.HasLookup("BRAND"));
			Assert.IsFalse(schema.HasLookup("category"));
			Assert.AreEqual(schema.GetLookup("brand").Name, "brand");
			Assert.AreEqual(schema.GetLookup("BRAND").Name, "brand");
		}


		[Test]
		public void Can_Load_From_Json()
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get("CMS.Loreal_Schema1.js");
			var appSchema = serializer.DeserializeSchema(content);

			Assert.AreEqual(appSchema.Models.Count, 2);
			Assert.AreEqual(appSchema.Models[0].Fields.Count, 5);
			Assert.AreEqual(appSchema.Models[1].Fields.Count, 18);
			Assert.AreEqual(appSchema.Lookups.Count, 4);
		}


		[Test]
		public void Can_Load_From_Json_And_Validate()
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get("CMS.Loreal_Schema1.js");
			var appSchema = serializer.DeserializeSchema(content);

			Assert.AreEqual(appSchema.Models.Count, 2);
			Assert.AreEqual(appSchema.Models[0].Fields.Count, 5);
			Assert.AreEqual(appSchema.Models[1].Fields.Count, 18);
			Assert.AreEqual(appSchema.Lookups.Count, 4);

			var validator = new AppSchemaValidator(appSchema);
			var result = validator.ValidateSchema();

			Assert.IsTrue(result.Success);
		}


		[Test]
		public void Can_Fail_Validation_With_Wrong_Types()
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get("CMS.Loreal_Schema_Invalid.js");
			var appSchema = serializer.DeserializeSchema(content);
			var validator = new AppSchemaValidator(appSchema);
			var result = validator.ValidateSchema();

			Assert.IsFalse(result.Success);			
		}
	}
}
