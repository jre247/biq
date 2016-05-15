using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Models;
using NUnit.Framework;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
    public class CmsAppImporterPropertyMetadataTests
    {
        private void AssertValues(DataModelProperty prop, string propName, bool required, bool isListType, bool isRefType, string dataType, int length, string refObject)
        {
            Assert.AreEqual(propName, prop.Name);
            Assert.AreEqual(dataType, prop.DataType);

            if(length > 0)
                Assert.AreEqual(length, prop.MaxLength);

            Assert.AreEqual(isListType, prop.IsListType);
            Assert.AreEqual(isRefType, prop.IsRefType);
            Assert.AreEqual(required, prop.Required);
            Assert.AreEqual(refObject, prop.RefObject);
        }


        [Test]
	    public void Can_Build_Property_Of_Basic_Type_Text()
        {
            // Check text ( with char limit )
	        var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "FEATURE", "text-50", "optional", "", null);
	        AssertValues(prop, "feature", false, false, false, DataModelConstants.DataType_Text, 50, null);
	    }


	    [Test]
	    public void Can_Build_Property_Of_Basic_Type_Bool()
	    {
	        // Check bool
	        var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "IS ACTIVE", "true/false", "required", "", null);
            AssertValues(prop, "isActive", true, false, false, DataModelConstants.DataType_Bool, -1, null);
	    }


	    [Test]
	    public void Can_Build_Property_Of_Basic_Type_Number()
	    {
	        // Check number
	        var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "TOTAL RATINGS", "number", "required", "", null);
            AssertValues(prop, "totalRatings", true, false, false, DataModelConstants.DataType_Number, -1, null);
	    }


        [Test]
        public void Can_Build_Property_Of_Basic_Type_DateTime()
        {
	        // Check datetime
            var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "ACTIVATION DATE", "datetime", "required", "", null);
            AssertValues(prop, "activationDate", true, false, false, DataModelConstants.DataType_Date, -1, null);
		}


		[Test]
		public void Can_Build_Property_Of_Refobjects()
		{
			var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "CATEGORY NAME", "ref-category", "required", "", null);
            AssertValues(prop, "categoryName", true, false, true, "ref-category", -1, "category");
		}


		[Test]
		public void Can_Build_List_Of_Refobjects()
		{
			var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "FEATURE NAME", "list:ref-feature", "required", "", null);
            AssertValues(prop, "featureName", true, true, true, "list:ref-feature", -1, "feature");
		}


		[Test]
		public void Can_Build_List_Of_Basic_Types()
		{
			var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "PLACES", "list:text", "required", "", null);
            AssertValues(prop, "places", true, true, false, "list:text", -1, "text");
		}


		[Test]
		public void Can_Build_List_Of_Text_With_Char_Limits()
		{
			var prop = new DataModelProperty();
            AppImporterHelper.SetupPropertyMetadata("Products", prop, "TAGS", "list:text-50", "required", "", null);
            AssertValues(prop, "tags", true, true, false, "list:text-50", 50, "text");
		}
	}
}
