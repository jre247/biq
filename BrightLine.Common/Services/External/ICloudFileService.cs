using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Common.Models;
using BrightLine.Utility;
using System.IO;

namespace BrightLine.Common.Services.External
{
	public interface ICloudFileService
	{
		string GenerateSignedUrl(Resource resource);
		string GenerateSignedUrl(string filename, string awsFilename);
		string Upload(ref Resource resource, byte[] content);
		string Upload(byte[] contents, string bucket, string filename, string awsFilename, bool deleteFile = true);
		string UploadFromPath(ref Resource resource, string filePath, bool deleteFile = true);
		byte[] GetFile(Resource resource, out string contentType);
		string GetFileText(Resource resource, out string contentType);
		void Delete(Resource resource);
		string ResourceBaseUrl { get;}
		BoolMessage UploadSpecificContentType(string contentType, Stream stream, string bucketName, string fileName);
	}
}