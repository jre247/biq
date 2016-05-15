using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.ViewModels.AdTrackingUrl;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BrightLine.Service.AdTrackingUrl
{
	public abstract class BaseAdTrackingUrlGenerator<T> where T : AdTrackingUrlViewModel
	{
		#region Members

		private readonly string BaseUrl;

		#endregion

		#region Init

		public BaseAdTrackingUrlGenerator()
		{
			var settings = IoC.Resolve<ISettingsService>();

			BaseUrl = settings.TrackingUrl + "?data=";
		}

		#endregion

		#region Abstract Methods

		public abstract string Generate(T viewModel);

		#endregion

		#region Protected Methods

		protected string BuildFullUrl(object viewModel)
		{
			var jsonStringifiedData = GetJsonStringifiedData(viewModel);

			return BaseUrl + jsonStringifiedData;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Build an encoded json stringified object based off of a specific ViewModel
		/// </summary>
		/// <param name="viewModel"></param>
		/// <returns></returns>
		private string GetJsonStringifiedData(object viewModel)
		{
			var json = JObject.FromObject(viewModel);

			var jsonStringified = json.ToString();
			var jsonStringifiedEncoded = HttpUtility.UrlEncode(jsonStringified);

			// For some reason UrlEncode utility replaces spaces with +, instead of %20
			jsonStringifiedEncoded = jsonStringifiedEncoded.Replace("+", "%20");

			return jsonStringifiedEncoded;
		}

		#endregion
	}
}
