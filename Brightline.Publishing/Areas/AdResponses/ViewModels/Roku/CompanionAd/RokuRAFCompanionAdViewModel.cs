using Brightline.Publishing.Areas.AdResponses.ViewModels.VAST;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using BrightLine.Publishing.Areas.AdResponses.ViewModels.VAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Brightline.Publishing.Areas.AdResponses.ViewModels.Roku.CompanionAd
{
	[Serializable()]
	public class RokuRAFCompanionAdViewModel : BaseCompanionAdViewModel
	{
		#region Init

		// parameterless constructor used for xml serialization
		public RokuRAFCompanionAdViewModel()
		{ }

		private RokuRAFCompanionAdViewModel(Ad ad)
		{
			var settings = IoC.Resolve<ISettingsService>();

			Id = ad.Id.ToString();
			ApiFramework = VASTConstants.ApiFramework;

			var video = ad.Creative.Resources.Where(v => !v.IsDeleted).FirstOrDefault();
			if (video != null)
			{
				if (video.Width.HasValue)
					Width = video.Width.Value;

				if (video.Height.HasValue)
					Height = video.Height.Value;
			}

			StaticResource = CompanionAdStaticResource.Parse(ad);
		}

		#endregion

		#region Public Methods

		public override BaseCompanionAdViewModel Parse(Ad ad)
		{
			if (ad == null)
				return null;

			return new RokuRAFCompanionAdViewModel(ad);
		}

		#endregion
	}
}
