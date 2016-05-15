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
		public static List<CmsRef> CreateCmsRefs()
		{
			var recs = new List<CmsRef>()
				{
					new CmsRef{
						Id = 1,
						CmsRefType = new CmsRefType{Id = 1},
						CmsModelDefinition = new CmsModelDefinition{Id = 1092}
					}
					
				};

			return recs;
		}

		private static CmsRef GetCmsRef(int id, int cmsRefTypeId = 1, string cmsRefTypeName = "known", int cmsModelDefinitionId = 1, string cmsModelDefinitionName = "choice", string cmsModelDefinitionDisplayName = "Choice")
		{
			return new CmsRef
			{
				Id = 1,
				CmsRefType = new CmsRefType
				{
					Id = cmsRefTypeId,
					Name = cmsRefTypeName
				},
				CmsModelDefinition = new CmsModelDefinition
				{
					Id = cmsModelDefinitionId,
					Name = "choice",
					DisplayName = "Choice"
				}
			};
		}

	}

}