using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.AppImport
{
    public class ModelConfig
    {
        public string ModelName { get; set; }
        public string SheetName { get; set; }
        public string FilterText { get; set; }
        public ModelFilter Filter { get; set; }
        public string Type { get; set; }
        public bool IsSettings { get; set; }

        public bool HasFilter()
        {
            return Filter != null;
        }


        /// <summary>
        /// String reprsentation for easier debugging in the visualizer.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SheetName + ", " + FilterText + ", " + Type;
        }


        /// <summary>
        /// Extracts the name of the model from the sheet and builds the filter from the string.
        /// </summary>
        public void ExtractData()
        {
            ModelName = AppImporterHelper.MassageName(SheetName);
            if (!string.IsNullOrEmpty(Type))
            {
                Type = Type.Trim();
                IsSettings = string.Compare(Type, "settings", true) == 0;
            }

            // Associate the filters w/ the models.
            if (!string.IsNullOrEmpty(FilterText))
            {
                var tokens = FilterText.Split(':');
                var filter = new ModelFilter();
                filter.ColumnName = tokens[0];
                filter.PropName = AppImporterHelper.MassageName(filter.ColumnName);
                filter.Value = tokens[1].ToLower().Trim();
                Filter = filter;
            }
        }


        /// <summary>
        /// Object representation for the filter.
        /// </summary>
        public class ModelFilter
        {
            public string ColumnName;
            public int ColumnIndex;
            public string PropName;
            public string Value;

            public override string ToString()
            {
                return ColumnName + " " + " : " + Value;
            }
        }
    }
}
