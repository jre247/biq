using BrightLine.Common.Models.Lookups;
using BrightLine.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BrightLine.Common.Models
{
	public class ResourceType : EntityBase, ILookup, IEntity
	{
		/// <summary>
		/// Name of the resource.
		/// </summary>
		[DataMember]
		[StringLength(255)]
		public string Name { get; set; }

		public string ManifestName { get; set; }
	}
}