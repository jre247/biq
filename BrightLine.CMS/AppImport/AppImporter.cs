using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Aspose.Cells;
using BrightLine.Common.Utility;


using BrightLine.CMS.Models;
using BrightLine.CMS.Validators;
using BrightLine.CMS.Serialization;
using BrightLine.Common.Utility.Spreadsheets;
//using BrightLine.Utility.Helpers;
using BrightLine.Utility.Validation;
using BrightLine.Utility;
using BrightLine.Utility.Helpers;


namespace BrightLine.CMS.AppImport
{
	public class AppImporter
	{
        private AppSchema _schema;
		private DataModels _dataModels;
	    private List<ModelConfig> _modelConfigs; 
		private ISpreadsheetReader _sheetReader;


		/// <summary>
		/// Import from bytes representing spreadsheet.
        /// </summary>
        /// <param name="appname">The name of the app ( e.g. loreal )</param>
		/// <param name="contents">The binary contents of the spreasheet</param>
		public BoolMessageItem<List<string>> Import(string appname, byte[] contents)
		{
            _schema = new AppSchema();
			_schema.Name = appname;
			_dataModels = new DataModels();
			_sheetReader = new SpreadsheetReader();
            _sheetReader.LoadFile(contents);
			return Import();
		}


	    /// <summary>
	    /// Import the schema and data form the excel file supplied.
	    /// </summary>
	    /// <param name="filePath">The path to the spreadsheet.</param>
	    /// <param name="appname">The name of the app ( e.g. loreal )</param>
	    public BoolMessageItem<List<string>> ImportFromPath(string filePath, string appname)
		{
			_schema = new AppSchema();
			_schema.Name = appname;
			_dataModels = new DataModels();

			//filePath = @"C:\Code\Brightline\proto\appmodels_loreal.xlsx";
            _sheetReader = new SpreadsheetReader();
            _sheetReader.LoadFile(filePath);
			return Import();
		}


		/// <summary>
		/// Get schema
		/// </summary>
		/// <returns></returns>
		public AppSchema GetSchema()
		{
			return _schema;
		}


		/// <summary>
		/// Save to folder.
		/// </summary>
		/// <param name="exportPath"></param>
		/// <param name="enableNewLines"></param>
		public void SaveToFolder(string exportPath, bool enableNewLines)
		{
			var serializer = new AppDataSerializer();
			serializer.SerializeAllDataToPublishedFormatFiles(_schema, _dataModels, exportPath, enableNewLines);
		}


		private BoolMessageItem<List<string>> Import()
		{
			// 1. Ensure sheets exists. e.g. KEY sheet contains lookup values, and 'MODELS' sheet contains models to load.
			EnsureSheets();

			// 2. Load all the models that need to be extracted by checking the 'MODELS' sheet.
            LoadModelConfigs();

            // 3. Make sure the sheets for the models exist.
		    EnsureModelSheets();

			// 4. Now load all the lookup tables in the 'KEY' sheet.
			LoadLookupNames();

            // 5. Load settings
		    LoadSettings();

			// 6. Now load the schema and data for all the models.
			LoadModels();

            // 7. Convert the raw data from excel into appropriates types for models.
            ConvertRawData();
		    
            // 8. Now validate all the models. ( This is majority of the work )
            // e.g. check values, text lengths, lists, references to lookups, references to other models, duplicate keys etc.
		    return ValidateSchema();
		}


        /// <summary>
        /// Load all the model names ( that should be imported ) by reading the "MODELS" sheet.
        /// </summary>
        /// <remarks>
        /// Each model, such as "videos" is located on a sheet.
        /// There could be several sheets ( some for models, other for notes ). 
        /// The PURPOSE of the 'MODELS' sheet is to :
        /// 
        /// 1. tell this importer which sheets represents models to load
        /// 2. tell this importer which of the models to load ( e.g. maybe only 5 of the 7 models/sheets ).
        /// 3. tell this importer to filter only records ( of a specific model ) based on a value of a specific column in that model sheet.
        /// e.g. Default filter is IS ACTIVE = true ( sometimes data is not yet ready for loading ).
        /// </remarks>
        private void LoadModelConfigs()
		{
			_sheetReader.SetCurrentSheet("MODELS");

            // 1. Read the table of valu MODELS   FILTER  TYPE
            var mapper = new EntityMapper<ModelConfig>(_sheetReader);
            mapper.AddField("MODELS", ExpressionHelper.GetPropertyName<ModelConfig>(s => s.SheetName),  typeof(string), true, string.Empty, 1, 50);
            mapper.AddField("FILTER", ExpressionHelper.GetPropertyName<ModelConfig>(s => s.FilterText), typeof(string), true, string.Empty, 1, 50);
            mapper.AddField("TYPE",   ExpressionHelper.GetPropertyName<ModelConfig>(s => s.Type),       typeof(string), false, string.Empty, 1, 50);
            mapper.RegisterProperties();

            // Limit to 50 models for now.
            var totalModels = _sheetReader.TotalRowsWithValues(2, 0, 50);

            // 2. Now that the mapper is setup... let it automap.
            _modelConfigs = mapper.MapEntitiesAcross("MODELS", 2, 0, 3, totalModels);
            if(_modelConfigs.Count == 0)
                throw new ArgumentException("No data models supplied in the 'MODELS' sheet in the first column 'MODELS'");

            // 3. Extract model name and filter.
            foreach (var modelConfig in _modelConfigs)
                modelConfig.ExtractData();
		}


