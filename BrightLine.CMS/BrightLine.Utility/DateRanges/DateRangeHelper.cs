using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.DateRanges
{
    public class DateRangeHelper
    {
        public static DateRanges Build(DateTime startDate, DateTime endDate, int numberOfWeeks, string sourcePrefix = "Week ")
        {
            var ranges = new List<DateRange>();
            var rangeStart = startDate;
            for (int ndx = 0; ndx < numberOfWeeks; ndx++)
            {
                var weeknum = ndx + 1;
                var range = new DateRange(rangeStart, rangeStart.AddDays(6), sourcePrefix + weeknum, weeknum);
                ranges.Add(range);
                rangeStart = rangeStart.AddDays(7);
            }

            var rangeAll = new DateRange(startDate, endDate, "Week 0", 0);
            return new DateRanges(rangeAll, ranges);
        }
    }
}
