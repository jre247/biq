using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace BrightLine.Common.Utility
{
	public class HttpHelper
	{
		/// <summary>
		/// Get the https protocol ( http or https )
		/// </summary>
		/// <returns></returns>
		public static string GetProtocol()
		{
			if(HttpContext.Current == null || HttpContext.Current.Request == null
				|| HttpContext.Current.Request.Url == null)
				return "http";
			
			// Certain properties of Url can throw exceptions, this is just a safegaurd
			var protocol = "http";
			try
			{
				protocol = HttpContext.Current.Request.Url.Scheme;									
			}
			catch(Exception)
			{
				
			}
			return protocol;
		}
	}
}
