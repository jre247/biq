using BrightLine.Core;
using BrightLine.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class Agency : EntityBase, IEntity, ILookup
	{
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		public int? Parent_Id { get; set; }

		[DataMember]
		[ForeignKey("Parent_Id")]
		public virtual Agency Parent { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
