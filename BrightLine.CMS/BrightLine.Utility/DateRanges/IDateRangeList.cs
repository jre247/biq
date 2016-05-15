using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.DateRanges
{
    public interface IDateRangeList<T> where T: class
    {
        /// <summary>
        /// A name for the date range.
        /// </summary>
        string RangeName { get; }


        /// <summary>
        /// Used externally by calling code to uniquely represent this data range.
        /// </summary>
        string ReferenceKey { get; set; }


        /// <summary>
        /// The range number .eg. 1, 2, 3
        /// </summary>
        int RangeNumber { get; set; }


        /// <summary>
        /// Date range
        /// </summary>
        DateRange Dates { get; set; }


        /// <summary>
        /// List of data points for each day in the date range.
        /// </summary>
        List<T> Items { get; set; }

        
        /// <summary>
        /// Reprsetns the data point/value for the whole date range. e.g. AdResult for whole week
        /// </summary>
        T ItemForRange { get; set; }

        
        /// <summary>
        /// Returns whether or not there are any data points for the whole range with option to include the item for the whole range in the total.
        /// </summary>
        /// <returns></returns>
        bool Any(bool includeItemForWholeRange);
        bool Any(int index);
        bool Any(DateTime date);


        /// <summary>
        /// Returns the total data points in the date range. and whether or not to include the itemfor whole range in the total.
        /// </summary>
        /// <param name="includeItemForRange"></param>
        /// <returns></returns>
        int Total(bool includeItemForRange);
        int  Total(int index);
        int  Total(DateTime date);


        T Get(int index);
        T Get(DateTime date);


        void Set(int index, T val);
        void Set(DateTime date, T val);
    }
}
