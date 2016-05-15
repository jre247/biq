using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.AppImport
{
	/// <summary>
	/// Contains unit-testable business rules.
	/// </summary>
	public class AppImportRules
	{
        /// <summary>
        /// Whether or not the property is a system level property ( used for cms infrastructure related purposes )
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static bool IsSystemLevelProperty(string propName)
        {
            if (string.IsNullOrEmpty(propName))
                return false;
            if (propName.ToLower().StartsWith("system:"))
                return true;
            return false;
        }
        
        
        /// <summary>
        /// Whether or not the property is a system level property ( used for cms infrastructure related purposes )
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static bool IsModelLevelProperty(string propName)
        {
            if (string.IsNullOrEmpty(propName))
                return false;
            if (propName.ToLower().StartsWith("model:"))
                return true;
            return false;
        }


		/// <summary>
		/// Whether or not the record for a specific model/sheet can be imported based on its filter.
		/// </summary>
		/// <returns></returns>
		public static bool CanImportRecord(ModelConfig.ModelFilter filter, List<object> record)
		{
			// 1. No filter ? import the record.
			if(filter == null)
				return  true;
			
			var valueToCheck = record[filter.ColumnIndex];

			// 2. Null value for cell in filter column ? Don't import.
			if(valueToCheck == null)
				return false;

			// 3. Now check the value matches the filter value expected.
			// Rule... must be lowercase text ( easiest way to go for now )
			var filterValText = valueToCheck.ToString().ToLower().Trim();
			if (filterValText == filter.Value)
				return true;

			// 4. Did not match
			return false;
		}



        public static void ApplyDefaults(List<Models.DataModelProperty> defaultedProps, List<object> record)
        {
            if (defaultedProps == null || defaultedProps.Count == 0)
                return;
            if (record == null)
                return;

            for (int ndx = 0; ndx < defaultedProps.Count; ndx++)
            {
                if (ndx < record.Count)
                {
                    var prop = defaultedProps[ndx];

                    // Only set default value if no value there currently
                    var val = record[prop.Position];

                    // Case 1: null value
                    if (val == null)
                    {
                        record[prop.Position] = prop.DefaultValue;
                    }
                    // Case 2: empty string.
                    else
                    {
                        if (val is string )
                        {
                            var s = val as string;
                            if (string.IsNullOrEmpty(s))
                            {
                                record[prop.Position] = prop.DefaultValue;
                            }
                        }
                    }
                }
            }
        }
    }
}
