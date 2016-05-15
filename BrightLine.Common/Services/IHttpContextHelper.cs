using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Services
{
	public interface IHttpContextHelper
	{
		IDictionary Items { get;}

		void RemoveItem(string key);
	}
}
