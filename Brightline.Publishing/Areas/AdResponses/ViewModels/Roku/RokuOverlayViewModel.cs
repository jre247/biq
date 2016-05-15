using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Areas.AdResponses.Enums;
using BrightLine.Publishing.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.ViewModels.Roku
{
	public class RokuOverlayViewModel
	{
		#region Members

		public MetaViewModel meta;
		public StatesViewModel states;
		public ActionsViewModel actions;
		public TimelineViewModel.TimelineItemViewModel[] timeline;
		public string initialState;

		#endregion

		#region Init

		//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
		[JsonConstructor]
		private RokuOverlayViewModel()
		{ }

		public RokuOverlayViewModel(Ad ad, RokuSdk rokuSdk)
		{
			meta = MetaViewModel.Parse(ad);
			states = StatesViewModel.Parse(ad);
			actions = ActionsViewModel.Parse(ad, rokuSdk);
			timeline = TimelineViewModel.Parse();
			initialState = PublishRokuOverlayConstants.InitialState;
		}

		#endregion

		#region Public Methods

		public static JObject ToJObject(RokuOverlayViewModel rokuDestinationViewModel)
		{
			if (rokuDestinationViewModel == null)
				return null;

			var json = JObject.FromObject(rokuDestinationViewModel);
			return json;
		}

		#endregion

		#region Subclasses

		public class MetaViewModel
		{
			#region Members

			public int ad_id;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private MetaViewModel()
			{ }

			private MetaViewModel(Ad ad)
			{
				ad_id = ad.Id;
			}

			#endregion

			#region Public Methods

			public static MetaViewModel Parse(Ad ad)
			{
				if (ad == null)
					return null;

				return new MetaViewModel(ad);
			}

			#endregion
		}

		public class StatesViewModel
		{
			#region Members

			public StateViewModel state0;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private StatesViewModel()
			{ }

			private StatesViewModel(Ad ad)
			{
				state0 = StateViewModel.Parse(ad);
			}

			#endregion

			#region Public Methods

			public static StatesViewModel Parse(Ad ad)
			{
				if (ad == null)
					return null;

				return new StatesViewModel(ad);
			}

			#endregion

			#region Subclasses

			public class StateViewModel
			{
				#region Members

				public HDLayersViewModel[] hdLayers;
				public SDLayersViewModel[] sdLayers;
				public ActionsViewModel actions;

				#endregion

				#region Init

				//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
				[JsonConstructor]
				private StateViewModel()
				{ }

				private StateViewModel(Ad ad)
				{
					var hdLayer = HDLayersViewModel.Parse(ad);
					hdLayers = new List<HDLayersViewModel>{hdLayer}.ToArray();

					var sdLayer = SDLayersViewModel.Parse(ad);
					sdLayers = new List<SDLayersViewModel> { sdLayer }.ToArray();
					actions = ActionsViewModel.Parse();
				}

				#endregion

				#region Public Methods

				public static StateViewModel Parse(Ad ad)
				{
					if (ad == null)
						return null;

					return new StateViewModel(ad);
				}

				#endregion

				#region Subclasses

				public class HDLayersViewModel
				{
					#region Members

					public int index;
					public ContentArrayViewModel.ContentItemViewModel[] content;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private HDLayersViewModel()
					{ }

					private HDLayersViewModel(Ad ad)
					{
						index = PublishRokuOverlayConstants.StateIndex;
						content = ContentArrayViewModel.Parse(ad, true);
					}

					#endregion

					#region Public Methods

					public static HDLayersViewModel Parse(Ad ad)
					{
						if (ad == null)
							return null;

						return new HDLayersViewModel(ad);
					}

					#endregion
				}

				public class SDLayersViewModel
				{
					#region Members

					public int index;
					public ContentArrayViewModel.ContentItemViewModel[] content;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private SDLayersViewModel()
					{ }

					private SDLayersViewModel(Ad ad)
					{
						index = PublishRokuOverlayConstants.StateIndex;
						content = ContentArrayViewModel.Parse(ad, false);
					}

					#endregion

					#region Public Methods

					public static SDLayersViewModel Parse(Ad ad)
					{
						if (ad == null)
							return null;

						return new SDLayersViewModel(ad);
					}	
					
					#endregion	
				}

				public class ContentArrayViewModel
				{
					#region Members

					public ContentItemViewModel[] contentArray;

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private ContentArrayViewModel()
					{ }

					private ContentArrayViewModel(Ad ad, bool isHd)
					{
						contentArray = new ContentItemViewModel[1];
						var contentItem = ContentItemViewModel.Parse(ad, isHd);
						contentArray[0] = contentItem;
					}

					#endregion

					#region Public Methods

					public static ContentItemViewModel[] Parse(Ad ad, bool isHd)
					{
						if (ad == null)
							return null;

						var contents = new ContentArrayViewModel(ad, isHd);
						return contents.contentArray;
					}

					#endregion

					#region Subclasses

					public class ContentItemViewModel
					{
						#region Members

						public string url;
						public TargetRectViewModel TargetRect;
						public string CompositionMode;

						#endregion

						#region Init

						//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
						[JsonConstructor]
						private ContentItemViewModel()
						{ }

						private ContentItemViewModel(Ad ad, bool isHd)
						{
							var settings = IoC.Resolve<ISettingsService>();
							var resourceHelper = IoC.Resolve<IResourceHelper>();

							string resourceTypeName = null;

							if (isHd)
								resourceTypeName = ResourceTypeConstants.ResourceTypeNames.HdImage;
							else 
								resourceTypeName = ResourceTypeConstants.ResourceTypeNames.SdImage;

							var resourceType = Lookups.ResourceTypes.HashByName[resourceTypeName];

							
							CompositionMode = PublishRokuOverlayConstants.CompositionMode; 

							if (ad.Creative != null)
							{
								var resource = ad.Creative.Resources.Where(r => r.ResourceType.Id == resourceType && !r.IsDeleted).FirstOrDefault();
								if (resource != null)
								{
									url = resourceHelper.GetResourceDownloadPath(resource, true);
									TargetRect = TargetRectViewModel.Parse(ad, resource, isHd);
								}
							}						
						}

						#endregion

						#region Public Methods

						public static ContentItemViewModel Parse(Ad ad, bool isHd)
						{
							if (ad == null)
								return null;

							return new ContentItemViewModel(ad, isHd);
						}

						#endregion

						#region Subclasses

						public class TargetRectViewModel
						{
							#region Members

							public int? x;
							public int? y;
							public int? w;
							public int? h;

							#endregion

							#region Init

							//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
							[JsonConstructor]
							private TargetRectViewModel()
							{ }

							private TargetRectViewModel(Ad ad, Resource resource, bool isHd)
							{
								if (isHd)
								{
									x = ad.XCoordinateHd;
									y = ad.YCoordinateHd;
								}
								else
								{
									x = ad.XCoordinateSd;
									y = ad.YCoordinateSd;
								}
								
								w = resource.Width;
								h = resource.Height;
							}

							#endregion

							#region Public Methods

							public static TargetRectViewModel Parse(Ad ad, Resource resource, bool isHd)
							{
								if (ad == null || resource == null)
									return null;

								return new TargetRectViewModel(ad, resource, isHd);
							}

							#endregion
						}

						#endregion
					}
				
					#endregion
				}

				public class ActionsViewModel
				{
					#region Members

					public string enter;

					#endregion

					#region Init

					private ActionsViewModel()
					{
						enter = PublishRokuOverlayConstants.StateActionsEnter; 
					}

					#endregion

					#region Public Methods

					internal static ActionsViewModel Parse()
					{
						return new ActionsViewModel();
					}

					#endregion
				}

				#endregion
			}
		
			#endregion
		}

		public class ActionsViewModel
		{
			#region Members

			public LaunchMicrositeViewModel launchMicrosite;

			#endregion

			#region Init

			//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
			[JsonConstructor]
			private ActionsViewModel()
			{ }

			private ActionsViewModel(Ad ad, RokuSdk rokuSdk)
			{
				launchMicrosite = LaunchMicrositeViewModel.Parse(ad, rokuSdk);
			}

			#endregion

			#region Public Methods

			public static ActionsViewModel Parse(Ad ad, RokuSdk rokuSdk)
			{
				if (ad == null)
					return null;

				return new ActionsViewModel(ad, rokuSdk);
			}

			#endregion

			#region Subclasses

			public class LaunchMicrositeViewModel
			{
				#region Members

				public string type;
				public TargetViewModel target;

				#endregion

				#region Init

				//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
				[JsonConstructor]
				private LaunchMicrositeViewModel()
				{ }

				private LaunchMicrositeViewModel(Ad ad, RokuSdk rokuSdk)
				{
					type = PublishRokuOverlayConstants.LaunchMicrositeType;
					target = TargetViewModel.Parse(ad, rokuSdk);
				}

				#endregion

				#region Public Methods

				public static LaunchMicrositeViewModel Parse(Ad ad, RokuSdk rokuSdk)
				{
					if (ad == null)
						return null;

					return new LaunchMicrositeViewModel(ad, rokuSdk);
				}

				#endregion

				#region Subclasses

				public class TargetViewModel
				{
					#region Members

					[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
					public string type;
					[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
					public int? id;
					public string url;
					[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
					public bool? includeCore;				

					#endregion

					#region Init

					//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
					[JsonConstructor]
					private TargetViewModel()
					{ }

					private TargetViewModel(Ad ad, RokuSdk rokuSdk)
					{
						var settings = IoC.Resolve<ISettingsService>();

						if (rokuSdk == RokuSdk.RokuAdFramework)
							type = "json"; // TODO: add in constants

						// TODO: BL-813: Once IQ returns the generated Destination Ad Response then uncomment this first line and delete the code below that is currently setting the url
						//var roku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
						//var adTagId = ad.CompanionAd.AdTag.Id.ToString();
						//var adTagUrlGenerator = new AdTagUrlGenerator();
						//var adTagUrl = adTagUrlGenerator.Generate(adTagId, roku);
						//url = string.Format("{0}&ts={3}", adTagUrl, DateTime.UtcNow.Ticks);

						url = string.Format("{0}/ads/{1}/1.0/ad.json?v={2}", settings.MediaG1CDNBaseUrl, ad.DestinationAd.RepoName, ad.Id);

						if (rokuSdk == RokuSdk.DirectIntegration)
						{
							id = ad.Id;
							includeCore = false;
						}
					}

					#endregion

					#region Public Methods

					public static TargetViewModel Parse(Ad ad, RokuSdk rokuSdk)
					{
						if (ad == null)
							return null;

						return new TargetViewModel(ad, rokuSdk);
					}

					#endregion
				}

				#endregion
			}
			
			#endregion
		}

		public class TimelineViewModel
		{
			#region Members

			public TimelineItemViewModel[] timelineArray;

			#endregion

			#region Init

			private TimelineViewModel()
			{
				timelineArray = new TimelineItemViewModel[1];
				var timelineItem = TimelineItemViewModel.Parse();
				timelineArray[0] = timelineItem;
			}

			#endregion

			#region Public Methods

			public static TimelineItemViewModel[] Parse()
			{
				var timeline = new TimelineViewModel();
				return timeline.timelineArray;
			}

			#endregion

			#region Subclasses

			public class TimelineItemViewModel
			{
				#region Members

				public int ts;
				public string state;

				#endregion

				#region Init

				private TimelineItemViewModel()
				{
					ts = 1;
					state = PublishRokuOverlayConstants.TimelineState; 
				}

				#endregion

				#region Public Methods

				public static TimelineItemViewModel Parse()
				{
					return new TimelineItemViewModel();
				}

				#endregion
			}

			#endregion
		}
	
		#endregion
	}
}
