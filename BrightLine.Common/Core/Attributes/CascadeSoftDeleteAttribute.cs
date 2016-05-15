using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Core.Attributes
{
	/// <summary>
	/// Specifies whether to soft delete a property. Should never be used on parent references, it will cause stack overflows.
	/// </summary>
	/// <remarks>This functionality is entirely opt-in and per property.</remarks>
	[AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class CascadeSoftDeleteAttribute : Attribute
	{
		//NOTE: this class intentionally left empty
	}

	public enum DeleteTypes
	{
		Soft,
		Hard,
		SoftCascade
	}
}
