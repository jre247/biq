using BrightLine.Common.Models.Enums;
using BrightLine.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	[DataContract(IsReference = false)]
	public class BlueprintPlatform : EntityBase, IEntity
	{
		[DataMember]
		[Required]
		public virtual Blueprint Blueprint { get; set; }

		[DataMember]
		[Required]
		public virtual Platform Platform { get; set; }

		[DataMember]
		[Required]
		[JsonConverter(typeof(StringEnumConverter))]
		public virtual CompletionStatus Status { get; set; }

		[DataMember]
		public virtual Resource Creative { get; set; }
	}
}
