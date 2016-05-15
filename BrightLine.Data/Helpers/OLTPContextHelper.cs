using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Data
{
	public class OLTPContextHelper : IOLTPContextHelper
	{
		public void SaveOLTPContext()
		{
			OLTPContextSingleton.Instance.SaveChanges();
		}
	}
}
