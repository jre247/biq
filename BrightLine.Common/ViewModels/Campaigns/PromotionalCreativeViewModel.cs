using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using System;
using System.Runtime.Serialization;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json.Linq;
using BrightLine.Common.ViewModels.Resources;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class PromotionalCreativeViewModel
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public int AdCount { get; set; }

		[DataMember]
		public DateTime DateUpdated { get; set; }

		[DataMember]
		public List<ResourceViewModel> Resources { get; set; }

		[DataMember]
		public Blueprint Blueprint { get; set; }

		[DataMember]
		public int? Height { get; set; }

		[DataMember]
		public int? Width { get; set; }

		[DataMember]
		public AdTypeViewModel AdType { get; set; }

		[DataMember]
		public AdFormatViewModel AdFormat { get; set; }

		[DataMember]
		public AdFunctionViewModel AdFunction { get; set; }

		public static IEnumerable<PromotionalCreativeViewModel> FromCreatives(IQueryable<Creative> creatives)
		{
			Mapper.CreateMap<Creative, PromotionalCreativeViewModel>().
				   ForMember(dest => dest.AdCount, opt => opt.MapFrom(c => c.Ads.Count())).
				   ForMember(dest => dest.DateUpdated, opt => opt.MapFrom<DateTime>(rd => DateHelper.ToUserTimezone(rd.DateUpdated ?? rd.DateCreated)));
			var cvms = Mapper.Map<IEnumerable<Creative>, IEnumerable<PromotionalCreativeViewModel>>(creatives);
			return cvms;

		}

		public static JObject ToJObject(PromotionalCreativeViewModel PromotionalCreativeViewModel)
		{
			return JObject.FromObject(PromotionalCreativeViewModel);
		} 

		public static PromotionalCreativeViewModel FromCreative(Creative creative)
		{
			var viewModel = new PromotionalCreativeViewModel{
				Id = creative.Id,
				Name = creative.Name,
				Description = creative.Description,
				AdType = AdTypeViewModel.FromAdType(creative.AdType),
				AdFormat = AdFormatViewModel.FromAdFormat(creative.AdFormat),
				AdFunction = AdFunctionViewModel.FromAdFunction(creative.AdFunction),
				Resources = ResourcesViewModel.FromResources(creative.Resources)
			};

			return viewModel;
		}

		public class ResourcesViewModel
		{
			public List<ResourceViewModel> Resources { get; set; }

			public static List<ResourceViewModel> FromResources(IEnumerable<Resource> resources)
			{
				var resourcesViewModel = new List<ResourceViewModel>();
				foreach(var resource in resources)
				{
					if(resource.IsDeleted)
						continue;

					var resourceViewModel = new ResourceViewModel(resource);
					resourcesViewModel.Add(resourceViewModel);
				}

				return resourcesViewModel;
			}
		}

		public class AdTypeViewModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsDeleted { get; set; }

			public static AdTypeViewModel FromAdType(AdType adType)
			{
				return new AdTypeViewModel
				{
					Id = adType.Id,
					Name = adType.Name,
					IsDeleted = adType.IsDeleted
				};
			}
		}

		public class AdFormatViewModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsDeleted { get; set; }

			public static AdFormatViewModel FromAdFormat(AdFormat adFormat)
			{
				if (adFormat == null)
					return null;

				return new AdFormatViewModel
				{
					Id = adFormat.Id,
					Name = adFormat.Name,
					IsDeleted = adFormat.IsDeleted
				};
			}
		}

		public class AdFunctionViewModel
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool IsDeleted { get; set; }

			public static AdFunctionViewModel FromAdFunction(AdFunction adFunction)
			{
				return new AdFunctionViewModel
				{
					Id = adFunction.Id,
					Name = adFunction.Name,
					IsDeleted = adFunction.IsDeleted
				};
			}
		}

		
	}
}
