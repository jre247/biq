using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Aspose.Cells;

namespace BrightLine.Common.Utility.Spreadsheets
{
	/// <summary>
	/// Helpe class to read columns/rows of data from excel using Aspose.Cells library.
	/// </summary>
	public class SpreadsheetReader : ISpreadsheetReader
	{
		private Workbook _book;
		private Worksheet _sheet;


		/// <summary>
		/// Initialize.
		/// </summary>
		public SpreadsheetReader()
		{
		}


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="setSheet">Specifies whether to set the first sheet as the active sheet (if it exists). True by default.</param>
		public void LoadFile(string filePath, bool setSheet = true)
		{
			_book = new Workbook(filePath);
			if (_book.Worksheets != null && _book.Worksheets.Count > 0)
				SetCurrentSheet(0);
		}


		/// <summary>
		/// Initialize.
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="setSheet">Specifies whether to set the first sheet as the active sheet (if it exists). True by default.</param>
		public void LoadFile(byte[] bytes, bool setSheet = true)
		{
			using (var stream = new MemoryStream(bytes))
			{
				_book = new Workbook(stream);
			}
			if (_book.Worksheets != null && _book.Worksheets.Count > 0)
				SetCurrentSheet(0);
		}


		/// <summary>
		/// Loads the sheet
		/// </summary>
		/// <param name="index"></param>
		public void SetCurrentSheet(int index)
		{
			_sheet = _book.Worksheets[index];
		}

		
		/// <summary>
		/// Loads the sheet
		/// </summary>
		/// <param name="name"></param>
		public void SetCurrentSheet(string name)
		{
			_sheet = _book.Worksheets[name];
		}


		/// <summary>
		/// Whether or not the workbook contains the sheet supplied.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool ContainsSheet(string name)
		{
			return _book.Worksheets[name] != null;
		}


		/// <summary>
		/// Gets the cell value at the row/col supplied.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public object GetCell(int row, int col)
		{
			if (row < 0 || col < 0)
				return null;

			var val = _sheet.Cells[row, col];
			if (val == null || val.Value == null)
				return null;
			return val;
		}


		/// <summary>
		/// Gets the cell value at the row/col supplied.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public object GetCellValue(int row, int col)
		{
			if (row < 0 || col < 0)
				return null;

			var val = _sheet.Cells[row, col];
			if (val == null || val.Value == null)
				return null;
			return val.Value;
		}


		/// <summary>
		/// Loads column data horizontally from the row and start column and including the columns specified.
		/// </summary>
		/// <param name="row">The row to read from</param>
		/// <param name="startColumn">The column to start reading data from</param>
		/// <param name="numColumns">The number of columns to read</param>
		/// <param name="stopAtEmptyColumn">whether to stop reading data when an null/empty column is hit</param>
		/// <param name="fillRemainderColumnsWithNulls">Whether or not to fill remainder columns, based on numColumns, with null values</param>
		/// <returns></returns>
		public List<string> LoadRow(int row, int startColumn, int numColumns, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls)
		{
			var values = new List<string>();
			var ndxCol = startColumn;
			var totalCols = 0;
			while (totalCols < numColumns)
			{
				var cell = _sheet.Cells[row, ndxCol];
				var val = cell.Value;

				if (val != null)
				{
					values.Add(val.ToString());
				}
				else if (stopAtEmptyColumn)
				{
					break;
				}
				else
				{
					values.Add(null);
				}
				totalCols++;
				ndxCol++;
			}
			if (fillRemainderColumnsWithNulls && values.Count < numColumns)
			{
				var count = (numColumns - values.Count) - 1;
				while (count > 0)
				{
					values.Add(null);
					count--;
				}
			}
			return values;
		}


