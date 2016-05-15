using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services.ModelInstance
{
	public class BaseModelInstanceService
	{
		protected ModelInstanceLookups GetLookupsForModelInstance(CmsModelInstance modelInstance)
		{
			var modelInstanceLookupsService = IoC.Resolve<IModelInstanceLookupsService>();

			var modelInstanceLookups = modelInstanceLookupsService.GetLookupsForModelInstance(modelInstance.Id);
			return modelInstanceLookups;
		}

	}
}
