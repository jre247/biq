using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class CmsPublish : EntityBase, IEntity
	{
		[Required]
		public Guid PublishId { get; set; }

		[Required]
		public virtual Campaign Campaign { get; set; }

		[Required]
		public string TargetEnvironment { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public string IPAddress { get; set; }

		[Required]
		public DateTime TimeStarted { get; set; }

		public DateTime? TimeEnded { get; set; }

		[ForeignKey("CmsPublishStatus_Id")]
		public virtual CmsPublishStatus CmsPublishStatus  { get; set; }
		public int CmsPublishStatus_Id { get; set; }

	}
}
