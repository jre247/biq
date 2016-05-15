using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.AdType;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Constants;
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

		public static List<Product> CreateProducts()
		{
			var items = new List<Product>()
				{
					new Product() {Id = Lookups.Products.HashByName[ProductConstants.ProductNames.AmexDeo], Name = ProductConstants.ProductNames.AmexDeo, Brand = new Brand{Id = 1}}
				};
			return items;
		}

		
	}

}