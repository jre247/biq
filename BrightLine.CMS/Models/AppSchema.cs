using System;
using System.Collections.Generic;
using System.Linq;

namespace BrightLine.CMS.Models
{
	/// <summary>
	/// The schema for an app ( client ). Contains a colleciton of both the lookup tables and models.
	/// 
	/// EXAMPLE ( LOREAL )
	/// 
	///	{	
	///		"Name": "Loreal",
	///		"Desc": "First version of loreal application schema",
	///		"Lookups":
	///		[
	///			{
	///				"Name": "Season",
	///				"Values": ["Winter", "Summer", "Spring", "Fall", "Evergreen" ]
	///			},
	///			{		
	///				"Name": "Brand",
	///				"Values": ["GAR","MNY","LOP","REDK","LANC","RL","YSL","LRP","SSC"]
	///			}
	///		],
	///		"Models" :
	///		[
	///				{
	///					"Name": "videos",
	///					"Fields":
	///					[
	///						{ "Name": "key", 	  		  		"DataType": "text", 		    "Required": true,  "DefaultValue": "0",		"RefObject": "",  "MaxLength": 20,	"RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "brand", 	  		  		"DataType": "@brand", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "category",   		  	"DataType": "@category",	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 0,   "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" },
	///						{ "Name": "name", 	  				"DataType": "text", 		    "Required": true,  "DefaultValue": "", 		"RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "urlLink", 	  			"DataType": "text", 		    "Required": false, "DefaultValue": "false", "RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }					
	///					]
	///				},
	///				{
	///					"Name": "products",
	///					"Fields":
	///					[
	///						{ "Name": "key", 	  		  		"DataType": "text", 		    "Required": true,  "DefaultValue": "0",		"RefObject": "",  "MaxLength": 20,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "active", 	  		  	"DataType": "bool", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "goLiveDate", 		  	"DataType": "datetime", 	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "brand", 	  		  		"DataType": "@brand", 		    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 50,  "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }, 
	///						{ "Name": "category",   		  	"DataType": "@category",	    "Required": true,  "DefaultValue": "false", "RefObject": "",  "MaxLength": 0,   "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" },
	///						{ "Name": "productname", 	  		"DataType": "text", 		    "Required": true,  "DefaultValue": "", 		"RefObject": "",  "MaxLength": 500, "RegularExpr": "", "Desc": "", "Example": "", "HelpText": "", "ValidationMessage": "" }	///						
	///					]
	///				}
	///		]
	///	}
	/// </summary>
	public class AppSchema
	{
	    private DataModels _models;
		private Dictionary<string, DataLookupTable> _tableLookup;


		public AppSchema()
		{
			Lookups = new List<DataLookupTable>();
            _models = new DataModels();
			_tableLookup = new Dictionary<string, DataLookupTable>();
			AppVersion = "1";
			SchemaVersion = "1.1.0.3";
		}


		/// <summary>
		/// App name e.g. loreal
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Description of app 
		/// </summary>
		public string Desc { get; set; }


		/// <summary>
		/// The schema version of the app.
		/// </summary>
		public string AppVersion { get; set; }


		/// <summary>
		/// The schema version of the app.
		/// </summary>
        public string SchemaVersion { get; set; }
        
        
        /// <summary>
        /// The schema in JSON format
        /// </summary>
        public string JsonSchema { get; set; }


        /// <summary>
        /// All the data in JSON format.
        /// </summary>
        public string JsonData { get; set; }


        /// <summary>
        /// All the data in raw JSON format ( this is json representation of the app models datastructure ).
        /// This is just a table of rows ( where each item in the row is an object )
        /// Table based format, not heirarchical format that is published to couch db for the design team.
        /// </summary>
        public string JsonDataRaw { get; set; }


		/// <summary>
		/// All the lookup tables supported.
		/// </summary>
		public List<DataLookupTable> Lookups { get; set; }


		/// <summary>
		/// All the models supported.
		/// </summary>
        public DataModels Models { get { return _models; } }
 

		/// <summary>
		/// Loads  the field lookup tables for each model.
		/// </summary>
		public void LoadLookups()
		{
			_models.LoadLookup();

			foreach (var lookup in Lookups)
			{
				if (lookup != null)
				{ 
					lookup.LoadLookup();
					if (!string.IsNullOrEmpty(lookup.Name))
					{
						_tableLookup[lookup.Name.ToLower()] = lookup;
					}
				}
			}
		}


        public bool HasSettings()
        {
            if (Models == null || Models.Count == 0)
                return false;
            var any = (from model in Models.Models where model.IsSettings select model).Any();
            return any;
        }


		/// <summary>
		/// Whether or not there exists a model with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasModel(string name)
		{
			if (name == null)
				return false; 
			return _models.HasModel(name.ToLower());
		}


		/// <summary>
		/// Gets the model with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DataModel GetModel(string name)
		{
			if (!HasModel(name))
				return null;
		    return _models.GetModel(name.ToLower());
		}


		/// <summary>
		/// Whether or not there exists a lookup table with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasLookup(string name)
		{
			if(name == null)
				return false;

			return _tableLookup.ContainsKey(name.ToLower());
		}


		public bool HasLookupValue(string name, string val )
		{
			var lookup = GetLookup(name);
			if(lookup == null)
				return false;

			return lookup.Contains(val);
		}


		/// <summary>
		/// Gets the lookup table with the supplied name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public DataLookupTable GetLookup(string name)
		{
			if (!HasLookup(name))
				return null;
			return _tableLookup[name.ToLower()];
		}


		/// <summary>
		/// Gets the data model properties for the supplied model and sequence of columns specified.
		/// </summary>
		/// <param name="modelName"></param>
		/// <param name="columns"></param>
		/// <returns></returns>
		public List<DataModelProperty> GetModelFields(string modelName, List<string> columns)
		{
			var model = GetModel(modelName);
			var fieldSequence = model.Schema.GetFields(columns);
			return fieldSequence;
		}
		
		
		/// <summary>
		/// Gets the fields in the sequence supplied.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public List<int> GetModelFieldsWithReferenceTypePositions(string modelName)
		{
			var positions = new List<int>();
			var model = GetModel(modelName);
			var fields = model.Schema.Fields;
			for (var ndx = 0; ndx < fields.Count; ndx++)
			{
				var field = fields[ndx];
				if (field.IsRefType)
				{
					if (HasModel(field.RefObject))
					{ 
						positions.Add(ndx);
					}
				}
			}
			return positions;
		}


        /// <summary>
        /// Returns list of id and corresponding key(non-changing).
        /// </summary>
        /// <returns></returns>
        public CmsModelSet GetModelSet()
        {
            var modelSet = new CmsModelSet();
            foreach (var model in _models.Models)
            {
                var uniqueIds = new Dictionary<string, int>();
                if (model.Rows != null && model.Rows.Data.Count > 0)
                {
                    int indexOfKey = model.Schema.IndexOfField(DataModelConstants.ColumnNameKey);
                    for(int ndx = 0; ndx < model.Rows.Data.Count; ndx++)
                    {
                        var row = model.Rows.Data[ndx];
                        if (row != null)
                        {
                            var keyObj = row[indexOfKey];
                            var keyText = keyObj == null ? "" : keyObj.ToString();
                            uniqueIds[keyText] = 0;
                        }
                    }
                    modelSet.Add(model.Name, uniqueIds);
                }
            }
            return modelSet;
        }
    }
}
