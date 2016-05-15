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
	public class AppSchemaValidator : ModelValidator
	{
		private AppSchema _schema;


		public AppSchemaValidator(AppSchema appSchema)
		{
			_schema = appSchema;
		}


		/// <summary>
		/// Validate
		/// </summary>
		/// <returns></returns>
		public override bool Validate()
		{
			var result = ValidateSchema();
			return result.Success;
		}


		/// <summary>
		/// Validates the app schema (models and lookups)
		/// </summary>
		/// <param name="appSchema"></param>
		public BoolMessageItem<AppSchema> ValidateSchema()
		{
			_schema.LoadLookups();

			// 1. No models ?
			if( _schema.Models == null || _schema.Models.Count == 0)
			{
				CollectError(null, "0 data models supplied");
				return BuildValidationResult<AppSchema>(_schema);
			}
				
			var validator = new DataModelSchemaValidator(_schema);

			// 2. Check each model.
			foreach (var model in _schema.Models.Models)
			{
				validator.Validate(model.Schema);
				
				// Now collect the errors.
				CollectErrors(validator);
			}
			return BuildValidationResult<AppSchema>(_schema);
		}
	}
}
