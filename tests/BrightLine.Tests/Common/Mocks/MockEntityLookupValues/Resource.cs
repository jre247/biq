using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<Resource> CreateResources()
		{
			var _resourceRepo = new List<Resource>();
			var resourceTypeId = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage];

			var resource1 = CreateResource(1, "apple.jpeg");
			var resource2 = CreateResource(2, "banana.jpeg");
			var resource3 = CreateResource(3, "orange.png");
			var resourceInvalidExtension = CreateResource(4, "orange.abc", 42323, "abc", 1, "fewjoifj");
			var resource5 = CreateResource(5, "blueberries.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo], ResourceTypeConstants.ResourceTypeNames.SdVideo, 10001);
			var resource6 = CreateResource(6, "blueberries2.mp4", Lookups.FileTypes.HashByName[FileTypeConstants.FileTypeNames.Mp4], FileTypeConstants.FileTypeNames.Mp4, Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo], ResourceTypeConstants.ResourceTypeNames.HdVideo, 1100);
			var resource7 = CreateResource(7, "blueberries3.mp432", 4234, "abcd", 322, "abced", 1100);

			_resourceRepo.Add(resource1);
			_resourceRepo.Add(resource2);
			_resourceRepo.Add(resource3);
			_resourceRepo.Add(resourceInvalidExtension);
			_resourceRepo.Add(resource5);
			_resourceRepo.Add(resource6);
			_resourceRepo.Add(resource7);

			return _resourceRepo;
		}

		public static Resource CreateResource(int resourceId = 1, string filename = "apple.jpeg", int fileTypeId = 1, string fileTypeName = FileTypeConstants.FileTypeNames.Jpeg, int resourceTypeId = 1, string resourceTypeName = ResourceTypeConstants.ResourceTypeNames.SdImage,
			int? duration = null, int size = 34567, int width = 12345, int height = 23456, int creativeId = 1, int campaignId = 1, string name = "apple")
		{
			var resource1 = new Resource
			{
				Id = resourceId,
				Name = name,
				Extension = new FileType { Id = fileTypeId, Name = fileTypeName, ResourceType = new ResourceType { Id = resourceTypeId, Name = resourceTypeName } },
				Filename = filename,
				Width = width,
				Height = height,
				Size = size,
				ResourceType = new ResourceType { Id = resourceTypeId, Name = resourceTypeName },
				Creative = new Creative { Id = creativeId, Campaign = new Campaign { Id = campaignId } },
				Duration = duration
			};
			return resource1;
		}

		public static Resource CreateResource(int resourceId, string name, string filename, int fileTypeId, string fileTypeName, int resourceTypeId, string resourceTypeName, int? duration, int size, int width, int height, int creativeId, int campaignId, DateTime dateCreated)
		{
			var resource1 = new Resource
			{
				Id = resourceId,
				Extension = new FileType { Id = fileTypeId, Name = fileTypeName, ResourceType = new ResourceType { Id = resourceTypeId, Name = resourceTypeName } },
				Filename = filename,
				Name = name,
				Width = width,
				Height = height,
				Size = size,
				ResourceType = new ResourceType { Id = resourceTypeId, Name = resourceTypeName },
				Creative = new Creative { Id = creativeId, Campaign = new Campaign { Id = campaignId } },
				Duration = duration,
				DateCreated = dateCreated
			};
			return resource1;
		}

		public static ResourceViewModel CreateResourceViewModel(string md5Hash, string fileName, int resourceTypeId)
		{
			var viewModel = new ResourceViewModel
			{
				md5Hash = md5Hash,
				filename = fileName,
				resourceType = resourceTypeId
			};
			return viewModel;
		}

	}

}