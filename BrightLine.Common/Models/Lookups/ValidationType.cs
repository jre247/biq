using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightLine.Common.Core.Attributes;
using BrightLine.Core;
using System.Runtime.Serialization;
using System;
using BrightLine.Common.Models.Lookups;

namespace BrightLine.Common.Models
{
	public class ValidationType : EntityBase, IEntity
	{
		public string Name { get; set; }
		public string SystemType { get; set; }
	}
}
