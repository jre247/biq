using BrightLine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<MediaPartner> CreateMediaPartners()
		{
			var mediaPartners = new List<MediaPartner>();

			mediaPartners.Add(new MediaPartner{Id = 1, Name = "ABC", Category = new Category{Id = 1}});
			mediaPartners.Add(new MediaPartner { Id = 2, Name = "ABC Family", Category = new Category { Id = 1 } });

			return mediaPartners;
		}
	}
}