		/// <summary>
		/// Loads column data horizontally from the row and start column and including the columns specified.
		/// </summary>
		/// <param name="startRow">The row to read from</param>
		/// <param name="startColumn">The column to start reading data from</param>
		/// <param name="numColumns">The number of columns to read</param>
		/// <param name="stopAtEmptyColumn">whether to stop reading data when an null/empty column is hit</param>
		/// <param name="fillRemainderColumnsWithNulls">Whether or not to fill remainder columns, based on numColumns, with null values</param>
		/// <returns></returns>
		public List<object> LoadRowAsObjects(int startRow, int startColumn, int numColumns, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls)
		{
			var values = new List<object>();
			var totalCols = 0;
			var ndxCol = startColumn;
			while (totalCols < numColumns)
			{
				var cell = _sheet.Cells[startRow, ndxCol];
				var val = cell.Value;

				if (val != null)
				{
					values.Add(val);
				}
				else if (val == null && stopAtEmptyColumn)
				{
					break;
				}
				else if (val == null && !stopAtEmptyColumn)
				{
					values.Add(null);
				}

				ndxCol++;
				totalCols++;
			}
			if (fillRemainderColumnsWithNulls && values.Count < numColumns)
			{
				var count = (numColumns - values.Count) - 1;
				while (count > 0)
				{
					values.Add(null);
					count--;
				}
			}
			return values;
		}


		/// <summary>
		/// Loads a single column of data vertically  from the start row and start column and including the number of rows supplied.
		/// </summary>
		/// <param name="startRow">The starting row to read from</param>
		/// <param name="column">The column to start reading data from</param>
		/// <param name="numRows">The number of rows to read</param>
		/// <param name="stopAtEmptyColumn">whether to stop reading data when an null/empty column is hit</param>
		/// <returns></returns>
		public List<string> LoadColumn(int startRow, int column, int numRows, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls)
		{
			var values = new List<string>();
			var ndxRow = startRow;
			var totalRows = 0;
			while (totalRows < numRows)
			{
				var cell = _sheet.Cells[ndxRow, column];
				var val = cell.Value;

				if (val != null)
				{
					values.Add(val.ToString());
				}
				else if (val == null && stopAtEmptyColumn)
				{
					break;
				}
				else if (val == null && !stopAtEmptyColumn)
				{
					values.Add(null);
				}
				ndxRow++;
				totalRows++;
			}

			if (fillRemainderColumnsWithNulls && values.Count < numRows)
			{
				var count = (numRows - values.Count) - 1;
				while (count > 0)
				{
					values.Add(null);
					count--;
				}
			}
			return values;
		}


		/// <summary>
		/// Reads a row of bools at the positions supplied.
		/// </summary>
		/// <param name="startRow">The row to read from</param>
		/// <param name="startCol">The starting column to read from ( 0 based )</param>
		/// <param name="count">The number of cells(columns) to read</param>
		/// <param name="stopAtEmptyColumn">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
		/// <param name="continueLoadPredicate"></param>
		/// <param name="fillRemainderColumns">Whether or not to fill in remainder columns ( left from 'count')</param>
		/// <returns>List of values of type T</returns>
		public List<T> LoadRowOf<T>(int startRow, int startCol, int count, bool stopAtEmptyColumn, Func<object, bool> continueLoadPredicate, bool fillRemainderColumns)
		{
			var values = new List<T>();
			var hasPredicate = continueLoadPredicate != null;

			var ndxCol = startCol;
			var total = 0;
			while (total < count)
			{
				var cell = _sheet.Cells[startRow, ndxCol];
				var val = cell.Value;

				// CASE 1: value available and no predicat
				if (val != null && !hasPredicate)
				{
					var tVal = Converter.ConvertTo<T>(val);
					values.Add(tVal);
				}
				// CASE 2: value with predicate check
				else if (val != null && hasPredicate)
				{
					var canInclude = continueLoadPredicate(val);
					if (canInclude)
					{
						var tVal = Converter.ConvertTo<T>(val);
						values.Add(tVal);
					}
					else
					{
						break;
					}
				}
				else if (val == null && stopAtEmptyColumn)
				{
					break;
				}
				else if (val == null)
				{
					values.Add(default(T));
				}
				total++;
				ndxCol++;
			}
			if (fillRemainderColumns && values.Count < count)
			{
				var remainder = (count - values.Count) - 1;
				while (remainder > 0)
				{
					values.Add(default(T));
					remainder--;
				}
			}
			return values;
		}


