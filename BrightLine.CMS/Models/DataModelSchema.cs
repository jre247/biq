using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Models;
using BrightLine.Utility;


namespace BrightLine.CMS
{
	/// <summary>
	/// Respresents the schema for a SINGLE data model ( e.g. products ).
	/// Just has the model name ( Name ) and the collection of properites for this model ( DataModelProperty )
	/// EXAMPLE BELOW:
	/// {
	///		"Name": "videos",
	///		"Fields":
	///		[
	///			{ "Name": "key", 	  		  		"DataType": "text", 		    "Required": true,  "DefaultValue": "0",		"RefObject": "",  "MaxLength": 20,	"RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///			{ "Name": "brand", 	  		  		"DataType": "@brand", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///			{ "Name": "category",   		  	"DataType": "@category",	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 0,   "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" },
	///			{ "Name": "name", 	  				"DataType": "text", 		    "Required": true,  "DefaultValue": "", 		"RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///			{ "Name": "urlLink", 	  			"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }					
	///		]
	///	},
	/// </summary>
	public class DataModelSchema
	{
		private Dictionary<string, DataModelProperty> _fieldLookup;
        
        public DataModelSchema()
        {
        }


        public DataModelSchema(string modelname)
        {
            Name = modelname;
        }

		public string Name { get; set; }
		public string App { get; set; }	
		public string Desc { get; set; }
        public string BaseType { get; set; }
		public List<DataModelProperty> Fields { get; set; }
	    public int TotalMetaFields;

        
		/// <summary>
		/// Loads the properties into a lookup table.
		/// </summary>
		public void LoadLookup()
		{
			if(_fieldLookup == null)
                _fieldLookup = new Dictionary<string,DataModelProperty>();
			if(Fields == null)
				return;

			foreach (var prop in Fields)
			{
				_fieldLookup[prop.Name] = prop;
			}
		}


        public void AddField(DataModelProperty field)
        {
            if (_fieldLookup == null)
                _fieldLookup = new Dictionary<string, DataModelProperty>();
            if (Fields == null)
                Fields = new List<DataModelProperty>();

            var fullname = field.FullName();
            _fieldLookup[fullname] = field;
            Fields.Add(field);
            field.Position = Fields.Count - 1;
        }


        public DataModelProperty AddFieldRaw(string propName, string dataType, string required, string defaultValue, string metaValue)
        {
            var prop = new DataModelProperty();
            var isSystemLevel = false;
            var isModelLevel = false;
            var modelName = string.Empty;
            
            // CHECK 1: Handle system level properties ( prefixed with "SYSTEM" ) used internally for CMS infrastructure
            if (AppImportRules.IsSystemLevelProperty(propName))
            {
                propName = AppImporterHelper.GetSystemLevelPropertyName(propName);
                isSystemLevel = true;
            }
            // CHECK 2: Handle implicit models defined as properties ( prefixed with "MODEL" ) used internally for CMS infrastructure
            else if (AppImportRules.IsModelLevelProperty(propName))
            {
                var fields = AppImporterHelper.GetModelLevelPropertyInfo(propName);
                propName = fields.Item2;
                isModelLevel = true;
                modelName = AppImporterHelper.MassageName(fields.Item1);
            }
            AppImporterHelper.SetupPropertyMetadata(Name, prop, propName, dataType, required, defaultValue, metaValue);
            prop.IsSystemLevel = isSystemLevel;
            prop.IsImplicitModel = isModelLevel;
            prop.ImplicitModelName = modelName;
            AddField(prop);
            return prop;
        }


        public void AddField(string propName, string dataType, bool isReferenceType, string refObject, bool isList, int length, bool required, string defaultValue, string metaValue)
        {
            var prop = new DataModelProperty();
            prop.Name = propName;
            prop.DataType = dataType;
            prop.IsRefType = isReferenceType;
            prop.RefObject = refObject;
            prop.IsListType = isList;
            prop.MaxLength = length;
            prop.Required = required;
            prop.DefaultValue = defaultValue;
            AppImporterHelper.ConfigureMetaValue(prop, metaValue);
            AddField(prop);
        }


		/// <summary>
		/// Whether or not this has the name supplied.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasField(string name)
		{
			return _fieldLookup.ContainsKey(name);
		}


		/// <summary>
		/// Gets the field with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DataModelProperty GetField(string name)
		{
			if(!HasField(name))
				return null;
			return _fieldLookup[name];
		}


		/// <summary>
		/// Index of field.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int IndexOfField(string name)
		{
			var index = -1;
			for (var ndx = 0; ndx < Fields.Count; ndx++)
			{
				if (Fields[ndx].Name == name)
				{
					index = ndx;
					break;
				}
			}
			return index;
		}


		/// <summary>
		/// Checks whether or not all the field names supplied are valid ( exist in this model ).
		/// </summary>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public BoolMessage ValidateFields(List<string> fieldNames)
		{
			var areColumnsValid = true;
			var message = "";
			foreach (var columnName in fieldNames)
			{
				if (!HasField(columnName))
				{
					areColumnsValid = false;
					message = "Column : " + columnName + " does not exist for model : " + this.Name;
					break;
				}
			}
			return new BoolMessage(areColumnsValid, message);
		}


		/// <summary>
		/// Gets the fields in the sequence supplied.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public List<DataModelProperty> GetFields(List<string> fieldNames)
		{
			var props = new List<DataModelProperty>();
			foreach (var columnName in fieldNames)
			{
				var prop = GetField(columnName);
				props.Add(prop);
			}
			return props;
		}


        public bool AnyFields(Func<DataModelProperty, bool> predicate )
        {
            if (predicate == null)
                return Fields != null && Fields.Count > 0;

            var success = false;
            for (var ndx = 0; ndx < Fields.Count; ndx++)
            {
                if (predicate(Fields[ndx]))
                {
                    success = true;
                    break;
                }
            }
            return success;
        }


        public List<DataModelProperty> GetFields(Func<DataModelProperty, bool> predicate)
        {
            if (predicate == null)
                return Fields;

            var matches = new List<DataModelProperty>();
            for (var ndx = 0; ndx < Fields.Count; ndx++)
            {
                var field = Fields[ndx];
                if (predicate(field))
                {
                    matches.Add(field);
                }
            }
            return matches;
        }


        public override string ToString()
        {
            return Name + ", " + App;
        }


        public bool HasAnyDefaults()
        {
            return AnyFields(prop => !String.IsNullOrEmpty(prop.DefaultValue));
        }


        public List<DataModelProperty> GetDefaultedFields()
        {
            var fields = GetFields(prop => !String.IsNullOrEmpty(prop.DefaultValue));
            return fields;
        }
    }
}