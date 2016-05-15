using System.ComponentModel.DataAnnotations;
using BrightLine.Core;

namespace BrightLine.Common.Models
{
	public class CampaignContentModelInstance : EntityBase, IEntity
	{
		public string Key { get; set; }
        public string ModelName { get; set; }
		public string InstanceData { get; set; }
		public string InstanceDataJson { get; set; }

		[Required]
        public virtual CampaignContentSchema Schema { get; set; }


        public virtual CampaignContentModel Model { get; set; }


        public virtual CampaignContentModelBaseType Format { get; set; }


        public virtual CampaignContentModelType Type { get; set; }


        public virtual CampaignContentModelInstance Parent { get; set; }


        public bool IsPublishable { get; set; }
	}
}