		/// <summary>
		/// Loads a column of data of the specified type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="startRow">The row to read from</param>
		/// <param name="column">The starting column to read from ( 0 based )</param>
		/// <param name="count">The number of cells(columns) to read</param>
		/// <param name="stopAtEmptyColumn">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
		/// <returns>List of values of type T</returns>
		public List<T> LoadColumnOf<T>(int startRow, int column, int count, bool stopAtEmptyColumn)
		{
			var values = new List<T>();
			var ndxRow = startRow;
			var total = 0;

			while (total < count)
			{
				var cell = _sheet.Cells[ndxRow, column];
				var val = cell.Value;

				// CASE 1: value available and no predicat
				if (val != null)
				{
					var tVal = Converter.ConvertTo<T>(val);
					values.Add(tVal);
				}
				else if (stopAtEmptyColumn)
				{
					break;
				}
				else
				{
					values.Add(default(T));
				}
				ndxRow++;
				total++;
			}
			return values;
		}


		/// <summary>
		/// Reads a column of strings at the positions supplied.
		/// </summary>
		/// <param name="startRow">The row to read from</param>
		/// <param name="startCol">The starting column to read from ( 0 based )</param>
		/// <param name="count">The number of cells(columns) to read</param>
		/// <param name="stopAtEmptyCellOrInvalidValue">Wheter or not to read the number of columns or to stop at an empty cell or cell with an invalid value.</param>
		/// <returns>List of dates</returns>
		public List<string> LoadColumnOfStringsUnique(int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue)
		{
			var values = new List<string>();
			var ndxRow = startRow;
			var total = 0;
			var lookup = new Dictionary<string, bool>();
			while (total < count)
			{
				var cell = _sheet.Cells[ndxRow, startCol];
				var val = cell.Value;

				// CASE 1: value available and no predicat
				if (val != null)
				{
					var tVal = Converter.ConvertTo<string>(val);
					if (!string.IsNullOrEmpty(tVal))
					{
						tVal = tVal.Trim();

						// Duplicate ? stop!
						if (lookup.ContainsKey(tVal))
						{
							break;
						}
						values.Add(tVal);
						lookup[tVal] = true;
					}
				}
				else if (stopAtEmptyCellOrInvalidValue)
				{
					break;
				}
				else
				{
					values.Add(string.Empty);
				}
				ndxRow++;
				total++;
			}
			return values;
		}


		/// <summary>
		/// Searches a row or column for the value supplied.
		/// </summary>
		/// <param name="startRow"></param>
		/// <param name="startCol"></param>
		/// <param name="maxCellsToSearch"></param>
		/// <param name="searchAcross"></param>
		/// <param name="expectedVal"></param>
		/// <returns></returns>
		public CellPosition FindCell(int startRow, int startCol, int maxCellsToSearch, bool searchAcross, string expectedVal)
		{
			var ndxRow = startRow;
			var ndxCol = startCol;
			var total = 0;
			CellPosition pos = null;

			// Keep search until max .
			while (total < maxCellsToSearch)
			{
				var cell = _sheet.Cells[ndxRow, ndxCol];
				var val = cell.Value;

				// CASE 1: value available and no predicat
				if (val != null)
				{
					var tVal = Converter.ConvertTo<string>(val);
					if (!string.IsNullOrEmpty(tVal))
					{
						tVal = tVal.Trim();
						if (string.Compare(tVal, expectedVal) == 0)
						{
							pos = new CellPosition();
							pos.Row = ndxRow;
							pos.Col = ndxCol;
							break;
						}
					}
				}
				if (searchAcross)
					ndxCol++;
				else
					ndxRow++;

				total++;
			}
			return pos;
		}


		/// <summary>
		/// Gets the total rows w/ values.
		/// </summary>
		/// <param name="startRow"></param>
		/// <param name="startCol"></param>
		/// <param name="maxRows"></param>
		/// <returns></returns>
		public int TotalRowsWithValues(int startRow, int startCol, int maxRows)
		{
			var totalRows = 0;
			var ndxRow = startRow;
			while (totalRows <= maxRows)
			{
				var cell = _sheet.Cells[ndxRow, startCol];
				if (cell.Value == null)
				{
					break;
				}
				totalRows++;
				ndxRow++;
			}
			return totalRows;
		}
	}
}