        /// <summary>
        /// Load all lookups which are small lists of predefined values for a specific type like CATEGORY = ( 'lips', 'hair', 'face' ).
        /// </summary>
        /// <remarks>
        /// In this example, we load the name of the lookup "CATEGORY" and it's values ( 'lips', 'hair', 'face' ) 
        /// NOTE: Example taken from l'oreal.
        /// </remarks>
		private void LoadLookupNames()
		{
			_sheetReader.SetCurrentSheet("KEY");

			var lookupNames = _sheetReader.LoadRow(0, 0, 20, true, false);
			for (var ndx = 0; ndx < lookupNames.Count; ndx++)
			{
				var lookup = new DataLookupTable();
				lookup.Name = AppImporterHelper.MassageName(lookupNames[ndx]);
				lookup.Values = _sheetReader.LoadColumn(2, ndx, 50, true, false);
				_schema.Lookups.Add(lookup);
			}
		}


	    /// <summary>
	    /// The is the core method that loads all the properties of each model and it's data.
	    /// </summary>
	    /// <returns></returns>
	    private void LoadModels()
	    {
	        // Get the models name
	        var formattedNames = (from config in _modelConfigs where !config.IsSettings select config.ModelName).ToList();

	        // 1. Load the schema and instance data.
	        for (var ndx = 0; ndx < _modelConfigs.Count; ndx++)
	        {
	            var modelConfig = _modelConfigs[ndx];

	            // Actual model.
	            if (!modelConfig.IsSettings)
	            {
	                var model = LoadModel(modelConfig, formattedNames);

	                // 2. Now add this model the appschema.
	                _schema.Models.Add(model);
	            }
	        }

	        // 3. Load the "lookups" on the scheam and instances
	        // this allows easily getting a model by name ( lookup tables )
	        _schema.LoadLookups();
	        _dataModels.LoadLookup();
	    }


        private DataModel LoadModel(ModelConfig modelConfig, List<string> allModelNames)
		{
			// 1. Move to sheet for model.
			_sheetReader.SetCurrentSheet(modelConfig.SheetName);

            // 2. Create schema for current model.
			var model = new DataModel(modelConfig.ModelName);
            var modelPropNames = new List<string>();
            
            // 3. Now load up all it's metadata ( properties )
		    LoadModelSchema(modelConfig, allModelNames, model, modelPropNames);

            // 4. Load up the data for the model.
			LoadModelData(model, modelConfig);

            return model;
		}


