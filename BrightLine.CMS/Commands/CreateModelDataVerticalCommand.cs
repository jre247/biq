using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.AppImport;
using BrightLine.CMS.Models;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Utility.Commands;
using BrightLine.Core;

namespace BrightLine.CMS.Commands
{
	public class CreateModelDataVerticalCommand : Command
	{
		private Campaign _campaign;
		private AppImporter _importer;
		private ElementLookup _elementLookup;
		private int _schemaId;
		private Dictionary<string, CampaignContentModelProperty> _propertyLookup;
		private CmsModelLookup _modelLookup;


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="campaign"></param>
		/// <param name="importer"></param>
		public CreateModelDataVerticalCommand(Campaign campaign, AppImporter importer, ElementLookup lookup, int schemaId, CmsModelLookup modelLookup)
			: base("CMS", false)
		{
			_campaign = campaign;
			_importer = importer;
			_elementLookup = lookup;
			_schemaId = schemaId;
			_modelLookup = modelLookup;
		}


		public static string CurrentModelName;
		public static string CurrentKey;


		/// <summary>
		/// Import spreadsheet using an existing importer.
		/// </summary>
		/// <returns></returns>
		protected override object ExecuteInternal(object[] args)
		{
			var campaignContentModelProperties = IoC.Resolve<ICrudService<CampaignContentModelProperty>>();
			var campaignContentModelPropertyValues = IoC.Resolve<ICrudService<CampaignContentModelPropertyValue>>();

			var schema = _importer.GetSchema();
			var anyVerticalFields = false;

			_modelLookup.LoadModels(true);

			// 1. Go through each model
			foreach (var model in schema.Models.Models)
			{
				CurrentModelName = model.Name;

				// 2. Check if schema has any meta fields ? need to put the values into the CampaignContentModelPropertyValue
				var metaFields = model.Schema.GetFields((prop) => prop.IsMetaType);

				if (metaFields != null && metaFields.Count > 0)
				{
					anyVerticalFields = true;
					var contentModel = _elementLookup.GetContentModel(model.Name);
					var contentModelProperties = campaignContentModelProperties.Where(p => p.Model.Id == contentModel.Id).ToList();
					_propertyLookup = new Dictionary<string, CampaignContentModelProperty>();
					foreach (var prop in contentModelProperties)
						_propertyLookup[prop.Name] = prop;

					// 3. Go through each record of the model and use existing vertical properties.
					int ndx = 0;
					_modelLookup.LoadModelInstances(model.Name);
					_modelLookup.LoadModelPropertiesValues(_schemaId, model.Name);
					var propValues = _modelLookup.GetAllPropertyValues(model.Name);

					// First time creating ?
					if (propValues == null)
						propValues = new List<CampaignContentModelPropertyValue>();
                    
                    // keep a collection of only new and changed entities
                    var changedAndNew = new List<CampaignContentModelPropertyValue>();

					for (var ndxRecord = 0; ndxRecord < model.Rows.Data.Count; ndxRecord++)
					{
						// Get the key for this model ( unique per model in the sheet where it came from ).
						List<object> rawRecord = model.Rows.Data[ndxRecord];
						var key = model.Rows.GetKeyAt(ndxRecord);
						var instance = _modelLookup.GetModelInstance(model.Name, key);
						CurrentKey = key;

						// Get the matching origin RAW record loaded from excel.
						// NOTE: The record may not exist
						if (rawRecord != null)
						{
							// 4. Now go through each of the metadata fields for this row.
							for (var ndxField = 0; ndxField < metaFields.Count; ndxField++)
							{
								var metaField = metaFields[ndxField];
								var metaValue = rawRecord[metaField.Position];

								// Now create all the metadata values.
								// NOTES: This is a vertical table for a record ( but only stores a partial set of the fields for the model instance ).
								// 1. The number of records created in vertical table size is N records x N fields.
								// 2. Instead of storing every property of the model, this only stores the metadata properties needed for reporting )
								// 3. Worst case scenario: 10 records , each record has 10 metafields = 100 rows of data
								// 4. Best  case scenario: 10 records , each record has 1 metafield = 10 rows of data.
								try
								{
									var property = _propertyLookup[metaField.Name];
									var propValue = GetOrCreatePropertyValue(contentModel, instance, metaField);
									var isNullValue = metaValue == null;
                                    var changed = false;
									if (!isNullValue)
									{
                                        changed = AssignPropertyValue(metaValue, property, propValue, changed);
									}
									if (propValue.IsNewEntity || changed)
									{
                                        changedAndNew.Add(propValue);
									}
								}
								catch (Exception ex)
								{
									// Rethrow exception here to provide more context to the error.
									// e.g. What model, what record # ?
									throw BuildException(model.Name, key, metaField.Name, ex);
								}
							}
						}
					}

					try
					{
						// 5. Create all the properties at once for this model.
						// NOTE: (CMS, PERFORMANCE, ERROR_REPORTING) => This was originally placed inside the loop above to
						// create / update prop values on a instance by instance basis rather than batch them up for the whole model.
						// The instance by instance save helps in error reporting so we know exactly what row failed.
						// However, that does slow down the save to IQ as there are multiple entity framework calls ( as many models * rows )
						// that have server / vertical properties to save.
						campaignContentModelPropertyValues.Cud(changedAndNew);
					}
					catch (Exception ex)
					{
						// Rethrow exception here to provide more context to the error.
						// e.g. What model, what record # ?
						throw BuildException(model.Name, "", ex);
					}
				}
			}
			return anyVerticalFields;
		}

