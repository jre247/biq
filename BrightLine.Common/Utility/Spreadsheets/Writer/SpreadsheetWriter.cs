using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aspose.Cells;
using System.Data;


namespace BrightLine.Common.Utility.Spreadsheets.Writer
{
	public class SpreadsheetWriter : ISpreadSheetWriter
	{
		private Workbook _workbook;
		private Worksheet _worksheet;

		/// <summary>
		/// Get the currrent worksheet name
		/// </summary>
		public string CurrentSheet
		{
			get { return _worksheet == null ? string.Empty : _worksheet.Name; }
		}


		/// <summary>
		/// Creates a new workbook.
		/// </summary>
		public void CreateWorkBook()
		{
			_workbook = new Workbook();

			// Remove Sheet1  if it exists
			if (_workbook.Worksheets[0] != null)
				_workbook.Worksheets.RemoveAt(0);
		}


		/// <summary>
		/// Creates a new worksheet and sets it as the current worksheet for writing.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="setAsCurrent"></param>
		public void CreateSheet(string name, bool setAsCurrent)
		{
			var sheet = _workbook.Worksheets.Add(name);
			if (setAsCurrent)
				_worksheet = sheet;
		}


		/// <summary>
		/// Saves the workbook to a file.
		/// </summary>
		/// <param name="fileName"></param>
		public void Save(string fileName)
		{
			_workbook.Save(fileName);
		}


		/// <summary>
		/// Saves the workbook to a memory stream and returns the stream.
		/// </summary>
		public MemoryStream Save()
		{
			var stream = new MemoryStream();
			//var options = SaveOptions.
			_workbook.Save(stream, SaveFormat.Xlsx);
			return stream;
		}


		/// <summary>
		/// Saves the workbook to a file.
		/// </summary>
		/// <param name="stream">The stream to save to</param>
		public void Save(Stream stream)
		{
			//var options = SaveOptions.
			_workbook.Save(stream, SaveFormat.Xlsx);
		}


		/// <summary>
		/// Writes a table of entities of type T given the properties to serialize.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="props"></param>
		/// <param name="items"></param>
		public void WriteTable<T>(List<System.Reflection.PropertyInfo> props, List<T> items)
		{
			WriteTable<T>(CurrentSheet, props, items);
		}


		/// <summary>
		/// Writes a table of entities of type T given the properties to serialize.
		/// </summary>
		/// <typeparam name="T">The type of data.</typeparam>
		/// <param name="worksheet">The name of the worksheet.</param>
		/// <param name="props"></param>
		/// <param name="items"></param>
		public void WriteTable<T>(string worksheet, List<System.Reflection.PropertyInfo> props, List<T> items)
		{
			WriteTable<T>(worksheet, 0, 0, props, items);
		}


		/// <summary>
		/// Write table from the list of items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sheetName"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="props"></param>
		/// <param name="items"></param>
		public void WriteTable<T>(string sheetName, int row, int col, List<System.Reflection.PropertyInfo> props, List<T> items)
		{
			var names = GetColumnNames(props);
			WriteTable<T>(sheetName, row, col, props, items, names);
		}

		/// <summary>
		/// Write table from the list of items.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sheetName"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="props"></param>
		/// <param name="items"></param>
		/// <param name="names"></param>
		public void WriteTable<T>(string sheetName, int row, int col, List<System.Reflection.PropertyInfo> props, List<T> items, string[] names)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			_worksheet.Cells.ImportCustomObjects(items, names, true, row, col, items.Count, true, "dd/mm/yyyy", true);
			_worksheet.AutoFitColumns();
		}