        private void LoadModelSchema(ModelConfig modelConfig, List<string> allModelNames, DataModel model, List<string> modelPropNames )
        {
            _sheetReader.SetCurrentSheet(modelConfig.SheetName);
            
            // 1. Get the meta data field names supplied in sheet ( listed vertically in each row on 1st column )
            // e.g. "type, required, default, meta, comment"
            var metadataFields = _sheetReader.ReadColumnOfStrings(0, 0, 10, true);
            int totalMetadataFields = _sheetReader.TotalRowsWithValues(0, 0, 10);

            // 2. How many columns / field namess ?
            const int maxFields = 40;
            var modelColNames = _sheetReader.ReadRowOfStrings(0, 1, maxFields, true, false);

            // 3. Now get a table of all the field names and field info( required, type, default value etc ).
            //    This information for each field is located in each column. 
            // 
            //          Col 0       Col 1           Col 2       Col 3           Col 4
            // 0        META        NAME	        THEME	    IS SPOTLIGHT	TAGS
            // 1        type        text-50	        text-50	    true/false	    list:text-50
            // 2        required    required	    required	required	    required
			// 3        default     -               -           -               -
	        // 4        meta        -               server		-               -
            // 5        commment    -               -		    -               -

            // Get the table of all the info for each model field (  names, types, defaults, meta props, comments ).
            // NOTE: The first 2 columns are KEY and IS ACTIVE.
            var metadataTable = _sheetReader.ReadMultiRowsOf<string>(0, 1, totalMetadataFields, modelColNames.Count, false);

            // 4. Validate the supplied types, default values, meta fields, required/optional flags 
            var validator = new AppImportModelValidator();
            var validationResult = validator.Validate(_schema, allModelNames, modelConfig.ModelName, metadataFields, metadataTable);
            if (!validationResult.Success)
            {
                throw new CmsValidationException() { Errors = validator.GetErrors() };
            }

            // 5. All data is valid. Now convert the raw metadata inputs into DataModelProperty and load the model schema.
            var metaPositions = AppImporterHelper.GetMetaPositions(metadataFields);
            int ndxName = metaPositions["name"];
            int ndxType = metaPositions["type"];
            int ndxReq  = metaPositions["required"];
            int ndxDef = metaPositions["default"];
            int ndxMeta = metaPositions.ContainsKey("meta") ? metaPositions["meta"] : -1;

            var rowNames = metadataTable[ ndxName ];
            var rowTypes = metadataTable[ndxType];
            var rowRequired = metadataTable[ndxReq];
            var rowDefaults = metadataTable[ndxDef];
            var rowMeta = ndxMeta == -1 ? null : metadataTable[ndxMeta];

            for (var ndx = 0; ndx < rowNames.Count; ndx++)
            {
                string metaValue = rowMeta == null ? null : rowMeta[ndx];
                var prop = model.Schema.AddFieldRaw(rowNames[ndx], rowTypes[ndx], rowRequired[ndx], rowDefaults[ndx], metaValue);
                modelPropNames.Add(prop.FullName());
            }
            model.Rows.Columns = modelPropNames;
            model.Rows.Schema = model.Schema;
            model.Schema.TotalMetaFields = ndxMeta == -1 ? 5 : 6;
        }


        private void LoadSettings()
        {
            var mapper = new EntityMapper<DataModelSetting>();
            mapper.Reader = _sheetReader;
            
            // 1. Set up the mapper for auto mapping the fields.
            mapper.AddField("NAME", ExpressionHelper.GetPropertyName<DataModelSetting>(s => s.Name), typeof(string), true, string.Empty, 1, 50);
            mapper.AddField("TYPE", ExpressionHelper.GetPropertyName<DataModelSetting>(s => s.Type), typeof(string), true, string.Empty, 1, 50);
            mapper.AddField("VALUE", ExpressionHelper.GetPropertyName<DataModelSetting>(s => s.Value), typeof(object), true, string.Empty, 1, 200);
            mapper.AddField("REQUIRED", ExpressionHelper.GetPropertyName<DataModelSetting>(s => s.Required), typeof(string), true, string.Empty, 1, 50);
            mapper.RegisterProperties();

            // 2. Now go trhough each settings object.
            EachModel(config => config.IsSettings, modelConfig =>
            {
                _sheetReader.SetCurrentSheet(modelConfig.SheetName);

                // How many settings in the rows ?
				// Check row starting at column 0
                var totalSettings = _sheetReader.TotalRowsWithValues(2, 0, 200);

                // Now let the mapper map the settings.
                var settings = mapper.MapEntitiesAcross(modelConfig.SheetName, 2, 0, 4, totalSettings);

				// Check if there are any duplicate key names.
	            var firstDuplicateKey = settings.GroupBy(o => o.Name).FirstOrDefault(o => o.Count() > 1);

				if (firstDuplicateKey != null)
	            {
					// Fail and tell the user which key has been duplicated.
					throw new CmsValidationException(String.Format("Setting name '{0}' cannot be used more than once.", firstDuplicateKey));
				}

                var modelFromSettings = ConvertSettingsToModel(modelConfig, settings);
                _schema.Models.Add(modelFromSettings);
            });
        }


