using BrightLine.Common.Models;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IResourceHelper
	{
		string GenerateMd5HashForFile(Stream fileStream = null);
		string GetResourceDownloadPath(int resourceId, string resourceName, int campaignId, int resourceType, bool useProtocol = false);
		string GetResourceDownloadPath(Resource resource, bool useProtocol = false);
		ResourceViewModel GetResourceViewModel(int resourceId, string resourceFilename, string resourceName, int campaignId, int resourceType);
		bool IsImage(int resourceTypeId);
		bool IsVideo(int resourceTypeId);
		string BuildMediaResourceUploadPath(Resource resource, int campaignId);
		byte[] ResizeImage(byte[] contents, ImageResizeOptions options);
		string GetBaseUrlForMediaResourceType(int campaignId, MediaResourceType resourceType);
	}
}