		/// <summary>
		/// Write table from the list of items.
		/// </summary>
		/// <param name="sheetName"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="items"></param>
		/// <param name="isVertical"></param>
		public void WriteList(string sheetName, int row, int col, IEnumerable items, bool isVertical)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var vals = new ArrayList();
			foreach (var item in items)
			{
				vals.Add(item);
			}
			_worksheet.Cells.ImportArrayList(vals, row, col, isVertical);
			_worksheet.AutoFitColumns();
		}

		public void WriteDataTable(string sheetName, DataTable dataTable, bool fieldNameIsShown, string startCell)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			_worksheet.Cells.ImportDataTable(dataTable, fieldNameIsShown, startCell);
			_worksheet.AutoFitColumns();
		}

		public void FormatColumn(string sheetName, int columnIndex)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var style = BaseStyle();

			var styleFlag = BaseStyleFlag();
			styleFlag.NumberFormat = false;

			_worksheet.Cells.Columns[columnIndex].ApplyStyle(style, styleFlag);
		}

		private string[] GetColumnNames(IEnumerable<PropertyInfo> props)
		{
			var names = (from p in props select p.Name).ToArray();
			return names;
		}

		public void SetIntegerColumn(string sheetName, int columnIndex)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var style = IntegerStyle();

			var styleFlag = BaseStyleFlag();

			_worksheet.Cells.Columns[columnIndex].ApplyStyle(style, styleFlag);
		}

		public void SetPercentageColumn(string sheetName, int columnIndex)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var style = PercentageStyle();

			var styleFlag = BaseStyleFlag();

			_worksheet.Cells.Columns[columnIndex].ApplyStyle(style, styleFlag);
		}

		public void SetDecimalColumn(string sheetName, int columnIndex)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var style = DecimalStyle();

			var styleFlag = BaseStyleFlag();

			_worksheet.Cells.Columns[columnIndex].ApplyStyle(style, styleFlag);
		}

		public void SetTimeStampColumn(string sheetName, int columnIndex)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			var style = TimeStampStyle();

			var styleFlag = BaseStyleFlag();

			_worksheet.Cells.Columns[columnIndex].ApplyStyle(style, styleFlag);
		}

		public void SetIntegerCells(string sheetName, params Tuple<string, string>[] ranges)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			foreach (var item in ranges)
			{
				var range = _worksheet.Cells.CreateRange(item.Item1, item.Item2);
				var style = IntegerStyle();
				var flag = BaseStyleFlag();
				flag.All = true;
				range.ApplyStyle(style, flag);
			}
		}

		public void SetPercentageCells(string sheetName, params Tuple<string, string>[] ranges)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			foreach (var item in ranges)
			{
				var range = _worksheet.Cells.CreateRange(item.Item1, item.Item2);
				var style = PercentageStyle();
				var flag = BaseStyleFlag();
				flag.All = true;
				range.ApplyStyle(style, flag);
			}
		}

		public void SetDecimalCells(string sheetName, params Tuple<string, string>[] ranges)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			foreach (var item in ranges)
			{
				var range = _worksheet.Cells.CreateRange(item.Item1, item.Item2);
				var style = DecimalStyle();
				var flag = BaseStyleFlag();
				flag.All = true;
				range.ApplyStyle(style, flag);
			}
		}

		public void SetTimeStampCells(string sheetName, params Tuple<string, string>[] ranges)
		{
			_worksheet = _workbook.Worksheets[sheetName];
			foreach (var item in ranges)
			{
				var range = _worksheet.Cells.CreateRange(item.Item1, item.Item2);
				var style = TimeStampStyle();
				var flag = BaseStyleFlag();
				flag.All = true;
				range.ApplyStyle(style, flag);
			}
		}

		public void AutoFitCells(string sheetName = null, Style style = null)
		{
			var sheet = (string.IsNullOrWhiteSpace(sheetName) ? _worksheet : _workbook.Worksheets[sheetName]);
			if (sheet == null)
				return;

			sheet.AutoFitRows();
			sheet.AutoFitColumns();
			SetCellsStyle(sheet, style);
		}

		public void AutoFitRows(string sheetName = null, Style style = null)
		{
			var sheet = (string.IsNullOrWhiteSpace(sheetName) ? _worksheet : _workbook.Worksheets[sheetName]);
			if (sheet == null)
				return;

			sheet.AutoFitRows();
			SetCellsStyle(sheet, style);
		}

		public void AutoFitColumns(string sheetName = null, Style style = null)
		{
			var sheet = (string.IsNullOrWhiteSpace(sheetName) ? _worksheet : _workbook.Worksheets[sheetName]);
			if (sheet == null)
				return;

			sheet.AutoFitColumns();
			SetCellsStyle(sheet, style);
		}

		private void SetCellsStyle(Worksheet sheet, Style style)
		{
			if (sheet == null || style == null)
				return;

			foreach (var cell in sheet.Cells)
			{
				var c = cell as Cell;
				c.SetStyle(style);
			}
		}


		private Style BaseStyle()
		{
			return new Style
			{
				HorizontalAlignment = TextAlignmentType.Center
			};
		}

		private StyleFlag BaseStyleFlag()
		{
			return new StyleFlag
			{
				HorizontalAlignment = true,
				NumberFormat = true,
			};
		}

		private Style IntegerStyle()
		{
			var style = BaseStyle();
			style.Number = 3; //  #,##0

			return style;
		}

		private Style DecimalStyle()
		{
			var style = BaseStyle();
			style.Number = 4; // #,##0.00

			return style;
		}

		private Style PercentageStyle()
		{
			var style = BaseStyle();
			style.Number = 10; // 0.00%

			return style;
		}

		private Style TimeStampStyle()
		{
			var style = BaseStyle();
			style.Number = 45; // mm:ss

			return style;
		}
	}
}
