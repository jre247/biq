using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Models;

namespace BrightLine.Common.Utility.Spreadsheets
{
    /// <summary>
    /// Class can be used to map a C# class from a row/column in a spreadsheet.
    /// </summary>
    public class EntityMapper<T> : IEntityMapper<T> where T : class
    {
        protected IDictionary<string, EntityField> Fields;
        protected IDictionary<string, PropertyInfo> _propsMap;
        protected List<EntityField> _fieldList;
        protected List<string> _errors;


        /// <summary>
        /// Entity mapper.
        /// </summary>
        public EntityMapper() : this(null)
        {
        }


        public EntityMapper(ISpreadsheetReader reader)
        {
            Reader = reader;
            Fields = new Dictionary<string, EntityField>();
            _fieldList = new List<EntityField>();
            _errors = new List<string>();
            EntityType = typeof (T);
        }


        #region Public properties and methods
        /// <summary>
        /// The spread sheet reader
        /// </summary>
        public ISpreadsheetReader Reader { get; set; }


        /// <summary>
        /// The type for the entity to map to.
        /// </summary>
        public Type EntityType { get; set; }


        /// <summary>
        /// The current data source being mapped.
        /// </summary>
        public string CurrentSource { get; set; }


        /// <summary>
        /// Wehther or not to enable filters during mapping ( setting to tru will call the CanLoad() ) method.
        /// </summary>
        public bool EnableFilter { get; set; }


        /// <summary>
        /// Creates an instance of the entity using the EntityType
        /// </summary>
        /// <returns></returns>
        public virtual object CreateEntity()
        {
            var instance = Activator.CreateInstance(EntityType);
            return instance;
        }


        /// <summary>
        /// Whether or not there are any errors.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasErrors()
        {
            return _errors != null && _errors.Count > 0;
        }


        /// <summary>
        /// Gets errors that occurred during mapping.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetErrors()
        {
            return _errors;
        }

        #endregion


        #region Setup and lookup
        /// <summary>
        /// Registers the sequence of fields ( either from a row or column ) that should be mapped using labels supplied.
        /// The mapping of the fields occurs in a linear approach by mapping the labels to the fields.
        /// </summary>
        /// <param name="fieldNames"></param>
        public void RegisterFieldSequence(List<string> fieldNames)
        {
            _fieldList.Clear();

            foreach (var name in fieldNames)
            {
                if (Fields.ContainsKey(name))
                    _fieldList.Add(Fields[name]);
            }
        }


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        public EntityField AddField<T>(string name, bool isRequired, string defaultVal)
        {
            var type = typeof (T);
            return AddField(name, name, typeof(T).Name, type, isRequired, defaultVal, -1, -1);
        }


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        public EntityField AddField(string label, string name, Type type, bool isRequired, string defaultVal, int minLen, int maxLen)
        {
            return AddField(name, name, type.Name, type, isRequired, defaultVal, -1, -1);
        }


        /// <summary>
        /// Adds a field that should be persisted.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="isRequired"></param>
        /// <param name="defaultVal"></param>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        public EntityField AddField(string label, string name, string typeName, Type type, bool isRequired, string defaultVal, int minLen, int maxLen)
        {
            EntityField field = new EntityField();
            field.Label = label.Trim();
            field.Name = name.Trim();
            field.DataType = type;
            field.IsRequired = isRequired;
            field.DefaultValue = defaultVal;
            field.MinLength = minLen;
            field.MaxLength = maxLen;
            field.TypeName = typeName;
            field.HasTarget = true;

            // Set the index number.
            field.Index = _fieldList.Count - 1;

            // Check if no target. 
            if (field.Name == "NO_TARGET" || field.Name == "CUSTOM_TARGET")
            {
                field.HasTarget = false;
            }
            // Add to map and list.
            Fields[field.Label] = field;
            _fieldList.Add(field);
            return field;
        }
        
        
        /// <summary>
        /// Get the properties map that contains the properties to persist.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, PropertyInfo> GetPropertiesMap()
        {
            return _propsMap;
        }


        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, EntityField> GetFields()
        {
            return Fields;
        }


