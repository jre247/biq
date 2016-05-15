using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Core;


namespace BrightLine.Common.Models
{
	public class CampaignContentSchema : EntityBase, IEntity
	{
		#region Properties
		
		[StringLength(50)]
		public string Tag { get; set; }
		public string DataSchema { get; set; }
		public string DataModelsRaw { get; set; }
		public string DataModelsPublished { get; set; }
		public bool IsPublished { get ;set; }
		
		public string LastPublishEnvironment { get; set; }
		public string ChangeComment { get; set; }
		public DateTime? LastPublishedDate { get; set; }
		/// <summary>
		///  Comma separated list of model types that were published last
		/// </summary>
		public string LastPublishedModelTypeList { get; set; }

		public int TotalModels { get; set; }
		public int TotalLookups { get; set; }

        public virtual ICollection<CampaignContentModelInstance> AppModelInstanceItems { get; set; }


        public virtual ICollection<CampaignContentModel> Models { get; set; }

        public virtual Campaign Campaign { get; set; }

	    #endregion
	}
}
