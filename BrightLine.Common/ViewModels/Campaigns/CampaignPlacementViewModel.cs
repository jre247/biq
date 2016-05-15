using BrightLine.Common.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
	public class CampaignPlacementViewModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public int? height { get; set; }
		public int? width { get; set; }
		public string locationDetails { get; set; }
		public int[] ads { get; set; }
		public int[] platforms { get; set; }
		public int? adTypeGroupId { get; set; }
		public int? categoryId { get; set; }
		public int? appId { get; set; }
		public int? mediaPartnerId { get; set; }

		public static JObject ToJObject(IEnumerable<Placement> placements)
		{
			var json = new JObject();
			json["placements"] = ParsePlacements(placements);
			return json;
		}

		private static JToken ParsePlacements(IEnumerable<Placement> placements)
		{
			var placementsJson = new JObject();
			if (placements == null || !placements.Any())
				return placementsJson;

			foreach (var placement in placements.Distinct())
			{
				if (placement == null)
					continue;

				var key = placement.Id.ToString(CultureInfo.InvariantCulture);
				var content = new CampaignPlacementViewModel
				{
					id = placement.Id,
					name = placement.Name,
					height = placement.Height,
					width = placement.Width,
					locationDetails = placement.LocationDetails,
					ads = placement.Ads.Select(a => a.Id).ToArray(),
					platforms = placement.Ads.Where(a => a.Platform != null).Select(a => a.Platform.Id).Distinct().ToArray(),
					adTypeGroupId = (placement.AdTypeGroup == null) ? null : (int?)placement.AdTypeGroup.Id,
					categoryId = (placement.Category == null) ? null : (int?)placement.Category.Id,
					appId = (placement.App == null) ? null : (int?)placement.App.Id,
					mediaPartnerId = (placement.MediaPartner == null) ? null : (int?)placement.MediaPartner.Id
				};
				var jo = JObject.FromObject(content);
				var placementJson = new JProperty(key, jo);
				placementsJson.Add(placementJson);
			}

			var json = JObject.FromObject(placementsJson);
			return json;
		}
	}
}