        /// <summary>
        /// Gets the property for the fieldname supplied.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public PropertyInfo GetPropertyForField(string fieldName)
        {
            if (!Fields.ContainsKey(fieldName) || _propsMap == null)
                return null;
            var field = Fields[fieldName];
            if (!_propsMap.ContainsKey(field.Name))
                return null;
            return _propsMap[field.Name];
        }


        /// <summary>
        /// Whether or not there are any fields ( representing mappings ) in this mapper.
        /// </summary>
        /// <returns></returns>
        public bool HasFields()
        {
            return (Fields != null && Fields.Count > 0);
        }


        /// <summary>
        /// Whether or not a field w/ the supplied name as a label exists in here.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasLabel(string name)
        {
            if (!HasFields())
                return false;
            var exists = (from f in Fields where string.Compare(f.Value.Label, name) == 0 select f).Any();
            return exists;
        }


        /// <summary>
        /// Initialize this mapper, can be used in subclasses.
        /// </summary>
        public virtual void Init()
        {
        }
        #endregion


        #region Entity mapper
        /// <summary>
        /// Whether or not this column/row data supplied can be loaded ( mapped to an entity ).
        /// </summary>
        /// <param name="mappedData"></param>
        /// <returns></returns>
        public virtual bool CanLoad(List<object> mappedData)
        {
            return true;
        }


        /// <summary>
        /// Maps an entity start from the row / col supplied. The number of fields to map is reprsented by the Fields that have been registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startRow">The start row containing the first entity to map</param>
        /// <param name="startCol">The start column containing the first entity to map</param>
        /// <param name="isVertical"></param>
        /// <returns></returns>
        public T MapEntity(int startRow, int startCol, bool isVertical)
        {
            List<object> data = null;

            // 1. Get the number of fields to map ( which reflect the number of columns or rows )
            var numberOfFields = _fieldList.Count;
            T instance = default(T);

            // 2. Get all the cells ( in column or row ) at once.
            if (isVertical)
            {
                data = Reader.LoadColumnOf<object>(startRow, startCol, numberOfFields, false);
            }
            else
            {
                data = Reader.LoadRowAsObjects(startRow, startCol, numberOfFields, false, true);
            }

            var r = startRow;
            var c = startCol;

            bool canLoad = !EnableFilter || (EnableFilter && CanLoad(data));

            // 3. Now map those values to an entity.
            if (canLoad)
            {
                instance = (T)CreateEntity();
                for (var ndxField = 0; ndxField < _fieldList.Count; ndxField++)
                {
                    var field = _fieldList[ndxField];

                    // Target field available ?
                    if (field.HasTarget)
                    {
                        object val = null;
                        try
                        {
                            var prop = _propsMap[field.Name];

                            // Note: Index of value in the cells is tied to index of the field list.
                            val = data[ndxField];

                            // 4. Try mapping each cell to entity field/property value.
                            SpreadsheetHelper.SetValue(instance, val, prop);
                        }
                        catch (Exception ex)
                        {
                            AddError(r, c, field, val);
                        }
                    }

                    // Next field based on mapping across or down.
                    // If entities are going down ( each property of entity is going down / vertical ), then the next property
                    // is on the next row.
                    if (isVertical)
                        r++;
                    else
                        c++;
                }

                // 5. Now run the template method to allow dervied class to massage any data or map any complex objects.
                OnAfterEntityMapped(instance, data, isVertical, startRow, startCol);
            }

            // 6. Finally return the entity fully mapped.
            return instance;
        }
        
        
        /// <summary>
        /// Maps entities either across or down.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet">The name of the sheet containing the entities</param>
        /// <param name="startRow">The start row to map from</param>
        /// <param name="startCol">The start column to map from</param>
        /// <param name="numCols">The number of columns to map</param>
        /// <param name="numRows">The number of rows to map</param>
        /// <param name="isVerticalEntity">Whether or not this mapping is done vertically.</param>
        /// <returns></returns>
        public List<T> MapEntities(string sheet, int startRow, int startCol, int numCols, int numRows, bool isVerticalEntity)
        {
            var items = new List<T>();
            Reader.SetCurrentSheet(sheet);

            var totalEntities = 0;
            var MAX = isVerticalEntity ? numCols : numRows;
            var ndxCol = startCol;
            var ndxRow = startRow;

            // Each col/entity
            while (totalEntities < MAX)
            {
                // Add to list
                var instance = MapEntity(ndxRow, ndxCol, isVerticalEntity);
                items.Add(instance);

                totalEntities++;

                // Move to next entity position.
                if (isVerticalEntity)
                    ndxCol++;
                else
                    ndxRow++;
            }
            return items;
        }


