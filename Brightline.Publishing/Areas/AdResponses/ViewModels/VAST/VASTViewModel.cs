using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.ResourceType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Helpers;
using System.Xml;
using System.IO;
using BrightLine.Publishing.Constants;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using System.Web;
using Brightline.Publishing.Areas.AdResponses.ViewModels.VAST;
using BrightLine.Publishing.Areas.AdResponses.Enums;
using BrightLine.Publishing.Areas.AdResponses.Factories;

namespace BrightLine.Publishing.Areas.AdResponses.ViewModels.VAST
{	
	[XmlRoot("VAST")]
	public class VAST
	{
		#region Properties

		[XmlElement("Ad")]
		public AdInfo AdModel { get; set; }

		#endregion

		#region Init

		// parameterless constructor used for xml serialization
		public VAST()
		{ }

		public VAST(Ad ad, VASTPlatform vastPlatform)
		{
			AdModel = AdInfo.Parse(ad, vastPlatform);
		}

		#endregion

		#region Public Methods

		public static string ToXml(VAST vast)
		{
			var xmlSerializer = new XmlSerializer(vast.GetType());

			var xmlDoc = new XmlDocument();
			var xmlNav = xmlDoc.CreateNavigator();
			using (var xs = xmlNav.AppendChild())
			{
				xmlSerializer.Serialize(xs, vast);
			}

			var xml = xmlDoc.OuterXml;

			return xml;
		}

		/// <summary>
		/// Replace the Brightline Tracking Events node in a VAST with a string macro that the Ad Server will then replace with the actual Brightline Tracking Events
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static string ReplaceBrightlineTrackingEventsNode(string xml)
		{
			// *Note: this Brightline Tracking Events macro on the VAST will get replaced by data defined on the Metadata object by the Ad Server
			var brightlineTrackingEventsNode = string.Format(@"<Tracking event=""{0}"" />", VASTConstants.BrightlineTrackingEventsNode);
			xml = xml.Replace(brightlineTrackingEventsNode, VASTConstants.Macros.BrightlineTrackingEvents);
			return xml;
		}

		#endregion

		#region Subclasses

		public class AdInfo
		{
			#region Properties

			[XmlAttribute(AttributeName = "id")]
			public int Id { get; set; }

			[XmlElement("Inline")]
			public InlineInfo Inline { get; set; }

			#endregion

			#region Init

			// parameterless constructor used for xml serialization
			public AdInfo()
			{ }

			public AdInfo(Ad ad, VASTPlatform vastPlatform)
			{
				Id = ad.Id;
				Inline = InlineInfo.Parse(ad, vastPlatform);
			}

			#endregion

			#region Public Methods

			public static AdInfo Parse(Ad ad, VASTPlatform vastPlatform)
			{
				if (ad == null)
					return null;

				return new AdInfo(ad, vastPlatform);
			}

			#endregion

			#region Subclasses

			public class InlineInfo
			{
				#region Properties

				[XmlElement("AdSystem")]
				public string AdSystem { get; set; }

				[XmlElement("AdTitle")]
				public string AdTitle { get; set; }

				[XmlElement("Description")]
				public string Description { get; set; }

				[XmlElement("Creatives")]
				public CreativesInfo Creatives { get; set; }

				#endregion

				#region Init

				// parameterless constructor used for xml serialization
				public InlineInfo()
				{ }

				private InlineInfo(Ad ad, VASTPlatform vastPlatform)
				{
					AdSystem = string.Format("<![CDATA[{0}]]>", VASTConstants.AdSystem);
					AdTitle = string.Format("<![CDATA[{0}]]>", ad.Name);
					Description = string.Format("<![CDATA[{0}]]>", ad.Creative.Description);
					Creatives = CreativesInfo.Parse(ad, vastPlatform);
				}

				#endregion

				#region Public Methods

				public static InlineInfo Parse(Ad ad, VASTPlatform vastPlatform)
				{
					if (ad == null)
						return null;

					return new InlineInfo(ad, vastPlatform);
				}

				#endregion

				#region Subclasses

				public class CreativesInfo
				{
					#region Properties

					[XmlElement("Creative", Namespace = "MediaFile")]
					public CreativeInfoForVideo CreativeForVideo { get; set; }

					[XmlElement("Creative", Namespace = "CompanionAd")]
					public CreativeInfoForCompanion CreativeForCompanion { get;set;}

					#endregion

					#region Init

					// parameterless constructor used for xml serialization
					public CreativesInfo()
					{ }

					private CreativesInfo(Ad ad, VASTPlatform vastPlatform)
					{
						CreativeForVideo = CreativeInfoForVideo.Parse(ad);
						CreativeForCompanion = CreativeInfoForCompanion.Parse(ad, vastPlatform);
					}

