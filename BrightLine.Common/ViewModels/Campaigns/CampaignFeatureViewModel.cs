using BrightLine.Common.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrightLine.Common.ViewModels.Campaigns
{
    public class CampaignFeatureViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public FeatureTypeViewModel featureType { get; set; }
        public bool isDeleted { get; set; }
        public Dictionary<int, PageViewModel> pages { get; set; }
        public int? blueprintId { get; set; }

        public class PageViewModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public PageDefinitionViewModel pageDefinition { get; set; }
        }

        public class FeatureTypeViewModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public IEnumerable<int> blueprints { get; set; }
        }

        public class PageDefinitionViewModel
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public static JObject ToJObject(IEnumerable<Feature> features)
        {
            var json = new JObject();
            json["features"] = ParseFeatures(features);
            return json;
        }

        private static JObject ParseFeatures(IEnumerable<Feature> features)
        {
            var featuresJson = new JObject();
            if (features == null || !features.Any())
                return featuresJson;

            foreach (var feature in features)
            {
				int? blueprintId = null;

				//feature might not be assigned to a blueprint
				if(feature.Blueprint != null)
					blueprintId = feature.Blueprint.Id;

                var key = feature.Id.ToString(CultureInfo.InvariantCulture);
                var content = new CampaignFeatureViewModel
                    {
                        id = feature.Id,
                        name = feature.Name,
                        blueprintId = blueprintId,
                        featureType = new FeatureTypeViewModel
                            {
                                id = feature.FeatureType.Id,
                                name = feature.FeatureType.Name,
                                blueprints = feature.FeatureType.Blueprints.Select(b => b.Id).ToList()
                            },
                        isDeleted = feature.IsDeleted
                    };

                BuildFeaturePages(feature, content);

                var jo = JObject.FromObject(content);
                var featureJson = new JProperty(key, jo);
                featuresJson.Add(featureJson);
            }

            var json = JObject.FromObject(featuresJson);
            return json;
        }

        private static void BuildFeaturePages(Feature feature, CampaignFeatureViewModel content)
        {
            content.pages = new Dictionary<int, PageViewModel>();

			if (feature.Blueprint == null)
				return;
			 
			foreach (var pageDefinition in feature.Blueprint.PageDefinitions)
			{
				var page = feature.Pages.FirstOrDefault(p => p != null && p.PageDefinition != null && p.PageDefinition.Id == pageDefinition.Id);

				var pageViewModel = new PageViewModel
				{
					id = page != null ? page.Id : 0,
					name = page != null ? page.Name : string.Empty,
					pageDefinition = new PageDefinitionViewModel
					{
						id = pageDefinition.Id,
						name = pageDefinition.Name
					}
				};

				content.pages.Add(pageViewModel.pageDefinition.id, pageViewModel);
			}
			
        }
    }
}