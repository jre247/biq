using BrightLine.Common.Campaigns.ViewModels;
using BrightLine.Common.Core;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Campaigns;
using BrightLine.Common.ViewModels.Entity;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using BrightLine.Publishing.Constants;
using BrightLine.Service;
using BrightLine.Service.Redis.Interfaces;
using BrightLine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BrightLine.Publishing.Areas.AdResponses.Html5BrandDestination.Interfaces;

namespace BrightLine.Publishing
{
	public class PublishService : CrudService<CmsPublish>, IPublishService
	{
		#region Members

		private IOLTPContextHelper OLTPContextHelper { get; set; }
		private ICampaignService Campaigns { get;set;}

		#endregion

		#region Init

		public PublishService(IRepository<CmsPublish> repo)
			: base(repo)
		{
			OLTPContextHelper = IoC.Container.GetInstance<BrightLine.Data.IOLTPContextHelper>();
			Campaigns = IoC.Resolve<ICampaignService>();
		}

		#endregion

		#region Public Methods

		public CmsPublishViewModel GetViewModel(int campaignId)
		{
			var campaign = Campaigns.Get(campaignId);
			if(!Campaigns.IsAccessible(campaign))
				throw new AccessViolationException(CmsPublishConstants.Errors.InaccessibleCampaign);

			var vm = new CmsPublishViewModel(campaignId);
	
			return vm;
		}

		public BoolMessageItem Publish(int campaignId, string env)
		{
			var cmsPublishes = IoC.Resolve<IRepository<CmsPublish>>();

			// Create a unique identifier that is tied to this publish
			var publishId = Guid.NewGuid();

			try
			{
				var campaign = Campaigns.Get(campaignId);
				var boolMessage = ValidateForPublish(campaign, env);
				if (!boolMessage.Success)
					return boolMessage;

				// Create a Publish record in SQL
				var cmsPublish = CreatePublishRecord(env, campaign, publishId);

				// Publish Ad Responses
				var adResponsesService = IoC.Resolve<IAdResponsesService>();
				adResponsesService.Publish(campaign, env.ToLower(), publishId);

				// Publish HTML 5 Brand Destination Ads
				var Html5BrandDestinationService = IoC.Resolve<IHtml5BrandDestinationService>();
				Html5BrandDestinationService.Publish(campaign, cmsPublish);

				// Publish Complete Processing
				ProcessPublishCompleted(cmsPublishes, cmsPublish);

				return BoolMessageItem.GetSuccessMessage();
			}
			catch (Exception ex)
			{
				ProcessPublishFailed(publishId);				

				throw ex;
			}			
		}

		public void PurgeCampaignAdResponses(int campaignId, string targetEnv)
		{
			var redisService = IoC.Resolve<IRedisService>();
			var campaigns = IoC.Resolve<CampaignService>();
			var keys = new List<string>();

			var campaign = campaigns.Get(campaignId);
			var ads = campaign.Ads.ToList();
			foreach (var ad in ads)
			{
				if (ad.AdTag == null)
					continue;

				var key = AdResponseHelper.GetAdResponseKey(targetEnv, ad);
				keys.Add(key);				
			}

			redisService.Purge(keys);
		}

		#endregion

		#region Private Methods

		private BoolMessageItem ValidateForPublish(Campaign campaign, string env)
		{		
			if (!Campaigns.IsAccessible(campaign))
				return new BoolMessageItem(false, CmsPublishConstants.Errors.InaccessibleCampaign);

			var allowedEnvironments = new HashSet<string>();
			allowedEnvironments.Add(CmsPublishConstants.Environments.Dev);
			allowedEnvironments.Add(CmsPublishConstants.Environments.Uat);
			allowedEnvironments.Add(CmsPublishConstants.Environments.Pro);
			var isAllowedEnvironment = allowedEnvironments.Contains(env);
			if (!isAllowedEnvironment)
				return new BoolMessageItem(false, CmsPublishConstants.Errors.InvalidEnvironment);

			return BoolMessageItem.GetSuccessMessage();
		}

		private CmsPublish CreatePublishRecord(string env, Campaign campaign, Guid publishId)
		{
			var dateHelperService = IoC.Resolve<IDateHelperService>();
			var cmsPublishStatuses = IoC.Resolve<IRepository<CmsPublishStatus>>();

			var user = Auth.UserModel;
			var userEmail = user.Email;
			var userId = user.Id;
			CmsPublish cmsPublish = new CmsPublish();

			cmsPublish.PublishId = publishId;
			cmsPublish.Campaign = campaign;
			cmsPublish.IPAddress = HttpContext.Current.Request.UserHostAddress;
			cmsPublish.TimeStarted = dateHelperService.GetDateUtcNow();
			cmsPublish.TargetEnvironment = env;
			cmsPublish.UserId = user.Id;
			cmsPublish.CmsPublishStatus_Id = Lookups.CmsPublishStatuses.HashByName[CmsPublishConstants.Statuses.InProgress];

			cmsPublish = Create(cmsPublish);

			return cmsPublish;
			
		}

		private void ProcessPublishCompleted(IRepository<CmsPublish> cmsPublishes, CmsPublish cmsPublish)
		{
			cmsPublish.CmsPublishStatus_Id = Lookups.CmsPublishStatuses.HashByName[CmsPublishConstants.Statuses.Completed];
			cmsPublish.TimeEnded = DateTime.Now;
			cmsPublishes.Update(cmsPublish);
			cmsPublishes.Save();
		}

		private void ProcessPublishFailed(Guid publishId)
		{
			var cmsPublishes = IoC.Resolve<IRepository<CmsPublish>>();

			var cmsPublish = cmsPublishes.Where(c => c.PublishId == publishId).SingleOrDefault();
			if (cmsPublish != null)
			{
				cmsPublish.CmsPublishStatus_Id = Lookups.CmsPublishStatuses.HashByName[CmsPublishConstants.Statuses.Failed];
				cmsPublish.TimeEnded = DateTime.Now;
				cmsPublishes.Update(cmsPublish);
				cmsPublishes.Save();
			}
		}

		#endregion	
	}
}
