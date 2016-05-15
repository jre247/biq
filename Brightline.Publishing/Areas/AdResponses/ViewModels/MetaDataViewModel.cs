using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.ViewModels
{
	/// <summary>
	/// This View Model represents the Metadata for an Ad Response
	/// </summary>
	public class MetaDataViewModel
	{
		#region Members

		public AdViewModel ad { get; set; }
		public string responseType { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> trackingUrls { get; set; }

		#endregion

		#region Init

		public MetaDataViewModel()
		{ }

		public MetaDataViewModel(Ad ad, string responseType, Dictionary<string, string> trackingUrls = null)
		{
			this.ad = AdViewModel.Parse(ad);
			this.responseType = responseType;
			this.trackingUrls = trackingUrls;
		}

		#endregion

		#region Public Methods

		public static MetaDataViewModel Parse(Ad ad, string responseType, Dictionary<string, string> trackingUrls = null)
		{
			if (ad == null || string.IsNullOrEmpty(responseType))
				return null;

			return new MetaDataViewModel(ad, responseType, trackingUrls);
		}

		#endregion

		#region Subclasses

		public class AdViewModel
		{
			#region Members

			public int id { get; set; }
			public AdTypeViewModel adType { get; set; }
			public int? adTagId { get; set; }
			public PlatformViewModel platform { get; set; }
			public CompanionAdViewModel companionAd { get; set; }
			public CampaignViewModel campaign { get;set;}
			public CreativeViewModel creative { get; set; }
			public DestinationAdViewModel destinationAd { get; set; }

			#endregion

			#region Init

			public AdViewModel()
			{ }

			public AdViewModel(Ad ad)
			{
				id = ad.Id;
				this.adType = AdTypeViewModel.Parse(ad);
				this.adTagId = ad.AdTag.Id;
				this.platform = PlatformViewModel.Parse(ad);
				this.companionAd = CompanionAdViewModel.Parse(ad);
				this.campaign = CampaignViewModel.Parse(ad);
				this.creative = CreativeViewModel.Parse(ad);
				this.destinationAd = DestinationAdViewModel.Parse(ad);
			}

			#endregion

			#region Public Methods

			public static AdViewModel Parse(Ad ad)
			{
				if (ad == null)
					return null;

				return new AdViewModel(ad);
			}

			#endregion

			#region Subclasses

			public class AdTypeViewModel
			{
				#region Members

				public string manifestName { get; set; }

				#endregion

				#region Init

				public AdTypeViewModel()
				{ }

				public AdTypeViewModel(Ad ad)
				{
					manifestName = ad.AdType.ManifestName;
				}

				#endregion

				#region Public Methods

				public static AdTypeViewModel Parse(Ad ad)
				{
					if (ad == null)
						return null;

					return new AdTypeViewModel(ad);
				}

				#endregion
			}

			public class PlatformViewModel
			{
				#region Members

				public string manifestName { get; set; }

				#endregion

				#region Init

				public PlatformViewModel()
				{ }

				public PlatformViewModel(Ad ad)
				{
					manifestName = ad.Platform.ManifestName;
				}

				#endregion

				#region Public Methods

				public static PlatformViewModel Parse(Ad ad)
				{
					if (ad == null)
						return null;

					return new PlatformViewModel(ad);
				}

				#endregion
			}

			public class CompanionAdViewModel
			{
				#region Members

				public int id { get; set; }
				public int? adTagId { get; set; }

				#endregion

				#region Init

				public CompanionAdViewModel()
				{ }

				public CompanionAdViewModel(Ad ad)
				{
					id = ad.CompanionAd.Id;
					adTagId = ad.AdTag != null ? ad.AdTag.Id : (int?)null;
				}

				#endregion

				#region Public Methods

				public static CompanionAdViewModel Parse(Ad ad)
				{
					if (ad == null || ad.CompanionAd == null)
						return null;

					return new CompanionAdViewModel(ad);
				}

				#endregion
			}

			public class CampaignViewModel
			{
				#region Members

				public int id { get; set; }

				#endregion

				#region Init

				public CampaignViewModel()
				{ }

				public CampaignViewModel(Ad ad)
				{
					id = ad.Campaign.Id;
				}

				#endregion

				#region Public Methods

				public static CampaignViewModel Parse(Ad ad)
				{
					if (ad == null)
						return null;

					return new CampaignViewModel(ad);
				}

				#endregion
			}

			public class CreativeViewModel
			{
				#region Members

				public int id { get; set; }

				[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
				public MetaDataViewModel.ResourcesViewModel.ResourceViewModel[] resources { get; set; }

				#endregion

				#region Init

				public CreativeViewModel()
				{ }

				public CreativeViewModel(Ad ad)
				{
					id = ad.Creative.Id;
					this.resources = ResourcesViewModel.Parse(ad);
				}

				#endregion

				#region Public Methods

				public static CreativeViewModel Parse(Ad ad)
				{
					if (ad == null)
						return null;

					return new CreativeViewModel(ad);
				}

				#endregion
			}

			public class DestinationAdViewModel
			{
				#region Members

				public int id { get; set; }
				public int adTagId { get; set; }

				#endregion

				#region Init

				public DestinationAdViewModel()
				{ }

				public DestinationAdViewModel(Ad ad)
				{
					id = ad.Id;
					adTagId = ad.AdTag.Id;
				}

				#endregion

				#region Public Methods

				public static DestinationAdViewModel Parse(Ad ad)
				{
					if (ad == null || ad.DestinationAd == null)
						return null;

					return new DestinationAdViewModel(ad);
				}

				#endregion
			}

			#endregion
		}

		public class ResourcesViewModel
		{
			#region Members

			public ResourceViewModel[] resources { get; set; }

			#endregion

			#region Init

			public ResourcesViewModel()
			{ }

			public ResourcesViewModel(Ad ad)
			{
				resources = ad.Creative.Resources.Select(r => ResourceViewModel.Parse(r)).ToArray();
			}

			#endregion

			#region Public Methods

			public static ResourceViewModel[] Parse(Ad ad)
			{
				var fireTv = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.FireTV];
				var samsung = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Samsung];

				// Only add Resources if the Ad's Platform is FireTv or Samsung
				var isResourcesAllowed = ad.Platform.Id == fireTv || ad.Platform.Id == samsung;

				if (ad == null || !isResourcesAllowed || ad.Creative.Resources == null)
					return null;

				var resourcesViewModel = new ResourcesViewModel(ad);
				return resourcesViewModel.resources;
			}

			#endregion

			#region Subclasses

			public class ResourceViewModel
			{
				#region Members

				public int id { get; set; }
				public string name { get; set; }
				public ResourceTypeViewModel resourceType { get;set;}
				public string filename { get; set; }
				public string md5Hash { get; set; }
				public int? height { get; set; }
				public int? width { get; set; }

				#endregion

				#region Init

				public ResourceViewModel()
				{ }

				public ResourceViewModel(Resource resource)
				{
					id = resource.Id;
					name = resource.Name;
					resourceType = ResourceTypeViewModel.Parse(resource);
					filename = resource.Filename;
					md5Hash = resource.MD5Hash;
					height = resource.Height;
					width = resource.Width;
				}

				#endregion

				#region Public Methods

				public static ResourceViewModel Parse(Resource resource)
				{
					if (resource == null)
						return null;

					return new ResourceViewModel(resource);
				}

				#endregion

				#region Subclasses

				public class ResourceTypeViewModel
				{
					#region Members

					public string manifestName { get; set; }

					#endregion

					#region Init

					public ResourceTypeViewModel()
					{ }

					public ResourceTypeViewModel(Resource resource)
					{
						manifestName = resource.ResourceType.ManifestName;
					}

					#endregion

					#region Public Methods

					public static ResourceTypeViewModel Parse(Resource resource)
					{
						if (resource == null)
							return null;

						return new ResourceTypeViewModel(resource);
					}

					#endregion
				}

				#endregion
			}

			#endregion
		}


		#endregion
	}
}
