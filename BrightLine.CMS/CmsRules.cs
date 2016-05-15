using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.CMS.Models;
using BrightLine.Common.Models;

namespace BrightLine.CMS
{
    public class CmsRules
    {
        public static bool CanSerializeProperty(DataModelProperty prop)
        {
            // CASE 1: Meta property for reporting
            if (prop.IsMetaType && prop.MetaValue == DataModelConstants.MetaType_Server)
                return false;

            // CASE 2: System level property ( for cms infrastructure itself )
            if (prop.IsSystemLevel)
                return false;

            return true;
        }


        /// <summary>
        /// Ensures that the 
        /// </summary>
        /// <param name="cmsKey"></param>
        /// <param name="fileName"></param>
        public static void EnsureCorrectFileName(string cmsKey, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("file must be supplied");

            fileName = fileName.Trim().ToLowerInvariant();

            // Rule 1: No short display ? any file name is ok
            if (string.IsNullOrEmpty(cmsKey))
                return;

            // Rule 2: file name must start with the same shortdisplay as campaign shortdisplay
            var shortDisplay = cmsKey.Trim().ToLowerInvariant() + "_";
            if(!fileName.StartsWith(shortDisplay))
                throw new ArgumentException("File name must begin with: '" + shortDisplay + "'");
        }
    }
}
