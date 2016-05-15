using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public interface IModelInstanceValidationService
	{
		BoolMessageItem ValidateModelInstanceAccessibility(CmsModel model);
		BoolMessageItem ValidateModelInstanceFields(CmsModelInstance modelInstance, ModelInstanceSaveViewModel viewModel);
	}

}
