using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Models
{
	/// <summary>
	/// Represents "lookup tables" used in the app schema.
	/// 
	/// EXAMPLES OF 4 LOOKUP TABLES FOR LOREAL
	/// 
	///	"Lookups":
	///	[
	///		{
	///			"Name": "Season",
	///			"Values": ["Winter", "Summer", "Spring", "Fall", "Evergreen" ]
	///		},
	///		{		
	///			"Name": "Brand",
	///			"Values": ["GAR","MNY","LOP","REDK","LANC","RL","YSL","LRP","SSC"]
	///		},
	///		{
	///			"Name": "ColorFamily",
	///			"Values": ["Red","Pink","Purple","Orange","Green","Blue","Yellow","Black","Brown","White","Gold","Silver"]			
	///		},
	///		{
	///			"Name": "Category",
	///			"Values": ["Eyes","Lips","Hair","Nails","Face","Fragrance"]			
	///		}
	///	],
	/// </summary>
	public class DataLookupTable
	{
		private Dictionary<string, int> _valuLookup;


		public string Name { get; set; }
		public List<string> Values { get; set; }
		public List<int> ValuesIds { get; set; }


		/// <summary>
		/// Loads the properties into a lookup table.
		/// </summary>
		public void LoadLookup()
		{
			_valuLookup = new Dictionary<string, int>();

			if (Values == null)
				return;

			var ndx = 1;
			ValuesIds = new List<int>();
			foreach (var val in Values)
			{ 
				ValuesIds.Add(ndx);
				_valuLookup[val] = ndx;
				ndx++;
			}
		}


		public bool Contains(string val)
		{
			return _valuLookup != null && _valuLookup.ContainsKey(val);
		}


		public int GetValue(string val)
		{
			if(!Contains(val))
				return -1;
			return _valuLookup[val];
		}


        public override string ToString()
        {
            return Name;
        }
	}
}
