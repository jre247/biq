using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;

namespace BrightLine.Common.Utility.Spreadsheets
{
	public interface ISpreadsheetReader
	{
        /// <summary>
        /// Load a spreadsheet using a file path.
        /// </summary>
		/// <param name="filePath"></param>
		/// <param name="setSheet">Specifies whether to set the first sheet as the active sheet (if it exists). True by default.</param>
		void LoadFile(string filePath, bool setSheet = true);


        /// <summary>
        /// Loads a spreadsheet using its content as a byte array
        /// </summary>
		/// <param name="bytes"></param>
		/// <param name="setSheet">Specifies whether to set the first sheet as the active sheet (if it exists). True by default.</param>
		void LoadFile(byte[] bytes, bool setSheet = true);


        /// <summary>
        /// Sets the current active sheet ( for reading data )
        /// </summary>
        /// <param name="name"></param>
		void SetCurrentSheet(string name);


        /// <summary>
        /// Checks whether or not the spreadsheet contains the sheet name provided.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		bool ContainsSheet(string name);


        /// <summary>
        /// Gets a single cell in the current sheet.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
		object GetCell(int row, int col);


        /// <summary>
        /// Gets a single cell value in the current sheet.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        object GetCellValue(int row, int col);

        
        /// <summary>
        /// Loads a sequence of columns ( on the same row ) as strings.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="numColumns"></param>
        /// <param name="stopAtEmptyColumn"></param>
        /// <param name="fillRemainderColumnsWithNulls"></param>
        /// <returns></returns>
		List<string> LoadRow(int startRow, int startColumn, int numColumns, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls);


        /// <summary>
        /// Loads a series of columns ( on the same row ) as objects.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="numColumns"></param>
        /// <param name="stopAtEmptyColumn"></param>
        /// <param name="fillRemainderColumnsWithNulls"></param>
        /// <returns></returns>
		List<object> LoadRowAsObjects(int startRow, int startColumn, int numColumns, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls);


	    /// <summary>
	    /// Loads a series of columns ( on the same row ) as objects.
	    /// </summary>
	    /// <param name="startRow"></param>
	    /// <param name="startColumn"></param>
	    /// <param name="count"></param>
	    /// <param name="stopAtEmptyColumn"></param>
	    /// <param name="predicate"></param>
	    /// <param name="fillRemainderColumns"></param>
	    /// <returns></returns>
	    List<T> LoadRowOf<T>(int startRow, int startColumn, int count, bool stopAtEmptyColumn, Func<object, bool> predicate, bool fillRemainderColumns);


        /// <summary>
        /// Loads a vertical set of column data ( only 1 coumn ) across multiple rows.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="numRows"></param>
        /// <param name="stopAtEmptyColumn"></param>
        /// <returns></returns>
		List<string> LoadColumn(int startRow, int startColumn, int numRows, bool stopAtEmptyColumn, bool fillRemainderColumnsWithNulls);


        /// <summary>
        /// Loads a vertical set of column data ( only 1 coumn ) across multiple rows as a list of T items.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="numRows"></param>
        /// <param name="stopAtEmptyColumn"></param>
        /// <returns></returns>
        List<T> LoadColumnOf<T>(int startRow, int startColumn, int numRows, bool stopAtEmptyColumn);


	    /// <summary>
	    /// Loads a vertical set of cells with unique values.
	    /// </summary>
	    /// <param name="startRow"></param>
	    /// <param name="startCol"></param>
	    /// <param name="count"></param>
	    /// <param name="stopAtEmptyCellOrInvalidValue"></param>
	    /// <returns></returns>
	    List<string> LoadColumnOfStringsUnique(int startRow, int startCol, int count, bool stopAtEmptyCellOrInvalidValue);


	    /// <summary>
	    /// Searches a row or column for the value supplied.
	    /// </summary>
	    /// <param name="startRow"></param>
	    /// <param name="startCol"></param>
	    /// <param name="maxCellsToSearch"></param>
	    /// <param name="searchAcross"></param>
	    /// <param name="expectedVal"></param>
	    /// <returns></returns>
	    CellPosition FindCell(int startRow, int startCol, int maxCellsToSearch, bool searchAcross, string expectedVal);


	    /// <summary>
	    /// Gets the total rows w/ values.
	    /// </summary>
	    /// <param name="startRow"></param>
	    /// <param name="startCol"></param>
	    /// <param name="maxRows"></param>
	    /// <returns></returns>
	    int TotalRowsWithValues(int startRow, int startCol, int maxRows);
	}
}
