using BrightLine.Common.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public class MockHttpContextHelper : IHttpContextHelper
	{
		private static IDictionary _Items { get; set; }

		public IDictionary Items
		{
			get 
			{
				if (_Items == null)
					_Items = new Dictionary<string, object>();

				return  _Items;
			}
			
		}

		public void RemoveItem(string key)
		{
			_Items.Remove(key);
		}
	}
}
