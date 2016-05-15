using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace BrightLine.Web
{
	#region TempData ModelState redirect read/write persistence
	// http://www.jefclaes.be/2012/06/persisting-model-state-when-using-prg.html

	public class PersistModelStateAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			filterContext.Controller.TempData["ModelState"] = filterContext.Controller.ViewData.ModelState;
		}
	}

	public class RestoreModelStateAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			if (!filterContext.Controller.TempData.ContainsKey("ModelState"))
				return;

			var modelState = filterContext.Controller.TempData["ModelState"] as ModelStateDictionary;
			if (modelState == null)
				return;

			filterContext.Controller.ViewData.ModelState.Merge(modelState);
		}
	}

	#endregion

	#region Compress attribute

	public class CompressAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
			if (string.IsNullOrEmpty(encodingsAccepted))
				return;

			encodingsAccepted = encodingsAccepted.ToLowerInvariant();
			var response = filterContext.HttpContext.Response;
			if (encodingsAccepted.Contains("deflate"))
			{
				response.AppendHeader("Content-encoding", "deflate");
				response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
			}
			else if (encodingsAccepted.Contains("gzip"))
			{
				response.AppendHeader("Content-encoding", "gzip");
				response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
			}
		}
	}

	#endregion

	#region BrightLineHandleErrorAttribute
	/// <summary>
	/// Represents an attribute that is used to handle an exception that is thrown by an action method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class BrightLineHandleErrorAttribute : FilterAttribute, IExceptionFilter
	{
		private readonly object _typeId = new object();
		private Type _exceptionType = typeof(Exception);
		private const string Default500View = "Error";
		private const string Default404View = "ResourceNotFoundError";
		private string _master;
		private string _view;

		/// <summary>
		/// Gets or sets the type of the exception.
		/// </summary>
		/// 
		/// <returns>
		/// The type of the exception.
		/// </returns>
		public Type ExceptionType
		{
			get
			{
				return this._exceptionType;
			}
			set
			{
				if (value == (Type)null)
					throw new ArgumentNullException("value");
				if (!typeof(Exception).IsAssignableFrom(value))
					throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The type '{0}' does not inherit from Exception", new object[1]
          {
            (object) value.FullName
          }));
				else
					this._exceptionType = value;
			}
		}

		/// <summary>
		/// Gets or sets the master view for displaying exception information.
		/// </summary>
		/// 
		/// <returns>
		/// The master view.
		/// </returns>
		public string Master
		{
			get
			{
				return this._master ?? string.Empty;
			}
			set
			{
				this._master = value;
			}
		}

		/// <summary>
		/// Gets the unique identifier for this attribute.
		/// </summary>
		/// 
		/// <returns>
		/// The unique identifier for this attribute.
		/// </returns>
		public override object TypeId
		{
			get
			{
				return this._typeId;
			}
		}

		/// <summary>
		/// Gets or sets the page view for displaying exception information.
		/// </summary>
		/// 
		/// <returns>
		/// The page view.
		/// </returns>
		public string View
		{
			get
			{
				if (string.IsNullOrEmpty(this._view))
					return "Error";
				else
					return this._view;
			}
			set
			{
				this._view = value;
			}
		}

		/// <summary>
		/// Called when an exception occurs.
		/// </summary>
		/// <param name="filterContext">The action-filter context.</param><exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
		public virtual void OnException(ExceptionContext filterContext)
		{
			if (filterContext == null)
				throw new ArgumentNullException("filterContext");
			if (filterContext.IsChildAction || filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
				return;
			var exception = filterContext.Exception;
			if ((new HttpException((string)null, exception).GetHttpCode() != 500 && new HttpException((string)null, exception).GetHttpCode() != 404) || !this.ExceptionType.IsInstanceOfType((object)exception))
				return;
			int exceptionType;
			if (new HttpException((string)null, exception).GetHttpCode() == 404)
			{
				exceptionType = 404;
				View = Default404View;
			}
			else
			{
				exceptionType = 500;
				View = Default500View;
			}
			var controllerName = (string)filterContext.RouteData.Values["controller"];
			var actionName = (string)filterContext.RouteData.Values["action"];
			var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
			var exceptionContext = filterContext;
			var viewResult1 = new ViewResult();
			viewResult1.ViewName = this.View;
			viewResult1.MasterName = this.Master;
			viewResult1.ViewData = (ViewDataDictionary)new ViewDataDictionary<HandleErrorInfo>(model);
			viewResult1.TempData = filterContext.Controller.TempData;
			var viewResult2 = viewResult1;
			exceptionContext.Result = (ActionResult)viewResult2;
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = exceptionType == 404 ? 404 : 500;
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}
	}

	#endregion

	#region Casing Attributes

	public class CamelCaseAttribute : Attribute, IControllerConfiguration
	{
		public void Initialize(HttpControllerSettings currentConfiguration, HttpControllerDescriptor currentDescriptor)
		{
			currentConfiguration.Formatters.Clear();
			var camelFormatter = new JsonMediaTypeFormatter
			{
				SerializerSettings =
				{
					ContractResolver = new PropertyContractResolver(isCamel:true),
					NullValueHandling = NullValueHandling.Include,
					PreserveReferencesHandling = PreserveReferencesHandling.None,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			};
			//add the camel case formatter
			currentConfiguration.Formatters.Add(camelFormatter);
		}
	}

	public class PascalCaseAttribute : Attribute, IControllerConfiguration
	{
		public void Initialize(HttpControllerSettings currentConfiguration, HttpControllerDescriptor controllerDescriptor)
		{
			currentConfiguration.Formatters.Clear();
			var pascalFormatter = new JsonMediaTypeFormatter
			{
				SerializerSettings =
				{
					ContractResolver = new PropertyContractResolver(isCamel:false),
					NullValueHandling = NullValueHandling.Include,
					PreserveReferencesHandling = PreserveReferencesHandling.None,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			};
			//add the pascal case formatter
			currentConfiguration.Formatters.Add(pascalFormatter);
		}
	}

	#endregion
}
