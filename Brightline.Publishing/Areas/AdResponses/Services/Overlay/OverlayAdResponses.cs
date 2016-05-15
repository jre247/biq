using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Publishing.Areas.AdResponses.Factories;

namespace BrightLine.Publishing.Areas.AdResponses.Services.Overlay
{
	public class OverlayAdResponses : IAdTypeAdResponses
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get;set;}

		#endregion

		#region Init

		public OverlayAdResponses(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Responses for Overlay and Platform Combination
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AdResponseViewModel> GetAdResponses()
		{
			var adResponses = new List<AdResponseViewModel>();
			var factory = new PlatformOverlayAdResponses();

			// Get the concrete service to get Ad Responses for a specific platform for Overlay
			var service = factory.GetService(Ad, TargetEnv);
			var adResponse = service.GetAdResponse();
			adResponses.Add(adResponse);

			return adResponses;
		}

		#endregion
	}
}
