using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	[DataContract]
	public class FileTypeValidation : EntityBase, IEntity
	{
		public int? FileType_Id { get; set; }

		[ForeignKey("FileType_Id")]
		public virtual FileType FileType { get; set; }

		[DataMember]
		public virtual Validation Validation { get; set; }
	}
}
