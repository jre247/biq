using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Utility.ResourceType;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.ViewModels.Resources
{
	public class ResourceViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public string filename { get; set; }
		public int? width { get;set;}
		public int? height { get; set; }
		public int? size { get; set; }
		public int resourceType { get;set;}
		public int? creativeId { get; set; }
		public int? campaignId { get;set;}
		public int? duration { get; set; }
		public string path { get; set; }
		public string md5Hash { get;set;}
		public string url { get; set; }
		public string fileType { get; set; }

		public ResourceViewModel()
		{ }

		public ResourceViewModel(Resource resource)
		{
			BuildResourceViewModel(resource);
		}

		public ResourceViewModel(int? resourceId)
		{
			var resourcesRepo = IoC.Resolve<IResourceService>();

			if(!resourceId.HasValue)
				return;

			var resource = resourcesRepo.Get(resourceId.Value);

			BuildResourceViewModel(resource);
		}

		public static JObject Parse(ResourceViewModel viewModel)
		{
			return JObject.FromObject(viewModel);
		}

		private void BuildResourceViewModel(Resource resource)
		{
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			var resourceTypesIdHash = Lookups.ResourceTypes.HashByName;

			this.id = resource.Id;

			// Get the extension, which will be after the last period
			var extensionSplit = resource.Filename.Split('.');
			var extension = extensionSplit[extensionSplit.Count() - 1];

			this.filename = string.Format("{0}.{1}", resource.Name, extension);

			this.campaignId = resource.Creative.Campaign.Id;
			this.resourceType = resource.ResourceType.Id;
			if (IsResourceImage(resource.Extension, resourceTypesIdHash))
			{
				this.fileType = ModelInstanceConstants.FieldResourceTypes.Image;
				this.url = resourceHelper.GetResourceDownloadPath(resource);
			}
			else
			{
				this.fileType = ModelInstanceConstants.FieldResourceTypes.Video;
				this.url = resourceHelper.GetResourceDownloadPath(resource);
			}
		}

		private static bool IsResourceImage(FileType extension, Dictionary<string, int> resourceTypesIdHash)
		{
			return extension.ResourceType.Id == resourceTypesIdHash[ResourceTypeConstants.ResourceTypeNames.SdImage] || extension.ResourceType.Id == resourceTypesIdHash[ResourceTypeConstants.ResourceTypeNames.HdImage];
		}

		
	}
}
