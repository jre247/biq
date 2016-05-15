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
	public class CmsAppDataValidatorTests
	{
		[Test]
		public void Can_Load_From_Json()
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get("CMS.Loreal_Data.js");
			var data = serializer.DeserializeData(content);

			Assert.IsTrue(data != null);
			Assert.IsTrue(data.ModelData != null);
			Assert.IsTrue(data.ModelData.Count == 2);
			Assert.IsTrue(data.ModelData[0].ModelName == "videos");
			Assert.IsTrue(data.ModelData[1].ModelName == "products");
		}


		[Test]
		public void Can_Validate_Data()
		{
			Validate("CMS.Loreal_Data.js", true);
		}


		[Test]
		public void Can_Fail_Validation_With_Wrong_Column_Counts()
		{
			Validate("CMS.Loreal_Data_Invalid_Wrong_Column_Counts.js", false);
		}


		[Test]
		public void Can_Fail_Validation_With_Wrong_Lookup_Value()
		{
			Validate("CMS.Loreal_Data_Invalid_Wrong_Lookup_Value.js", false);
		}


		[Test]
		public void Can_Fail_Validation_With_Wrong_Model_Key()
		{
			Validate("CMS.Loreal_Data_Invalid_Wrong_Model_Key.js", false);
		}


		[Test]
		public void Can_Fail_Validation_With_Wrong_DataType_For_Value()
		{
			Validate("CMS.Loreal_Data_Invalid_Wrong_DataType.js", false);
		}


		private void Validate(string pathToDataFile, bool success)
		{
			var serializer = new AppDataSerializer();
			var content = ResourceLoader.Get(pathToDataFile);
			var data = serializer.DeserializeData(content);
			var schemaContent = ResourceLoader.Get("CMS.Loreal_Schema1.js");
			var schema = serializer.DeserializeSchema(schemaContent);

			var validator = new DataModelInstanceValidator(schema, data);
			var result = validator.ValidateModels();

			Assert.IsTrue(result.Success == success);
		}
	}
}
