using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Models
{
	/// <summary>
	/// Respresents the metadata for each property of a data model.
	/// Ultimately this is used for validating the data ( of model instances ) and generating the UI for the web to put in data.
	/// 
	/// NOTE: The main fields for VALIDATION are :
	/// 1. DataType
	/// 2. Required
	/// 3. DefaultValue
	/// 4. MaxLength
	/// 
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
	public class DataModelProperty
	{

		/// <summary>
		/// Model id ( represents a specific model ( e.g. videos/products ).
		/// </summary>
		public	int		ModelId				{ get; set; }
        public	string	Name				{ get; set; }
		public	string	DataType			{ get; set; }
		public	string	Desc				{ get; set; }
		public	bool	Required			{ get; set; }
		public	string	DefaultValue		{ get; set; }
		public	string	RefObject			{ get; set; }
		public	int		MaxLength			{ get; set; }
		public	string	RegularExpr			{ get; set; }
		public	string	Example				{ get; set; }
		public	string	HelpText			{ get; set; }
		public	string	ValidationMessage	{ get; set; }
		public  bool    IsListType			{ get; set; }
		public bool     IsRefType			{ get; set; }
        public bool     IsSystemLevel       { get; set; }
        public int      Position            { get; set; }

        public bool     IsImplicitModel     { get; set; }
        public string   ImplicitModelName    { get; set; }
        public bool     IsMetaType          { get; set; }
        public string   MetaValue           { get; set; }

        public bool IsNumber() { return !IsListType && DataType == DataModelConstants.DataType_Number; }
        public bool IsDate()   { return !IsListType && DataType == DataModelConstants.DataType_Date; }
        public bool IsString() { return !IsListType && DataType == DataModelConstants.DataType_Text; }
        public bool IsBool()   { return !IsListType && DataType == DataModelConstants.DataType_Bool; }


		/// <summary>
		/// Whether or not his is a list of reference objects e.g. list:ref-brand ( where brand is a lookup table or another model ).
		/// </summary>
		/// <returns></returns>
		public bool IsListOfRefTypes()
		{
			return IsListType && IsRefType;
		}


        public bool IsListOfNumbers()
        {
            return IsListType && RefObject == DataModelConstants.DataType_Number;
        }


        public bool IsListOfStrings()
        {
            return IsListType && RefObject == DataModelConstants.DataType_Text;
        }


        public bool IsListOfBools()
        {
            return IsListType && RefObject == DataModelConstants.DataType_Bool;
        }


        public bool IsListOfDates()
        {
            return IsListType && RefObject == DataModelConstants.DataType_Date;
        }


        public override string ToString()
        {
            return Name + ", type: " + DataType + ", required: " + Required + ", length: " + MaxLength;
        }


        public string FullName()
        {
            if (IsSystemLevel)
                return "system:" + Name;
            return Name;
        }


        internal void ConfigureReferenceType(string referenceModelName, string name)
        {
            this.Name = name;
            this.IsRefType = true;
            this.DataType = "ref-" + referenceModelName;
            this.RefObject = referenceModelName;
        }
    }
}
