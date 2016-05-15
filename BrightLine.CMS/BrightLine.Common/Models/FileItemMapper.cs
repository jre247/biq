using System;
using System.Collections.Generic;
using System.Web;
using BrightLine.Common.Models.Helpers;
using BrightLine.Utility;


namespace BrightLine.Common.Models
{
	/// <summary>
	/// Media file helper class.
	/// </summary>
	public class FileItemMapper
	{

		/// <summary>
		/// Create media files in the underlying datastore using the files supplied in the httprequest.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static BoolMessageItem<FileItem> GetFile(HttpRequestBase request)
		{
			// 1. Map files from the request.
			FileItem file = FileItemMapper.MapFile(request);

			// 2. No files ? Error out.
			if (file == null)
			{
				return new BoolMessageItem<FileItem>(false, "No file was uploaded", null);
			}

			// 3. Validate
			var result = FileItemHelper.Validate(file);
			if (!result.Success)
			{
				return new BoolMessageItem<FileItem>(false, result.Message, file);
			}
			return new BoolMessageItem<FileItem>(true, string.Empty, file);
		}
		

		/// <summary>
		/// Map a Media File from the Form to an object.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="fieldSuffix"></param>
		/// <returns></returns>
		public static FileItem MapFile(HttpRequestBase request, string fieldSuffix)
		{
			var file = new FileItem();
			MapFile(request, fieldSuffix, file);
			return file;
		}


		/// <summary>
		/// Maps data from the media file edit form to the media file object.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		public static void MapFile(HttpRequestBase request, string fieldSuffix, FileItem file)
		{		
			HttpPostedFileBase hpf = request.Files["file" + fieldSuffix];
			string externalfilename = request.Params["externalfile" + fieldSuffix];
			string filename = hpf.ContentLength == 0 ? externalfilename : hpf.FileName;
			
			if (file.LastWriteTime == DateTime.MinValue)
				file.LastWriteTime = DateTime.Now;

			// No Content?
			if (hpf.ContentLength == 0 && string.IsNullOrEmpty(externalfilename))
				return;

			// Get the file as a byte[]
			if (hpf.ContentLength > 0)
				file.Contents = GetContentsOfUploadedFileAsBytes2(hpf);

			// This will autoset the Name and Extension properties.
			file.FullNameRaw = filename;
			file.Length = hpf.ContentLength;
		}

		/// <summary>
		/// Get the content of an upload file as a string.
		/// </summary>
		/// <param name="inputFile">Path to input file.</param>
		/// <returns>Byte array with file contents.</returns>
		public static byte[] GetContentsOfUploadedFileAsBytes2(HttpPostedFileBase inputFile)
		{
			byte[] data = new byte[inputFile.ContentLength];
			int contentLength = inputFile.ContentLength;

			inputFile.InputStream.Read(data, 0, contentLength);
			return data;
		}

		/// <summary>
		/// Whether or not the user upload a file or specified an external file link.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="fieldSuffix"></param>
		/// <returns></returns>
		public static bool IsFileUploaded(HttpRequestBase request, string fieldSuffix)
		{
			HttpPostedFileBase hpf = request.Files["file" + fieldSuffix];
			string externalfilename = request.Params["externalfile" + fieldSuffix];

			// Both file and external file not supplied? 
			// Then it's an empty media file entry, skip this.
			if (hpf.ContentLength == 0 && string.IsNullOrEmpty(externalfilename))
				return false;
			return true;
		}


		/// <summary>
		/// Maps Form Collection data into FileItem objects.
		/// </summary>
		/// <param name="request">Http Request containing the form data.</param>
		/// <param name="refid">Applicable refid to apply. This can either be the id of an event or the id of a media folder</param>
		/// <param name="isParentDirectoryId">Whether or not the refid represents a media folder</param>
		/// <returns></returns>
		public static IList<FileItem> MapFiles(HttpRequestBase request)
		{
			IList<FileItem> files = new List<FileItem>();
			for (int ndx = 0; ndx < request.Files.Count; ndx++)
			{
				if (IsFileUploaded(request, ndx.ToString()))
				{
					var file = MapFile(request, ndx.ToString());
					files.Add(file);
				}
			}
			return files;
		}


		/// <summary>
		/// Maps Form Collection data into FileItem objects.
		/// </summary>
		/// <param name="request">Http Request containing the form data.</param>
		/// <param name="refid">Applicable refid to apply. This can either be the id of an event or the id of a media folder</param>
		/// <param name="isParentDirectoryId">Whether or not the refid represents a media folder</param>
		/// <returns></returns>
		public static FileItem MapFile(HttpRequestBase request)
		{
			FileItem file = null;
			if (IsFileUploaded(request, string.Empty))
			{
				file = MapFile(request, string.Empty);
			}
			return file;
		}
	}
}