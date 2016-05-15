using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Utility.Commands;
using BrightLine.Core;

namespace BrightLine.CMS.Commands
{
    public class CreateImplicitModelsCommand : Command
    {
        private CmsModelLookup _modelLookup;
        private AppSchema _appSchema;
        private ElementLookup _elementLookup;


        public CreateImplicitModelsCommand(AppSchema schema, CmsModelLookup modelLookup, ElementLookup elementLookup)
            : base("CMS", false)
        {
            _modelLookup = modelLookup;
            _appSchema = schema;
            _elementLookup = elementLookup;
        }


        /// <summary>
        /// This creates all the data for the cms which includes the models, model properties, model instances/records.
        /// This does NOT create the JSON yet, as that is done afterwards once the primary keys are known. ( See CmsServer.ImportSpreadsheet )
        /// </summary>
        /// <returns></returns>
        protected override object ExecuteInternal(object[] args)
        {
            // 3. Build up the instances.
            var dataModels = _appSchema.Models;
            for (var ndx = 0; ndx < dataModels.Models.Count; ndx++)
            {
                // Model data ( Raw + published format from the importer ).
                var model = dataModels.Models[ndx];
                CreateImplicitModels(model);
            }
            return null;
        }


        private void CreateImplicitModels(DataModel model)
        {
			var campaignContentModels = IoC.Resolve<ICrudService<CampaignContentModel>>();

            // Any properties that should become their own models ?
            var fields = model.Schema.GetFields(p => p.IsImplicitModel);
            if (fields == null || fields.Count == 0)
                return;

            // Create the models for the props
            foreach (var modelProp in fields)
            {
                var childModel = _modelLookup.GetOrCreateModel(modelProp.ImplicitModelName);
                childModel.Instances = new List<CampaignContentModelInstance>();

                // Match up the key
                for (var ndxRecord = 0; ndxRecord < model.Rows.Data.Count; ndxRecord++)
                {
                    var record = model.Rows.Data[ndxRecord];

                    var key = record[0];
                    var keyVal = key == null ? "" : key.ToString();
                    var parent = _elementLookup.GetModelRecord(model.Name, keyVal);
                    var instance = _modelLookup.GetOrCreateModelInstance(childModel, keyVal, true);
                    instance.Parent = parent;
                    childModel.Instances.Add(instance);
                }

                campaignContentModels.Cud(childModel);
            }
        }
    }
}
