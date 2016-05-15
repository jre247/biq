using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Models.Validators;
using BrightLine.Common.Utility;
using BrightLine.Utility.Validation;
using BrightLine.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.AppImport
{
	/// <summary>
	/// Validates the metadata supplied in excel ( only checks for missing values ) - does not check reference etc.
	/// </summary>
	public class AppImportModelValidator : ValidatorBase
	{
		private AppSchema _schema;
		private List<string> _modelNames;
		private List<string> _names;
		private List<string> _types;
		private List<string> _reqs;
	    private List<string> _meta; 
		private List<string> _defaults;
		private string _modelName;



        /// <summary>
        /// Validate the meta data supplied.
        /// This method dynamically figures out which metadata field ( type, required, defaults, meta, comment etc ) is in which row.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="allModelNames"></param>
        /// <param name="modelName"></param>
        /// <param name="metadataFields"></param>
        /// <param name="metadataTable"></param>
        /// <returns></returns>
        public BoolMessageItem<AppSchema> Validate(AppSchema schema, List<string> allModelNames, string modelName, List<string> metadataFields, List<List<string>> metadataTable)
        {
            List<string> names = null;
            List<string> types = null;
            List<string> reqs = null;
            List<string> meta = null;
            List<string> defaults = null;

            var metaPositions = AppImporterHelper.GetMetaPositions(metadataFields);
            if (!metaPositions.ContainsKey("name"))
                throw new CmsValidationException("The NAME metadata column is missing from '" + modelName + "'");
            names = metadataTable[metaPositions["name"]];
            types = metadataTable[metaPositions["type"]];
            reqs = metadataTable[metaPositions["required"]];
            defaults = metadataTable[metaPositions["default"]];
            meta = metaPositions.ContainsKey("meta") ? metadataTable[metaPositions["meta"]] : null;

            return Validate(schema, allModelNames, modelName, names, types, reqs, defaults, meta);
        }


        /// <summary>
        /// Validates the app data model schema against its schema.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="allModelNames"></param>
        /// <param name="modelName"></param>
        /// <param name="names"></param>
        /// <param name="types"></param>
        /// <param name="reqs"></param>
        /// <param name="defaults"></param>
        /// <param name="meta"></param>
        /// <returns></returns>
		public BoolMessageItem<AppSchema> Validate(AppSchema schema, List<string> allModelNames, string modelName, List<string> names, List<string> types, List<string> reqs, List<string> defaults, List<string> meta )
		{
			_schema = schema;
			_modelName = modelName;
			_modelNames = allModelNames;
			_names = names;
			_types = types;
            _meta = meta;
			_reqs = reqs;
			_defaults = defaults;

			EnsureColumnCounts();
			ValidateTypes();
			ValidateRequired();
            ValidateMeta();

			return BuildValidationResult<AppSchema>(schema);
		}


        /// <summary>
        /// Ensure that the number of values for type, required, etc are consistent.
        /// </summary>
        private void EnsureColumnCounts()
        {
            var columnCount = _names.Count;
            var metaCount = _meta == null ? 0 : _meta.Count;

            // Check column counts match
            Ensure(_types.Count == columnCount     , "Number of columns ( " + _types.Count    + " ) for meta-data field 'type' in model' "      + _modelName + "' does not match number of column names (" + columnCount + ")");
            Ensure(_reqs.Count == columnCount      , "Number of columns ( " + _reqs.Count     + " ) for meta-data field 'required' in model '"  + _modelName + "' does not match number of column names (" + columnCount + ")");
            Ensure(_defaults.Count == columnCount  , "Number of columns ( " + _defaults.Count + " ) for meta-data field 'defaults' in model '"  + _modelName + "' does not match number of column names (" + columnCount + ")");
            Ensure((_meta == null || (_meta.Count == columnCount)), "Number of columns ( " + metaCount + " ) for meta-data field 'meta' in model '" + _modelName + "' does not match number of column names (" + columnCount + ")");
        }


        /// <summary>
        /// Validate the "required" field input values are either ( "required", "req", "optional" "opt" ).
        /// </summary>
        private void ValidateRequired()
        {
            for (var ndx = 0; ndx < _reqs.Count; ndx++)
            {
                var val = _reqs[ndx];
                if (val != null)
                {
                    val = val.ToLower().Trim();
                    if (val != "required" && val != "optional" && val != "req" && val != "opt")
                        CollectModelError(ndx + 1, " does not have a correct value for required/optional");
                }
            }
        }


        /// <summary>
        /// Validate the "required" field input values are either ( "required", "req", "optional" "opt" ).
        /// </summary>
        private void ValidateMeta()
        {
            if (_meta == null || _meta.Count == 0)
                return;

            for (var ndx = 0; ndx < _meta.Count; ndx++)
            {
                var val = _meta[ndx];
                if (!string.IsNullOrEmpty(val))
                {
                    val = val.ToLower().Trim();
                    if (val != DataModelConstants.MetaType_Both && val != DataModelConstants.MetaType_Client && val != DataModelConstants.MetaType_Server)
                        CollectModelError(ndx + 1, " does not have a correct value for meta property");
                }
            }
        }


        /// <summary>
        /// Validate  types ( e.g. text-50, number, datetime, true/false, list:text-50 ) etc.
        /// </summary>
		private void ValidateTypes()
        {
            var cmsFormatValidator = new CmsModelFormatValidator();

			for (var ndx = 0; ndx < _types.Count; ndx++)
			{
				var val = _types[ndx];
			    var name = _names[ndx];

                // CASE 1: Not supplied!
				if (string.IsNullOrEmpty(val))
				{
					CollectModelError(ndx + 1, " data-type must be supplied.");
					continue;
				}

				// Check that types are specified correctly.
				val = val.Trim();
				var isList = val.StartsWith("list:");
				var isRef = val.StartsWith("ref-");
			    var isImplicitModel = AppImportRules.IsModelLevelProperty(name);
                
                // CASE 2: Implicity model ( a property of a parent model but it's actually it's own model )
                if (isImplicitModel)
                {
                    var result = cmsFormatValidator.Validate(val);
                    if (!result.Success)
                        CollectModelError(ndx, "Invalid type for field");
                }
				// CASE 2: basic type ( text, number, true/false, datetime, text-50 )
				else if (!isList && !isRef)
				{
					ValidateType(val, ndx, false);
				}				
				// CASE 3: Reference to either another model or lookup value. ( ref-products | ref-lookup )
				else if(isRef)
				{
					var refType = val.Substring(4);
					ValidateType(refType, ndx, true);
				}
				// CASE 3: List of ( text, number, true/false, reference to another model/lookup ) list:
				else if (isList)
				{
					var refType = val.Substring(5);
					var checkForModel = refType.StartsWith("ref-");
					if(checkForModel)
					{
						refType = refType.Substring(4);
					}
					ValidateType(refType, ndx, checkForModel);
				}
			}
		}


		private void ValidateType(string val, int columnIndex, bool isRef)
		{
			// CASE 1: basic type ( text,  datetime, number, true/false )
			if (val == DataModelConstants.DataType_Text || val == DataModelConstants.DataType_Bool
				|| val == DataModelConstants.DataType_Number || val == DataModelConstants.DataType_Date || val == "true/false")
			{
				return;
			}

			// CASE 2: text-50
			else if (val.StartsWith("text-"))
			{
				var lenText = val.Substring(5);
				var len = 0;

				// Invalid length e.g. text-abc
				if (!int.TryParse(lenText, out len))
				{
					CollectModelError(columnIndex + 1, "Invalid length for text field: " + lenText);
				}				
			}
			else if (!isRef)
			{
				CollectModelError(columnIndex + 1, "Unknown type: " + val);
			}
		}


		private void CollectModelError(int column, string message)
		{
			CollectError("Model : '" + _modelName, "', Column number : " + (column) + ", " + message);
		}
    }
}
