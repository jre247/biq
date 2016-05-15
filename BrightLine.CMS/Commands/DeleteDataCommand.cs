using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Utility.Commands;
using BrightLine.Common.Models;
using BrightLine.Core;

namespace BrightLine.CMS.Commands
{
    public class DeleteDataCommand : Command
    {
        private int _schemaId;
        private AppSchema _schema;
        private CmsModelLookup _modelLookup;
        private CmsModelSet _modelSetFromDatabase;
        private CmsModelSet _modelSetFromExcel;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="schemaId"></param>
        /// <param name="appSchema"></param>
        /// <param name="modelSetFromDatabase"></param>
        /// <param name="modelSetFromExcel"></param>
        public DeleteDataCommand(int schemaId, AppSchema appSchema, CmsModelSet modelSetFromDatabase, CmsModelSet modelSetFromExcel)
            : base("CMS", false)
        {
            _schemaId = schemaId;
            _source = "Schema id : " + _schemaId;
            _schema = appSchema;
            _modelSetFromDatabase = modelSetFromDatabase;
            _modelSetFromExcel = modelSetFromExcel;
        }


        /// <summary>
        /// Execute the clearing of cms data for the supplied schemaid
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override object ExecuteInternal(object[] args)
        {
			var campaignContentModelPropertyValues = IoC.Resolve<ICrudService<CampaignContentModelPropertyValue>>();
			var campaignContentModelInstances = IoC.Resolve<ICrudService<CampaignContentModelInstance>>();

            // 1. Get the delta ( removed items )
            var modelSetDelta = _modelSetFromDatabase.Minus(_modelSetFromExcel);

            foreach(var entry in modelSetDelta.Items())
            {
                // 1. Track removed items by getting distinct 
                var ids = entry.Value;

                if (ids != null && ids.Count > 0)
                {
                    var idList = (from id in ids select id.Value);
                    if (idList != null && idList.Any())
                    {
                        var idArray = idList.ToArray();

                        // 1. Delete all the property values ( in vertical table )
                        campaignContentModelPropertyValues.Delete(p => p.Instance.Id, DeleteTypes.Soft, idArray);

                        // 2. Delete all the child objects ( e.g. 1-to-1 relationships where a column represents a model instance in excel )
                        campaignContentModelInstances.Delete(i => i.Parent.Id, DeleteTypes.Soft, idArray);
                        
                        // 3. Batch delete the instances.
                        campaignContentModelInstances.Delete(i => i.Id, DeleteTypes.Soft, idArray);
                    }
                }
            }
            return null;
        }
    }
}
