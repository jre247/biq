using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Utility;
using BrightLine.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.Validators
{
	public class DataModelSchemaValidator : ModelValidator
	{
		private AppSchema _schema;
		private DataModelSchema _currentModel;
		private AppSchemaBasicTypes _basicTypes;


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="schema"></param>
		public DataModelSchemaValidator(AppSchema schema)
		{
			_schema = schema;
		}


		/// <summary>
		/// Validates the instances against the schema.
		/// </summary>
		/// <param name="instances"></param>
		/// <returns></returns>
		public BoolMessageItem<DataModelSchema> Validate(DataModelSchema model)
		{
			_currentModel = model;
			ClearErrors();

			// 3. No fields ?
			if (model.Fields == null || model.Fields.Count == 0)
			{
				CollectError(_currentModel.Name, "0 fields on model");
				return BuildValidationResult<DataModelSchema>(_currentModel);
			}

			_basicTypes = new AppSchemaBasicTypes();	

			// 4. Check datatypes
			foreach (var field in model.Fields)
			{
				try
				{
				    var hasName = !string.IsNullOrEmpty(field.Name);
				    var hasType = !string.IsNullOrEmpty(field.DataType);
                    
					// Error 1 : No field name
					CollectError(!hasName, _currentModel.Name, "Field name must be supplied");

					// Error 2: No type specified.
					CollectError(!hasType, _currentModel.Name, "Field data type for : " + field.Name + " must be supplied.");

					// Check type
				    if (!hasName || !hasType)
				        continue;

					var type = field.DataType.ToLower();
					    
                    // Case 1: implicity model ( property is type image/video/copy )
                    if (field.IsImplicitModel)
                    {
                            
                    }
                    // Case 1: @category ( reference to lookup value ).
                    else if (field.IsRefType)
                    {
                        ValidateReference(field, type);
                    }
                    // Case 2: list:number
                    else if (field.IsListType)
                    {
                        ValidateList(field, type);
                    }
                    // Case 3: basic type ? number,bool,text,date
                    else if (!_basicTypes.HasType(type))
                    {
                        CollectError(_currentModel.Name, "Unknown type supplied : " + type);
                    }
				}
				catch (Exception ex)
				{
					throw new ArgumentException("Erorr while validating schema for model : " + model.Name + ", field : " + field.Name);
				}
			}
			return BuildValidationResult<DataModelSchema>(_currentModel);
		}


		private void ValidateList(DataModelProperty prop, string type)
		{
			var fieldName = prop.Name;
			if (!_schema.HasLookup(prop.RefObject) && !_basicTypes.HasType(prop.RefObject) && !_schema.HasModel(prop.RefObject))
			{
				CollectError(_currentModel.Name, "Unknown lookup type : " + prop.RefObject + " for field " + fieldName);
			}
		}


		private void ValidateReference(DataModelProperty prop, string type)
		{
			if (!_schema.HasLookup(prop.RefObject) && !_schema.HasModel(prop.RefObject))
			{
				CollectError(_currentModel.Name, "Unknown lookup type : " + prop.RefObject + " for field " + prop.Name);
			}
		}
	}
}
