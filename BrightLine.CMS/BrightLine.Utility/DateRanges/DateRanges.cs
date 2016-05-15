using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.DateRanges
{
    public class DateRanges
    {
        public DateRanges(DateRange overall, List<DateRange> ranges)
        {
            Overall = overall;
            Ranges = ranges;
            NumberOfRanges = ranges.Count;
        }


        public DateRange Overall;

        public List<DateRange> Ranges;

        public int NumberOfRanges;
    }
}
