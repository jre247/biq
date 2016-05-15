using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Services.External;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using BrightLine.Web.Helpers;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BrightLine.Web.Controllers
{
	[RoutePrefix("api/{action}/{model:alpha}")]
	[PascalCase]
	public class EntityApiController : ApiController
	{
		private IFileHelper FileHelper { get; set; }

		public EntityApiController()
		{

		}

		public EntityApiController(IFileHelper fileHelper)
		{
			FileHelper = fileHelper;
		}

		[GET("{id:int}/{deleteType:DeleteTypes=Hard}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public bool Delete([FromUri]string model, [FromUri]int id, [FromUri]DeleteTypes deleteType = DeleteTypes.Hard)
		{
			try
			{
				var em = EntityManager.GetManager(model);
				var deleted = em.Delete(id, deleteType);
				return deleted;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("{id:int}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public bool Restore([FromUri]string model, [FromUri] int id)
		{
			try
			{
				var em = EntityManager.GetManager(model);
				em.Restore(id);
				return true;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("{id:int}/{properties}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public IEntity Get([FromUri]string model, [FromUri]int id, [FromUri]string properties = null)
		{
			try
			{
				var em = EntityManager.GetManager(model);
				var instance = em.Get(id);
				SetFormatter(properties);
				return instance;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("{properties}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public IEntity[] GetAll([FromUri]string model, [FromUri]string properties = null)
		{
			try
			{
				var em = EntityManager.GetManager(model);
				var instances = em.GetAll();
				SetFormatter(properties);
				return instances.ToArray();
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("{addSelect:bool=false}/{addNA:bool=false}")]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public ILookup[] GetLookup([FromUri]string model, [FromUri] bool addSelect = false, [FromUri]bool addNA = false)
		{
			try
			{
				var em = EntityManager.GetManager(model);
				if (!em.IsInstance<ILookup>())
					throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad lookup type: (" + model + ")."));

				var instances = em.GetLookup();
				if (addNA)
					instances = instances.InsertLookups(0, EntityLookup.NotApplicable);
				if (addSelect)
					instances = instances.InsertLookups(0, EntityLookup.Select);

				return instances.ToArray();
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("api/models", IgnoreRoutePrefix = true)]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject GetModels()
		{
			try
			{
				var models = MetadataViewModel.GetModels();
				var json = MetadataViewModel.ToJObject(models);
				return json;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("api/lookups", IgnoreRoutePrefix = true)]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject GetLookups()
		{
			try
			{
				var models = MetadataViewModel.GetLookups();
				var json = MetadataViewModel.ToJObject(models);
				return json;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[POST("{id:int}")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public int Save([FromUri] string model, [FromUri] int id, [FromBody] JObject json)
		{
			var em = EntityManager.GetManager(model);
			var instance = em.GetOrNew(id);
			try
			{
				ReflectionHelper.TrySetProperties(instance, json);
				instance = em.Upsert(instance);
				return instance.Id;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[POST]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		public int Save([FromUri] string model)
		{
			try
			{
				var root = HttpContext.Current.Server.MapPath("~/App_Data/Resources");
				var provider = new MultipartFormDataStreamProvider(root);
				var task = Task.Run(async () => { await Request.Content.ReadAsMultipartAsync(provider); });
				task.Wait();

				int id;
				if (!int.TryParse(provider.FormData["Id"], out id))
					throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id missing."));

				var em = EntityManager.GetManager(model);
				var instance = em.GetOrNew(id);
				var nvc = new NameValueCollection(provider.FormData);
				var resourceType = provider.FormData["FileEntity"] ?? "Resource";
				CreateFile(provider, resourceType, ref nvc);
				ReflectionHelper.TrySetProperties(instance, nvc);
				instance = em.Upsert(instance);
				return instance.Id;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[POST("{id:int}/{async:bool=false}")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Admin + "," + AuthConstants.Roles.Developer + "," + AuthConstants.Roles.Employee)]
		public Resource UploadFile([FromUri] string model, [FromUri] int id, [FromUri]bool async)
		{
			try
			{
				// get the file data from the request
				var root = HttpContext.Current.Server.MapPath("~/App_Data/Resources");
				var provider = new MultipartFormDataStreamProvider(root);
				var task = Task.Run(async () => { await Request.Content.ReadAsMultipartAsync(provider); });

				// need to wait until this is over to return resources.
				task.Wait();

				var resource = CreateFile(provider, model);
				return resource;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[POST("{id:int}")]
		[System.Web.Http.AcceptVerbs("POST")]
		[System.Web.Http.HttpPost]
		[System.Web.Http.Authorize(Roles = AuthConstants.Roles.Admin + "," + AuthConstants.Roles.Developer + "," + AuthConstants.Roles.Employee)]
		public bool DeleteFile([FromUri] string model, [FromUri] int id)
		{
			try
			{
				var cloudFiles = IoC.Resolve<ICloudFileService>();

				var em = EntityManager.GetManager(model);
				var resource = em.Get(id) as Resource;
				if (resource == null)
					return false;

				cloudFiles.Delete(resource);
				em.Delete(id, DeleteTypes.Hard);
				return true;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		[GET("api/download/{id:int}", IgnoreRoutePrefix = true)]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		[System.Web.Http.Authorize]
		public void Download([FromUri] int id)
		{
			IoC.Log.Info("Api/download api endpoint is still being called when it shouldn't. Please find the code that is calling this api endpoint and modify it to not call it.");
		}

		[GET("api/userinfo", IgnoreRoutePrefix = true)]
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.HttpGet]
		public JObject UserInfo()
		{
			try
			{
				var users = IoC.Resolve<IUserService>();

				var user = Auth.UserModel;
				var ui = users.GetUserInfo(user);
				return ui;
			}
			catch (ValidationException vex)
			{
				IoC.Log.Error(vex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, vex.Message));
			}
			catch (Exception ex)
			{
				IoC.Log.Error(ex);
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error processing request."));
			}
		}

		#region Private methods

		private Resource CreateFile(MultipartFormDataStreamProvider provider, string resourceType)
		{
			var nvc = new NameValueCollection();
			return CreateFile(provider, resourceType, ref nvc);
		}

		private Resource CreateFile(MultipartFormDataStreamProvider provider, string resourceType, ref NameValueCollection nvc)
		{
			var cloudFiles = IoC.Resolve<ICloudFileService>();
			var auditEvents = IoC.Resolve<IAuditEventService>();

			var em = EntityManager.GetManager(resourceType);
			var fileData = provider.FileData;
			var resource = new Resource();
			foreach (var fd in fileData)
			{
				// the root provider has already written the file to localFilename so write the file, upload it to AWS (append ticks to filename to prevent collisions)
				var localFilename = fd.LocalFileName;
				var uploadedFilename = fd.Headers.ContentDisposition.FileName.Trim('\"');
				var extension = (Path.GetExtension(uploadedFilename) ?? "").Replace(".", "");
				var resourceNvc = new NameValueCollection
					{
						{"Name", uploadedFilename},
						{"Display", uploadedFilename},
						{"ShortDisplay", uploadedFilename},
						{"Url", ""}, // set in the AWS service
						{"Filename", uploadedFilename}, // set in the AWS service
						{"Extension", extension ?? ""},
						{"Source", "AWS"},
						{"User", Auth.UserModel.Id.ToString(CultureInfo.InvariantCulture)}
					};

				using (var fs = File.Open(fd.LocalFileName, FileMode.Open))
				{
					var size = fs.Length;
					if (size == 0)
						break;

					resourceNvc.Add("Size", size.ToString(CultureInfo.InvariantCulture));

					// if an image, get height and width for the Image
					var contentType = fd.Headers.ContentType.MediaType;
					if (FileHelper.ImageContentTypes.Contains(contentType))
					{
						using (var img = Image.FromStream(fs, false, false))
						{
							resourceNvc.Add("Width", img.Width.ToString(CultureInfo.InvariantCulture));
							resourceNvc.Add("Height", img.Height.ToString(CultureInfo.InvariantCulture));
						}
					}
					//TODO: if image, resize create thumbnail, preview, ...
				}

				ReflectionHelper.TrySetProperties(resource, resourceNvc);
				var url = cloudFiles.UploadFromPath(ref resource, localFilename, deleteFile: true);
				resource = em.Upsert(resource);
				nvc.Add(provider.FormData["FileProperty"] ?? "Resource", resource.Id.ToString(CultureInfo.InvariantCulture));
				var resourceProperty = provider.FormData["FileProperty"] ?? "Resource";
				nvc[resourceProperty] = resource.Id.ToString(CultureInfo.InvariantCulture);
				auditEvents.Audit("UploadFile", em.Model, "EntityApiController.UploadFile: " + uploadedFilename);
			}

			return resource;
		}

		private void SetFormatter(string properties)
		{
			var formatter = (JsonMediaTypeFormatter)this.Configuration.Formatters.First(f => f.GetType() == typeof(JsonMediaTypeFormatter));
			if (string.IsNullOrEmpty(properties))
			{
				formatter.SerializerSettings.ContractResolver = new PropertyContractResolver(isCamel: false);
				return;
			}

			var ps = properties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			formatter.SerializerSettings.ContractResolver = new PropertyContractResolver(isCamel: false, properties: ps);
		}

		#endregion
	}
}
