using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.FieldType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Helpers
{
	public class CmsFieldHelper : ICmsFieldHelper
	{
		public bool IsFieldResource(CmsField cmsField)
		{
			return cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image] || cmsField.Type.Id == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Video];
		} 
	}
}
