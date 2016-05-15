using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.Platform;
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
		/// <summary>
		/// Gets all platforms
		/// </summary>
		/// <returns></returns>
		public static List<Platform> GetAllPlatforms()
		{
			var recs = new List<Platform>()
				{
					new Platform() {Id = 1, Name = "DirecTV"},
					new Platform() {Id = 2, Name = "Dish"},
					new Platform() {Id = 3, Name = "AT&T U-Verse"},
					new Platform() {Id = 4, Name = "Verizon FiOS"},
					new Platform() {Id = 5, Name = "Cablevision"},
					new Platform() {Id = 6, Name = "Charter"},
					new Platform() {Id = 7, Name = "Cox"},
					new Platform() {Id = 8, Name = "Comcast"},
					new Platform() {Id = 9, Name = "Time Warner Cable"},
					new Platform() {Id = 10, Name = "Brighthouse"},
					new Platform() {Id = 11, Name = "Navic"},
					new Platform() {Id = 12, Name = "Xbox LIVE"},
					new Platform() {Id = 13, Name = "PlayStation"},
					new Platform() {Id = 14, Name = "iOS"},
					new Platform() {Id = 15, Name = "Android"},
					new Platform() {Id = 16, Name = "Samsung"},
					new Platform() {Id = 17, Name = "LG"},
					new Platform() {Id = 18, Name = "Sony"},
					new Platform() {Id = 19, Name = "On Demand Network"},
					new Platform() {Id = 20, Name = "Rovi"},
					new Platform() {Id = 21, Name = "Rovi Connected"},
					new Platform() {Id = 22, Name = "TiVo"},
					new Platform() {Id = 23, Name = "Product Watch"},
					new Platform() {Id = 25, Name = "Roku"}
				};
			return recs;
		}

		public static List<Platform> CreatePlatforms()
		{
			var platforms = new List<Platform>();

			platforms.Add(new Platform { Id = 1, Name = PlatformConstants.PlatformNames.Roku });

			return platforms;
		}

	}

}