using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using System;
using System.Runtime.Serialization;
using BrightLine.Common.ViewModels.Models;
using Newtonsoft.Json.Linq;
using BrightLine.Common.Framework;
using BrightLine.Common.ViewModels.Resources;
using BrightLine.Common.ViewModels.Ads;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class DestinationCreativeSaveViewModel
	{
		[DataMember]
		public int id { get; set; }

		[DataMember]
		public string name { get; set; }

		[DataMember]
		public string description { get; set; }

		[DataMember]
		public ResourceViewModel resource { get; set; }

		[DataMember]
		public int? inactivityThreshold { get; set; }

		[DataMember]
		public int campaign { get; set; }

		[DataMember]
		public int? Thumbnail_Id { get; set; }

		[DataMember]
		public string ResourceIds { get; set; }

		[DataMember]
		public int adFunction { get;set;}

		[DataMember]
		public int adType { get;set;}

		[DataMember]
		public IEnumerable<FeatureViewModel> features { get; set; }

		[DataMember]
		public IEnumerable<AdEditRepoNameViewModel> ads { get; set; }

		public class FeatureViewModel
		{
			public int id { get; set; }
			public int blueprint { get; set; }
			public int campaign { get; set; }
			public int featureType { get; set; }
			public string name { get; set; }
			public IEnumerable<PageViewModel> pages { get; set; }

			public class PageViewModel
			{
				public int id { get; set; }
				public string name { get; set; }
				public int pageDefinition { get; set; }
			}
		}	
	}
}
