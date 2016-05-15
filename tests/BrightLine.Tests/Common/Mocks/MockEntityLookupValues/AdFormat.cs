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
		public static List<AdFormat> CreateAdFormats()
		{
			var recs = new List<AdFormat>
			{
				new AdFormat
				{
					Id = 1,				
				},
				new AdFormat
				{
					Id = 2				
				}
			};

			return recs;
		}
	}
}