        /// <summary>
        /// Determines if ModelPropertyValueCampaignContentModelPropertyValue has changed and updates value accordingly
        /// </summary>
		/// <returns>Whether the CampaignContentModelPropertyValue has changed from previous value</returns>
        private static bool AssignPropertyValue(object metaValue, CampaignContentModelProperty property, CampaignContentModelPropertyValue propValue, bool changed)
        {
			// for each type of property, if the value does not exist or has changed, set the value, set all other values as null, and set changed to true

            if (property.PropertyType.Name == "string")
            {
                var val = (string)metaValue;
  
                if (propValue.StringValue == null || !propValue.StringValue.Equals(val))
                {
					changed = true;
                    propValue.StringValue = val;
                    propValue.DateValue = null;
                    propValue.NumberValue = null;
                    propValue.BoolValue = null;
                }

            }
            else if (property.PropertyType.Name == "number")
            {
                var val = (double)metaValue;

                if (propValue.NumberValue == null || !propValue.NumberValue.Equals(val))
                {
					changed = true;
                    propValue.NumberValue = val;
                    propValue.StringValue = null;
                    propValue.DateValue = null;
                    propValue.BoolValue = null;
                }

            }
            else if (property.PropertyType.Name == "bool")
            {
                var val = (bool)metaValue;

                if (propValue.BoolValue == null || !propValue.BoolValue.Equals(val))
                {
					changed = true;
                    propValue.BoolValue = val;
                    propValue.StringValue = null;
                    propValue.DateValue = null;
                    propValue.NumberValue = null;
                }

            }
            else if (property.PropertyType.Name == "datetime")
            {
                var val = (DateTime)metaValue;

                if (propValue.DateValue == null || !propValue.DateValue.Equals(val))
                {
                    changed = true;
                    propValue.DateValue = val;
                    propValue.StringValue = null;
                    propValue.NumberValue = null;
                    propValue.BoolValue = null;
                }

            }
            return changed;
        }


		private CampaignContentModelPropertyValue GetOrCreatePropertyValue(CampaignContentModel contentModel, CampaignContentModelInstance instance, DataModelProperty metaField)
		{
			var property = _propertyLookup[metaField.Name];
			var propValue = _modelLookup.GetOrCreateModelPropertyValue(contentModel, instance.Key, metaField.Name, true);
			propValue.Campaign = _campaign;
			propValue.Model = contentModel;
			propValue.Instance = instance;
			propValue.Property = property;
			propValue.PropertyType = property.PropertyType;
			return propValue;
		}


		private Exception BuildException(string model, string rowKey, Exception ex)
		{
			var message = "Error saving vertical properties for reporting. Model : " + model + ", key : " + rowKey;
			return new Exception(message, ex);
		}


		private Exception BuildException(string model, string rowKey, string prop, Exception ex)
		{
			var message = "Error saving vertical properties for reporting. Model : " + model + ", key : " + rowKey + ", prop : " + prop;
			return new Exception(message, ex);
		}
	}
}
