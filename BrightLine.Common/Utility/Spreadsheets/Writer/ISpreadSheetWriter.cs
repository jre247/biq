using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace BrightLine.Common.Utility.Spreadsheets.Writer
{
    public interface ISpreadSheetWriter
    {
        string CurrentSheet { get; }
        
        void CreateWorkBook();

        void CreateSheet(string name, bool setAsCurrent);

        void Save(string fileName);

        void Save(Stream stream);

	    MemoryStream Save();

        void WriteTable<T>(List<System.Reflection.PropertyInfo> props, List<T> items);

        void WriteTable<T>(string sheetName, List<System.Reflection.PropertyInfo> props, List<T> items);

        void WriteTable<T>(string sheetName, int row, int col, List<System.Reflection.PropertyInfo> props, List<T> items);
		
		void WriteTable<T>(string sheetName, int row, int col, List<System.Reflection.PropertyInfo> props, List<T> items, string[] names);

        void WriteList(string sheetName, int row, int col, IEnumerable items, bool isVertical);
	    
		void WriteDataTable(string sheetName, DataTable dataTable, bool fieldNameIsShown, string startCell);

		void FormatColumn(string sheetName, int columnIndex);

		void SetIntegerColumn(string sheetName, int columnIndex);

		void SetPercentageColumn(string sheetName, int columnIndex);

		void SetDecimalColumn(string sheetName, int columnIndex);
		
		void SetTimeStampColumn(string sheetName, int columnIndex);

	    void SetIntegerCells(string sheetName, params Tuple<string, string>[] ranges);

	    void SetPercentageCells(string sheetName, params Tuple<string, string>[] ranges);

	    void SetDecimalCells(string sheetName, params Tuple<string, string>[] ranges);

	    void SetTimeStampCells(string sheetName, params Tuple<string, string>[] ranges);
    }
}
