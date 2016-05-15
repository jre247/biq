using BrightLine.Common.ViewModels.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public interface IModelService
	{
		CreativeFeaturesViewModel GetModelsForCreative(int creativeId);
	}
}
