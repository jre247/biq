using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
    /// <summary>
    /// Used to keep track of positions / starting points for different data sources.
    /// </summary>
    public class CellPosition
    {
        /// <summary>
        /// E.g. start/periods, a description that says what the cell position is used for.
        /// </summary>
        public string Name;


        /// <summary>
        /// Cell row ( 0 based )
        /// </summary>
        public int Row;


        /// <summary>
        /// Cell column ( 0 based )
        /// </summary>
        public int Col;


        public CellPosition Clone()
        {
            return new CellPosition() {Name = this.Name, Row = this.Row, Col = this.Col};
        }
    }
}
