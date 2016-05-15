using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Utility;
using BrightLine.CMS.Factories;
using BrightLine.Common.Utility.Enums;
using BrightLine.CMS.Services.Publish;
using BrightLine.Common.ViewModels.Cms;
using BrightLine.Common.Utility.Constants;

namespace BrightLine.CMS.Services
{
	public class ModelInstanceService : IModelInstanceService
	{
		public ModelInstanceService()
		{
		}

		public JObject Get(int modelInstanceId)
		{
			var modelInstanceRetrievalService = IoC.Resolve<IModelInstanceRetrievalService>();

			return modelInstanceRetrievalService.Get(modelInstanceId);
		}

		public Dictionary<int, ModelInstanceListViewModel> GetModelInstancesForModel(int modelId, bool verbose = false)
		{
			var modelInstanceRetrievalService = IoC.Resolve<IModelInstanceRetrievalService>();

			return modelInstanceRetrievalService.GetModelInstancesForModel(modelId, verbose);
		}

		public BoolMessageItem Save(ModelInstanceSaveViewModel viewModel)
		{
			var modelInstanceSaveService = IoC.Resolve<IModelInstanceSaveService>();

			return modelInstanceSaveService.Save(viewModel);
		}
	}
}
