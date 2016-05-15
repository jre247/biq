using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Serialization
{
	public class AppDataJSON
	{
		/// <summary>
		/// The schema in JSON format
		/// </summary>
		public string Schema { get; set ;}


		/// <summary>
		/// All the data in JSON format.
		/// </summary>
		public string Data { get; set; }


		/// <summary>
		/// All the data in raw JSON format ( this is json representation of the app models datastructure ).
		/// This is just a table of rows ( where each item in the row is an object )
		/// Table based format, not heirarchical format that is published to couch db for the design team.
		/// </summary>
		public string DataRaw { get; set; }


		/// <summary>
		/// A key value of each model type and all the models instances in JSON.
		/// </summary>
		public Dictionary<string, AppDataModelJSON> Models { get; set; }	
	}
}
