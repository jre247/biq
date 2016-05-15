using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using System;
using System.Runtime.Serialization;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Common.ViewModels.Ads;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.AdType;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class DestinationCreativeViewModel
	{
		[DataMember]
		public int id { get; set; }

		[DataMember]
		public string name { get; set; }

		[DataMember]
		public string description { get; set; }

		[DataMember]
		public ResourceViewModel resource { get; set; }

		[DataMember]
		public int? inactivityThreshold { get; set; }

		[DataMember]
		public IEnumerable<DestinationCreativeFeatureViewModel> features { get; set; }

		[DataMember]
		public IEnumerable<AdEditRepoNameViewModel> ads { get; set; }

		public static JObject ToJObject(DestinationCreativeViewModel DestinationCreativeViewModel)
		{
			return JObject.FromObject(DestinationCreativeViewModel);
		} 

		public static DestinationCreativeViewModel FromCreative(Creative creative)
		{
			var adsService = IoC.Resolve<IAdService>();
			var brandDestination = Lookups.AdTypes.HashByName[AdTypeConstants.AdTypeNames.BrandDestination];

			// Find all Brand Destination Ads where their Creative is equal to the current Creative
			var ads = adsService.Where(a => 
					a.AdType.Id == brandDestination &&
					a.Creative.Id == creative.Id
				).Select(a => new AdEditRepoNameViewModel
				{
					id = a.Id,
					name = a.Name,
					platformId = a.Platform.Id,
					platformName = a.Platform.Name,
					repoName = a.RepoName
				}).ToList();

			var viewModel = new DestinationCreativeViewModel{
				id = creative.Id,
				name = creative.Name,
				description = creative.Description,
				features = DestinationCreativeFeatureViewModel.FromFeatures(creative.Features),
				resource = creative.Thumbnail_Id.HasValue ? new ResourceViewModel(creative.Thumbnail_Id) : null,
				inactivityThreshold = creative.InactivityThreshold,
				ads = ads
			};

			return viewModel;
		}

		public class DestinationCreativeFeatureViewModel
		{
			public int Id { get; set; }

			public static IEnumerable<DestinationCreativeFeatureViewModel> FromFeatures(ICollection<Feature> features)
			{
				if (features == null)
					return null;

				return features.Select(f => new DestinationCreativeFeatureViewModel{
					Id = f.Id
				}).ToList();
			}
		}

		
	}
}
