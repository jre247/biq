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
using BrightLine.Common.Models.Validators;
using BrightLine.Common.Utility;
using BrightLine.Utility.Commands;
using BrightLine.Core;
using BrightLine.Utility.Helpers;

namespace BrightLine.CMS.Commands
{
    public class CreateModelDataCommand : Command
    {
        private Campaign _campaign;
        private AppImporter _importer;
        private CmsModelLookup _modelLookup;
        private int _schemaId;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="importer"></param>
        /// <param name="modelLookup"></param>
        /// <param name="schemaId"></param>
        public CreateModelDataCommand(Campaign campaign, AppImporter importer, CmsModelLookup modelLookup, int schemaId) : base("CMS", false)
        {
            _campaign = campaign;
            _importer = importer;
            _modelLookup = modelLookup;
            _schemaId = schemaId;
            if (_campaign != null)
                _source = "Campaign : " + _campaign.Id;
        }


        /// <summary>
        /// This creates all the data for the cms which includes the models, model properties, model instances/records.
        /// This does NOT create the JSON yet, as that is done afterwards once the primary keys are known. ( See CmsServer.ImportSpreadsheet )
        /// </summary>
        /// <returns></returns>
        protected override object ExecuteInternal(object[] args)
        {
            // 1. Get the schema and data.
            var appSchema = _importer.GetSchema();
            var models = appSchema.Models;

            // 2. Now create each instance of each model type in the database.
            var modelsSvc = IoC.Resolve<ICrudService<CampaignContentModel>>();
			var modelBaseFormats = ExpressionHelper.KeyToEntityiesBy<string, CampaignContentModelBaseType>(e => e.Name);
			var modelBaseTypes = ExpressionHelper.KeyToEntityiesBy<string, CampaignContentModelType>(e => e.Name);
			var propertyTypes = ExpressionHelper.KeyToEntityiesBy<string, CampaignContentModelPropertyType>(e => e.Name);
               
            // 3. Model map
            var modelMap = new Dictionary<string, CampaignContentModel>();

            // 3. Build up the instances.
            for (var ndx = 0; ndx < models.Models.Count; ndx++)
            {
                // Model data ( Raw + published format from the importer ).
                var model = models.Models[ndx];
                var modelSchema = model.Schema;

                // 4. Create content model
                var contentModel = _modelLookup.GetOrCreateModel(model.Name);
                if (contentModel.Instances == null)
                {
                    contentModel.Instances = new List<CampaignContentModelInstance>();
                }
                // 5. Load existing models instance and properties
                _modelLookup.LoadModelProperties(_schemaId, contentModel.ModelName);
                _modelLookup.LoadModelInstances(contentModel.ModelName);

                // 6. Set the model basetype if applicable.
                CmsHelper.ConfigureBaseType(modelSchema, contentModel, modelBaseFormats);

                // 7. Setup the model type / format.
                var anySystemFields = model.Schema.AnyFields(p => p.IsSystemLevel);
                if (anySystemFields)
                {
                    _modelLookup.LoadSystemProperites(model);
                }
                // 7. Create all the instances of the model.
                for (var ndxRecord = 0; ndxRecord < model.Rows.Data.Count; ndxRecord++)
                {
                    var record = model.Rows.Data[ndxRecord];

                    var key = record[0];
                    var keyVal = key == null ? "" : key.ToString();
                    var instance = _modelLookup.GetOrCreateModelInstance(contentModel, keyVal, true);
                    
                    // Update the model type/format
                    if (anySystemFields)
                    {
                        _modelLookup.UpdateSystemFields(instance, record);
                    }

                    if (instance.IsNewEntity)
                    {
                        contentModel.Instances.Add(instance);
                    }
                }

                // 8. Persist the model in the dabase.
                modelsSvc.Cud(contentModel);
                
                // 9. Create the model meta properties if available
                CreateModelProperties(modelSchema, contentModel, propertyTypes);

                // 10. We have the model type info
                modelMap[modelSchema.Name] = contentModel;
            }

            return modelMap;
        }


        /// <summary>
        /// Creates model properties ( in a vertical table ) for properties that need to be reported on.
        /// e.g. Where meta flag is true for a DataModelProperty.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="contentModel"></param>
        /// <param name="propertyTypes"></param>
        public void CreateModelProperties(DataModelSchema model, CampaignContentModel contentModel, IDictionary<string, CampaignContentModelPropertyType> propertyTypes)
        {
			var campaignContentModelProperties = IoC.Resolve<ICrudService<CampaignContentModelProperty>>();

            if (model.AnyFields(p => p.IsMetaType))
            {
                var metaFields = model.GetFields(p => p.IsMetaType);
                foreach (var field in metaFields)
                {
                    var modelProp = _modelLookup.GetOrCreateModelProperty(contentModel, field.Name);
                    var dataType = propertyTypes["string"];
                    if (propertyTypes.ContainsKey(field.DataType))
                        dataType = propertyTypes[field.DataType];
                    modelProp.Name = field.Name;
                    modelProp.IsRequired = field.Required;
                    modelProp.Model = contentModel;
                    modelProp.Metatype = "";
                    modelProp.PropertyType = dataType;

                    campaignContentModelProperties.Cud(modelProp);
                }
            }
        }
    }
}
