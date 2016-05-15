using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;

namespace BrightLine.CMS.Serialization
{
	public class DataModelJSONPropertyValueBuilder
	{
		private AppSchema _schema;
		private DataModels _dataModels;
		private AppSchemaBasicTypes _basicTypes;
		private DataModelProperty _prop;


		public DataModelJSONPropertyValueBuilder(AppSchema schema, DataModels dataModels)
		{
			_schema = schema;
			_dataModels = dataModels;
			_basicTypes = new AppSchemaBasicTypes();
		}


		/// <summary>
		/// Builds up a JSON value for property and it's raw value supplied.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public string Build(DataModelProperty prop, object val)
		{
			_prop = prop;
			return Build(prop.DataType, prop.IsRefType, prop.RefObject, val);
		}


		/// <summary>
		/// Overloaded method to explicity pass in information about type to build json property value.
		/// NOTE: This is useful for arrays when you explicitly set the type of the array item.
		/// </summary>
		/// <param name="dataType"></param>
		/// <param name="isRefType"></param>
		/// <param name="refObject"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public string Build(string dataType, bool isRefType, string refObject, object val)
		{
			if (val == null)
			{ 
				if(dataType == DataModelConstants.DataType_Text)
					return "\"\"";
				else
					return "null";
			}

			if (_basicTypes.HasType(dataType))
			{
				var valText = BuildJSBasicValue(dataType, val);
				return valText;
			}
			else if (isRefType)
			{
				var model = refObject;
				var valText = val.ToString();				
					
				if (_schema.HasLookup(model) && _schema.HasLookupValue(model, valText))
				{
					var id = _schema.GetLookup(model).GetValue(valText);
					return "\"" + valText.Replace("\"", "\\\"") + "\"";
				}
			}
			return "null";
		}


		/// <summary>
		/// Builds a json value for a basic data type.
		/// </summary>
		/// <param name="dataType"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		private string BuildJSBasicValue(string dataType, object val)
		{
			// 1. Check null values.
			if (val == null)
			{
				var nullVal = dataType == DataModelConstants.DataType_Text ?  "\"\"" : "null";
				return nullVal;
			}

			var valText = val.ToString();
			var isEmpty = string.IsNullOrEmpty(valText);

			// 2. Text
			if (dataType == DataModelConstants.DataType_Text)
			{
				if (isEmpty) 
				return "\"\"";

				var stringVal = val.ToString().Replace("\"", "\\\"");
				stringVal = stringVal.Replace("\t", "   ");
				stringVal = stringVal.Replace("\r\n", " ");
				stringVal = stringVal.Replace("\n", " ");
				stringVal = "\"" + stringVal + "\"";
				return stringVal;
			}

			// 3. Bool
			if (dataType == DataModelConstants.DataType_Bool)
			{
				if(isEmpty) 
					return "false";

				return Convert.ToBoolean(val).ToString().ToLower();
			}

			// 4. Date
			if (dataType == DataModelConstants.DataType_Date)
			{
				if (isEmpty)
					return "null";

				var date = Convert.ToDateTime(val);

				// var d1 = new Date("11/28/2013 2:13:00 PM");
				// return "'new Date(" + date.Year + ", " + (date.Month-1) + ", " + date.Day + ", 0, 0, 0, 0)'";
				return "\"" + date.ToString("MM/dd/yyyy hh:mm:ss tt") + "\"";
			}

			// 5. Number
			if (dataType == DataModelConstants.DataType_Number)
			{
				if (isEmpty)
					return "0";
				return Convert.ToDouble(val).ToString();
			}
			return "null";
		}
	}
}
