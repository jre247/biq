using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.Validators
{
	public class DataModelPropertyValueValidator : ModelValidator
	{
		private AppSchemaBasicTypes _basicTypes;
		private AppSchema _schema;
		private DataModelSchema _modelSchema;
		private DataModelProperty _modelProp;
        private object _currentKey;


		/// <summary>
		/// Initialize.
		/// </summary>
		public DataModelPropertyValueValidator(AppSchema schema, DataModelSchema modelSchema)
		{
			_schema = schema;
			_modelSchema = modelSchema;
			_basicTypes = new AppSchemaBasicTypes();
		}


		/// <summary>
		/// The instances data for the model.
		/// </summary>
		public DataModelRecords Instances { get; set; }


		/// <summary>
		/// Validates the value against the property metadata.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="val"></param>
		public void Validate(DataModelProperty prop, int recordNum, object key, int colNumber, object val, bool instanceIsPublished)
		{
			_modelProp = prop;
            _currentKey = key;
            
			// Check validate required
			if (prop.Required && val == null)
			{
				CollectModelRecordError(Instances.ModelName, recordNum, key, prop.Name, "Value required for column " + prop.Name);
				return;
			}

			if (val != null)
			{ 
				// 1. Check lenght of string.
				if (prop.MaxLength > 0 && val.GetType() == typeof(string))
				{
					var text = (string)val;
					if (text.Length > prop.MaxLength)
						CollectModelRecordError(Instances.ModelName, recordNum, key, prop.Name, "Value at column " + prop.Name + ", exceeds length of " + prop.MaxLength);
				}
				// 2. Check basic data type.
				if( IsBasicType() )
				{					
					ValidateBasicValue(recordNum, colNumber, val.ToString());
				}
				// 3. Check reference. ( ref-brand )
				else if (!prop.IsListType && prop.IsRefType)
				{
					ValidateReference(prop, recordNum, colNumber, val, instanceIsPublished);
				}
                // 4. List of basic types
                // NOTE: Can ignore validation here because the AppImporterDataConverter checks lists/values
                else if (IsListOfBasicTypes())
                {
                }
                // 4. List
                else if (IsListOfLookup())
                {
                    var items = (List<string>)val;
                    foreach (var item in items)
                    {
                        // Only validate 
                        if (prop.IsRefType)
                        {
							ValidateReference(prop, recordNum, colNumber, item, instanceIsPublished);
                        } 
                    }
                }
			}
		}


		/// <summary>
		/// Valdiates a lookup or model instance
		/// </summary>
		private void ValidateReference(DataModelProperty prop, int recordNum, int colNumber, object val, bool instanceIsPublished)
		{
			if (_schema.HasLookup(prop.RefObject))
			{
				ValidateLookup(recordNum, colNumber, val.ToString());
			}
			else if (_schema.HasModel(prop.RefObject))
			{
				ValidateModelInstance(recordNum, colNumber, val.ToString(), instanceIsPublished);
			}
		}


		private bool IsBasicType()
		{
			return _basicTypes.HasType(_modelProp.DataType);
		}


        private bool IsListOfBasicTypes()
        {
            return _modelProp.IsListType && !_modelProp.IsRefType && _basicTypes.HasType(_modelProp.RefObject);
        }


        private bool IsListOfLookup()
        {
            return _modelProp.IsListType && _modelProp.IsRefType;
        }


		private void ValidateBasicValue(int recordNum, int colNum, string val)
		{
			var type = typeof(string);

            if (_modelProp.IsBool())
                type = typeof(bool);

            else if (_modelProp.IsDate())
                type = typeof(DateTime);

            else if (_modelProp.IsNumber())
                type = typeof(double);

            else if (_modelProp.IsString())
                type = typeof(string);

			if (!string.IsNullOrEmpty(val))
			{ 
				if(!Converter.CanConvertTo(type, val))
					CollectModelRecordError(Instances.ModelName, recordNum, _currentKey, _modelProp.Name, "Value : '" + val + "' is an invalid value for type : " + _modelProp.DataType);			
			}
		}


		private void ValidateLookup(int recordNum, int colNum, string val)
		{
			var type = _modelProp.RefObject;
			if(!_schema.HasLookupValue(type, val))
			{
                CollectModelRecordError(Instances.ModelName, recordNum, _currentKey, _modelProp.Name, "Value : '" + val + "', is an invalid lookup value for type : '" + type + "'");
			}
		}


		/// <summary>
		/// Validates that a key is valid for a given model type
		/// </summary>
		private void ValidateModelInstance(int recordNum, int colNum, string val, bool instanceIsPublished)
		{
			var type = _modelProp.RefObject;
			var model = _schema.Models.GetModel(type);

			if (!model.Rows.HasKey(val))
			{
				// model instances does not exist
				CollectModelRecordError(Instances.ModelName, recordNum, _currentKey, _modelProp.Name, "Value : '" + val + "', is an invalid reference model for type : '" + type + "'");
			}
			else
			{
				// model instance does exist, now check if the instance is published
				if (!instanceIsPublished)
				{
					// this instance is not published and may reference an unpublished instance
					return;
				}
				var instance = model.Rows.GetRecordByKey(val);
				var indexOfPublish = model.Schema.IndexOfField(DataModelConstants.SystemPublishFieldName);

				if (indexOfPublish < 0)
				{
					// this model does not yet use publishing
					return;
				}

				var published = Convert.ToBoolean(instance[indexOfPublish]);

				if (!published)
				{
					CollectModelRecordError(Instances.ModelName, recordNum, _currentKey, _modelProp.Name, "Value : '" + val + "', is an unpublished reference model");
				}
			}
		}
	}
}
