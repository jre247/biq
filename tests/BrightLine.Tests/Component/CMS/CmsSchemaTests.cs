using BrightLine.CMS;
using BrightLine.CMS.Models;
using NUnit.Framework;


namespace BrightLine.Tests.Component.CMS
{
    [TestFixture]
    public class CmsSchemaTests
    {
        [Test]
        public void Can_Build_Model()
        {
            var schema = new AppSchema();

            // 1. Create a sample model of Cell phone products ( e.g. phones with OS( iphone, android ), cost, manufacturer, cost, associatedVideos etc )
            // Use the different data types available and lists/reference to lookups.

            // 2. Just like a normal database table. a model needs to have a schema ( of fields with name, type, length, etc )
            var model = new DataModelSchema("Products");
            model.AddField("name"                , dataType : DataModelPropertyTypes.Text   , isReferenceType: false, refObject : null,           isList: false,  length : 50,   required : true  , defaultValue: "", metaValue: null);
            model.AddField("os"                  , dataType : DataModelPropertyTypes.Text   , isReferenceType: false, refObject : null,           isList: false,  length : 50,   required : true  , defaultValue: "", metaValue: null);
            model.AddField("cost"                , dataType : DataModelPropertyTypes.Number , isReferenceType: false, refObject : null,           isList: false,  length : -1,   required : true  , defaultValue: "", metaValue: null);
            model.AddField("manufacturer"        , dataType : "ref-manufacturer"            , isReferenceType: true,  refObject : "manufacturer", isList: false,  length : 50,   required : false , defaultValue: "", metaValue: null);
            model.AddField("associatedVideos"    , dataType : "list:ref-video"              , isReferenceType: true,  refObject : "video",        isList: true,   length : 50,   required : true  , defaultValue: "", metaValue: null);
            model.AddField("stores"              , dataType : "list:text-50"                , isReferenceType: false, refObject : "text",         isList: true,   length : 50,   required : true  , defaultValue: "", metaValue: null);

            AssertModelFields(model);
        }


        [Test]
        public void Can_Build_Model_With_Raw_Inputs()
        {
            var schema = new AppSchema();

            // 1. Add fields using raw values that would be entered on the spreadsheet.
            var model = new DataModelSchema("Products");
            model.AddFieldRaw("name"                , "text-50"         ,  "required", defaultValue: "", metaValue: null);
            model.AddFieldRaw("os"                  , "text-50"         ,  "required", defaultValue: "", metaValue: null);
            model.AddFieldRaw("cost"                , "number"          ,  "required", defaultValue: "", metaValue: null);
            model.AddFieldRaw("manufacturer"        , "ref-manufacturer",  "optional", defaultValue: "", metaValue: null);
            model.AddFieldRaw("associated videos"   , "list:ref-video"  ,  "required", defaultValue: "", metaValue: null);
            model.AddFieldRaw("stores"              , "list:text-50"    ,  "required", defaultValue: "", metaValue: null);

            AssertModelFields(model);
        }


        private void AssertModelFields(DataModelSchema model)
        {
            // Check the properties were created correctly.
            AssertField(model, "name"            , dataType : DataModelPropertyTypes.Text   , isReferenceType: false, refObject : null,           isList: false,  length : 50,   required : true  , defaultValue: "");
            AssertField(model, "os"              , dataType : DataModelPropertyTypes.Text   , isReferenceType: false, refObject : null,           isList: false,  length : 50,   required : true  , defaultValue: "");
            AssertField(model, "cost"            , dataType : DataModelPropertyTypes.Number , isReferenceType: false, refObject : null,           isList: false,  length : -1,   required : true  , defaultValue: "");
            AssertField(model, "manufacturer"    , dataType : "ref-manufacturer"            , isReferenceType: true,  refObject : "manufacturer", isList: false,   length : 50,   required : false, defaultValue: "");
            AssertField(model, "associatedVideos", dataType : "list:ref-video"              , isReferenceType: true,  refObject : "video",        isList: true,   length : 50,   required : true  , defaultValue: "");
            AssertField(model, "stores"          , dataType: "list:text-50"                 , isReferenceType: false, refObject: "text",          isList: true,   length: 50,    required: true   , defaultValue: "");
        }


        private void AssertField(DataModelSchema model, string field, string dataType, bool isReferenceType, string refObject, bool isList, int length, bool required, string defaultValue)
        {
            Assert.IsTrue(model.HasField(field));
            Assert.IsNotNull(model.GetField(field));
            Assert.AreEqual(dataType, model.GetField(field).DataType);
            Assert.AreEqual(isReferenceType, model.GetField(field).IsRefType);
            Assert.AreEqual(refObject, model.GetField(field).RefObject);
            Assert.AreEqual(isList, model.GetField(field).IsListType);
        }
    }
}
