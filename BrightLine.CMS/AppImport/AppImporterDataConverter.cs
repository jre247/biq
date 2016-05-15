using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Utility;
using BrightLine.Utility.Validation;


namespace BrightLine.CMS.AppImport
{
    /// <summary>
    /// Converts the data extracted from excel file from its raw format to a more meaningful format that is reflective of the data types of each model.
    /// For example: Model videos may have a column to store a list of numbers such as "1,2". 
    /// 1. Convert strings to their actual property type ( e.g. date, bool, number, etc )
    /// 2. Convert a list of strings to a list of numbers ( if property type is list of numbers )
    /// </summary>
    public class AppImporterDataConverter : ModelValidator
	{
		private AppSchema _schema;
		private DataModels _models;
		private AppSchemaBasicTypes _basicTypes;
        private DataModel _currentModel;
        private DataModelProperty _currentProp;
        private string _currentColumnName;
        private string _currentRowKey;
        private int _currentRow;


		/// <summary>
		/// Initialize
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="models"></param>
		public AppImporterDataConverter(AppSchema schema)
		{
			_schema = schema;
		    _models = _schema.Models;
			_basicTypes = new AppSchemaBasicTypes();
		}


		public void ConvertData()
		{
			// Handle references to other models. ( convert from strings to number or list of number )
		    var implicitModels = new List<DataModel>();
			foreach (var model in _schema.Models.Models)
			{
				var instances = _models.GetModel(model.Name);

                // Used for recording errors about current model.
                _currentModel = instances;

                ConvertData(model);

			    var implicitModelFields = _currentModel.Schema.GetFields(p => p.IsImplicitModel);
                if (implicitModelFields != null && implicitModelFields.Count > 0)
                {
                    CreateImplicitModel(_currentModel, implicitModelFields, implicitModels);
                }
			}
		    foreach (var implicitModel in implicitModels)
		        _schema.Models.Add(implicitModel);
		}
		
		
		/// <summary>
		/// Gets the fields in the sequence supplied.
		/// </summary>
		/// <param name="fields"></param>
		/// <returns></returns>
		public List<int> GetModelFieldsToMassage(List<DataModelProperty> fields )
		{
			var positions = new List<int>();
			for (var ndx = 0; ndx < fields.Count; ndx++)
			{
				var field = fields[ndx];
				if (field.IsRefType || field.IsListType)
					positions.Add(ndx);
                else if (!field.IsListType && (field.IsString() || field.IsDate() || field.IsBool()))
                    positions.Add(ndx);
                        
			}
			return positions;
		}


        private void ConvertData(DataModel model)
        {
            // Ref fields, lists, etc.
            var refColumns = GetModelFieldsToMassage(model.Schema.Fields);
            ConvertData(refColumns, model.Schema.Fields, model.Rows.Data);
        }



        private void ConvertData(List<int> refColumns, List<DataModelProperty> fields, List<List<object>> records   )
        {
            if (refColumns == null || refColumns.Count == 0)
                return;

            for (var ndx = 0; ndx < refColumns.Count; ndx++)
            {
                var colIndex = refColumns[ndx];
                var prop = fields[colIndex];
                _currentColumnName = prop.FullName();

                // Now convert data into either a single id or a list of ids.
                _currentRow = 0;
                _currentRowKey = "";
                foreach (var record in records)
                {
                    var data = record[colIndex];
                    _currentRow++;
                    _currentRowKey = record[0] == null ? "" : record[0].ToString();

                    if (data == null)
                        continue;

                    // NON-LIST TYPE
                    var isList = prop.IsListType;
                    // CASE : Text field
                    if (!isList && prop.DataType == DataModelConstants.DataType_Text && data is string)
                    {
                        var text = (string)data;
                        record[colIndex] = ConvertString(text);
                    }
                    // CASE : boolean field
                    if (!isList && prop.DataType == DataModelConstants.DataType_Bool && data is string)
                    {
                        var text = (string)data;
                        record[colIndex] = ConvertBool(text);
                    }
                    // CASE : DateTime field
                    if (!isList && prop.DataType == DataModelConstants.DataType_Date && data is string)
                    {
                        var text = (string)data;
                        record[colIndex] = ConvertDate(text);
                    }
                    // CASE : DateTime field
                    if (!isList && prop.DataType == DataModelConstants.DataType_Text && !(data is string))
                    {
                        var text = ConvertStringExact(data);
                        record[colIndex] = text;
                    }
                    // CASE 1: Single object reference e.g. ref-brand
                    else if (prop.IsRefType && !prop.IsListType && data.GetType() == typeof(double))
                    {
                        var refId = Convert.ToString(data);
                        record[colIndex] = refId;
                    }
                    // CASE 2a: List of reference e.g. list:ref-brand but only 1 supplied.
                    else if (prop.IsListType && data.GetType() == typeof(double))
                    {
                        var refId = Convert.ToString(data);
                        record[colIndex] = new List<string>() { refId };
                    }
                    // CASE 2b: List of reference e.g. list:ref-brand but only 1 supplied.
                    else if (prop.IsListType && prop.IsRefType && data.GetType() == typeof(string))
                    {
                        record[colIndex] = AppImporterHelper.ParseStringList(data, true);
                    }
                    // CASE 3a: List of strings ( "k,dog,bx" )
                    else if (prop.IsListOfStrings())
                    {
                        record[colIndex] = AppImporterHelper.ParseStringList(data, false);
                    }
                    // CASE 3b List of numbers ( "1,2,3,4" )
                    else if (prop.IsListOfNumbers())
                    {
                        record[colIndex] = BuildListOf<double>(data, ConvertNumber);
                    }
                    // CASE 3c List of bools ( true, false, true, false )
                    else if (prop.IsListOfBools())
                    {
                        record[colIndex] = BuildListOf<bool>(data, ConvertBool);
                    }
                    // CASE 3d List of dates
                    else if (prop.IsListOfDates())
                    {
                        record[colIndex] = BuildListOf<DateTime>(data, ConvertDate);
                    }
                }
            }
        }


