using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Resources;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.Utility
{
	public class ResourceHelper : IResourceHelper
	{
		/// <summary>
		/// Get a viewmodel for a Resource
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="resourceFilename"></param>
		/// <param name="resourceName"></param>
		/// <param name="resourceType"></param>
		/// <param name="campaignId"></param>
		/// <returns></returns>
		public ResourceViewModel GetResourceViewModel(int resourceId, string resourceFilename, string resourceName, int resourceType, int campaignId)
		{
			return new ResourceViewModel
			{
				id = resourceId,
				name = resourceName,
				path = GetResourceDownloadPath(resourceId, resourceFilename, resourceType, campaignId),
				resourceType = resourceType
			};	
		}

		/// <summary>
		/// Generate a MD5 Hash for a file
		/// </summary>
		/// <param name="fileStream"></param>
		/// <returns></returns>
		public string GenerateMd5HashForFile(Stream fileStream)
		{
			using (var md5 = MD5.Create())
			{
				var md5Hash = BitConverter.ToString(md5.ComputeHash(fileStream)).Replace("-", "").ToLower();

				return md5Hash;
			}
		}

		/// <summary>
		/// Check if a Resource is an Image
		/// </summary>
		/// <param name="resourceTypeId"></param>
		/// <returns></returns>
		public bool IsImage(int resourceTypeId)
		{
			var hdImage = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdImage];
			var sdImage = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdImage];

			return resourceTypeId == hdImage || resourceTypeId == sdImage;
		}

		/// <summary>
		/// Check if a Resource is a Video
		/// </summary>
		/// <param name="resourceTypeId"></param>
		/// <returns></returns>
		public bool IsVideo(int resourceTypeId)
		{
			var hdVideo = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.HdVideo];
			var sdVideo = Lookups.ResourceTypes.HashByName[ResourceTypeConstants.ResourceTypeNames.SdVideo];

			return resourceTypeId == hdVideo || resourceTypeId == sdVideo;
		}

		/// <summary>
		/// Get the Resource download path
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public string GetResourceDownloadPath(Resource resource, bool useProtocol = false)
		{
			return GetResourceDownloadPath(resource.Id, resource.Filename, resource.ResourceType.Id, resource.Creative.Campaign.Id, useProtocol);
		}

		/// <summary>
		/// Get the Resource download path
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="resourceName"></param>
		/// <param name="campaignId"></param>
		/// <param name="resourceType"></param>
		/// <returns></returns>
		public string GetResourceDownloadPath(int resourceId, string resourceName, int resourceType, int campaignId, bool useProtocol = false)
		{
			return BuildResourceDownloadPath(resourceId, resourceName, resourceType, campaignId, useProtocol);
		}

		/// <summary>
		/// Build an upload path for a Media Resource
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public string BuildMediaResourceUploadPath(Resource resource, int campaignId)
		{
			string mediaAssetType = GetMediaResourceType(resource);
			return string.Format("campaigns/{0}/{1}/{2}", campaignId, mediaAssetType, resource.Filename);
		}

		/// <summary>
		/// Resize an image
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public byte[] ResizeImage(byte[] contents, ImageResizeOptions options)
		{
			using (MagickImage image = new MagickImage(contents))
			{
				var size = new MagickGeometry(options.Width, options.Height);

				// This will resize the image to a fixed size without maintaining the aspect ratio.
				// Normally an image will be resized to fit inside the specified size.
				size.IgnoreAspectRatio = options.IgnoreAspectRatio;

				image.Resize(size);

				using (var stream = new MemoryStream())
				{
					image.Write(stream);

					// Convert stream to byte array
					var contentsResized = stream.ToArray();

					return contentsResized;
				}
			}
		}

		/// <summary>
		/// Get the Media base url for a specific Resource Type (i.e. cdn-local-m.brightline.tv/campaigns/20395/images for Images)
		/// </summary>
		/// <param name="campaignId"></param>
		/// <param name="resourceType"></param>
		/// <returns></returns>
		public string GetBaseUrlForMediaResourceType(int campaignId, MediaResourceType resourceType)
		{
			string resourceTypeDirectory = null;
			var settings = IoC.Resolve<ISettingsService>();
			var baseUrl = settings.MediaCDNBaseUrl;

			if (resourceType == MediaResourceType.Images)
				resourceTypeDirectory = ResourceConstants.MediaResourceTypes.Images;
			else if (resourceType == MediaResourceType.Fonts)
				resourceTypeDirectory = ResourceConstants.MediaResourceTypes.Fonts;
			else
				resourceTypeDirectory = ResourceConstants.MediaResourceTypes.Videos;

			return string.Format("{0}/campaigns/{1}/{2}", baseUrl, campaignId, resourceTypeDirectory);
		}

		#region Private Methods

		/// <summary>
		/// Get a Media Resource type. This type will be used to determine which folder the Media Resource is stored in the cloud.
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		private string GetMediaResourceType(Resource resource)
		{
			var resourceType = resource.ResourceType.Id;

			return GetMediaResourceType(resourceType);
		}

		/// <summary>
		/// Get a Media Resource type. This type will be used to determine which folder the Media Resource is stored in the cloud.
		/// </summary>
		/// <param name="resourceType"></param>
		/// <returns></returns>
		private string GetMediaResourceType(int resourceType)
		{
			string mediaAssetType = null;

			if (IsImage(resourceType))
				mediaAssetType = ResourceConstants.MediaResourceTypes.Images;
			else if (IsVideo(resourceType))
				mediaAssetType = ResourceConstants.MediaResourceTypes.Videos;
			else
				throw new ArgumentException("Incorrect Resource Type is being used to build a Resource upload path.");
			return mediaAssetType;
		}

		/// <summary>
		/// Build the Resource download path url
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="resourceName"></param>
		/// <param name="resourceTypeId"></param>
		/// <param name="campaignId"></param>
		/// <param name="useProtocol">Value indicates whether or not to have http protocol in the url</param>
		/// <returns></returns>
		private string BuildResourceDownloadPath(int? resourceId, string resourceName, int resourceTypeId, int campaignId, bool useProtocol)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var campaigns = IoC.Resolve<ICampaignService>();
			var baseUrl = settings.MediaCDNBaseUrl;
			
			string mediaAssetType = GetMediaResourceType(resourceTypeId);

			var path = string.Format("{0}/campaigns/{1}/{2}/{3}", baseUrl, campaignId, mediaAssetType, resourceName);		

			if(useProtocol)
				path = "http:" + path;

			return path;
		} 

		#endregion
	}
}
