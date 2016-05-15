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
		public static List<AdTrackingEvent> CreateAdTrackingEvents()
		{
			var recs = new List<AdTrackingEvent>
			{
				new AdTrackingEvent
				{
					Id = 1,
					Ad = new Ad{Id = 1},
					TrackingUrl = "www.tracking1.com/track",
					TrackingEvent = new TrackingEvent{Id = 1, Name = "start"}			
				}
			};

			return recs;
		}
	}
}
