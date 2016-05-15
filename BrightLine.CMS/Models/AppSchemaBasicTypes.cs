using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Models
{
	/// <summary>
	/// Used to easily lookup the basic data-types supported for properties ( text, number, book, datetime ).
	/// </summary>
	public class AppSchemaBasicTypes
	{
		private Dictionary<string, bool> _basicTypes;

		
		/// <summary>
		/// Initialize types/names.
		/// </summary>
		public AppSchemaBasicTypes()
		{
			_basicTypes = new Dictionary<string, bool>();
			_basicTypes[DataModelConstants.DataType_Text]   = true;
			_basicTypes[DataModelConstants.DataType_Bool]   = true;
			_basicTypes[DataModelConstants.DataType_Date]   = true;
			_basicTypes[DataModelConstants.DataType_Number] = true;
		}


		/// <summary>
		/// Whther or not the type supplied is a supported data type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool HasType(string name)
		{
			if(name == null)
				return false;

			return _basicTypes.ContainsKey(name);
		}
	}
}
