using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public class MockDateHelperService : IDateHelperService
	{
		public DateTime CurrentDate { get;set;}

		public MockDateHelperService()
		{
			CurrentDate = new DateTime(2015, 5, 1);
		}

		public DateTime GetDateUtcNow()
		{
			return CurrentDate;
		}
	}
}