					#endregion

					#region Public Methods

					public static CreativesInfo Parse(Ad ad, VASTPlatform vastPlatform)
					{
						if (ad == null)
							return null;

						return new CreativesInfo(ad, vastPlatform);
					}

					#endregion

					#region Subclasses

					public class CreativeInfoForVideo
					{
						#region Properties

						[XmlAttribute(AttributeName = "id")]
						public string Id { get; set; }

						[XmlAttribute(AttributeName = "sequence")]
						public int Sequence { get; set; }

						[XmlElement("Linear")]
						public LinearInfo Linear { get;set;}

						#endregion

						#region Init

						// parameterless constructor used for xml serialization
						public CreativeInfoForVideo()
						{ }

						private CreativeInfoForVideo(Ad ad)
						{
							Id = ad.Id.ToString(); 
							Sequence = PublishConstants.Sequence; 
							Linear = LinearInfo.Parse(ad);
						}

						#endregion

						#region Public Methods

						public static CreativeInfoForVideo Parse(Ad ad)
						{
							if (ad == null)
								return null;

							return new CreativeInfoForVideo(ad);
						}

						#endregion

						#region Subclasses

						public class LinearInfo
						{
							#region Properties

							[XmlElement("Duration")]
							public string Duration { get; set; }

							[XmlArray("TrackingEvents")]
							[XmlArrayItem("Tracking")]
							public TrackingInfo[] TrackingEvents { get; set; }

							[XmlElement("Tracking")]
							public TrackingInfo Tracking {get; set;}				

							[XmlArray("MediaFiles")]
							[XmlArrayItem("MediaFile")]
							public MediaFileInfo[] MediaFiles { get; set; }

							[XmlElement("MediaFile")]
							public MediaFileInfo MediaFile {get;set;}

							#endregion

							#region Init

							// parameterless constructor used for xml serialization
							public LinearInfo()
							{ }

							private LinearInfo(Ad ad)
							{
								var settings = IoC.Resolve<ISettingsService>();

								TrackingEvents = GetTrackingEvents(ad, settings);

								// only build MediaFile if the Ad's Creative has a resource
								var resource = ad.Creative.Resources.Where(v => !v.IsDeleted).FirstOrDefault();
								if (resource != null)
								{
									MediaFiles = new MediaFileInfo[1];
									var mediaFile = MediaFileInfo.Parse(ad);
									MediaFiles[0] = mediaFile;

									var seconds = (double)resource.Duration;
									var time = TimeSpan.FromSeconds(seconds);
									Duration = time.ToString(@"hh\:mm\:ss");
								}								
							}

							#endregion

							#region Public Methods

							public static LinearInfo Parse(Ad ad)
							{
								if (ad == null)
									return null;

								return new LinearInfo(ad);
							}

							#endregion

							#region Private Methods

							private static TrackingInfo[] GetTrackingEvents(Ad ad, ISettingsService settings)
							{
								var trackingEvents = new List<TrackingInfo>();
								IEnumerable<TrackingInfo> trackingEventsAll;

								// Define Brightline Tracking Events which will first be represented as a node in the VAST xml, and then later during post-processing will be replaced by a macro string. 
								// This macro string will then be replaced on the Ad Server by the following Tracking Events (in xml format):
								//		1) 4 Quartile Tracking Events 
								//		2) 1 Start Tracking Event 
								// *Note: this is a bit janky, since adding the string replace macro into the VAST xml will cause the xml to technically not be valid, however
								//		  this seems to be the best way of sending xml to the Ad Server that will tell the Ad Server to replace the Brightline Tracking Events macro string with the actual Brightline Tracking Events
								var brightlineTrackingEventsNode = new TrackingInfo(VASTConstants.BrightlineTrackingEventsNode, null);
								trackingEvents.Add(brightlineTrackingEventsNode);

								if (ad.AdTrackingEvents != null)
								{
									// Concat Externally defined Tracking Events (defined in Ad Edit UI) and Brightline defined Tracking Events (For now, there's only one Brightline Tracking Event for "start")
									var trackingEventsThirdParty = ad.AdTrackingEvents.Select(t => TrackingInfo.Parse(t.TrackingEvent.Name, t.TrackingUrl)).ToList();

									trackingEventsAll = trackingEvents.Concat(trackingEventsThirdParty);
								}
								else
								{
									trackingEventsAll = trackingEvents;
								}
								
								return trackingEventsAll.ToArray();
							}

							#endregion

							#region Subclasses

							// TODO: figure out logic for this after TrackingEvent is set up in Ad Edit UI
							public class TrackingInfo
							{
								#region Properties

								[XmlAttribute(AttributeName = "event")]
								public string Event { get; set; }