        /// <summary>
        /// Maps the entities where each entity exists in its own COLUMN.
        /// </summary>
        /// <returns></returns>
        public List<T> MapEntitiesDown(string sheet, int startRow, int startCol, int numCols, int numRows)
        {
            return MapEntities(sheet, startRow, startCol, numCols, numRows, true);
        }


        /// <summary>
        /// Maps the entities where each entity exists in its own ROW
        /// </summary>
        /// <returns></returns>
        public List<T> MapEntitiesAcross(string sheet, int startRow, int startCol, int numCols, int numRows)
        {
            return MapEntities(sheet, startRow, startCol, numCols, numRows, false);
        }
        
        
        /// <summary>
        /// Maps the metadata in the sheet so that the mappings themselves can by made dynamic by putting 
        /// getting them from the spreadsheet rather than hardcoding.
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// </summary>
        public void MapMetadata(string sheet, int startRow, int startCol)
        {
            Reader.SetCurrentSheet(sheet);

            var labelCol = startCol + 1;
            var rowCol = Reader.LoadColumn(startRow, labelCol, 60, true, false);
            var lastRow = startRow + rowCol.Count;

            for (var ndx = startRow; ndx < lastRow; ndx++)
            {
                var row = Reader.LoadRow(ndx, labelCol, 7, false, true);
                var isNumber = row[2] == "number";
                var isRquired = row[3] == "required";
                var dataType = isNumber ? typeof(int) : typeof(string);
                AddField(row[0], row[1], row[2], dataType, isRquired, row[6], 0, 0);
            }
        }
        #endregion


        #region Template methods for overriding
        /// <summary>
        /// Template method to execute after an entity has been mapped.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="data"></param>
        /// <param name="isVerticallyMapped"></param>
        /// <param name="row">The row this entity was mapped from</param>
        /// <param name="col">The column this entity was mapped from</param>
        public virtual void OnAfterEntityMapped(T entity, List<object> data, bool isVerticallyMapped, int row, int col)
        {
        }
        #endregion


        #region Helpers
        /// <summary>
        /// Registers all the public/instance properties.
        /// </summary>
        public void RegisterProperties()
        {
            // Already initialized
            if (_propsMap != null && _propsMap.Count > 0)
                return;

            var item = CreateEntity();
            _propsMap = new Dictionary<string, PropertyInfo>();

            // 1. The get properties Name = "getName"
            var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var prop in props)
                _propsMap[prop.Name] = prop;
        }


        /// <summary>
        /// Adds/collects a list of errors during the mapping phase.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="field"></param>
        /// <param name="val"></param>
        protected void AddError(int row, int col, EntityField field, object val)
        {
            var valText = val == null ? null : val.ToString();
            var format = "Unable to load field '{0}' with value '{1}' for type : '{2}' at row : {3}, column {4}";
            var msg = string.Format(format, field.Name, valText, typeof (T).Name, row, col);
            _errors.Add(msg);
        }


        /// <summary>
        /// Adds an error given the message, row, col, fieldname.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="fieldName"></param>
        /// <param name="val"></param>
        /// <param name="message"></param>
        protected void AddError(int row, int col, string fieldName, object val, string prefix)
        {
            var valText = val == null ? null : val.ToString();
            var format = "Unable to load field '{0}' with value '{1}' for type : '{2}' at row : {3}, column {4}";
            var msg = prefix + string.Format(format, fieldName, valText, typeof(T).Name, row, col);
            _errors.Add(msg);
        }
        #endregion
    }
}
