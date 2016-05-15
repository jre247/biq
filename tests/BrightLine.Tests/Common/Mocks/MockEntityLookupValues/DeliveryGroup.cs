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
		internal static List<DeliveryGroup> CreateDeliveryGroups()
		{
			var deliveryGroups = new List<DeliveryGroup>();

			deliveryGroups.Add(new DeliveryGroup
			{
				Id = 1,
				ImpressionGoal = 10,
				Campaign = new Campaign { Id = 1 },
				MediaPartner = new MediaPartner { Id = 1 },
				Ads = new List<Ad>(),
				Name = "Delivery Group 1"
				//Ads = new List<Ad>{new Ad{BeginDate = new DateTime(2015, 10, 2), EndDate = new DateTime}, new Ad{BeginDate = new DateTime(2015, 10, 2)} }			
			});

			deliveryGroups.Add(new DeliveryGroup
			{
				Id = 2,
				ImpressionGoal = 20,
				Campaign = new Campaign { Id = 1 },
				MediaPartner = new MediaPartner { Id = 1 },
				Ads = new List<Ad>(),
				Name = "Delivery Group 2"
			});

			deliveryGroups.Add(new DeliveryGroup
			{
				Id = 3,
				ImpressionGoal = 30,
				Campaign = new Campaign { Id = 2 },
				MediaPartner = new MediaPartner { Id = 1 },
				Ads = new List<Ad>(),
				Name = "Delivery Group 3"
			});

			return deliveryGroups;
		}

	}

}