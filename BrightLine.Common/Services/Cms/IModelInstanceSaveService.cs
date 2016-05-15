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
	public interface IModelInstanceSaveService
	{
		BoolMessageItem Save(ModelInstanceSaveViewModel viewModel);
		BoolMessageItem SaveModelInstance(ModelInstanceSaveViewModel viewModel, CmsModelInstance modelInstance);
		
	}
}
