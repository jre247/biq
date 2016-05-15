using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS
{
    public class CmsModelSet
    {
        private Dictionary<string, Dictionary<string, int>> _uniqueInstancesByModel;


        public CmsModelSet()
        {
            _uniqueInstancesByModel = new Dictionary<string, Dictionary<string, int>>();
        }


        /// <summary>
        /// Add the set of unique records ( id, string ) 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="uniques"></param>
        public void Add(string modelName, Dictionary<string, int> uniques)
        {
            _uniqueInstancesByModel[modelName] = uniques;
        }



        /// <summary>
        /// Creates a delta set of model instances ( to know which ones were deleted ) by subtracting "this" minus "set2"
        /// </summary>
        /// <param name="set2"></param>
        /// <returns></returns>
        public CmsModelSet Minus(CmsModelSet excelSet)
        {
            var removed = new CmsModelSet();

            foreach (var modelEntry in _uniqueInstancesByModel)
            {
                var instancesThis = modelEntry.Value;
                var modelName = modelEntry.Key;
                if (excelSet._uniqueInstancesByModel.ContainsKey(modelName))
                {
                    var excelInstances = excelSet._uniqueInstancesByModel[modelName];

                    var removedInstances = new Dictionary<string, int>();

                    // Now get unique primary keys (ids)
                    foreach (var tuple in instancesThis)
                    {
                        // REMOVED !
                        if (!excelInstances.ContainsKey(tuple.Key))
                            removedInstances[tuple.Key] = tuple.Value;
                    }
                    removed.Add(modelName, removedInstances);
                }
            }
            return removed;
        }


        public Dictionary<string, Dictionary<string, int>> Items()
        {
            return _uniqueInstancesByModel;
        }
    }
}
