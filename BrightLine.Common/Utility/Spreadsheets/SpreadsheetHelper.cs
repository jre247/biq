using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility.Spreadsheets.Writer;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public class SpreadsheetHelper
    {
        /// <summary>
        /// Simple "factory" method to return default reader ( using aspose cells )
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetReader GetReader()
        {
            return new SpreadsheetReader();
        }


        /// <summary>
        /// Simple "factory" method to return default reader ( using aspose cells ) using file path.
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetReader GetReader(string filePath)
        {
            var reader = new SpreadsheetReader();
            reader.LoadFile(filePath);
            return reader;
        }


        /// <summary>
        /// Simple "factory" method to return default reader ( using aspose cells ) using file path.
        /// </summary>
        /// <returns></returns>
        public static ISpreadsheetReader GetReader(byte[] content)
        {
            var reader = new SpreadsheetReader();
            reader.LoadFile(content);
            return reader;
        }


        /// <summary>
        /// Converts a value ( from excel ) to its property type value.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="val"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static void SetValue(object instance, object val, PropertyInfo prop )
        {

            // Conver to correct type.
            if (prop.PropertyType == typeof(int?) && val == null)
            {
                prop.SetValue(instance, null, null);
            }
            else if (prop.PropertyType == typeof(int?) && val is double || val is int)
            {
                var intVal = Convert.ToInt32(val);
                prop.SetValue(instance, intVal, null);
            }
            else if (prop.PropertyType == typeof(int?) && val is DateTime)
            {
                var seconds = Convert.ToInt32(((DateTime)val).TimeOfDay.TotalSeconds);
                prop.SetValue(instance, seconds, null);
            }
            else if (prop.PropertyType == typeof(double?) && val == null)
            {
                prop.SetValue(instance, null, null);
            }
            else if (prop.PropertyType == typeof(double?) && val != null)
            {
                var dVal = Convert.ToDouble(val);
                prop.SetValue(instance, dVal, null);
            }
            else if (prop.PropertyType == typeof (byte?) && val != null)
            {
                var bval = Convert.ToByte(val);
                prop.SetValue(instance, bval, null);
            }
            else
            {
                var tVal = Convert.ChangeType(val, prop.PropertyType);
                prop.SetValue(instance, tVal, null);
            }
        }


        public static ISpreadSheetWriter GetWriter()
        {
            return new SpreadsheetWriter();
        }
    }
}
