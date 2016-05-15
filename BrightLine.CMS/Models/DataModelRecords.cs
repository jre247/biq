using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility;
using BrightLine.Utility;

namespace BrightLine.CMS.Models
{
    public class DataModelRecords
    {
        private Dictionary<string, List<object>> _lookup;
        private List<string> _keys;
 

        /// <summary>
        /// The schema for this instances.
        /// </summary>
        public DataModelSchema Schema;


        /// <summary>
        /// The name of the model these instances are associated with
        /// </summary>
        public string ModelName { get; set; }


        /// <summary>
        /// The columns that are loaded.
        /// </summary>
        public List<string> Columns { get; set; }


        /// <summary>
        /// The records
        /// </summary>
        public List<List<object>> Data { get; set; }


        /// <summary>
        /// Loads lookups by KEY ( which should be the first column )
        /// </summary>
        public void LoadLookup()
        {
            if (Columns == null || Columns.Count == 0)
                return;

            if (Columns[0] != "key")
                return;

            _lookup = new Dictionary<string, List<object>>();
            _keys = new List<string>();
            foreach (var record in Data)
            {
                var key = Convert.ToString(record[0]).ToLower();
                _lookup[key] = record;
                _keys.Add(key);
            }
        }


        /// <summary>
        /// Whether or not there exists a model with the supplied name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasKey(string name)
        {
            return _lookup.ContainsKey(name.ToLower());
        }


        public string GetKeyAt(int rowIndex)
        {
            return _keys[rowIndex];
        }


        /// <summary>
        /// Gets the model with the supplied name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<object> GetRecordByKey(string name)
        {
            if (!HasKey(name))
                return null;
            return _lookup[name.ToLower()];
        }


        /// <summary>
        /// Validates that the number of columns in each record matches the number of columns supplied.
        /// </summary>
        /// <returns></returns>
        public BoolMessage ValidateRecordColumnCounts()
        {
            var expectedColumnCount = Columns.Count;

            var allValid = true;
            var message = "";
            for (var ndx = 0; ndx < Data.Count; ndx++)
            {
                var record = Data[ndx];
                if (record.Count != expectedColumnCount)
                {
                    allValid = false;
                    message = "Record : " + (ndx + 1) + " does not have " + expectedColumnCount + " columns.";
                    break;
                }
            }
            return new BoolMessage(allValid, message);
        }


        public override string ToString()
        {
            return ModelName;
        }
    }
}
