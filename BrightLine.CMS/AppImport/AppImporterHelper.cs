using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;

namespace BrightLine.CMS.AppImport
{
	public class AppImporterHelper
	{
		private static AppSchemaBasicTypes _basicTypes = new AppSchemaBasicTypes();


        /// <summary>
        /// Whether or not the property is a system level property ( used for cms infrastructure related purposes )
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetSystemLevelPropertyName(string propName)
        {
            if (!AppImportRules.IsSystemLevelProperty(propName))
                return propName;
            propName = propName.Substring(propName.IndexOf(":") + 1);
            return propName;
        }
        
        
        /// <summary>
        /// Whether or not the property is a system level property ( used for cms infrastructure related purposes )
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetModelLevelPropertyInfo(string propName)
        {
            if (!AppImportRules.IsModelLevelProperty(propName))
                return null;
            var modelAndPropName = propName.Substring(propName.IndexOf(":") + 1);
            var model = modelAndPropName.Substring(0, modelAndPropName.IndexOf(":"));
            propName = modelAndPropName.Substring(modelAndPropName.IndexOf(":") + 1);
            return new Tuple<string, string>(model, propName);
        }


        /// <summary>
        /// Each model property field (Name, type, required, default, meta, comment ) can be placed in a different sequential order.
        /// This figures out that type is in the 2nd pos, required is in the 3rd. etc.
        /// </summary>
        /// <param name="metadataFields"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetMetaPositions(List<string> metadataFields)
        {
            var map = new Dictionary<string, int>();

            for (int ndx = 0; ndx < metadataFields.Count; ndx++)
            {
                var field = metadataFields[ndx];
                if (String.Compare(field, "name", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["name"] = ndx;
                else if (String.Compare(field, "type", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["type"] = ndx;
                else if (String.Compare(field, "required", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["required"] = ndx;
                else if (String.Compare(field, "default", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["default"] = ndx;
                else if (String.Compare(field, "meta", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["meta"] = ndx;
                else if (String.Compare(field, "comment", System.StringComparison.OrdinalIgnoreCase) == 0)
                    map["comment"] = ndx;
            }
            return map;
        }



		/// <summary>
		/// Populates the data model property with the metadata values from the excel file
		/// </summary>
		/// <param name="modelName"></param>
		/// <param name="prop"></param>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="required"></param>
		/// <param name="defaultVal"></param>
		public static void SetupPropertyMetadata(string modelName, DataModelProperty prop, string name, string type, string required, string defaultVal, string metaValue)
		{
			prop.Name = MassageName(name);
			prop.IsListType = false;

			type = type.ToLower();

			// 1. Check type ( handle slight variations.
			if (type == DataModelConstants.DataType_Text)
			{
				prop.DataType = DataModelConstants.DataType_Text;
			}
			// 2. Bool
			else if (type == DataModelConstants.DataType_Bool || type == "true/false" || type == "true-false" || type == "true\\false" || type == "true false")
			{
				prop.DataType = DataModelConstants.DataType_Bool;
			}
			// 3. Date
			else if (type == DataModelConstants.DataType_Date || type == "date")
			{
				prop.DataType = DataModelConstants.DataType_Date;
			}
			// 4. Number
			else if (type == DataModelConstants.DataType_Number || type == "num")
			{
				prop.DataType = DataModelConstants.DataType_Number;
			}
			// 5. Text ( limit chars ) e.g text-100
			else if (type.StartsWith("text-"))
			{
				prop.DataType = DataModelConstants.DataType_Text;
				prop.MaxLength = GetLength(modelName, prop.Name, type);
			}
			// 6. List.			
			else if (type.StartsWith("list:"))
			{
				ConfigureList(modelName, prop, type);
			}
			// 7. 
			else if (type.StartsWith("ref-"))
			{
				prop.DataType = type;
				prop.RefObject = type.Substring(4);
				prop.IsRefType = true;			
			}
			else
			{
				prop.DataType = type;
			}

			// 2. Setup required or optional
			ConfigureRequired(prop, required);

		    ConfigureMetaValue(prop, metaValue);

			// 3. Default value.
			prop.DefaultValue = defaultVal == null ? "" : defaultVal;
		}


        public static void ConfigureMetaValue(DataModelProperty prop, string metaValue)
        {
            prop.IsMetaType = !string.IsNullOrEmpty(metaValue);
            prop.MetaValue = metaValue;
        }


		/// <summary>
		/// Builds a valid column name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string MassageName(string name)
		{
			name = name.ToLower();
			name = name.Replace("-", "_");


			// 1. Some contain parenthesis e.g. ACTIVATION LOGO FILE NAME (SPOTLIGHT ONLY)
			// Only get ACTIVATION LOGO FILE NAME
			var ndxParenthesis = name.IndexOf("(");
			if(ndxParenthesis > 0)
				name = name.Substring(0, ndxParenthesis).Trim();

			// 2. Now remove spaces and make into camelCase
			var ndxSpace = name.IndexOf(" ");
			if(ndxSpace < 0 )
				return GetValidName(name);

			// 4. Build camel case name.
			var camelCaseName = "";
			var tokens = name.Split(' ');
			for(var ndx = 0; ndx < tokens.Length; ndx++)
			{
				var token = tokens[ndx];
				if (!string.IsNullOrEmpty(token))
				{ 
					if(ndx == 0)
						camelCaseName = token;
					else
						camelCaseName += char.ToUpper(token[0]) + token.Substring(1);
				}
			}

			return GetValidName(camelCaseName);
		}


        /// <summary>
        /// Builds up a list of strings( delimited by ',' ) from the string supplied.
        /// This parses the text char by char to handle escaping of the delimeter ',' itself.
        /// This also strips away newlines/carriage returns,tabs.
        /// </summary>
        /// <param name="dataText"></param>
        /// <param name="removeWhiteSpace"></param>
        /// <returns></returns>
        public static List<string> ParseStringList(object dataText, bool removeWhiteSpace)
        {
            var items = new List<string>();
            if (dataText == null)
                return items;
            var data = dataText as string;
            if (string.IsNullOrEmpty(data))
                return items;

            var word = "";
            var ndx = 0;
            var len = data.Length;
            while (ndx < len)
            {
                var c = data[ndx];
                if (c == '\\')
                {
                    ndx++;
                    if (ndx < len)
                    {
                        word += data[ndx];
                    }
                }
                else if (c == ',')
                {
                    items.Add(word);
                    word = "";
                }
                else if (c != '\t' && c != '\r' && c != '\n')
                {
                    if (c != ' ')
                        word += c;
                    else if (!removeWhiteSpace)
                        word += c;
                }
                ndx++;
            }
            if (!string.IsNullOrEmpty(word))
                items.Add(word);

            // Trim start/end
            var finallist = new List<string>();
            foreach (var rawItem in items)
            {
                if (!string.IsNullOrEmpty(rawItem))
                {
                    var finalword = rawItem.Trim();
                    if (!string.IsNullOrEmpty(finalword))
                    {
                        finallist.Add(finalword);
                    }
                }
            }
            return finallist;
        }


		private static string GetValidName(string text)
		{
			var validName = "";
			for (var ndx = 0; ndx < text.Length; ndx++)
			{
				var ch = text[ndx];
				if (char.IsLetterOrDigit(ch) || ch == '_')
					validName += ch;
			}
			return validName;
		}



		private static void ConfigureRequired(DataModelProperty prop, string required)
		{
			// 2. Check the required ( handle slight-variations )
			if (required == "required" || required == "req")
			{
				prop.Required = true;
			}
			else if (required == "optional" || required == "opt" || required == "notrequired" || required == "not-required")
			{
				prop.Required = false;
			}
		}


		private static void ConfigureList(string modelName, DataModelProperty prop, string type)
		{
			prop.DataType = type;
			prop.IsListType = true;
			
			var refType = type.Substring(5);
			if (refType.StartsWith("ref-"))
			{
				prop.RefObject = refType.Substring(4);
				prop.IsRefType = true;			
			}
			else if(refType.StartsWith("text-"))
			{
				prop.RefObject = DataModelConstants.DataType_Text;
				prop.MaxLength = GetLength(modelName, prop.Name, refType);
			}
			else if (_basicTypes.HasType(refType))
			{
				prop.RefObject = refType;
			}
		}


		private static int GetLength(string modelName, string columnName, string type)
		{
			var lenText = type.Substring(5);
			int len = 0;
			if (!int.TryParse(lenText, out len))
			{
				throw new ArgumentException("Model : " + modelName + " column : " + columnName + " does not have a valid text length specified");
			}
			return len;
		}
	}
}
