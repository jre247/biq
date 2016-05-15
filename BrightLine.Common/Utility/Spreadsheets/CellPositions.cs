using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public class CellPositions : Dictionary<string, CellPosition>
    {
        /// <summary>
        /// Adds a new cell position w/ the supplied name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void Add(string name, int row, int col)
        {
            this[name] = new CellPosition() {Name = name, Row = row, Col = col};
        }
    }
}
