using BrightLine.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class LogEntry : EntityBase, IEntity
	{
		[DataMember]
		public string Logger { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string Level { get; set; }

		[DataMember]
		[StringLength(255)]
		public string User { get; set; }

		[DataMember]
		[StringLength(255)]
		public string Host { get; set; }
	}
}
