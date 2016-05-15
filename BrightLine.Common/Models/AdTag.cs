using BrightLine.Core;
using System.Collections.Generic;

namespace BrightLine.Common.Models
{
	public class AdTag : EntityBase, IEntity
	{
		public virtual ICollection<Ad> Ads { get; set; }
	}
}
