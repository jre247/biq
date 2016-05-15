using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Models;
using BrightLine.CMS.Serialization;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Utility;
using BrightLine.Utility.Commands;
using BrightLine.Core;

namespace BrightLine.CMS.Commands
{
	public class CreateModelDataJsonCommand : Command
	{
		private Campaign _campaign;
		private AppImporter _importer;
		private ElementLookup _elementLookup;
		private int _schemaId;


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="campaign"></param>
		/// <param name="importer"></param>
		/// <param name="elementLookup"></param>
		/// <param name="schemaId"></param>
		public CreateModelDataJsonCommand(Campaign campaign, AppImporter importer, ElementLookup elementLookup, int schemaId)
			: base("CMS", false)
		{
			_campaign = campaign;
			_importer = importer;
			_elementLookup = elementLookup;
			_schemaId = schemaId;
		}


		/// <summary>
		/// This updates the existing models ( now with primary keys known ) by generating the json with the "_meta" property that contains ids.
		/// </summary>
		/// <returns></returns>
		protected override object ExecuteInternal(object[] args)
		{
			var campaignContentSchemas = IoC.Resolve<ICrudService<CampaignContentSchema>>();
			var campaignContentModels = IoC.Resolve<ICrudService<CampaignContentModel>>();

			var schema = _importer.GetSchema();
			var models = schema.Models;
			var elementLookup = _elementLookup;

			// 4. Now serialize the data as json ( include the "_meta" field that will show the primary 
			var serializer = new AppDataSerializer();
			serializer.SerializeAllDataToPublishedFormat(schema, schema.Models, true, elementLookup);

			// 5. Update the json for all the models.
			for (var ndx = 0; ndx < schema.Models.Models.Count; ndx++)
			{
				var model = schema.Models.Models[ndx];
				var contentModel = elementLookup.GetContentModel(model.Name);

				// Plug in the json data.
				var jsonRecords = model.JsonData;
				var instances = contentModel.Instances.ToList();
				contentModel.InstancesDataJson = jsonRecords.All;

				for (var ndxRecord = 0; ndxRecord < model.Rows.Data.Count; ndxRecord++)
				{
					var key = model.Rows.GetKeyAt(ndxRecord);
					var instance = (from i in contentModel.Instances
									where string.Compare(i.Key, key, StringComparison.InvariantCultureIgnoreCase) == 0
									select i).FirstOrDefault();
					if (instance != null)
					{
						// Have to match up the instances by the key.
						// NOTE: Can not rely on the indexes because there could be fewere rows in excel ( deleted, is active = false )
						// compared to the number of rows from the database.
						instance.InstanceDataJson = jsonRecords.InstancesPublished[ndxRecord];
						instance.InstanceData = jsonRecords.InstancesRaw[ndxRecord];
					}
				}
				campaignContentModels.Update(contentModel);
			}

			// 6. Update the schema with all the json data
			var contentSchema = campaignContentSchemas.Get(_schemaId);
			contentSchema.DataModelsPublished = schema.JsonData;
			campaignContentSchemas.Update(contentSchema);

			return null;
		}
	}
}