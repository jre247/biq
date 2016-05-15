using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Constants;
using BrightLine.Publishing.Areas.AdResponses.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Brightline.Publishing.Areas.AdResponses.ViewModels.VAST
{
	public class CompanionAdStaticResource
	{
		#region Properties

			[XmlAttribute(AttributeName = "creativeType")]
			public string CreativeType { get; set; }

			[XmlText]
			public string Value { get; set; }

			#endregion

			#region Init

			private CompanionAdStaticResource(Ad ad)
			{
				var settings = IoC.Resolve<ISettingsService>();

				CreativeType = VASTConstants.CreativeType;

				if (ad.CompanionAd.AdTag != null)
					// The query parameter "Roku_Ad_Id=ROKU_ADS_APP_ID" Needs to be present for both RAF and DI. The client app will then replace the Roku_Ad_Id macro with an actual Id for only RAF. This Roku_Ad_Id macro will still be present for DI.
					Value = string.Format("<![CDATA[{0}/?id={1}&{2}&ver=%%SDK_VER%%&cb=%%CACHEBUSTER%%&{3}={4}]]>", settings.AdServerUrl, ad.CompanionAd.AdTag.Id, AdTagUrlConstants.RokuAdIdMacro, AdTagUrlConstants.QueryParams.MBList, settings.MBList);
			}

			// parameterless constructor used for xml serialization
			public CompanionAdStaticResource()
			{ }

			#endregion

			#region Public Methods

			public static CompanionAdStaticResource Parse(Ad ad)
			{
				if (ad == null)
					return null;

				return new CompanionAdStaticResource(ad);
			}

			#endregion
	}
}
