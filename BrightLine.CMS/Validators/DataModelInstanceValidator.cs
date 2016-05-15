using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Models.Validators;
using BrightLine.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.Validators
{
	public class DataModelInstanceValidator : ModelValidator
	{
		private AppSchema _schema;
		private DataModels _data;
		private DataModelRecords _records;


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="schema"></param>
		public DataModelInstanceValidator(AppSchema schema, DataModels models)
		{
			_schema = schema;
			_data = models;
		}


		/// <summary>
		/// Validate
		/// </summary>
		/// <returns></returns>
		public override bool Validate()
		{
			var result = ValidateModels();
			return result.Success;
		}


		/// <summary>
		/// Validates the instances against the schema.
		/// </summary>
		/// <param name="instances"></param>
		/// <returns></returns>
		public BoolMessageItem<DataModels> ValidateModels()
		{
			_schema.LoadLookups();
			var recordNum = 0;
			var colNum = 0;
		    var boolVal = false;

			foreach (var model in _schema.Models.Models)
			{
				try
				{
					_records = model.Rows;
				    recordNum = 0;
				    colNum = 0;

                    // Any system level ( format / type ) which indicates 
                    // 1. model/instance is image, video 
                    // 2. model/instance is a how-to, music, tips, recipes ( e.g. business context around instance )
				    var anySystemFields = model.Schema.AnyFields(p => p.IsSystemLevel);
				    var validatorForType = new CmsModelTypeValidator();
				    var validatorForFormat = new CmsModelFormatValidator();
				    DataModelProperty formatProperty = anySystemFields ? model.Schema.GetFields( p => p.IsSystemLevel && p.Name == DataModelConstants.SystemFormatFieldName).FirstOrDefault() : null;
                    DataModelProperty typeProperty = anySystemFields ? model.Schema.GetFields(p => p.IsSystemLevel && p.Name == DataModelConstants.SystemTypeFieldName).FirstOrDefault() : null;
                    DataModelProperty publishProperty = anySystemFields ? model.Schema.GetFields(p => p.IsSystemLevel && p.Name == DataModelConstants.SystemPublishFieldName).FirstOrDefault() : null;

					// Ensure the inputs ( throw if anything required is not supplied )
					EnsureInputs();

					// Any records ?
                    if (_records.Data == null || _records.Data.Count == 0)
						return BuildValidationResult<DataModels>(_data);

					// Get properties for this model.
                    var props = _schema.GetModelFields(_records.ModelName, _records.Columns);
					var uniqueKeys = new Dictionary<string, bool>();

					// Check All instances.
                    var propValueValidator = new DataModelPropertyValueValidator(_schema, model.Schema);
                    for (var ndx = 0; ndx < _records.Data.Count; ndx++)
					{
                        var record = _records.Data[ndx];
						recordNum = ndx + 1;
						var key = record[0];

						// CHECK 1: Any data ?
						if (record == null || record.Count == 0)
						{
                            CollectModelRecordError(_records.ModelName, recordNum, 0, "No data supplied");
							continue;
						}

						// CHECK 2: duplicate key ?
						if (key != null)
						{
							var keyText = key.ToString();
							if (uniqueKeys.ContainsKey(keyText))
							{
								CollectModelRecordError(model.Name, recordNum, key, "Key", "Duplicate key found : " + keyText);
							}
							else
								uniqueKeys[keyText] = true;
						}

						
						// store a value of whether the current instance is published
						// unpublished instances may reference unpublished instances of other models
						var published = true;
						var indexOfPublish = model.Schema.IndexOfField(DataModelConstants.SystemPublishFieldName);
						if (indexOfPublish > 0)
						{
							published = Convert.ToBoolean(record[indexOfPublish]);
						}
						
						// CHECK 3: Check column values
                        propValueValidator.Instances = _records;
						for (var ndxC = 1; ndxC < record.Count; ndxC++)
						{
							var val = record[ndxC];
							var prop = props[ndxC];

							colNum = ndxC + 1;

							// Collect all the errors.
							propValueValidator.Validate(prop, recordNum, key, colNum, val, published);
						}	
					
                        // CHECK 4: Format / type
                        if (anySystemFields)
                        {
                            if (formatProperty != null)
                            {
                                var format = record[formatProperty.Position];
                                var formatResult = validatorForFormat.Validate(format as string);
                                CollectModelRecordError(formatResult.Success, model.Name, recordNum, key, formatProperty.Name, formatResult.Message);
                            }
                            if (typeProperty != null)
                            {
                                var type = record[typeProperty.Position];
                                var typeResult = validatorForType.Validate(type as string);
                                CollectModelRecordError(typeResult.Success, model.Name, recordNum, key, typeProperty.Name, typeResult.Message);
                            }
                            if (publishProperty != null)
                            {
                                var type = record[publishProperty.Position];
                                var isValidBool = bool.TryParse(type.ToString(), out boolVal);
								CollectModelRecordError(isValidBool, model.Name, recordNum, key, publishProperty.Name, "Invalid true/false value");
                            }
                        }
					}
					// Collect errors from this validator.
					CollectErrors(propValueValidator);
				}
				catch (Exception ex)
				{
                    CollectModelRecordError(_records.ModelName, recordNum, colNum, ex.Message);
				}
			}
			return BuildValidationResult<DataModels>(_data);
		}


		private void EnsureInputs()
		{
			// Model name not specified.
			if (string.IsNullOrEmpty(_records.ModelName))
				throw new ArgumentException("Model name not specified");

			// Model name invalid.		
			if (!_schema.HasModel(_records.ModelName))
				throw new ArgumentException("Unknown model name : " + _records.ModelName);

			// Invalid fields ?
			var model = _schema.GetModel(_records.ModelName);
			var result = model.Schema.ValidateFields(_records.Columns);
			if (!result.Success)
				throw new ArgumentException(result.Message);

			// All records should have same column counts
			result = _records.ValidateRecordColumnCounts();
			if(!result.Success)
				throw new ArgumentException(result.Message);
		}		
	}
}
