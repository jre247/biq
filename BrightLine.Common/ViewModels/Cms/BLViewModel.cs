using BrightLine.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Cms
{
	public class BLViewModel
	{
		public int campaignId;
		public int creativeId;
		public int featureId;
		public int modelId;
		public int instanceId;

		//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
		[JsonConstructor]
		private BLViewModel()
		{ }

		private BLViewModel(CmsModelInstance modelInstanceIn)
		{
			campaignId = modelInstanceIn.Model.Feature.Campaign.Id;
			creativeId = modelInstanceIn.Model.Feature.Creative.Id;
			featureId = modelInstanceIn.Model.Feature.Id;
			modelId = modelInstanceIn.Model.Id;
			instanceId = modelInstanceIn.Id;
		}

		public static BLViewModel Parse(CmsModelInstance modelInstanceIn)
		{
			return new BLViewModel(modelInstanceIn);
		}

		public static JObject ToJObject(BLViewModel viewModel)
		{
			if (viewModel == null)
				return null;

			var json = JObject.FromObject(viewModel);
			return json;
		}
	}
}
