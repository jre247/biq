using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Publishing.Areas.AdResponses.Helpers;
using BrightLine.Publishing.Areas.AdResponses.Factories;

namespace BrightLine.Publishing.Areas.AdResponses.Services.CommercialSpot
{
	public class CommercialSpotAdResponses : IAdTypeAdResponses
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get;set;}

		#endregion

		#region Init

		public CommercialSpotAdResponses(Ad ad, string targetEnv)
		{
			Ad = ad;
			TargetEnv = targetEnv;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Responses for Commercial Spot and Platform Combination
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AdResponseViewModel> GetAdResponses()
		{
			var adResponses = new List<AdResponseViewModel>();
			var factory = new PlatformCommercialSpotAdResponses();

			// Get the concrete service to get Ad Responses for a specific platform for Commercial Spot
			var service = factory.GetService(Ad, TargetEnv);
			var adResponse = service.GetAdResponse();
			adResponses.Add(adResponse);

			// Clone the Ad Response, but use a different Ad Tag
			//	*Reference BL-727 for background info on why this is being done. Note that this is temporary and will be removed in the future
			var adResponseShim = AdResponseViewModel.Clone(adResponse);
			adResponseShim.Key = AdResponseHelper.GetAdResponseKeyForShim(TargetEnv, Ad); // Specify a specific Key for the Ad Response Shim
			adResponses.Add(adResponseShim);

			return adResponses;
		}

		#endregion
	}
}
