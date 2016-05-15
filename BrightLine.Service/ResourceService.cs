using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.Utility.FileType;
using BrightLine.Common.Utility.Resources;
using BrightLine.Common.Utility.StorageSource;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service
{
	public class ResourceService : CrudService<Resource>, IResourceService
	{
		public ResourceService(IRepository<Resource> repo)
			: base(repo)
		{ }

		/// <summary>
		/// Register a Resource
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ResourceViewModel Register(ResourceViewModel model)
		{
			var newResource = CreateNewResource(model);

			var resourceRegistered = new ResourceViewModel
			{
				id = newResource.Id,
				name = newResource.Name,
				filename = newResource.Filename,
				width = newResource.Width,
				height = newResource.Height,
				size = newResource.Size,
				resourceType = newResource.ResourceType.Id
			};

			return resourceRegistered;
		}

		/// <summary>
		/// Upload a Resource
		/// </summary>
		/// <param name="resourceId"></param>
		/// <param name="campaignId"></param>
		/// <param name="Request"></param>
		public void Upload(int resourceId, int campaignId, byte[] contents, bool isCms = false)
		{
			if (contents == null)
				throw new ResourceUploadException("There is no specified file to upload.");

			UploadFile(resourceId, campaignId, contents, isCms);
		}

		#region Private Methods

		/// <summary>
		/// Calculate the bitrate for a video
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		private int CalculateBitrateForVideo(Resource resource)
		{
			var size = (decimal)resource.Size.Value / 1000; // get resource size in kilobits
			var bitrate = (decimal)(size / resource.Duration.Value); // Formula: size (kilobits) / duration (seconds)
			var bitrateRounded = (int)Math.Round(bitrate);

			return bitrateRounded;
		}

		/// <summary>
		/// Upload a file to the cloud
		/// </summary>
		/// <param name="resourceId">The Resource's Id</param>
		/// <param name="campaignId">The campaign Id for the Resource</param>
		/// <param name="contents">A byte array of the image/video</param>
		/// <param name="isCms">A Resource is considered part of the Cms if it is associated with models/settings.</param>
		/// <returns></returns>
		private BoolMessageItem UploadFile(int resourceId, int campaignId, byte[] contents, bool isCms)
		{
			var resourceHelper = IoC.Resolve<IResourceHelper>();
			var cloudFiles = IoC.Resolve<ICloudFileService>();
			var settings = IoC.Resolve<ISettingsService>();
			var campaigns = IoC.Resolve<ICampaignService>();
			var bucketName = settings.MediaS3Bucket;

			var campaign = campaigns.Get(campaignId);
			if (campaign == null)
				return new BoolMessageItem(false, "Campaign does not exist.");

			var resource = Get(resourceId);
			if (resource == null)
				return new BoolMessageItem(false, "Resource does not exist.");

			if (resource.Extension == null)
				return new BoolMessageItem(false, "Resource does not have an extension.");

			var filePath = resourceHelper.BuildMediaResourceUploadPath(resource, campaignId);

			// *** BL-708: There is an exception being thrown when using Magick.NET in dev and uat, but for some reason not locally.
			// *********** This code is commented out for now and this issue with this library will be revisited later.
			// Upload resized images
			//if (isCms)
			//{
			//	var resizedImages = GetResizedCmsImages(contents, filePath);
			//	foreach (var resizedImage in resizedImages)
			//		cloudFiles.Upload(resizedImage.Contents, bucketName, resource.Name, resizedImage.Filename);
			//}
			
			// Upload initial raw image
			cloudFiles.Upload(contents, bucketName, resource.Name, filePath);

			ProcessResourceUploadComplete(resource, contents);

			return new BoolMessageItem(true, null);
		}

		/// <summary>
		/// Get a list of resized Cms images. 
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="filePath"></param>
		/// <returns>A list of resized Cms images with widths: 1920, 1280, and 960</returns>
		private List<ImageResizeViewModel> GetResizedCmsImages(byte[] contents, string filePath)
		{
			var resizedImages = new List<ImageResizeViewModel>();

			var image1920 = new ImageResizeViewModel(contents, filePath, 1920, 1080);
			resizedImages.Add(image1920);

			var image1280 = new ImageResizeViewModel(contents, filePath, 1280, 720);
			resizedImages.Add(image1280);

			var image960 = new ImageResizeViewModel(contents, filePath, 960, 540);
			resizedImages.Add(image960);

			return resizedImages;
		}

		/// <summary>
		/// Run some post-processing after the Resource has been uploaded successfully
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="contents"></param>
		private void ProcessResourceUploadComplete(Resource resource, byte[] contents)
		{
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			using (var stream = new MemoryStream(contents))
			{
				resource.MD5Hash = resourceHelper.GenerateMd5HashForFile(stream);
				resource.IsUploaded = true;

				if (resource.Size.HasValue && resource.Duration.HasValue)
					resource.Bitrate = CalculateBitrateForVideo(resource);

				SetResourceParent(resource);
			}
		}

		/// <summary>
		/// Set the Resource's Parent
		/// </summary>
		/// <param name="resource"></param>
		private void SetResourceParent(Resource resource)
		{
			//get existing resource by md5Hash and where resource doesn't reference a parent resource 
			//we want the original resource which can be determined when that resource's parent field is null
			// **note: there can be multiple resources that reference the same physical file
			//		 the original physical file is linked to the resource that has parent field set to null, 
			//		 while the other resources referencing that physical file would have their parent_id column set to the original resource's id
			var existingResource = Where(r => !r.IsDeleted && r.MD5Hash == resource.MD5Hash && r.Parent == null && r.Id != resource.Id).SingleOrDefault();

			//if there's existing resource then:
			//	1) new resource's parentId field will reference existing resource's id
			//	2) new resource's filename field will reference existing resource's filename
			if (existingResource != null)
			{
				resource.Parent_Id = existingResource.Id;
				Update(resource);
			}			
		}

		/// <summary>
		/// Create a Resource
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private Resource CreateNewResource(ResourceViewModel model)
		{
			var resourceTypes = IoC.Resolve<IRepository<ResourceType>>();

			var resourceType = resourceTypes.Get(model.resourceType);
			if (resourceType == null)
				throw new ValidationException("Invalid resource type when registering resource.");

			var newResource = CreateResourceInstance(model, resourceType);

			newResource = Create(newResource);

			return newResource;
		}

		/// <summary>
		/// Determines if Resource is externally hosted by checking for http and https protocol in filename string
		/// </summary>
		/// <returns>Whether the resource is external or not</returns>
		private bool IsResourceExternal(ResourceViewModel model)
		{
			return model.filename.ToLower().Contains(ResourceConstants.PROTOCOL_HTTP) || model.filename.ToLower().Contains(ResourceConstants.PROTOCOL_HTTPS);
		}

		/// <summary>
		/// Get the Storage Source
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private StorageSource GetStorageSource(ResourceViewModel model)
		{
			var isResourceExternal = IsResourceExternal(model);
			int storageSourceId = 0;

			var storageSourceExternal = Lookups.StorageSources.HashByName[StorageSourceConstants.StorageSourceNames.External];
			var storageSourceMedia = Lookups.StorageSources.HashByName[StorageSourceConstants.StorageSourceNames.Media];
			if (isResourceExternal)
				storageSourceId = storageSourceExternal;
			else
				storageSourceId = storageSourceMedia;

			var storageSources = IoC.Resolve<IStorageSourceService>();

			var storageSource = storageSources.Get(storageSourceId);
			if (storageSource == null)
				throw new NullReferenceException("Storage Source does not exist for Id.");

			return storageSource;
		}

		/// <summary>
		/// Get the formatted filename for a Resource
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private string GetFormattedFileName(ResourceViewModel model)
		{
			string formattedFilename = null;

			// Decode filename to remove things like "%20"
			formattedFilename = HttpUtility.UrlDecode(model.filename);

			// Replace spaces with underscores
			formattedFilename = formattedFilename.Replace(" ", "_");

			// Remove characters that would conflict with resource upload
			formattedFilename = formattedFilename.Replace("%", "").Replace("<", "").Replace(">", "");

			// Format for 3rd party resource urls - publishers like Vivo might want to plug in their own resource url instead of uploading one in IQ
			var isResourceExternal = IsResourceExternal(model);
			if (isResourceExternal)
			{
				//strip protocol from filename and prepend filename with "//"
				formattedFilename = Regex.Replace(formattedFilename, ResourceConstants.PROTOCOL_HTTP, "", RegexOptions.IgnoreCase);
				formattedFilename = Regex.Replace(formattedFilename, ResourceConstants.PROTOCOL_HTTPS, "", RegexOptions.IgnoreCase);
				formattedFilename = ResourceConstants.SLASH_PREFIX + formattedFilename;
			}
			else
			{
				var fileExtension = getExtension(formattedFilename);
				formattedFilename = string.Format("{0}_{1}.{2}", getFilenameWithoutExtension(formattedFilename), Guid.NewGuid(), fileExtension.Name);
			}

			return formattedFilename;
		}

		/// <summary>
		/// Create a Resource Instance
		/// </summary>
		/// <param name="model"></param>
		/// <param name="resourceType"></param>
		/// <returns></returns>
		private Resource CreateResourceInstance(ResourceViewModel model, ResourceType resourceType)
		{
			var storageSource = GetStorageSource(model);
			var filenameFormatted = GetFormattedFileName(model);
			var extension = getExtension(filenameFormatted);
			var name = GetResourceName(filenameFormatted);
			var creative = GetCreative(model);

			var newResource = new Resource
			{
				Filename = filenameFormatted,
				Name = name,
				Extension = extension,
				Size = model.size,
				Width = model.width,
				Height = model.height,
				Creative = creative,
				Duration = model.duration,
				ResourceType = resourceType,
				StorageSource = storageSource
			};
			return newResource;
		}

		/// <summary>
		/// Get Resource Name
		/// </summary>
		/// <param name="filenameFormatted"></param>
		/// <returns></returns>
		private static string GetResourceName(string filenameInitial)
		{
			// Remove Guid and extension from filename
			//	*Note: It is assumed that the guid comes after the last underscore in the Resource's filename
			var indexOfGuid = filenameInitial.LastIndexOf("_");

			if(indexOfGuid == -1)
				return filenameInitial;

			var filenameFormatted = filenameInitial.Substring(0, indexOfGuid);

			return filenameFormatted;
		}

		/// <summary>
		/// Get a Creative instance
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private static Creative GetCreative(ResourceViewModel model)
		{
			Creative creative = null;
			var creatives = IoC.Resolve<ICreativeService>();
			if (model.creativeId.HasValue)
				creative = creatives.Get(model.creativeId.Value);

			return creative;
		}

		/// <summary>
		/// The extension will be after the last period of the filename.
		/// </summary>
		/// <param name="fullFilename"></param>
		/// <param name="fileExtension"></param>
		/// <param name="fileNameBase"></param>
		private FileType getExtension(string fullFilename)
		{
			var fileSplit = fullFilename.Split('.');
			var fileExtension = fileSplit[fileSplit.Length - 1];

			var fileExtensionId = Lookups.FileTypes.HashByName[fileExtension.ToLower()];

			var fileTypes = IoC.Resolve<IRepository<FileType>>();

			var extension = fileTypes.Get(fileExtensionId);

			return extension;
		}

		/// <summary>
		/// Get the Filename without its extension. 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		private string getFilenameWithoutExtension(string filename)
		{
			// The extension is after the last period in the filename, so removing everything after the last period will give you the filename base.
			var fileSplit = filename.Split('.');	
			var fileSplitWithoutExtension = fileSplit.Take(fileSplit.Length - 1).ToArray();
			var filenameWithoutExtension = string.Join("", fileSplitWithoutExtension);

			return filenameWithoutExtension;
		}

		#endregion


		
	}
}