		private void LoadModelData(DataModel model, ModelConfig modelConfig)
		{
			model.Rows.Data = new List<List<object>>();			
			
            const int MAX_RECORDS = 2000;

			int totalRecords = 0;
		    var row = model.Schema.TotalMetaFields;
			var startCol = 1;
		    var hasFilter = modelConfig.HasFilter();
			ModelConfig.ModelFilter filter = null;

            // Can filter?
			if (hasFilter)
			{
			    filter = modelConfig.Filter;
				filter.ColumnIndex = model.Schema.IndexOfField(filter.PropName);
			}
		    var anyDefaults = model.Schema.HasAnyDefaults();
		    var defaultedProps = model.Schema.GetDefaultedFields();

			while (true)
			{
                if (totalRecords >= MAX_RECORDS)
                    throw new ArgumentException("Error while loading row " + row + " for model '" + model.Name + "' : importer is currently limited to : " + MAX_RECORDS + " records per model");
				
                try
				{
					var val = _sheetReader.GetCell(row, startCol);
					if (val == null )
					{
						break;
					}

					// 1. Load the record from sheet.
                    var record = _sheetReader.LoadRowAsObjects(row, startCol, model.Schema.Fields.Count, false, true);

                    // 2. Apply default values.
                    if (anyDefaults)
                    {
                        AppImportRules.ApplyDefaults(defaultedProps, record);
                    }

					// 2. Add record if applicable.
					if(AppImportRules.CanImportRecord(filter, record))
						model.Rows.Data.Add(record);

					row++;
					totalRecords++;
				}
				catch (Exception ex)
				{
					throw new ArgumentException("Error while loading row : " + row + ". " + ex.Message);
				}
			}
        }


        private DataModel ConvertSettingsToModel(ModelConfig modelConfig, List<DataModelSetting> settings)
        {
            // Now pivot the settings and convert them into a normal model.
            // This allows it to go through the same exact workflow ( data conversion, validation, serialization )
            // every other model.
            var model = new DataModel(modelConfig.ModelName);
            model.Schema = new DataModelSchema(modelConfig.ModelName);
            model.Schema.AddFieldRaw("KEY", "text", "required", string.Empty, string.Empty);
            
            // Rows of data
            model.Rows  = new DataModelRecords();
            model.Rows.ModelName = modelConfig.ModelName;
            model.Rows.Schema = model.Schema;
            model.Rows.Data = new List<List<object>>();
            model.Rows.Columns = new List<string>();
            model.Rows.Columns.Add("key");
            model.IsSettings = true;

            // add the only 1st row
            model.Rows.Data.Add(new List<object>());
            model.Rows.Data[0].Add(1);
            foreach (var setting in settings)
            {
                var prop = model.Schema.AddFieldRaw(setting.Name, setting.Type, setting.Required, string.Empty, string.Empty);
                model.Rows.Columns.Add(prop.Name);
                model.Rows.Data[0].Add(setting.Value);
            }
                
            // Now create lookup to link a key to its record.
            model.Rows.LoadLookup();
            return model;
        }


        private void ConvertRawData()
        {
            // 1. Convert the data ( basically massaging the data bit ) before valdiation.
            // This includes converting strings for model references from "1,2" to an array [1,2] of ids.
            var converter = new AppImporterDataConverter(_schema);
            converter.ConvertData();

            // 2. Error conversion failed ? don't continue
            Ensure(!converter.HasErrors(), "App data conversion failed", converter.GetErrors());
        }


        private BoolMessageItem<List<string>> ValidateSchema()
        {
            var validators = new List<ValidatorBase>()
			{
				  new AppSchemaValidator(_schema), 
				  new DataModelInstanceValidator(_schema, _dataModels), 
				  new DataModelReferenceValidator(_schema, _dataModels) 
			};
            return ValidatorHelper.Validate(validators, true);
        }


        private void EachModel(Func<ModelConfig, bool> filter, Action<ModelConfig> callback)
        {
            foreach (var modelConfig in _modelConfigs)
            {
                if (filter(modelConfig))
                {
                    callback(modelConfig);
                }
            }
        }


        private void EnsureSheets()
        {
            Ensure(_sheetReader.ContainsSheet("KEY"),    "Sheet KEY does not exist");
            Ensure(_sheetReader.ContainsSheet("MODELS"), "Sheet MODELS does not exist");
        }


        private void EnsureModelSheets()
        {
            foreach (var modelConfig in _modelConfigs)
            {
                Ensure(_sheetReader.ContainsSheet(modelConfig.SheetName), "Sheet for model : " + modelConfig.SheetName + " does not exist");
            }
        }


        protected void Ensure(bool isSuccess, string errorMessage)
        {
            if (isSuccess)
                return;
            throw new ArgumentException(errorMessage);
        }


        protected void Ensure(bool isSuccess, string errorMessage, List<string> errors )
        {
            if (isSuccess)
                return;
            throw new CmsValidationException(errorMessage) { Errors = errors };
        }
	}
}
