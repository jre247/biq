using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using BrightLine.Utility;
using System;
using System.Configuration;
using System.IO;
using System.Text;


namespace BrightLine.Service.External.AmazonStorage
{
	// DOCS: http://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html
	public class AmazonS3Service : ICloudFileService
	{
		private readonly string _accessId;
		private readonly string _accessKey;
		public readonly string _bucket;

		public AmazonS3Service(AmazonS3Settings settings)
		{
			_accessId = settings.AccessId;
			_accessKey = settings.AccessKey;
			_bucket = settings.Bucket;
		}

		public string ResourceBaseUrl
		{
			get
			{
				return _bucket;
			}
		}

		/// <summary>
		/// Generates a signed url for S3 for the resource.
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public string GenerateSignedUrl(Resource resource)
		{
			var url = string.Empty;
			if (resource == null)
				return url;

			url = GenerateSignedUrl(resource.Name, resource.Filename);
			return url;
		}


		/// <summary>
		/// Generates a signed url for S3 with the awsFilename with the content disposition filename set.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="awsFilename"></param>
		/// <returns></returns>
		public string GenerateSignedUrl(string filename, string awsFilename)
		{
			var url = string.Empty;
			if (string.IsNullOrWhiteSpace(awsFilename))
				return url;
			if (string.IsNullOrWhiteSpace(filename))
				filename = awsFilename;

			try
			{
				var key = awsFilename;
				var request = new GetPreSignedUrlRequest
				{
					BucketName = _bucket,
					Key = key,
					Expires = DateTime.Now.AddMinutes(10),
					ResponseHeaderOverrides = { ContentDisposition = "attachment; filename=" + filename }
				};
				using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_accessId, _accessKey))
				{
					url = client.GetPreSignedURL(request);
				}
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return url;
		}


		/// <summary>
		/// Uploads a text file to S3.
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="content">The content of the file</param>
		/// <returns></returns>
		public string Upload(ref Resource resource, byte[] content)
		{
			var url = string.Empty;
			if (resource == null)
				return url;

			try
			{
				var awsFilename = BuildAwsFilename(ref resource);
				using (var transfer = new TransferUtility(_accessId, _accessKey))
				using (var stream = new MemoryStream(content))
				{
					transfer.Upload(stream, _bucket, awsFilename);
					url = GenerateSignedUrl(resource);
				}
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return url;
		}


		/// <summary>
		/// Uploads a local file to S3.
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="filePath">The local file path to upload to s3</param>
		/// <param name="deleteFile"></param>
		/// <returns></returns>
		public string UploadFromPath(ref Resource resource, string filePath, bool deleteFile = true)
		{
			var url = string.Empty;
			if (resource == null)
				return url;
			if (string.IsNullOrEmpty(filePath))
				return url;

			try
			{
				var awsFilename = BuildAwsFilename(ref resource);
				using (var transfer = new TransferUtility(_accessId, _accessKey))
				using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				{
					transfer.Upload(stream, _bucket, awsFilename);
					url = GenerateSignedUrl(resource);
				}
				//TODO: delete files at end of transfering to AWS S3
				//if (deleteFile && File.Exists(filePath))
				//	File.Delete(filePath);
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return url;
		}

		/// <summary>
		/// Upload an image or video file to S3
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="bucket"></param>
		/// <param name="filename"></param>
		/// <param name="awsFilename"></param>
		/// <param name="deleteFile"></param>
		/// <returns></returns>
		public string Upload(byte[] contents, string bucket, string filename, string awsFilename, bool deleteFile = true)
		{
			var url = string.Empty;

			try
			{
				using (var transfer = new TransferUtility(_accessId, _accessKey))
				{
					using (var stream = new MemoryStream(contents))
					{
						transfer.Upload(stream, bucket, awsFilename);
						url = GenerateSignedUrl(filename, awsFilename);
					}
				}
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return url;
		}

		/// <summary>
		/// Get the file contents from S3.
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public byte[] GetFile(Resource resource, out string contentType)
		{
			contentType = null;
			byte[] contents;
			if (resource == null)
				return null;

			try
			{
				var key = resource.Url;
				using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_accessId, _accessKey))
				{
					var request = new GetObjectRequest { BucketName = _bucket, Key = key };
					using (var response = client.GetObject(request))
					using (var stream = response.ResponseStream)
					using (var ms = new MemoryStream())
					{
						stream.CopyTo(ms);
						contents = ms.ToArray();
						contentType = response.ContentType;
					}
				}
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return contents;
		}


		/// <summary>
		/// Get the file contents from S3 as text.
		/// </summary>
		/// <param name="resource"></param>
		/// <param name="contentType"></param>
		/// <returns></returns>
		public string GetFileText(Resource resource, out string contentType)
		{
			var text = string.Empty;
			var contents = GetFile(resource, out contentType);
			if (contents == null)
				return text;

			text = Encoding.Default.GetString(contents);
			return text;
		}


		/// <summary>
		/// Delete a resource from S3.
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public void Delete(Resource resource)
		{
			if (resource == null)
				return;

			try
			{
				var key = resource.Filename;
				var req = new DeleteObjectRequest
					{
						BucketName = _bucket,
						Key = key
					};
				using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_accessId, _accessKey))
				{
					client.DeleteObject(req);
				}
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}
		}

		/// <summary>
		/// Uploads a file with a specified content type
		/// </summary>
		/// <param name="stream">The Stream to upload.</param>
		/// <param name="bucketName">The S3 Bucket to upload to.</param>
		/// <param name="fileName">The File Name/Key on S3 to use.</param>
		/// <returns></returns>
		public BoolMessage UploadSpecificContentType(string contentType, Stream stream, string bucketName, string fileName)
		{
			if (contentType == null)
				throw new ArgumentNullException("contentType");

			if (stream == null)
				throw new ArgumentNullException("stream");

			if (string.IsNullOrEmpty(bucketName))
				throw new ArgumentNullException("bucketName");

			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");

			try
			{
				AmazonS3Client client = new AmazonS3Client(_accessId, _accessKey);
				var request = new PutObjectRequest
				{
					BucketName = bucketName,
					ContentType = contentType,
					InputStream = stream,
					Key = fileName,
				};
				
				PutObjectResponse response = client.PutObject(request);
	
			}
			catch (AmazonS3Exception as3Ex)
			{
				if (as3Ex.ErrorCode != "InvalidAccessKeyId" && as3Ex.ErrorCode != "InvalidSecurity")
					throw;

				var aex = new UnauthorizedAccessException("Invalid AWS Credentials.", as3Ex);
				throw aex;
			}

			return new BoolMessage(true, null);		
		}
		

		#region Private methods

		private static string BuildAwsFilename(ref Resource resource)
		{
			var url = string.Format("{0}_{1}_{2:yyyyMMdd.tthhmmssffff}-{3}", Env.Name, resource.GetBaseType().Name, DateTime.UtcNow, resource.Filename);
			resource.Url = url;
			return url;
		}

		#endregion
	}
}