using BrightLine.Core;
using BrightLine.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models.Lookups
{
	public class TrackingEvent : EntityBase, ILookup, IEntity
	{
		public string Name { get; set; }
	}
}
