using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
    public class EntityField
    {
        /// <summary>
        /// Label of field.
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// Name of the field
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Index of the field.
        /// </summary>
        public int Index { get; set; }


        /// <summary>
        /// Data type of the field
        /// </summary>
        public Type DataType { get; set; }


        /// <summary>
        /// The "friendly" name for a type ( read in from excel and converted to a .net type ). 
        /// e.g. "Number" can be mapped to "double". "text" can be mapped to "string".
        /// This is for user input.
        /// </summary>
        public string TypeName { get; set; }


        /// <summary>
        /// Whether or not there is a target field this can be mapped to.
        /// </summary>
        public bool HasTarget { get; set; }
        
        
        /// <summary>
        /// Is required
        /// </summary>
        public bool IsRequired { get; set; }


        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; set; }


        /// <summary>
        /// Max length
        /// </summary>
        public int MaxLength { get; set; }


        /// <summary>
        /// Min length 
        /// </summary>
        public int MinLength { get; set; }


        public override string ToString()
        {
            var val = Label + ", " + Name + ", " + TypeName + ", " + IsRequired + ", " + DefaultValue;
            return val;
        }
    }
}
