using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.DateRanges
{
    public class DateRange
    {   
        /// <summary>
        /// Initialize
        /// </summary>
        public DateRange()
        {
            Dates = new List<DateTime>();
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public DateRange(DateTime start, DateTime end, string source, int rangeNumber)
        {
            var dates = new List<DateTime>();
            var current = start;
            while (current.Date != end.Date)
            {
                dates.Add(current);
                current = current.AddDays(1);
            }
            dates.Add(end);
            Init(dates, source);
            RangeNumber = rangeNumber;
        }


        /// <summary>
        /// Initialize with dates.
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="source"></param>
        public void Init(List<DateTime> dates, string source)
        {
            Source = source;
            Dates = dates;
        }


        /// <summary>
        /// Source ( used for referencing something )
        /// </summary>
        public string Source;


        /// <summary>
        /// The range number ( can be used externally to track this range ).
        /// </summary>
        public int RangeNumber;


        /// <summary>
        /// List of the dates.
        /// </summary>
        public List<DateTime> Dates;


        /// <summary>
        /// First date in range.
        /// </summary>
        public DateTime First
        {
            get { return Dates[0]; }
        }


        /// <summary>
        /// Last date in range.
        /// </summary>
        public DateTime Last
        {
            get { return Dates[Dates.Count - 1]; }
        }


        /// <summary>
        /// Days in range.
        /// </summary>
        public int DaysInRange
        {
            get { return Dates.Count; }
        }


        /// <summary>
        /// Start index of the cell where start date occurred.
        /// </summary>
        public int IndexStart;


        /// <summary>
        /// End index of the cell where the end date occurred
        /// </summary>
        public int IndexEnd;
        


        public string ToRangeText()
        {
            var val = "";
            if (Dates != null && Dates.Count > 0)
            {
                val = Dates[0].ToString("MM/dd/yyyy") + " - " + Dates[Dates.Count - 1].ToString("MM/dd/yyyy");
            }
            return val;
        }



        /// <summary>
        /// Ensures the validity of this range.
        /// </summary>
        public void EnsureValidity()
        {
            // Ensure dates supplied.
            if (Dates.Count == 0)
                throw new ArgumentException("0 dates supplied");

            // Ensure no duplicates
            var lookup = new Dictionary<DateTime, bool>();
            foreach (var date in Dates)
            {
                if (lookup.ContainsKey(date.Date))
                    throw new ArgumentException("Dulicate date : " + date.ToString());
                lookup[date.Date] = true;
            }
        }


        public override string ToString()
        {
            var val = "";
            if (Dates != null && Dates.Count > 0)
            {
                val = Dates[0].ToString() + " - " + Dates[Dates.Count - 1].ToString() + ", " + Dates.Count;
            }
            return val;
        }

        
        /// <summary>
        /// Index of the date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int IndexOf(DateTime date)
        {
            return Dates.IndexOf(date.Date);
        }
    }
}
