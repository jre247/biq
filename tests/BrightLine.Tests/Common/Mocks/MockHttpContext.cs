using BrightLine.Web.Models.Security;
using System.Web;
using System.Web.Security;

namespace BrightLine.Tests.Common.Mocks
{
	public class MockHttpContext : HttpContextBase
	{
		private static object _lock = new object();

		public static void Init(string username)
		{
			if (HttpContext.Current == null)
				lock (_lock)
					if (HttpContext.Current == null)
					{
						var request = new HttpRequest(null, "http://tempuri.org", null);
						var response = new HttpResponse(null);
						HttpContext.Current = new HttpContext(request, response);
						var ticket = new FormsAuthenticationTicket(username, true, 20000);
						var identity = new CustomIdentity(ticket);
						var principal = new CustomPrincipal(identity);
						HttpContext.Current.User = principal;
					}
		}
	}
}