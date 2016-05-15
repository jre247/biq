using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BrightLine.Common.Models
{
	public class Placement : EntityBase, IEntity
	{
		[StringLength(255)]
		public virtual string Name { get; set; }

		public int? App_Id { get; set; }

		[ForeignKey("App_Id")]
		public virtual App App { get; set; }

		public virtual MediaPartner MediaPartner { get; set; }

		public virtual AdTypeGroup AdTypeGroup { get; set; }
		public virtual int? Height { get; set; }
		public virtual int? Width { get; set; }
		public virtual string LocationDetails { get; set; }

		[DataMember]
		public virtual Category Category { get; set; }

		public virtual ICollection<Ad> Ads { get; set; }



	
	}
}