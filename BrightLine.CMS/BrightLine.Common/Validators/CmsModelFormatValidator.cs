using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using BrightLine.Utility.Helpers;

namespace BrightLine.Common.Models.Validators
{
    public class CmsModelFormatValidator: EntityNameValidator<CampaignContentModelBaseType>
    {

        public CmsModelFormatValidator() : this(string.Empty)
        {
        }


        public CmsModelFormatValidator(string modelType)
            : base("model format", modelType, GetLookup)
        {
        }


        private static IDictionary<string, CampaignContentModelBaseType> GetLookup()
        {
			return ExpressionHelper.KeyToEntityiesBy<string, CampaignContentModelBaseType>(atg => atg.Name);
        }
    }
}
