using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class Contract : EntityBase, IEntity
	{
		public MediaPartner MediaPartner { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsCurrent { get; set; }
	}
}
