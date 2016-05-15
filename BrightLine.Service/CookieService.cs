using BrightLine.Common.Services;
using BrightLine.Common.Utility.Authentication;
using System;
using System.Web;
using System.Web.Security;

namespace BrightLine.Service
{
	public class CookieService : ICookieService
	{
		public string Get(string key)
		{
			var cookies = HttpContext.Current.Request.Cookies;
			if (cookies[key] == null)
				return null;

			var value = cookies[key].Value;
			string decrypted = null;
			try
			{
				decrypted = FormsAuthentication.Decrypt(value).UserData;
			}
			catch { } // we really don't need to worry when a cookie name doesn't exist.

			return decrypted;
		}

		public void Set(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return;

			var cookies = HttpContext.Current.Response.Cookies;
			if (cookies[key] != null) // Remove Cookie that is being updated
				cookies.Remove(key);

			var ticket = new FormsAuthenticationTicket(2, key, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, value);
			var encryptedValue = FormsAuthentication.Encrypt(ticket);
			var cookie = new HttpCookie(key, encryptedValue);
			cookies.Add(cookie);
		}

		public void Remove(string key)
		{
			var cookies = HttpContext.Current.Response.Cookies;
			if (cookies[key] == null)
				return;

			var httpCookie = cookies[key];
			if (httpCookie != null)
				httpCookie.Expires = DateTime.Now.AddYears(-100);
		}

		public void Clear()
		{
			if (HttpContext.Current.Response.Cookies.Count == 0)
				return;

			foreach (var cookie in HttpContext.Current.Response.Cookies.AllKeys)
			{
				Remove(cookie);
			}
		}

		public void SetAuth(string email, bool rememberMe)
		{
			Set(AuthConstants.Cookies.Email, email);
			FormsAuthentication.SetAuthCookie(email, rememberMe);
		}

		public FormsAuthenticationTicket GetAuthenticationTicket()
		{
			var key = FormsAuthentication.FormsCookieName;
			var cookies = HttpContext.Current.Request.Cookies;
			if (cookies[key] == null)
				return null;

			var authCookie = cookies[key];
			if (authCookie == null)
				return null;

			var value = authCookie.Value;
			if (string.IsNullOrEmpty(value))
				return null;

			FormsAuthenticationTicket ticket = null;
			try
			{
				ticket = FormsAuthentication.Decrypt(value);
			}
			catch { }

			return ticket;
		}

		public void SignOut(HttpSessionStateBase session)
		{
			FormsAuthentication.SignOut();
			Clear();
			if (session != null)
				session.Abandon();
		}
	}
}
