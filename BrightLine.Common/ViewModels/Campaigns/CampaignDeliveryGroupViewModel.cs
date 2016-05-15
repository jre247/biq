using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Campaigns
{
	[DataContract]
	public class CampaignDeliveryGroupViewModel
	{
		[DataMember]
		public int id { get; set; }
		[DataMember]
		public double? mediaSpend { get; set; }
		[DataMember]
		public string name { get; set; }
		[DataMember]
		public int? mediaPartnerId { get; set; }
		[DataMember]
		public int? impressionGoal { get; set; }
		[DataMember]
		public string beginDate { get
			{
				if (!beginDateRaw.HasValue)
					return null;

				return beginDateRaw.Value.ToString(DateHelper.DATE_FORMAT);
			} 
		}
		[DataMember]
		public string endDate
		{
			get
			{
				if (!endDateRaw.HasValue)
					return null;

				return endDateRaw.Value.ToString(DateHelper.DATE_FORMAT);
			}
		}

		public DateTime? beginDateRaw { get; set; }
		public DateTime? endDateRaw { get; set; }

		[DataMember]
		public string Status
		{
			get
			{
				return CampaignHelper.GetCampaignStatus(beginDateRaw, endDateRaw);
			}
		}

		
		public static JObject ToJObject(Dictionary<int, CampaignDeliveryGroupViewModel> deliveryGroups)
		{
			if (deliveryGroups == null)
				return null;

			return JObject.FromObject(deliveryGroups);
		}

	
	}
}
