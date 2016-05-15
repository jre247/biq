using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;

namespace BrightLine.Data
{
    public sealed class OLTPContextSingleton
    {
        private static readonly object padlock = new object();
		private const string REQUEST_KEY = "brightline_dbcontext";


        OLTPContextSingleton() { }

		/// <summary>
		/// Gets an instance of the brightline context based on the request.
		/// </summary>
		public static OLTPContext Instance
		{
			get
			{
				if (HttpContext.Current == null)
					return null;

				// Does not exist ? Add ctx and make available for request.
				if (!HttpContext.Current.Items.Contains(REQUEST_KEY))
				{
					var ctx = new OLTPContext();
					HttpContext.Current.Items.Add(REQUEST_KEY, ctx);
					return ctx;
				}

				return HttpContext.Current.Items[REQUEST_KEY] as OLTPContext;
			}
		}


		/// <summary>
		/// Dispose the database context per request.
		/// </summary>
		public static void Dispose()
		{
			if (HttpContext.Current == null)
				return;

			// Does not exist ? Add ctx and make available for request.
			if (!HttpContext.Current.Items.Contains(REQUEST_KEY))
			{
				return;
			}

			// Dispose 
			var ctx = HttpContext.Current.Items[REQUEST_KEY] as OLTPContext;
			if (ctx != null)
			{
				ctx.Dispose();
			}
		}
    }
}