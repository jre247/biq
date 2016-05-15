using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
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

		public static List<Creative> CreateCreatives()
		{
			var creatives = new List<Creative>();

			creatives.Add(new Creative { Id = 1, Name = "Creative 1", Campaign = new Campaign { Id = 10 }, Features = new List<Feature>() });
			creatives.Add(new Creative { Id = 2, Name = "Creative 2", Campaign = new Campaign { Id = 10 }, Features = new List<Feature>() });
			creatives.Add(new Creative { Id = 3, Name = "Creative 3", Campaign = new Campaign { Id = 10 }, Features = new List<Feature>() });
			creatives.Add(new Creative { Id = 4, Thumbnail_Id = 1, AdType = new AdType { IsPromo = false }, Name = "Creative 4", Campaign = new Campaign { Id = 10 }, Features = new List<Feature>() });
			creatives.Add(new Creative { Id = 5, Thumbnail_Id = 2, AdType = new AdType { IsPromo = false }, Name = "Creative 5", Campaign = new Campaign { Id = 10 }, Features = new List<Feature>() });

			return creatives;
		}

		public static Creative BuildCreative(int creativeId, string creativeName, bool isPromo = false, int campaignId = 10, string resourceIds = "1", int resourceTypeId = 1, string resourceName = "abc.txt", int resourceId = 1)
		{
			var resourceTypes = IoC.Resolve<IRepository<ResourceType>>();

			var resourceType = resourceTypes.Get(resourceTypeId);

			return new Creative
			{
				Id = creativeId,
				Name = creativeName,
				Features = new List<Feature>(),
				ResourceIds = resourceIds,
				Campaign = new Campaign
				{
					Id = campaignId
				},
				AdType = new AdType
				{
					IsPromo = isPromo
				},
				AdFunction = new AdFunction { Id = 1 },
				Thumbnail_Id = resourceId,
				Resources = new List<Resource>
				{
					new Resource
					{
						Id = resourceId,
						ResourceType = resourceType,
						Filename = resourceName,
						Creative = new Creative{ Id = 1, Campaign = new Campaign{Id = campaignId}},
						Extension = new FileType{Id = 1, ResourceType = resourceType}
					}
				}
			};
		}

	}

}