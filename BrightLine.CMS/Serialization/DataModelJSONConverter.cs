using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;

namespace BrightLine.CMS.Serialization
{
	/// <summary>
	/// Converts data model instances to JSON format 
	/// 
	/// EXAMPLE : LOREAL VIDEOS
	/// 
	/// {
	/// 	"active"					: true,
	/// 	"goLiveDate"				: 'new Date(2013, 11, 2, 0, 0, 0, 0)',
	/// 	"brand"						: { "id": 5, "value": "LANC" },
	/// 	"category"					: { "id": 1, "value": "Eyes" },
	/// 	"videoTitle"				: "Do a Smoky Eye",
	/// 	"videoFileName"				: "119 How to do smokey eye makeup.mov",
	/// 	"videoThumbnailFileName"	: "",
	/// 	"videoUrlLink"				: "",
	/// 	"trt"						: "12/31/1899 3:11:00 AM",
	/// 	"associatedProducts"		: "Color Design Palette in Gris Fatale, Artliner, Hypnôse Drama Mascara",
	/// 	"in_house"					: "Color Design Palette, Hypnôse Drama Mascara",
	/// 	"newTag"					: null,
	/// 	"expiresSoonDate"			: null,
	/// 	"season"					: null,
	/// 	"colorFamily"				: null,
	/// 	"socialMessage"				: "",
	/// 	"socialDriverImageFileName"	: ""
	/// },
	/// </summary>
	public class DataModelJSONConverter
	{
		private AppSchema _schema;
		private AppSchemaBasicTypes _basicTypes;
		private DataModels _dataModels;
		private StringBuilder _buffer;
		private DataModelRecords _instances;
		private List<DataModelProperty> _props;
		private string _currentModelName;
		private string _newLine = "";
		private bool _enableNewLines = true;
		private bool _enableTabs = true;

		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="schema"></param>
		public DataModelJSONConverter(AppSchema schema, DataModels dataModels)
		{
			_schema = schema;
			_dataModels = dataModels;
			_basicTypes = new AppSchemaBasicTypes();
			ConfigureNewLines(true);
		}


		public void ConfigureNewLines(bool enable)
		{
			_enableNewLines = enable;
			_enableTabs = enable;
			_newLine = enable ? Environment.NewLine : " ";
		}


		public ElementLookup ElementLookup;


		/// <summary>
		/// Converts a single instance inot a json object.
		/// </summary>
		/// <param name="instances"></param>
		/// <returns></returns>
		public AppDataModelJSON ConvertToDocument(DataModelRecords instances)
		{
			_enableTabs = true;
			_instances = instances;
			_buffer = new StringBuilder();

			// 1. Indent and start collection
			var indent = IndentGet();
			_buffer.Append(indent + "{" + _newLine);
			indent = IndentInc();

			// 2. write out prop "instances:"
			AddPropStringValue("SchemaVersion", _schema.SchemaVersion, true);
			AddPropStringValue("AppVersion", _schema.AppVersion, true);
			AddPropStringValue("ModelName", instances.ModelName, true);

			PropStartCollection("docs", "[");

			var jsonData = ConvertToRecords(instances, true);
			var lastIndex = jsonData.InstancesPublished.Count - 1;
			for (var ndx = 0; ndx < jsonData.InstancesPublished.Count; ndx++)
			{
				// 2. Convert each record.
				var jsonRecord = jsonData.InstancesPublished[ndx];
				_buffer.Append(jsonRecord);

				var separator = ndx != lastIndex ? "," : "";
				separator += _newLine;

				// 3. append separator ( ',' + newline )
				_buffer.Append(separator);
			}
			PropEndCollection("]");

			// 4. done with collection
			indent = IndentDec();
			_buffer.Append(indent + "}" + _newLine);

			var json = _buffer.ToString();
			jsonData.All = json;
			return jsonData;
		}


		/// <summary>
		/// Converts a single instance inot a json object.
		/// </summary>
		/// <param name="instances"></param>
		/// <returns></returns>
		public AppDataModelJSON ConvertToTable(DataModelRecords instances)
		{
			ConfigureNewLines(false);
			_instances = instances;
			var buffer = new StringBuilder();

			var jsonData = ConvertToRecords(instances, true);
			var lastIndex = jsonData.InstancesPublished.Count - 1;
			buffer.Append("[" + Environment.NewLine);
			for (var ndx = 0; ndx < jsonData.InstancesPublished.Count; ndx++)
			{
				// 2. Convert each record.
				var jsonRecord = jsonData.InstancesPublished[ndx];
				buffer.Append(jsonRecord);

				var separator = ndx != lastIndex ? "," : "";
				separator += _newLine;

				// 3. append separator ( ',' + newline )
				buffer.Append(separator);

				buffer.Append(Environment.NewLine);
			}
			buffer.Append(Environment.NewLine + "]");
			var text = buffer.ToString();
			jsonData.All = text;
			return jsonData;
		}