								[XmlText]
								public string Value { get; set; }

								#endregion

								#region Init

								// parameterless constructor used for xml serialization
								public TrackingInfo()
								{ }

								public TrackingInfo(string trackingEventName, string trackingUrl)
								{
									Event = trackingEventName;

									if (!string.IsNullOrEmpty(trackingUrl))
										Value = string.Format("<![CDATA[{0}]]>", trackingUrl);
								}

								#endregion

								#region Public Methods

								public static TrackingInfo Parse(string trackingEventName, string trackingUrl)
								{
									if (string.IsNullOrEmpty(trackingEventName) || string.IsNullOrEmpty(trackingUrl))
										return null;

									return new TrackingInfo(trackingEventName, trackingUrl);
								}

								#endregion
							}

							public class MediaFileInfo
							{
								#region Properties

								[XmlAttribute(AttributeName = "id")]
								public int Id { get; set; }

								[XmlAttribute(AttributeName = "delivery")]
								public string Delivery { get; set; }

								[XmlAttribute(AttributeName = "width")]
								public int Width { get; set; }

								[XmlAttribute(AttributeName = "height")]
								public int Height { get; set; }

								[XmlAttribute(AttributeName = "type")]
								public string Type { get; set; }

								[XmlAttribute(AttributeName = "bitrate")]
								public int Bitrate { get; set; }

								[XmlAttribute(AttributeName = "scalable")]
								public bool Scalable { get; set; }

								[XmlAttribute(AttributeName = "maintainAspectRatio")]
								public bool MaintainAspectRatio { get; set; }

								[XmlAttribute(AttributeName = "apiFramework")]
								public string ApiFramework { get; set; }

								[XmlText]
								public string Value { get; set; }

								#endregion

								#region Init

								// parameterless constructor used for xml serialization
								public MediaFileInfo()
								{ }

								private MediaFileInfo(Ad ad)
								{
									var resourceHelper = IoC.Resolve<IResourceHelper>();

									Id = ad.Id;
									Delivery = VASTConstants.DeliveryProgressive;

									var video = ad.Creative.Resources.Where(v => !v.IsDeleted).FirstOrDefault();
									if (video != null)
									{
										var settings = IoC.Resolve<ISettingsService>();

										if (video.Width.HasValue)
											Width = video.Width.Value;

										if (video.Height.HasValue)
											Height = video.Height.Value;

										Type = VASTAdResponseHelper.GetMediaFileTypeForVASTResponse(video);
										Bitrate = video.Bitrate.HasValue ? video.Bitrate.Value : 0;
										Scalable = true;  
										MaintainAspectRatio = true;
										ApiFramework = VASTConstants.ApiFramework;

										var url = resourceHelper.GetResourceDownloadPath(video, true);
										Value = string.Format("<![CDATA[{0}]]>", url);
									}
								}

								#endregion

								#region Public Methods

								public static MediaFileInfo Parse(Ad ad)
								{
									if (ad == null)
										return null;

									return new MediaFileInfo(ad);
								}

								#endregion
							}

							#endregion
						}

						#endregion
					}
					
					public class CreativeInfoForCompanion
					{
						#region Properties

						[XmlAttribute(AttributeName = "id")]
						public string Id { get; set; }

						[XmlAttribute(AttributeName = "sequence")]
						public int Sequence { get; set; }

						[XmlAttribute(AttributeName = "apiFramework")]
						public string ApiFramework { get; set; }

						[XmlArray("CompanionAds")]
						[XmlArrayItem("Companion")]
						public BaseCompanionAdViewModel[] CompanionAds { get; set; }

						#endregion

						#region Init

						// parameterless constructor used for xml serialization
						public CreativeInfoForCompanion()
						{ }

						private CreativeInfoForCompanion(Ad ad, VASTPlatform vastPlatform)
						{
							Id = ad.Id.ToString();
							Sequence = 1; // the sequence is used in vast to let the parser know the order to display creatives and if the sequence is the same, they’re showed at the same time. Right now sequence is always 1
							ApiFramework = VASTConstants.ApiFramework;	
							
							var factory = new VASTCompanionAd();
							var companionAd = factory.GetCompanionAd(vastPlatform);
							var companionAdParsed = companionAd.Parse(ad);

							// Build Companion Ads Array from Companion Ad
							var companionAds = new List<BaseCompanionAdViewModel>();
							companionAds.Add(companionAdParsed);	
							CompanionAds = companionAds.ToArray();		
						}

						#endregion

						#region Public Methods

						public static CreativeInfoForCompanion Parse(Ad ad, VASTPlatform vastPlatform)
						{
							if (ad == null)
								return null;

							return new CreativeInfoForCompanion(ad, vastPlatform);
						}

						#endregion
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

