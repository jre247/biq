using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BrightLine.Web.Controllers
{
	[RoutePrefix("api/cms")]
	public partial class CmsApiController : ApiController
	{


		/// <summary>
		/// Resource registration will create a new resource. One or more resources can self reference another 
		/// resource via their Parent field - this is used for resources that point to the same file, since filename is unique
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[POST("resources/register")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public JObject Register(ResourceViewModel model)
		{
			try
			{
				var resources = IoC.Resolve<IResourceService>();

				var resourceRegistered = resources.Register(model);
				if (resourceRegistered == null)
					return null;

				return ResourceViewModel.Parse(resourceRegistered);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "Error processing request." });
			}
		}

		/// <summary>
		/// Upload a resource to the media server.
		/// </summary>
		/// <param name="resourceId">The Id for the Resource</param>
		/// <param name="campaignId">The Campaign Id for the Resource</param>
		/// <param name="isCms">This will be true when a Resource is associated with Cms models/settings.</param>
		[POST("resources/upload/{resourceId:int}/campaign/{campaignId:int}?isCms={isCms:bool}")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public void Upload([FromUri] int resourceId, [FromUri] int campaignId, [FromUri] bool isCms = false)
		{
			try
			{
				var resources = IoC.Resolve<IResourceService>();

				// Get file
				var file = HttpContext.Current.Request.Files[0];

				// Convert the file stream to a byte array
				byte[] contents;
				using (var reader = new BinaryReader(file.InputStream))
				{
					file.InputStream.Position = 0;
					contents = reader.ReadBytes(file.ContentLength);
				}

				// Upload resource
				resources.Upload(resourceId, campaignId, contents, isCms);
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = "There was a problem uploading the Resource." });
			}
		}


	}

}