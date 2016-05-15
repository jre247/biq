using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility;
using BrightLine.Utility.Validation;
using BrightLine.Core;
using BrightLine.Utility.Helpers;

namespace BrightLine.Common.Models.Validators
{
    /// <summary>
    /// Class to validate / lookup the name in campaign content model type.
    /// </summary>
    public class CmsModelTypeValidator: EntityNameValidator<CampaignContentModelType>
    {

        public CmsModelTypeValidator() : this(string.Empty)
        {
        }


        public CmsModelTypeValidator(string modelType) : base("model type", modelType, GetLookup)
        {
        }


        private static IDictionary<string, CampaignContentModelType> GetLookup()
        {
			return ExpressionHelper.KeyToEntityiesBy<string, CampaignContentModelType>(atg => atg.Name);
        }
    }
}
