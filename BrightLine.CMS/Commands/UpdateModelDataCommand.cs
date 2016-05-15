using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Models;
using BrightLine.Utility.Commands;

namespace BrightLine.CMS.Commands
{
    public class UpdateModelDataCommand : Command
    {
        private Campaign _campaign;
        private AppImporter _importer;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="importer"></param>
        public UpdateModelDataCommand(Campaign campaign, AppImporter importer) : base("CMS", false)
        {
            _campaign = campaign;
            _importer = importer;
        }


         /// <summary>
         /// This creates all the data for the cms which includes the models, model properties, model instances/records.
         /// This does NOT create the JSON yet, as that is done afterwards once the primary keys are known. ( See CmsServer.ImportSpreadsheet )
         /// </summary>
         /// <returns></returns>
        protected override object ExecuteInternal(object[] args)
         {
             return null;
         }
    }
}
