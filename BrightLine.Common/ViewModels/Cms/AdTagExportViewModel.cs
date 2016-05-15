using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Cms
{
	public class AdTagExportViewModel
	{
		public int AdId { get; set; }
		public int? AdTagId { get; set; }
		public string PlatformName { get; set; }
		public string MediaPartnerName { get; set; }
		public string AdName { get; set; }
		public string CreativeName { get; set; }
		public string AdTypeName { get; set; }
		public string AdFormatName { get; set; }
		public string AdFunctionName { get; set; }
		public DateTime? BeginDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int? AdTypeId { get; set; }
		public string DeliveryGroupName { get; set; }
		public int PlacementId { get; set; }
		public string PlacementName { get; set; }
		public string AdTag { get; set; }
		public string ImpressionUrl { get; set; }
		public string ClickUrl { get; set; }

		public int? PlatformId { get; set; }

	}
}
