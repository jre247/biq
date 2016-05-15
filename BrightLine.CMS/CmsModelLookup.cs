using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Validators;
using BrightLine.Core;

namespace BrightLine.CMS
{
    public class CmsModelLookup
    {   
        private Campaign _campaign;
        private bool _isExistingSchema;
        private Dictionary<string, CampaignContentModel> _modelLookup;
        private Dictionary<string, Dictionary<string, CampaignContentModelInstance>> _modelInstanceLookup;
        private Dictionary<string, Dictionary<string, CampaignContentModelProperty>> _modelProperties;
        private Dictionary<string, Dictionary<string, CampaignContentModelPropertyValue>> _modelPropertiesValues;
        private DataModelProperty _systemFormatProperty;
        private DataModelProperty _systemTypeProperty;
        private DataModelProperty _systemPublishableProperty;
        private CmsModelTypeValidator _validatorForType;
        private CmsModelFormatValidator _validatorForFormat;
        private int _schemaId;
        private CampaignContentSchema _contentSchema;


        public CmsModelLookup(Campaign campaign, int schemaId)
        {
            _campaign = campaign;
            _schemaId = schemaId;

			var campaignContentSchemas = IoC.Resolve<ICrudService<CampaignContentSchema>>();

            _contentSchema = campaignContentSchemas.Get(_schemaId);
            _validatorForType = new CmsModelTypeValidator();
            _validatorForFormat = new CmsModelFormatValidator();
        }


        public void LoadModels(bool reload)
        {
			var campaignContentModels = IoC.Resolve<ICrudService<CampaignContentModel>>();

            // Already loaded.
            if (_modelLookup != null && _modelLookup.Count > 0 && !reload)
                return;

            _modelLookup = new Dictionary<string, CampaignContentModel>();

            // New import - schema doesn't exist.
            if (_schemaId < 0)
                return;

            var items = campaignContentModels.Where(m => m.Schema.Id == _schemaId);
            if (items != null && items.Any())
            {
                foreach (var model in items)
                {
                    _modelLookup[model.ModelName] = model;
                }
            }
        }


        public void LoadModelInstances()
        {
            if (_modelLookup == null || _modelLookup.Count == 0)
                return;

            foreach (var modelPair in _modelLookup)
            {
                // Load all the instances by name of model.
                LoadModelInstances(modelPair.Key);
            }
        }


        public void LoadModelInstances(string modelName)
        {
            // Create lookup
            if (_modelInstanceLookup == null)
                _modelInstanceLookup = new Dictionary<string, Dictionary<string, CampaignContentModelInstance>>();

            // New import - schema doesn't exist.
            if (_schemaId < 0)
                return;

            // New model from import ( doesn't exist ).
            if (!_modelLookup.ContainsKey(modelName))
                return;

            // Get existing model
            var model = _modelLookup[modelName];

            // Get existing instances.
            var items = model.Instances;
            if (items != null && items.Any())
            {
                var first = items.First();

                // Create lookup for model instances.
                if (!_modelInstanceLookup.ContainsKey(first.Model.ModelName))
                    _modelInstanceLookup[first.Model.ModelName] = new Dictionary<string, CampaignContentModelInstance>();

                var instances = _modelInstanceLookup[first.Model.ModelName];

                foreach (var instance in items)
                {
                    instances[instance.Key] = instance;
                }
            }
        }


        public void LoadModelProperties(int schemaId, string modelName)
        {
			var campaignContentModelProperties = IoC.Resolve<ICrudService<CampaignContentModelProperty>>();

            // Create lookup
            if (_modelProperties == null)
                _modelProperties = new Dictionary<string, Dictionary<string, CampaignContentModelProperty>>();

            // New import - schema doesn't exist.
            if (schemaId < 0)
                return;

            // New model from import ( doesn't exist ).
            if (!_modelLookup.ContainsKey(modelName))
                return;

            // Get existing model
            var model = _modelLookup[modelName];

            // Get existing instances.
            var items = campaignContentModelProperties.Where(m => m.Model.Id == model.Id);
            if (items != null && items.Any())
            {
                var first = items.First();

                // Create lookup for model instances.
                if (!_modelProperties.ContainsKey(first.Model.ModelName))
                    _modelProperties[first.Model.ModelName] = new Dictionary<string, CampaignContentModelProperty>();

                var props = _modelProperties[first.Model.ModelName];

                foreach (var prop in items)
                {
                    props[prop.Name] = prop;
                }
            }
        }


        public List<CampaignContentModelPropertyValue> GetAllPropertyValues(string modelName)
        {
			var campaignContentModelPropertyValues = IoC.Resolve<ICrudService<CampaignContentModelPropertyValue>>();

            // New model from import ( doesn't exist ).
            if (!_modelLookup.ContainsKey(modelName))
                return null;

            // Get existing model
            var model = _modelLookup[modelName];

            // Get existing instances.
            var items = campaignContentModelPropertyValues.Where(m => m.Model.Id == model.Id, true);
            if (items == null || !items.Any())
                return null;
            return items.ToList();
        }

