using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Campaigns.ViewModels
{
	public class CmsPublishViewModel
	{
		public int CampaignId { get; set; }

		public IEnumerable<CmsPublishItemViewModel> PublishHistory { get; set; }

		public CmsPublishViewModel(int campaignId)
		{
			var cmsPublishService = IoC.Resolve<IPublishService>();

			CampaignId = campaignId;

			PublishHistory = cmsPublishService.Where(c => c.Campaign.Id == campaignId).OrderByDescending(c => c.TimeStarted).ToList().Select(c => new CmsPublishItemViewModel(c)).ToList();
		}
	}

	public class CmsPublishItemViewModel
	{
		public CmsPublishItemViewModel(CmsPublish cmsPublish)
		{
			if (cmsPublish == null) 
				return;

			var users = IoC.Resolve<IUserService>();

			Id = cmsPublish.Id;
			CampaignId = cmsPublish.Campaign.Id;
			PublishId = cmsPublish.PublishId;
			TimeStarted = cmsPublish.TimeStarted;
			TargetEnvironment = cmsPublish.TargetEnvironment;
			var user = users.Get(cmsPublish.UserId);	
			UserEmail = user.Email;
			Status = cmsPublish.CmsPublishStatus.Name;
		}

		public int Id { get;set;}

		public int CampaignId { get; set; }

		public Guid PublishId { get; set; }

		[Display(Name = "Target Environment")]
		public string TargetEnvironment { get; set; }

		[Display(Name = "User")]
		public string UserEmail { get; set; }

		[Display(Name = "Time Started")]
		public DateTime TimeStarted { get; set; }

		public string Status { get; set; }
	}
}
