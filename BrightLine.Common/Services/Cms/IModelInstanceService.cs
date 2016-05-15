using BrightLine.Common.Models;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public interface IModelInstanceService
	{
		JObject Get(int modelInstanceId);
		BoolMessageItem Save(ModelInstanceSaveViewModel viewModel);
		Dictionary<int, ModelInstanceListViewModel> GetModelInstancesForModel(int modelId, bool verbose = false);
	}
}
