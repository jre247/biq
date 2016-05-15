using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Models
{
	public class RateCard : EntityBase, IEntity
	{
		public Contract Contract { get; set; }
		public RateType RateType { get; set; }
		public int? MinImpressionCount { get; set; }
		public int? MaxImpressionCount { get; set; }
		public float? Rate { get; set; }
	}
}
