using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class DateHelperService : IDateHelperService
	{
		public DateTime GetDateUtcNow()
		{
			return DateTime.UtcNow;
		}


		public DateTime CurrentDate
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
