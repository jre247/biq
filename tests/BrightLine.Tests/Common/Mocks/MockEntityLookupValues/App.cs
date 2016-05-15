using BrightLine.Common.Models;
using BrightLine.Common.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<App> CreateApps()
		{
			var recs = new List<App>
			{
				new App
				{
					Id = 1,				
				},
				new App
				{
					Id = 2				
				}
			};

			return recs;
		}
	}
}
