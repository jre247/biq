using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Platform;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Service.RabbitMQ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Html5BrandDestination.Services
{
	public class Html5BrandDestinationService : IHtml5BrandDestinationService
	{
		#region Public Methods

		/// <summary>
		/// Publish a Html5 Brand Destination Ad
		/// </summary>
		/// <param name="campaign"></param>
		/// <param name="cmsPublish"></param>
		public void Publish(Campaign campaign, CmsPublish cmsPublish)
		{
			var settings = IoC.Resolve<ISettingsService>();
			var rabbitMQService = IoC.Resolve<IRabbitMQService>();

			// Build up a Manifest 
			var manifestVm = GetManifestToPublish(campaign, cmsPublish);
			var manifestVmAsJson = ManifestViewModel.ToJObject(manifestVm);

			// Publish Manifest to Rabbit MQ
			var exchange = settings.RabbitMQExchange;
			var queue = settings.RabbitMQQueue;
			var routingKey = settings.RabbitMQRoutingKey;
			rabbitMQService.Send(exchange, queue, manifestVmAsJson.ToString(), routingKey);
		}

		#endregion

		#region Private Methods

		private ManifestViewModel GetManifestToPublish(Campaign campaign, CmsPublish cmsPublish)
		{
			var user = Auth.UserModel;

			var manifestVm = new ManifestViewModel(campaign, user, cmsPublish);
		
			var platformRoku = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.Roku];
			var platformTvos = Lookups.Platforms.HashByName[PlatformConstants.PlatformNames.TVOS];
			var platformsToExclude = new HashSet<int> { platformRoku, platformTvos };

			// Filter Manifest Ads to be Brand Destination and their Platform to not be Roku or TVOS
			manifestVm.campaign.ads = manifestVm.campaign.ads.Where(a => !a.adType.isPromo && !platformsToExclude.Contains(a.platform.id)).ToArray();

			return manifestVm;
		}

		#endregion
	}
}
