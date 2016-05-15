using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Enums;
using System;
using System.Web;
using System.Web.Mvc;

namespace BrightLine.Common.Utility
{
	public class FlashMessageExtensions : IFlashMessageExtensions
	{
		/// <summary>
		/// Creates a custom message cookie. These will not be picked up by the default msgGrowl checks.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		public void Custom(string message, bool isRedirect = false, Exception exception = null)
		{
			CreateCookieWithFlashMessage(FlashMessageNotification.Custom, message, isRedirect, exception);
		}


		/// <summary>
		/// Creates a success message cookie.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		public void Success(string message, bool isRedirect = false)
		{
			CreateCookieWithFlashMessage(FlashMessageNotification.Success, message, isRedirect);
		}


		/// <summary>
		/// Creates an info message cookie.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		public void Info(string message, bool isRedirect = false)
		{
			CreateCookieWithFlashMessage(FlashMessageNotification.Info, message, isRedirect);
		}


		/// <summary>
		/// Creates an debug message on the page.
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="isRedirect"></param>
		/// <returns></returns>
		public void Debug(Exception exception, bool isRedirect = false)
		{
			if (!Auth.IsUserInRole(AuthConstants.Roles.Developer) || exception == null)
				return;

			var errorCookie = new HttpCookie(string.Format("Flash.{0}", FlashMessageNotification.Debug), exception.Message) { Path = "/" };
			HttpContext.Current.Response.Cookies.Add(errorCookie);
		}


		/// <summary>
		/// Creates an error message cookie.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="isRedirect"></param>
		/// <param name="exception"></param>
		public void Error(string message, bool isRedirect = false, Exception exception = null)
		{
			CreateCookieWithFlashMessage(FlashMessageNotification.Error, message, isRedirect, exception);
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
		private void CreateCookieWithFlashMessage(FlashMessageNotification FlashMessageNotification, string message, bool isRedirect, Exception exception = null)
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