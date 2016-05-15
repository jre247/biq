using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces
{
	public interface IHtml5BrandDestinationService
	{
		void Publish(Campaign campaign, CmsPublish cmsPublish);
	}
}
