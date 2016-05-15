using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BrightLine.Tests.Common;


using BrightLine.CMS;
using BrightLine.CMS.AppImport;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.CMS.Service;
using BrightLine.Tests.Component.CMS.Validator_Services;
using BrightLine.Tests.Common.Mocks;
using BrightLine.Core;
using BrightLine.Data;
using BrightLine.CMS.Services;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json;
using BrightLine.Service;
using BrightLine.Common.Services;
using BrightLine.Common.Framework.Exceptions;
using BrightLine.Utility;
using BrightLine.Common.Framework;

namespace BrightLine.Tests.Component.CMS
{
	[TestFixture]
	public class PagesTests
	{
		IocRegistration Container;
		ICreativeService Creatives { get;set;}

		[SetUp]
		public void Setup()
		{
			MockUtilities.SetupIoCContainer(Container);

			Creatives = IoC.Resolve<ICreativeService>();
		}

		[Test(Description = "Get list of pages for non existent creative.")]
		public void Pages_For_Non_Existent_Creative()
		{
			var pages = Creatives.GetPagesForCreative(1234);

			Assert.IsTrue(pages.Count() == 0);
		}

		[Test(Description = "Get list of pages for existing creative.")]
		public void Pages_For_Existing_Creative()
		{
			var pages = Creatives.GetPagesForCreative(1);

			Assert.IsTrue(pages.Count() == 2);
		}

		[Test(Description = "Pages for creative has correct properties.")]
		public void Pages_For_Creative_Has_Correct_Properties()
		{
			var page = Creatives.GetPagesForCreative(1)[1];

			Assert.IsTrue(page.id == 1, "Page doesn't have correct id");
			Assert.IsTrue(page.name == "page 1", "Page doesn't have correct name");
		}
	}
}