        private List<T> BuildListOf<T>(object data, Func<string,T> converter)
        {
            var items = new List<T>();
            var text = Convert.ToString(data);

            if (string.IsNullOrEmpty(text))
                return items;

            // Only 1.
            if (text.IndexOf(",") < 0)
            {
                T val = converter(text);
                items.Add(val);
                return items;
            }
            // Multiple
            if (text.IndexOf(",") > 0)
            {
                var tokens = text.Split(',');
                foreach (var token in tokens)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        T val = converter(token);
                        items.Add(val);
                    }
                }
            }
            return items;
        }


        private double ConvertNumber(string valText)
        {
            var val = 0.0;
            if (!double.TryParse(valText, out val))
            {
                CollectModelRecordError(_currentModel.Name, _currentRow, _currentRowKey, _currentColumnName, "Invalid number : " + valText);
            }
            return val;
        }


        private bool ConvertBool(string valText)
        {
            var val = false;
            if (!bool.TryParse(valText, out val))
            {
                CollectModelRecordError(_currentModel.Name, _currentRow, _currentRowKey, _currentColumnName, "Invalid true/false : " + valText);
            }
            return val;
        }


        private DateTime ConvertDate(string valText)
        {
            var val = DateTime.Now;
            if (!DateTime.TryParse(valText, out val))
            {
                CollectModelRecordError(_currentModel.Name, _currentRow, _currentRowKey, _currentColumnName, "Invalid date : " + valText);
            }
            return val;
        }


        private string ConvertString(string val)
        {
            if (string.IsNullOrEmpty(val))
                return string.Empty;
            return val.Trim();
        }


        private string ConvertStringExact(object data)
        {
            if (data is string)
                return data.ToString().Trim();
            if (data is DateTime)
                return ((DateTime) data).ToShortDateString();
            return data.ToString();
        }


        private void CreateImplicitModel(DataModel parentModel, List<DataModelProperty> implicitModelFields, List<DataModel> implicitModels )
        {
            foreach (var propModel in implicitModelFields)
            {
                // 1. Create the data model.
                var implicitModel = new DataModel(propModel.ImplicitModelName);
                implicitModel.IsImplicit = true;
                implicitModel.Rows.Data = new List<List<object>>();
                implicitModel.Rows.Columns = new List<string>();

                // 2. Create the columns ( key/property )
                var hasIsActive = parentModel.Schema.HasField("isActive");
                var propIsActive = parentModel.Schema.GetField("isActive");
                implicitModel.Schema.AddField("key", DataModelPropertyTypes.Text, false, string.Empty, false, -1, propModel.Required, propModel.DefaultValue, string.Empty);
                if(hasIsActive) implicitModel.Schema.AddField("isActive", DataModelPropertyTypes.Bool, false, string.Empty, false, -1, propModel.Required, propModel.DefaultValue, string.Empty);
                implicitModel.Schema.AddField(propModel.Name, DataModelPropertyTypes.Text, false, string.Empty, false, -1, propModel.Required, propModel.DefaultValue, string.Empty);
                
                // 3. Create the sequence of columns in the rows
                implicitModel.Rows.Columns.Add("key");
                if (hasIsActive) implicitModel.Rows.Columns.Add("isActive");
                implicitModel.Rows.Columns.Add(propModel.Name);
                implicitModel.Rows.Schema = implicitModel.Schema;

                // 4. Now populate records.
                foreach (var record in _currentModel.Rows.Data)
                {
                    var implicitModelRecord = new List<object>();
                    var key = record[0].ToString();
                    implicitModelRecord.Add(key);
                    if (hasIsActive)
                    {
                        var isActive = record[propIsActive.Position];
                        implicitModelRecord.Add(isActive);
                    }
                    implicitModelRecord.Add(record[propModel.Position]);
                    implicitModel.Rows.Data.Add(implicitModelRecord);

                    // 5. Alter the origin property value to refer to this new record
                    record[propModel.Position] = key;
                }

                // 5. Add to list of implicit models.
                implicitModels.Add(implicitModel);

                // 6. Now change the model has been auto-created ( instead of user having to do so ), change the type of the property 
                // to reflect the reference to the model and type
                propModel.ConfigureReferenceType(implicitModel.Name, getSingularName(implicitModel.Name));

                // 7. Load the schemas models lookup ( with the new implicit model ).
                _schema.Models.LoadLookup(implicitModel);
            }
        }


        private string getSingularName(string name)
        {
            if (name.EndsWith("s"))
                return name.Substring(0, name.Length - 1);
            return name;
        }
	}
}
