using Brightline.Publishing.Areas.AdResponses.ViewModels.Html5.CompanionAd;
using Brightline.Publishing.Areas.AdResponses.ViewModels.Roku.CompanionAd;
using Brightline.Publishing.Areas.AdResponses.ViewModels.VAST;
using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Publishing.Areas.AdResponses.Factories
{
	public class VASTCompanionAd
	{
		/// <summary>
		/// Return a concrete CompanionAd based on a specific Platform
		/// </summary>
		/// <param name="platform"></param>
		/// <returns></returns>
		public BaseCompanionAdViewModel GetCompanionAd(VASTPlatform platform)
		{
			BaseCompanionAdViewModel companionAd = null;

			switch (platform)
			{
				case VASTPlatform.RokuAdFramework:
					companionAd = new RokuRAFCompanionAdViewModel();
					break;
				case VASTPlatform.RokuDirectIntegration:
					companionAd = new RokuDICompanionAdViewModel();
					break;
				case VASTPlatform.Html5:
					companionAd = new Html5CompanionAdViewModel();
					break;
				default:
					throw new ArgumentException("The following platform is not allowed in VASTCompanionAd: " + platform.ToString());
			}

			return companionAd;
		}
	}
}
