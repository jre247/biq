using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Enums;
using System;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Common.Utility
{
	public static class FlashMessageExtensionMethods
	{
		/// <summary>
		/// Creates a custom message cookie. These will not be picked up by the default msgGrowl checks.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static ActionResult Custom(this ActionResult result, string message, Exception exception = null)
		{
			var isRedirect = (result.GetType() == typeof(RedirectToRouteResult));
			CreateCookieWithFlashMessage(FlashMessageNotification.Custom, message, isRedirect, exception);
			return result;
		}


		/// <summary>
		/// Displays a success message on the page.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <returns></returns>
		public static ActionResult Success(this ActionResult result, string message)
		{
			var isRedirect = (result.GetType() == typeof(RedirectToRouteResult));
			CreateCookieWithFlashMessage(FlashMessageNotification.Success, message, isRedirect);
			return result;
		}


		/// <summary>
		/// Creates a info message on the page.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <returns></returns>
		public static ActionResult Info(this ActionResult result, string message)
		{
			var isRedirect = (result.GetType() == typeof(RedirectToRouteResult));
			CreateCookieWithFlashMessage(FlashMessageNotification.Info, message, isRedirect);
			return result;
		}


		/// <summary>
		/// Creates an debug message on the page.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static ActionResult Debug(this ActionResult result, Exception exception = null)
		{
			if (!Auth.IsUserInRole(AuthConstants.Roles.Developer) || exception == null)
				return result;

			var isRedirect = (result.GetType() == typeof(RedirectToRouteResult));
			var errorCookie = new HttpCookie(string.Format("Flash{0}.{1}", (isRedirect ? ".Redirect" : ""), FlashMessageNotification.Debug), exception.Message) { Path = "/" };
			HttpContext.Current.Response.Cookies.Add(errorCookie);
			return result;
		}


		/// <summary>
		/// Creates an error message on the page.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static ActionResult Error(this ActionResult result, string message, Exception exception = null)
		{
			var isRedirect = (result.GetType() == typeof(RedirectToRouteResult));
			CreateCookieWithFlashMessage(FlashMessageNotification.Error, message, isRedirect, exception);
			return result;
		}

		/// <summary>
		/// Creates the flash message in the cookie so it is displayed in the client.
		/// NOTE: The cookie is deleted after message is displayed.
		/// Refer to : brightline.common.flashmessage.js
		/// </summary>
		/// <param name="FlashMessageNotification"></param>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		private static void CreateCookieWithFlashMessage(FlashMessageNotification FlashMessageNotification, string message, bool isRedirect, Exception exception = null)
		{
			if (Auth.IsUserInRole(AuthConstants.Roles.Developer) && exception != null)
			{
				var errorCookie = new HttpCookie(string.Format("Flash.{0}.{1}", (isRedirect ? ".Redirect" : ""), FlashMessageNotification.Debug), exception.Message) { Path = "/" };
				HttpContext.Current.Response.Cookies.Add(errorCookie);
			}
			var cookieName = string.Format("Flash{0}.{1}", (isRedirect ? ".Redirect" : ""), FlashMessageNotification);
			var cookie = new HttpCookie(cookieName, message) { Path = "/", Expires = DateTime.Now.AddDays(1), HttpOnly = false, Secure = false };
			HttpContext.Current.Response.Cookies.Remove(cookieName);
			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}
}