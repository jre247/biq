using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
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
		public static List<TrackingEvent> CreateTrackingEvents()
		{
			var recs = new List<TrackingEvent>
			{
				new TrackingEvent
				{
					Id = 1,
					Name = "start"				
				},
				new TrackingEvent
				{
					Id = 2,
					Name = "collapse"			
				}
			};

			return recs;
		}
	}
}
