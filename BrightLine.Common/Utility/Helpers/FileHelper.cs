using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace BrightLine.Common.Utility
{
	public class FileHelper : IFileHelper
	{
		private IResourceService Resources { get;set;}
		private ISettingsService Settings { get;set;}
		private ICloudFileService CloudFiles { get;set;}
		private IAuditEventService AuditEvents { get; set; }

		public FileHelper()
		{
			Resources = IoC.Resolve<IResourceService>();
			Settings = IoC.Resolve<ISettingsService>();
			CloudFiles = IoC.Resolve<ICloudFileService>();
			AuditEvents = IoC.Resolve<IAuditEventService>();

		}

		public readonly string[] _ImageContentTypes = new[] { "image/bmp", "image/cis-cod", "image/gif", "image/jpeg", "image/jpg", "image/pipeg", "image/png", "image/svg+xml", "image/tiff", "image/x-cmu-raster", "image/x-cmx", "image/x-icon", "image/x-portable-bitmap", "image/x-portable-graymap", "image/x-portable-anymap", "image/x-portable-pixmap", "image/x-rgb", "image/x-xbitmap", "image/x-xpixmap", "image/x-xwindowdump", };
		public string[] ImageContentTypes
		{
			get
			{
				return _ImageContentTypes;
			}
		}


		public bool IsFilePresent(HttpPostedFileBase file)
		{
			return IsFilePresent(file.FileName);
		}

		public bool IsFilePresent(HttpPostedFile file)
		{
			return IsFilePresent(file.FileName);
		}

		private bool IsFilePresent(string filename)
		{
			return !string.IsNullOrEmpty(filename);
		}

		public Resource CreateFile(HttpPostedFileBase file)
		{
			return UploadFile(file);
		}

		public Resource CreateFile(HttpFileCollectionBase files)
		{
			if (files == null || files.Count == 0)
				return null;

			var file = files[0];
			if (file == null || IsFilePresent(file))
				return null;

			return UploadFile(file);
		}

		public string GetCloudFileDownloadUrl(Resource resource)
		{
			var bucketName = CloudFiles.ResourceBaseUrl;
			var awsS3BaseUrl = Settings.MediaBaseUrl;
			var fileDownloadUrl = string.Format("{0}/{1}/{2}", awsS3BaseUrl, bucketName, resource.Url);

			return fileDownloadUrl;
		}

		private Resource UploadFile(HttpPostedFileBase file)
		{
			if (file == null || !IsFilePresent(file))
				return null;

			byte[] contents;
			using (var reader = new BinaryReader(file.InputStream))
			{
				contents = reader.ReadBytes(file.ContentLength);
			}

			var uploadedFilename = file.FileName.Trim('\"');
			var extension = (Path.GetExtension(uploadedFilename) ?? "").Replace(".", "");
			var nvc = new NameValueCollection
				{
					{"Url", ""}, // set in the AWS service
					{"Name", uploadedFilename.PadLeft(5, '-')},
					{"Filename", uploadedFilename},
					{"Display", uploadedFilename},
					{"ShortDisplay", uploadedFilename},
					{"Extension", extension ?? ""},
					{"Source", "ResourceDoc"},
					{"Size", contents.Length.ToString(CultureInfo.InvariantCulture)}
				};

			if (ImageContentTypes.Contains(file.ContentType))
			{
				using (var ms = new MemoryStream(contents))
				using (var img = Image.FromStream(ms, false, false))
				{
					var width = img.Width;
					var height = img.Height;
					nvc.Add("Width", width.ToString(CultureInfo.InvariantCulture));
					nvc.Add("Height", height.ToString(CultureInfo.InvariantCulture));
				}
			}

			var resource = new Resource();
			ReflectionHelper.TrySetProperties(resource, nvc);
			CloudFiles.Upload(ref resource, contents);
			resource = Resources.Create(resource);
			AuditEvents.Audit("CreateFile", "Resource", "FileHelper.CreateFile: " + uploadedFilename);
			return resource;
		}
	}
}