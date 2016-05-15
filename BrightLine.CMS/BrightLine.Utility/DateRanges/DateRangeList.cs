using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.DateRanges
{
    /// <summary>
    /// Represents a set of data within a bounded date range such as Feb 1 to Feb 7.
    /// There can be only 1 data value for each day/date in this set. so Feb 1 has 1 data value of type T.
    /// </summary>
    /// <remarks>
    /// 1. There will always be a list of x items that represent x days in the range.
    /// 2. This can be easily extended such as that each data value for a specific day itself could be a list of support multiple values.
    /// 3. For example. For Feb 1, the data value can be a list of Hourly results. The data type T is used to represent that data value.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class DateRangeList<T> : IDateRangeList<T> where T: class, new()
    {
        protected List<T> _data;
        protected DateRange _dateRange;


        /// <summary>
        /// Initialize.
        /// </summary>
        public DateRangeList()
        {
        }
        
        
        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="name"></param>
        public DateRangeList(DateRange range, string name)
        {
            Init(range, name);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="refKey"></param>
        /// <param name="rangeInfo"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        public void Init(string refKey, DateRange rangeInfo, string name, int number)
        {
            ReferenceKey = refKey;
            RangeNumber = number;
            Init(rangeInfo, name);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="name"></param>
        public void Init(DateRange range, string name)
        {
            _dateRange = range;
            RangeName = name;
            _data = new List<T>();
            for (var ndx = 0; ndx < range.DaysInRange; ndx++)
            {
                _data.Add(default(T));
            }
        }


        /// <summary>
        /// Gets the name of this date range.
        /// </summary>
        public string RangeName { get; set; }


        /// <summary>
        /// Used externally by calling code to uniquely represent this data range.
        /// </summary>
        public string ReferenceKey { get; set; }


        /// <summary>
        /// The range number .eg. 1, 2, 3
        /// </summary>
        public int RangeNumber { get; set; }


        /// <summary>
        /// Returns the data points associated w/ this date range ( there will be null values for index, representing days that don't have any data points ).
        /// </summary>
        public List<T> Items
        {
            get { return _data;  }
            set { _data = value; }
        }


        /// <summary>
        /// Gets the item for the whole range.
        /// </summary>
        public T ItemForRange { get; set; }


        /// <summary>
        /// Any data for the supplied index representing the date.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool Any(int index)
        {
            return Total(index) > 0;
        }


        /// <summary>
        /// Any data for the supplied date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual bool Any(DateTime date)
        {
            return Total(date) > 0;
        }


        /// <summary>
        /// Total at the index ( day index ) specified e.g. 0 = data at day 0
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual int Total(int index)
        {
            if (!IsValidIndex(index))
                return 0;
            if (_data == null || _data.Count == 0)
                return 0;

            if (index >= _data.Count)
                return 0;

            if (_data[index] == null)
                return 0;
            return 1;
        }


        /// <summary>
        /// Gets the total items at the date supplied.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual int Total(DateTime date)
        {
            var ndx = _dateRange.IndexOf(date);
            return Total(ndx);
        }


        /// <summary>
        /// Gets the data point for the supplied day index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T Get(int index)
        {
            if (!IsValidIndex(index))
                return default(T);

            if (_data == null || _data.Count == 0)
                return default(T);

            if (index >= _data.Count)
                return default(T);

            return _data[index];
        }


        /// <summary>
        /// Gets or creates a datapoint at the supplied index ( representing day ).
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T GetOrCreateAt(int index)
        {
            if (_data == null)
                _data = new List<T>();
            
            var isEmpty = _data.Count == 0;

            if (!isEmpty && Any(index))
            {
                return Get(index);
            }
            var item = new T();
            Set(index, item);
            return item;
        }


        /// <summary>
        /// Gets the data point for the supplied date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual T Get(DateTime date)
        {
            var ndx = _dateRange.IndexOf(date);
            return Get(ndx);
        }


        /// <summary>
        /// Returns whethr or not there are any data values in this date range.
        /// </summary>
        /// <returns></returns>
        public virtual bool Any(bool includeItemForWholeRange)
        {
            return Total(includeItemForWholeRange) > 0;
        }


        /// <summary>
        /// Gets the total data values across the date range.
        /// </summary>
        /// <returns></returns>
        public virtual int Total(bool includeItemForRange)
        {
            var total = 0;
            for (var ndx = 0; ndx < _data.Count; ndx++)
            {
                if (_data[ndx] != null)
                    total++;
            }
            if (includeItemForRange && ItemForRange != null)
                total++;
            return total;
        }


        /// <summary>
        /// Gets the date ranges.
        /// </summary>
        public virtual DateRange Dates
        {
            get { return _dateRange;  }
            set { _dateRange = value; }
        }


        /// <summary>
        /// Sets the data point value at the supplied day index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="val"></param>
        public virtual void Set(int index, T val)
        {
            if (!IsValidIndex(index))
                return;

            // Case 1: Index < count
            if (index < _data.Count)
            {
                _data[index] = val;
            }
            // Case 2: Index == Count ( add to end )
            else if (index == _data.Count)
            {
                _data.Add(val);
            }
            // Case 3: Index > Count.
            else
            {
                while (_data.Count <= index)
                {
                    _data.Add(null);
                }
                _data[index] = val;
            }
        }


        /// <summary>
        /// Sets the data point value for the supplied date.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="val"></param>
        public virtual void Set(DateTime date, T val)
        {
            var ndx = _dateRange.IndexOf(date);
            Set(ndx, val);
        }


        /// <summary>
        /// Used to generate quick lookups for maps and visual studio debugger.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ReferenceKey + " : " + RangeName;
        }


        protected virtual bool IsValidIndex(int index)
        {
            if (index < 0 || index >= _dateRange.DaysInRange)
                return false;
            return true;
        }
    }
}
