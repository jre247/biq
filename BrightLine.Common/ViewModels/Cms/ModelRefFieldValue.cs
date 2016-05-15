using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Models
{
	public class ModelRefFieldValue
	{
		private const string ModelRefSchema =
		@"{
			'type': 'array',
			'items': {
				'type': 'object',
				'properties': {
					'model': {'type':'string'},
					'instanceId': {'type':'integer'}
				}
			}
		}";

		public string model { get; set; }
		public int instanceId { get; set; }

		public ModelRefFieldValue() { }

		public ModelRefFieldValue(string modelIn, int instanceIdIn)
		{
			model = modelIn;
			instanceId = instanceIdIn;
		}

		/// <summary>
		/// Validate that a string value can be deserialized to ModelRefFieldValue
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsValidModelRef(string value)
		{
			var schema = JsonSchema.Parse(ModelRefSchema);
			var valueParsed = JArray.Parse(value);
			var isValid = valueParsed.IsValid(schema);
			return isValid;
		}
	}
}
