using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BrightLine.CMS.Models;
using BrightLine.Common.Models;
using Newtonsoft.Json;

namespace BrightLine.CMS.Serialization
{
	public class AppDataSerializer
	{
		/// <summary>
		/// Serialize app schema to json content
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public string SerializeSchema(AppSchema schema)
		{
			var json = JsonConvert.SerializeObject(schema, Formatting.Indented);
			return json;
		}
		
		
		/// <summary>
		/// Serialize app schema to json content
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public string SerializeData(DataModels models)
		{
			var json = JsonConvert.SerializeObject(models, Formatting.Indented);
			return json;
		}


		/// <summary>
		/// Serialize the schema and data models to file path supplied.
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="dataModels"></param>
		/// <param name="path"></param>
		public void SerializeAllDataToPublishedFormatFiles(AppSchema schema, DataModels dataModels, string directory, bool enableNewLines)
		{
			if (string.IsNullOrEmpty(directory))
				throw new ArgumentException("File path must be supplied");
			
			SerializeAllDataToPublishedFormat(schema, dataModels, enableNewLines, null);

			// 1. write out schema
			var path = Path.Combine(directory, "app_" + schema.Name + "_schema.js");
			File.WriteAllText(path, schema.JsonSchema);

			// 2. write out data contains ALL models and their instances.
			path = Path.Combine(directory, "app_" + schema.Name + "_data.js");
			File.WriteAllText(path, schema.JsonData);

			// 3. Write out individual model instances.
			foreach (var model in schema.Models.Models)
			{
				path = Path.Combine(directory, "app_" + schema.Name + "_" + model.Name + "_data.js");			
				File.WriteAllText(path, model.JsonData.All);
			}
		}


		/// <summary>
		/// Serialize the data into 
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="dataModels"></param>
		/// <param name="path"></param>
		public void SerializeAllDataToPublishedFormat(AppSchema schema, DataModels dataModels, bool enableNewLines, ElementLookup elementLookup)
		{
			var jsonAll = "{ \"models\": [";

			// 1. Go trhough each model type.
			var lastIndex = dataModels.Models.Count -1;
			for(var ndx = 0; ndx < dataModels.Models.Count; ndx++)			
			{
				var converter = new DataModelJSONConverter(schema, dataModels);
				var model = dataModels.Models[ndx];
			    converter.ElementLookup = elementLookup;

                var jsonModel = enableNewLines
									   ? converter.ConvertToDocument(model.Rows)
									   : converter.ConvertToTable(model.Rows);

				// 2. Add model name and it's full json to lookup
			    model.JsonData = jsonModel;

				// 3. Build up json with all the model data.
				var separator = lastIndex == ndx ? "" : ",";
				jsonAll += jsonModel.All + separator ;
			}
			jsonAll += "] }";
			
			// 4. Now set the full data ( all models in json )
			schema.JsonData = jsonAll;
			schema.JsonSchema = SerializeSchema(schema);
			schema.JsonDataRaw = SerializeData(dataModels);
		}


		/// <summary>
		/// Deserialize app models from json content
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public DataModels DeserializeData(string content)
		{
			var data = JsonConvert.DeserializeObject<DataModels>(content);
			return data;
		}


		/// <summary>
		/// Deserialize app schema from json content
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public AppSchema DeserializeSchema(string content)
		{
			var appSchema = JsonConvert.DeserializeObject<AppSchema>(content);
			return appSchema;
		}
	}
}
