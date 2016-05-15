using BrightLine.Common.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Common.Utility.Helpers
{
	public class HttpContextHelper : IHttpContextHelper
	{
		private IDictionary _Items { get;set;}

		public IDictionary Items
		{
			get
			{
				if (_Items != null)
					return _Items;

				if (HttpContext.Current == null)
					throw new InvalidOperationException("HttpContext not available");

				return HttpContext.Current.Items;
			}
		}

		/// <summary>
		/// This method is only implemented for Unit Tests
		/// </summary>
		/// <param name="key"></param>
		public void RemoveItem(string key)
		{
			
		}
	}
}
