using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public static class SpreadsheetExtensions
    {
        /// <summary>
        /// Reads a row of dates at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <param name="fillRemainderColumns">Whether or not to fill in remainder columns ( left from 'count')</param>
        /// <returns>List of dates</returns>
        public static List<string> ReadRowOfStrings(this ISpreadsheetReader reader, int startRow, int startCol, int count,
                                               bool stopAtEmptyCellOrInvalidValue, bool fillRemainderColumns)
        {
            return reader.LoadRowOf<string>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue, null, fillRemainderColumns);
        }
        
        
        /// <summary>
        /// Reads a row of dates at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <param name="fillRemainderColumns">Whether or not to fill in remainder columns ( left from 'count')</param>
        /// <returns>List of dates</returns>
        public static List<DateTime> ReadRowOfDates(this ISpreadsheetReader reader, int startRow, int startCol, int count,
                                               bool stopAtEmptyCellOrInvalidValue, bool fillRemainderColumns)
        {
            return reader.LoadRowOf<DateTime>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue, null, fillRemainderColumns);
        }


        /// <summary>
        /// Reads a row of numbers at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <param name="fillRemainderColumns">Whether or not to fill in remainder columns ( left from 'count')</param>
        /// <returns>List of dates</returns>
        public static List<double> ReadRowOfNumbers(this ISpreadsheetReader reader, int startRow, int startCol, int count,
                                               bool stopAtEmptyCellOrInvalidValue, bool fillRemainderColumns)
        {
            return reader.LoadRowOf<double>(startRow, startCol, count, true, null, true);
        }


        /// <summary>
        /// Reads a row of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <param name="fillRemainderColumns">Whether or not to fill in remainder columns ( left from 'count')</param>
        /// <returns>List of dates</returns>
        public static List<bool> ReadRowOfBools(this ISpreadsheetReader reader, int startRow, int startCol, int count,
                                               bool stopAtEmptyCellOrInvalidValue, bool fillRemainderColumns)
        {
            return reader.LoadRowOf<bool>(startRow, startCol, count, true, null, true);
        }


        /// <summary>
        /// Reads a column of strings at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<string> ReadColumnOfStrings(this ISpreadsheetReader reader, int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
        {
            return reader.LoadColumnOf<string>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue);
        }


        /// <summary>
        /// Reads a column of strings at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<string> ReadColumnOfStringsUnique(this ISpreadsheetReader reader, int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
        {
            return reader.LoadColumnOfStringsUnique(startRow, startCol, count, stopAtEmptyCellOrInvalidValue);
        }
        
        
        /// <summary>
        /// Reads a column of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<DateTime> ReadColumnOfDates(this ISpreadsheetReader reader, int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
        {
            return reader.LoadColumnOf<DateTime>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue);
        }


        /// <summary>
        /// Reads a column of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<double> ReadColumnOfNumbers(this ISpreadsheetReader reader, int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
        {
            return reader.LoadColumnOf<double>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue);
        }


        /// <summary>
        /// Reads a column of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="count">The number of cells(columns) to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<bool> ReadColumnOfBools(this ISpreadsheetReader reader, int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
        {
            return reader.LoadColumnOf<bool>(startRow, startCol, count, stopAtEmptyCellOrInvalidValue);
        }


        /// <summary>
        /// Reads a column of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="countRows">The number of rows to read</param>
        /// /// <param name="countCols">The number of cols to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<List<T>> ReadMultiColumnOf<T>(this ISpreadsheetReader reader, int startRow, int startCol, int countRows, int countCols, bool stopAtEmptyCellOrInvalidValue)
        {
            var table = new List<List<T>>();
            for (var ndx = startCol; ndx < startCol + countCols; ndx++)
            {
                var colData = reader.LoadColumnOf<T>(startRow, ndx, countRows, stopAtEmptyCellOrInvalidValue);
                table.Add(colData);
            }
            return table;
        }


        /// <summary>
        /// Reads a column of bools at the positions supplied.
        /// </summary>
        /// <param name="reader">The spreadsheet reader</param>
        /// <param name="startRow">The row to read from</param>
        /// <param name="startCol">The starting column to read from ( 0 based )</param>
        /// <param name="countRows">The number of rows to read</param>
        /// /// <param name="countCols">The number of cols to read</param>
        /// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
        /// <returns>List of dates</returns>
        public static List<List<T>> ReadMultiRowsOf<T>(this ISpreadsheetReader reader, int startRow, int startCol, int countRows, int countCols, bool stopAtEmptyCellOrInvalidValue)
        {
            var table = new List<List<T>>();
            for (var ndx = startRow; ndx < startRow + countRows; ndx++)
            {
                var colData = reader.LoadRowOf<T>(ndx, startCol, countCols, false, null, true);
                table.Add(colData);
            }
            return table;
        }


        /// <summary>
        /// Gets the cell value and converts to the type specified or supplys the default value supplied if there is no cell value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetCellValueOrDefault<T>(this ISpreadsheetReader reader, int startRow, int startCol, T defaultValue)
        {
            var val = reader.GetCellValue(startRow, startCol);
            if (val == null)
                return defaultValue;
            if (!Converter.CanConvertTo<T>(val.ToString()))
                return defaultValue;
            T convertedVal = Converter.ConvertTo<T>(val);
            return convertedVal;
        }
    }
}
