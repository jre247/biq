using System;
using System.Collections.Generic;

namespace BrightLine.Core
{
	public interface IEntity
	{
		int Id { get; set; }
		string Display { get; set; }
		bool IsDeleted { get; set; }
		DateTime DateCreated { get; set; }
		DateTime? DateUpdated { get; set; }
		DateTime? DateDeleted { get; set; }
		bool IsNewEntity { get; }
        bool IsValid();
	}
}
