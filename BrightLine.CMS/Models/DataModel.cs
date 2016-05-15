using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Serialization;

namespace BrightLine.CMS.Models
{
    public class DataModel
    {
        public DataModel(string name)
        {
            Name = name;
            Schema = new DataModelSchema(name);
            Rows = new DataModelRecords() {ModelName = name};
            JsonData = new AppDataModelJSON();
        }


        /// <summary>
        /// The name of this model
        /// </summary>
        public string Name { get; set; }


        public bool IsImplicit { get; set; }


        public bool IsSettings { get; set; }


        /// <summary>
        /// The schema of this model ( e.g. products -> list of model properties )
        /// </summary>
        public DataModelSchema Schema;

        /// <summary>
        /// The rows or data for this model.
        /// </summary>
        public DataModelRecords Rows;


        public AppDataModelJSON JsonData;
    }
}
