using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public static class WebAuthenticationHelper
	{
		public static void SetBasicAuthHeader(WebRequest request, string username, string password)
		{
			string authInfo = username + ":" + password;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			request.Headers["Authorization"] = "Basic " + authInfo;
		}

		public static void SetBasicAuthHeader(WebClient client, string username, string password)
		{
			string authInfo = username + ":" + password;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			client.Headers["Authorization"] = "Basic " + authInfo;
		}
	}
}
