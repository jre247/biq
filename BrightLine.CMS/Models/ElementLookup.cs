using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Models;

namespace BrightLine.CMS.Models
{
    public class ElementLookup
    {

        private AppSchema _schema;
        private Dictionary<string, CampaignContentModel> _modelMap;
        private Dictionary<string, Dictionary<string, CampaignContentModelInstance>> _modelToModelKeyIds;
        private int _schemaId;

        public ElementLookup(AppSchema schema, Dictionary<string, CampaignContentModel> map, int schemaId)
        {
            _schema = schema;
            _modelMap = map;
            _schemaId = schemaId;
            _modelToModelKeyIds = new Dictionary<string, Dictionary<string, CampaignContentModelInstance>>();
        }


        public CampaignContentModelInstance GetModelRecord(string modelName, string key)
        {
            if (!_modelToModelKeyIds.ContainsKey(modelName))
                return null;

            var model = _modelToModelKeyIds[modelName];
            if (!model.ContainsKey(key))
                return null;

            return model[key];
        }


        public int GetModelRecordId(string modelName, string key)
        {
            var model = _modelToModelKeyIds[modelName];
            return model[key].Id;
        }


        public int GetModelRecordFormatId(string modelName, string key)
        {
            var model = _modelToModelKeyIds[modelName];
            if(!model.ContainsKey(key))
                return -1;
            var instance = model[key];
            return instance.Format == null ? -1 : instance.Format.Id;
        }


        public int GetModelRecordTypeId(string modelName, string key)
        {
            var model = _modelToModelKeyIds[modelName];
            if (!model.ContainsKey(key))
                return -1;
            var instance = model[key];
            return instance.Type == null ? -1 : instance.Type.Id;
        }


        public CampaignContentModel GetContentModel(string name)
        {
            return _modelMap[name];
        }


        public void InitLookups()
        {
            foreach (var model in _schema.Models.Models)
            {
                var contentModel = _modelMap[model.Name];

                // Create a lookup of key => id 
                var idmap = new Dictionary<string, CampaignContentModelInstance>();
                foreach (var instance in contentModel.Instances)
                {
                    idmap[instance.Key] = instance;
                }
                _modelToModelKeyIds[model.Name] = idmap;
            }
        }
    }
}
