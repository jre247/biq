using BrightLine.Common.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
	public class CampaignPlatformViewModel
	{
		public int id { get; set; }
		public string name { get; set; }

		public static JObject ToJObject(IEnumerable<Platform> platforms)
		{
			if (platforms == null)
				return null;

			var json = new JObject();
			json["platforms"] = ParsePlatforms(platforms);
			return json;
		}

		private static JArray ParsePlatforms(IEnumerable<Platform> platforms)
		{
			var platformsJson = new JArray();
			if (platforms == null)
				return platformsJson;

			var ps = (from p in platforms where p != null select p.Id);
			platformsJson = new JArray(ps);
			return platformsJson;
		}
	}
}