		/// <summary>
		/// Converts a single instance inot a json object.
		/// </summary>
		/// <param name="instances"></param>
		/// <returns></returns>
		public AppDataModelJSON ConvertToRecords(DataModelRecords records, bool serializeRawFormat)
		{
			var jsonData = new AppDataModelJSON();
			jsonData.InstancesPublished = new List<string>();
			jsonData.InstancesRaw = new List<string>();

			_instances = records;
			_currentModelName = records.ModelName;
			var converter = new DataModelJSONPropertyValueBuilder(_schema, _dataModels);

			// 1. Go through all instances.
			for (var ndx = 0; ndx < records.Data.Count; ndx++)
			{
				var record = records.Data[ndx];
				var props = GetFields();

				// 2. Store the published version.				
				var jsonRecord = BuildInstance(records, props, record, converter, true, false);
				jsonData.InstancesPublished.Add(jsonRecord);

				// 3. Store the raw version ( just a table with rows/columns, List<List<object>> which can be used later on the UI )
				if (serializeRawFormat)
				{
					var rawRecord = Newtonsoft.Json.JsonConvert.SerializeObject(record);
					jsonData.InstancesRaw.Add(rawRecord);
				}
			}
			return jsonData;
		}


		private string ConvertNestedObjectRecord(object refIds, DataModelProperty parentProp, DataModelSchema refModelSchema)
		{
			// CASE 1: Single object
			if (refIds == null)
			{
				return "null";
			}

			var refObjectProps = refModelSchema.Fields;
			var refModelRecords = _dataModels.GetModel(refModelSchema.Name).Rows;

			var indent = IndentGet();
			var buffer = new StringBuilder();
			indent = IndentInc();

			var converter = new DataModelJSONPropertyValueBuilder(_schema, _dataModels);

			// CASE 2: single model ref.
			var refKeys = refIds.GetType() == typeof(List<string>)
					 ? (List<string>)refIds
					 : new List<string>() { (string)refIds };

			var lastPropIndex = refObjectProps.Count - 1;
			var lastRefIndex = refKeys.Count - 1;
			//buffer.Append("[" + _newLine );

			for (var ndxKey = 0; ndxKey < refKeys.Count; ndxKey++)
			{
				var key = refKeys[ndxKey];
				var record = refModelRecords.GetRecordByKey(key);
				if (record == null)
				{
					buffer.Append("null");
				}
				else
				{
					var instance = BuildInstance(refModelRecords, refObjectProps, record, converter, false, false);
					buffer.Append(instance);
				}

				if (ndxKey != lastRefIndex)
					buffer.Append(", " + _newLine);
				else
					buffer.Append(_newLine);
			}

			indent = IndentDec();
			//buffer.Append(indent + "]");
			return buffer.ToString();
		}


		/// <summary>
		/// Builds a single instance of a model as a JSON string.
		/// </summary>
		/// <param name="records"></param>
		/// <param name="props"></param>
		/// <param name="record"></param>
		/// <param name="converter"></param>
		/// <returns></returns>
		private string BuildInstance(DataModelRecords records, List<DataModelProperty> props, List<object> record, DataModelJSONPropertyValueBuilder converter, bool enableNestedObject, bool newLineOnNestedObject)
		{
			var buffer = new StringBuilder();
			var indent = IndentGet();

			// 1. Beginning brace of instance.
			buffer.Append(indent + "{" + _newLine);
			indent = IndentInc();

			// 2. meta fields
			var ndxKey = _instances.Schema.IndexOfField("key");
			object key = ndxKey < 0 ? -1 : record[ndxKey];
			var instanceId = 0;
			var modelFormatId = 0;
			var modelTypeId = 0;
			var modelId = 0;

			if (ElementLookup != null)
			{
				var keyAsString = key.ToString();
				instanceId = ElementLookup.GetModelRecordId(records.ModelName, keyAsString);
				var contentModel = ElementLookup.GetContentModel(records.ModelName);
				modelId = contentModel.Id;
				modelFormatId = ElementLookup.GetModelRecordFormatId(records.ModelName, keyAsString);
				modelTypeId = ElementLookup.GetModelRecordTypeId(records.ModelName, keyAsString);
			}
			var meta = indent + "\"_meta\": {  \"instanceId\" : " + instanceId +
					   ", \"modelId\" : " + modelId +
					   ", \"modelFormatId\" : " + modelFormatId +
					   ", \"modelTypeId\" : " + modelTypeId + " }, " + _newLine;
			buffer.Append(meta);


			// 3. Go through each property and build it up.
			var serializableProps = props.Where(prop => CmsRules.CanSerializeProperty(prop)).ToList();
			var lastIndex = serializableProps.Count - 1;

			for (var ndx = 0; ndx < serializableProps.Count; ndx++)
			{
				var prop = serializableProps[ndx];

				// Get the value.
				var objectValue = record[prop.Position];

				// Write out value.
				var val = prop.IsListType
						? BuildArrayPropValues(prop, objectValue as IList, converter, enableNestedObject)
						: BuildSinglePropValue(prop, objectValue, converter, enableNestedObject, newLineOnNestedObject, false);

				// Get the comma/new line at the end of a value.
				var valEnd = ndx != lastIndex ? "," : "";
				valEnd += _newLine;

				// Append propname + value.
				var propName = prop.Name;
				if (propName == "key")
				{
					propName = "_id";
					val = val.Replace("\"", "");
				}
				buffer.Append(indent + "\"" + propName + "\": " + val + valEnd);
			}

			// 4. Ending brace of the instnace.
			indent = IndentDec();
			buffer.Append(indent + "}");
			var json = buffer.ToString();
			return json;
		}