        public void LoadModelPropertiesValues(int schemaId, string modelName)
        {
			var campaignContentModelPropertyValues = IoC.Resolve<ICrudService<CampaignContentModelPropertyValue>>();

            // Create lookup
            if (_modelPropertiesValues == null)
                _modelPropertiesValues = new Dictionary<string, Dictionary<string, CampaignContentModelPropertyValue>>();

            // New import - schema doesn't exist.
            if (schemaId < 0)
                return;

            // New model from import ( doesn't exist ).
            if (!_modelLookup.ContainsKey(modelName))
                return;

            // Get existing model
            var model = _modelLookup[modelName];

            // Get existing instances.
            var items = campaignContentModelPropertyValues.Where(m => m.Model.Id == model.Id, true);
            if (items != null && items.Any())
            {
                var first = items.First();

                // Create lookup for model instances.
                if (!_modelPropertiesValues.ContainsKey(first.Model.ModelName))
                    _modelPropertiesValues[first.Model.ModelName] = new Dictionary<string, CampaignContentModelPropertyValue>();

                var propsValues = _modelPropertiesValues[first.Model.ModelName];

                foreach (var propValue in items)
                {
                    var propName = propValue.Instance.Key + propValue.Property.Name;
                    propsValues[propName] = propValue;
                }
            }
        }


        /// <summary>
        /// Returns list of id and corresponding key(non-changing).
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public CmsModelSet GetModelSet()
        {
            var modelSet = new CmsModelSet();
            foreach (var modelPair in _modelLookup)
            {
                var model = modelPair.Value;
                var uniqueIds = new Dictionary<string, int>();
                if (model.Instances != null)
                {
                    foreach (var instance in model.Instances)
                    {
                        if (instance != null )
                            uniqueIds[instance.Key] = instance.Id;
                    }
                    modelSet.Add(modelPair.Key, uniqueIds);
                }
            }
            return modelSet;
        }


        public CampaignContentModel GetOrCreateModel(string modelName)
        {
            if (_modelLookup.ContainsKey(modelName))
                return _modelLookup[modelName];

            var model = new CampaignContentModel
            {
                InstancesDataJson = string.Empty,
                ModelName = modelName,
                Schema = _contentSchema
            };
            return model;
        }


        public CampaignContentModelInstance GetModelInstance(string modelName, string key)
        {
            if (!_modelLookup.ContainsKey(modelName))
                return null;

            var modelInstances = _modelInstanceLookup[modelName];
            if (modelInstances.ContainsKey(key))
            {
                return modelInstances[key];
            }
            return null;
        }


        public CampaignContentModelInstance GetOrCreateModelInstance(CampaignContentModel contentModel, string key, bool removeDeletedFlags)
        {
            string modelName = contentModel.ModelName;

            if (_modelInstanceLookup.ContainsKey(modelName))
            {
                var modelInstances = _modelInstanceLookup[modelName];
                if (modelInstances.ContainsKey(key))
                {
                    var item = modelInstances[key];
                    if (item.IsDeleted && removeDeletedFlags)
                    {
                        item.IsDeleted = false;
                        item.DateDeleted = null;
                    }
                    return item;
                }
            }

            var instance = new CampaignContentModelInstance()
            {
                Schema = _contentSchema,
                Model = contentModel,
                Key = key,
                ModelName = modelName,
                InstanceData = string.Empty,
                InstanceDataJson = string.Empty,
                IsPublishable = true
            };
            return instance;
        }


        public CampaignContentModelProperty GetOrCreateModelProperty(CampaignContentModel contentModel, string propName)
        {
            string modelName = contentModel.ModelName;

            if (_modelProperties.ContainsKey(modelName))
            {
                var props = _modelProperties[modelName];
                if (props.ContainsKey(propName))
                    return props[propName];
            }

            var prop = new CampaignContentModelProperty();
            return prop;
        }


        public CampaignContentModelPropertyValue GetOrCreateModelPropertyValue(CampaignContentModel contentModel, string key, string propName, bool removeDeletedFlags)
        {
            string modelName = contentModel.ModelName;
            CampaignContentModelPropertyValue prop = null;

            if (_modelPropertiesValues.ContainsKey(modelName))
            {
                var recordPropKey = key + propName;
                var props = _modelPropertiesValues[modelName];
                if (props.ContainsKey(recordPropKey))
                {
                    prop = props[recordPropKey];
                    if (prop.IsDeleted && removeDeletedFlags)
                    {
                        prop.IsDeleted = false;
                        prop.DateDeleted = null;
                    }

                    return props[recordPropKey];
                }
            }

            prop = new CampaignContentModelPropertyValue();
            return prop;
        }


        public void UpdateSystemFields(CampaignContentModelInstance instance, List<object> record)
        {
            if (_systemFormatProperty != null)
            {
                var format = record[_systemFormatProperty.Position];
                var formatResult = _validatorForFormat.Validate(format as string);
                instance.Format = formatResult.Item;
            }
            if (_systemTypeProperty != null)
            {
                var type = record[_systemTypeProperty.Position];
                var typeResult = _validatorForType.Validate(type as string);
                instance.Type = typeResult.Item;
            }
            if (_systemPublishableProperty != null)
            {
                var isPublishableText = record[_systemPublishableProperty.Position];
                var isPublishable = Convert.ToBoolean(isPublishableText);
                instance.IsPublishable = isPublishable;
            }
            // Default to true.
            else
            {
                instance.IsPublishable = true;
            }
        }


        public void LoadSystemProperites(DataModel model)
        {
            _systemFormatProperty = model.Schema.GetField("system:format");
            _systemTypeProperty = model.Schema.GetField("system:type");
            _systemPublishableProperty = model.Schema.GetField("system:publish");
        }
    }
}
