using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class AdTrackingEventService : CrudService<AdTrackingEvent>, IAdTrackingEventService
	{
		public AdTrackingEventService(IRepository<AdTrackingEvent> repo)
			: base(repo)
		{ }
	}
}
