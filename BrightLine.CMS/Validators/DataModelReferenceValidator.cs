using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.Validators
{
	public class DataModelReferenceValidator : ModelValidator
	{
		private AppSchema _schema;
		private DataModels _models;


		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="models"></param>
		public DataModelReferenceValidator(AppSchema schema, DataModels models)
		{
			_schema = schema;
			_models = models;
		}


		/// <summary>
		/// Validate
		/// </summary>
		/// <returns></returns>
		public override bool Validate()
		{
			var result = ValidateReferences();
			return result.Success;
		}


		/// <summary>
		/// Validates the value against the property metadata.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="val"></param>
		public BoolMessageItem<DataModels> ValidateReferences()
		{
			foreach (var model in _models.Models)
			{
				ValidateReferences(model.Schema, model.Rows);
			}
			return BuildValidationResult<DataModels>(_models);
		}


		private void ValidateReferences(DataModelSchema modelSchema, DataModelRecords instances)
		{
			var refColumns = _schema.GetModelFieldsWithReferenceTypePositions(modelSchema.Name);
			var model = _schema.GetModel(modelSchema.Name);

			if (refColumns == null || refColumns.Count == 0)
				return;

			// Check all column that have model references.
			for (var ndx = 0; ndx < refColumns.Count; ndx++)
			{
				var colIndex = refColumns[ndx];
				var prop = model.Schema.Fields[colIndex];
				var refType = prop.RefObject;
				var refInstances = _models.GetModel(refType).Rows;
				
				// Now convert data into either a single id or a list of ids.
				var rowNum = 0;
				foreach (var record in instances.Data)
				{
					rowNum ++;

					var key = record[0];
					var data = record[colIndex];

					// No model references... go to next record.
					if (data == null)
						continue;					

					// CASE 1: single model reference.
					if (data.GetType() == typeof(string))
					{
						var refkey = (string) data;
						if (!refInstances.HasKey(refkey))
						{
							CollectModelRecordError(modelSchema.Name, rowNum, key, prop.Name, "reference model id : " + refkey + " not found for model " + refInstances.ModelName );
						}
					}
					// CASE 2: multiple model references.
					else if (data.GetType() == typeof(List<string>))
					{
						var refkeys = (List<string>)data;
						foreach (var refkey in refkeys)
						{ 
							// Avoid "null" explicit checks.
							if (refkey != "null")
							{
								if (!refInstances.HasKey(refkey))
								{
									CollectModelRecordError(modelSchema.Name, rowNum, key, prop.Name, "reference model id : " + refkey + " not found for model " + refInstances.ModelName);
								}
							}
							else
							{
								//Console.WriteLine("null found for : " + instances.ModelName + " row: " + rowNum + ", key : "+ key + ", prop : " + prop.Name);
							}
						}
					}					
				}
			}
		}
	}
}
