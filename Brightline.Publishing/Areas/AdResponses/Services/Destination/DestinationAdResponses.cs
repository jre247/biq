using BrightLine.Common.Models;
using BrightLine.Publishing.Areas.AdResponses.ViewModels;
using BrightLine.Publishing.Areas.AdResponses.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Publishing.Areas.AdResponses.Factories;

namespace BrightLine.Publishing.Areas.AdResponses.Services.Destination
{
	public class DestinationAdResponses : IAdTypeAdResponses
	{
		#region Members

		private Ad Ad { get;set;}
		private string TargetEnv { get;set;}
		private Guid PublishedId { get; set; }

		#endregion

		#region Init

		public DestinationAdResponses(Ad ad, string targetEnv, Guid publishedId)
		{
			Ad = ad;
			TargetEnv = targetEnv;
			PublishedId = publishedId;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get Ad Responses for Destination and Platform Combination
		/// </summary>
		/// <returns></returns>
		public IEnumerable<AdResponseViewModel> GetAdResponses()
		{
			var adResponses = new List<AdResponseViewModel>();
			var factory = new PlatformDestinationAdResponses();

			// Get the concrete service to get Ad Responses for a specific platform for Destination
			var service = factory.GetService(Ad, TargetEnv, PublishedId);
			var adResponse = service.GetAdResponse();
			adResponses.Add(adResponse);

			return adResponses;
		}

		#endregion
	}
}
