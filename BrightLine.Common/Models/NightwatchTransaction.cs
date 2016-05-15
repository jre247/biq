using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class NightwatchTransaction : EntityBase, IEntity
	{
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public Guid TransactionId { get; set; }

		[DataMember]
		public string Status { get; set; }

		[DataMember]
		public DateTime? DateCompleted { get; set; }

		[DataMember]
		public string BuildVersion { get; set; }

		[DataMember]
		public string BuildCommitHash { get; set; }
	}
}
