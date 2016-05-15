using BrightLine.Common.Models;
using BrightLine.Common.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<Advertiser> GetAdvertisers()
		{
			var advertisers = new List<Advertiser>
			{
				new Advertiser
				{
					Id = 1,
					Name = AdvertiserConstants.AdvertiserNames.GlaxoSmithKlein
				},
				new Advertiser
				{
					Id = 2,
					Name = AdvertiserConstants.AdvertiserNames.AmericanExpress
				},
				new Advertiser
				{
					Id = 3,
					Name = AdvertiserConstants.AdvertiserNames.Unilever
				}
			};

			return advertisers;
		}
	}
}
