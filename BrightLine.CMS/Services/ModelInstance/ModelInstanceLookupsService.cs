using BrightLine.CMS.Services;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.CMS.Service
{
	public class ModelInstanceLookupsService : IModelInstanceLookupsService
	{
		public ModelInstanceLookups CreateModelInstanceLookups(CmsModelInstance modelInstance)
		{
			var modelInstanceLookupsDictionary = new Dictionary<int, ModelInstanceLookups>();

			if (!IsModelInstanceLookupsCached())
				AddModelInstanceLookupsDictionaryToCache(modelInstanceLookupsDictionary);

			modelInstanceLookupsDictionary = GetModelInstanceLookupsDictionary();
			if (modelInstanceLookupsDictionary.ContainsKey(modelInstance.Id))
				throw new Exception("There is already a Model Instance cached in the Request object.");

			var modelInstanceLookups = new ModelInstanceLookups();
			modelInstanceLookups.ModelInstance = modelInstance;

			modelInstanceLookupsDictionary.Add(modelInstance.Id, modelInstanceLookups);
			CacheModelInstanceLookupsDictionary(modelInstanceLookupsDictionary);

			return modelInstanceLookups;
		}

		public ModelInstanceLookups GetLookupsForModelInstance(int modelInstanceId)
		{
			if (!IsModelInstanceLookupsCached())
				throw new NullReferenceException("Model Instance Lookups is not being cached.");

			var modelInstanceLookupsDictionary = GetModelInstanceLookupsDictionary();
			var modelInstanceLookups = modelInstanceLookupsDictionary[modelInstanceId];

			return modelInstanceLookups;
		}

		public void SaveLookupsForModelInstance(ModelInstanceLookups modelInstanceLookups)
		{
			var modelInstanceId = modelInstanceLookups.ModelInstance.Id;
			if (!IsModelInstanceLookupsCached())
				throw new NullReferenceException("Model Instance Lookups is not being cached.");

			var modelInstanceLookupsDictionary = GetModelInstanceLookupsDictionary();
			modelInstanceLookupsDictionary[modelInstanceId] = modelInstanceLookups;

			CacheModelInstanceLookupsDictionary(modelInstanceLookupsDictionary);
		}

		#region Private Methods

		private static void AddModelInstanceLookupsDictionaryToCache(Dictionary<int, ModelInstanceLookups> modelInstanceLookupsDictionary)
		{
			HttpContext.Current.Items.Add(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY, modelInstanceLookupsDictionary);
		}

		private static bool IsModelInstanceLookupsCached()
		{
			return HttpContext.Current.Items.Contains(ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY);
		}

		private static Dictionary<int, ModelInstanceLookups> GetModelInstanceLookupsDictionary()
		{
			return (Dictionary<int, ModelInstanceLookups>)HttpContext.Current.Items[ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY];
		}

		private static void CacheModelInstanceLookupsDictionary(Dictionary<int, ModelInstanceLookups> modelInstanceLookupsDictionary)
		{
			HttpContext.Current.Items[ModelInstanceConstants.MODEL_INSTANCE_LOOKUPS_KEY] = modelInstanceLookupsDictionary;
		}

		#endregion
	}
}