		/// <summary>
		/// Builds an array of property values. e.g. list:string, or list:ref-brand
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="values"></param>
		/// <param name="converter"></param>
		/// <param name="enableNested"></param>
		/// <returns></returns>
		private string BuildArrayPropValues(DataModelProperty prop, IList values, DataModelJSONPropertyValueBuilder converter, bool enableNested)
		{
			if (values == null)
				return "[ ]";

			var arrayVal = "[ ";
			var lastIndex = values.Count - 1;
			for (var ndx = 0; ndx < values.Count; ndx++)
			{
				var item = values[ndx];
				var val = BuildSinglePropValue(prop, item, converter, enableNested, true, true);
				var valEnd = ndx != lastIndex ? "," : "";
				arrayVal += val + valEnd;
			}
			arrayVal += " ]";
			return arrayVal;
		}



		/// <summary>
		/// Builds a single property value.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="objectValue"></param>
		/// <param name="converter"></param>
		/// <param name="enableNested"></param>
		/// <returns></returns>
		private string BuildSinglePropValue(DataModelProperty prop, object objectValue, DataModelJSONPropertyValueBuilder converter, bool enableNested, bool newLineOnNestedObject, bool isArrayItem)
		{
			var val = "";

			// CASE 1: basic type ( number, text-50 )
			if (_basicTypes.HasType(prop.DataType))
			{
				return converter.Build(prop, objectValue);
			}
			// CASE 1: basic type ( number, text-50 )
			else if (isArrayItem && _basicTypes.HasType(prop.RefObject))
			{
				return converter.Build(prop.RefObject, false, prop.RefObject, objectValue);
			}

			// CASE 2: reference: ( ref-brand )
			var refName = prop.RefObject;
			var isModelRef = _schema.HasModel(refName);

			// CASE 2A: null for nested objects.
			if (isModelRef && !enableNested)
			{
				val = objectValue == null ? "null" : converter.Build(DataModelConstants.DataType_Text, false, null, objectValue.ToString());
			}
			// CASE 2B:  model reference ( ref-products)
			else if (isModelRef && enableNested)
			{
				val = "";
				var refModel = _schema.GetModel(refName);
				if (newLineOnNestedObject)
					val = _newLine + IndentGet();

				val += ConvertNestedObjectRecord(objectValue, prop, refModel.Schema);
			}
			// CASE 2C:  lookup value e.g. ( brand, category )
			else if (_schema.HasLookup(refName))
			{
				val = converter.Build(prop, objectValue);
			}
			else
				val = "null";
			return val;
		}


		private List<DataModelProperty> GetFields()
		{
			// Cache the props
			if (_currentModelName == _instances.ModelName && _props != null)
				return _props;

			_currentModelName = _instances.ModelName;
			_props = _schema.GetModelFields(_instances.ModelName, _instances.Columns);
			return _props;
		}



		private void AddPropStringValue(string p1, string p2, bool addComma)
		{
			var indent = IndentGet();
			var valEnd = addComma ? ", " : " ";
			if (_enableNewLines)
				valEnd += Environment.NewLine;

			_buffer.Append(indent + "\"" + p1 + "\": \"" + p2 + "\"" + valEnd);
		}


		#region Indentaiton
		private void PropStartCollection(string name, string collectionVal)
		{
			var indent = IndentGet();
			_buffer.Append(indent + "\"" + name + "\":" + _newLine);
			_buffer.Append(indent + collectionVal + _newLine);
			IndentInc();
		}


		private void PropEndCollection(string collectionVal)
		{
			var indent = IndentDec();
			_buffer.Append(indent + collectionVal + _newLine);
		}


		private int indentLevel = 0;
		private string IndentGet()
		{
			return IndentBuild();
		}


		private string IndentInc()
		{
			indentLevel++;
			return IndentBuild();
		}


		private string IndentDec()
		{
			indentLevel--;
			return IndentBuild();
		}


		private string IndentBuild()
		{
			if (!_enableTabs)
				return " ";

			var indent = "";
			for (var ndx = 0; ndx < indentLevel; ndx++)
			{
				indent += "	";
			}
			return indent;
		}
		#endregion
	}
}