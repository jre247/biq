using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility
{
	public interface IValidatorService
	{
		BoolMessageItem Validate(Validation validation, string fieldValue, FieldSaveViewModel field = null, ModelInstanceSaveViewModel viewModel = null, CmsField modelInstanceField = null, CmsModelInstance modelInstance = null);
	}
}
