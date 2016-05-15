using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Models
{
    /// <summary>
    /// Each setting has a name, type, default value, description, etc.
    /// All these fields can ( just like the DataModelProperty ) be used to display a 
    /// full user interface on the web-ui ( when the time comes ).
    /// NOTE: The desc can be used for tool-tips.
    /// </summary>
    public class DataModelSetting
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public string Required { get; set; }
    }
}
