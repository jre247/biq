using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IModelInstanceLookupsService
	{
		ModelInstanceLookups CreateModelInstanceLookups(CmsModelInstance modelInstance);
		ModelInstanceLookups GetLookupsForModelInstance(int modelInstanceId);
		void SaveLookupsForModelInstance(ModelInstanceLookups modelInstanceLookups);
	}
}
