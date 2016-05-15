using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Utility.DateRanges;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public class DateRangeLoader
    {
        /// <summary>
        /// The spreadsheet reader.
        /// </summary>
        public ISpreadsheetReader Reader;


        /// <summary>
        /// Loads all the ranges found the int row.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<DateRange> Load(CellPosition pos)
        {
            return Load(pos.Row, pos.Col);
        }


        /// <summary>
        /// Loads all the ranges found the int row.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <returns></returns>
        public List<DateRange> Load(int startRow, int startCol)
	    {
		    var data = Reader.LoadRowAsObjects(startRow, startCol, 100, true, false);
		    var ranges = new List<DateRange>();
		    DateRange lastRange = null;
            int rangeNumber = 0;
            
            var col = startCol;
		    foreach(var obj in data)
		    {
			    if(obj is string)
			    {
				    if(lastRange != null)
				    {
					    lastRange.Source = (string)obj;
					    ranges.Add(lastRange);
				    }
			        lastRange = null;
			    }
			    else if (obj is DateTime)
			    {
			        if (lastRange == null)
			        {
			            lastRange = new DateRange();
                        lastRange.IndexStart = col;
			            rangeNumber++;
			            lastRange.RangeNumber = rangeNumber;
			        }

			        lastRange.Dates.Add((DateTime)obj);
			    }
					
		        col++;
		    }
		    return ranges;
	    }
    }
}