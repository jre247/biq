using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class RateType : EntityBase, IEntity
	{
		[StringLength(255)]
		public string Name { get; set; }
	}
}
