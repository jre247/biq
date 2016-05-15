﻿using BrightLine.CMS.Service;
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
		public static List<SubSegment> GetSubSegments()
		{
			var subsegments = new List<SubSegment>();
			subsegments.Add(new SubSegment { Id = 1, Name = "Consumer Packaged Goods" });
			subsegments.Add(new SubSegment { Id = 2, Name = "Food & Beverage" });

			return subsegments;
		}

		public static List<Segment> GetSegments()
		{
			var segments = new List<Segment>();
			segments.Add(new Segment { Id = 1, Name = "Consumer Packaged Goods" });
			segments.Add(new Segment { Id = 2, Name = "Food & Beverage" });

			return segments;
		}

	}

}