using System.Web;
using System.Web.Security;

namespace BrightLine.Common.Services
{
	public interface ICookieService
	{
		void Set(string key, string value);
		string Get(string key);
		void Remove(string key);
		void Clear();

		void SetAuth(string email, bool rememberMe);
		FormsAuthenticationTicket GetAuthenticationTicket();
		void SignOut(HttpSessionStateBase session);
	}
}
