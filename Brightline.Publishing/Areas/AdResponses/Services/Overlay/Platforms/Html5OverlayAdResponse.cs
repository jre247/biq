using BrightLine.Common.Models;
using BrightLine.Common.Utility.Constants;
using BrightLine.Common.Utility.Enums;
using BrightLine.Common.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.AdTypes.PlatformAdResponses.Interfaces;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BrightLine.Publishing.Areas.AdResponses.Services.Overlay.Platforms
{
	public class Html5OverlayAdResponse : IPlatformAdResponseService
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get; set; }

		#endregion

		#region Init

		public Html5OverlayAdResponse(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Response for Overlay and Html5 (Samsung or FireTv) combination
		/// </summary>
		/// <returns></returns>
		public AdResponseViewModel GetAdResponse()
		{
			return AdResponseHelper.BuildGenericHtml5AdResponse(Ad, TargetEnv);
		}
		
		#endregion	
	}
}
