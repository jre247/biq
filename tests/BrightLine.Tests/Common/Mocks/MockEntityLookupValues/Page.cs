using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<Page> CreatePages()
		{
			var pages = new List<Page>();

			pages.Add(new Page { Id = 1, Name = "page 1", Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 } } });
			pages.Add(new Page { Id = 2, Name = "page 2", Feature = new Feature { Id = 1, Creative = new Creative { Id = 1 } } });
			pages.Add(new Page { Id = 3, Name = "page 3", Feature = new Feature { Id = 1, Creative = new Creative { Id = 2 } } });
			pages.Add(new Page { Id = 4, Name = "page 4", Feature = new Feature { Id = 1, Creative = new Creative { Id = 2 } } });

			return pages;
		}

		public static List<Page> GetPages()
		{
			var recs = new List<Page>()
				{
					new Page(){Id=1,Name = "It's nice here", RelativeUrl = "/happy"},
					new Page(){Id=2,Name = "This is fun", RelativeUrl = "/great"},
					new Page(){Id=3,Name = "We can do it", RelativeUrl = "/yes"},
					new Page(){Id=4,Name = "Alas", RelativeUrl = "/complete"},
					new Page(){Id=5,Name = "Woe is me", RelativeUrl = "/done"},
					new Page(){Id=6,Name = "We're very nice", RelativeUrl = "/nice"},
				};

			return recs;
		}

		public static List<PageDefinition> CreatePageDefinitions()
		{
			var recs = new List<PageDefinition>()
				{
					new PageDefinition{
						Id = 1000,
						Name = "Test 1",
						Blueprint = new Blueprint
						{
							Id = 1000
						}
					}
					
				};

			return recs;
		}

	}

}