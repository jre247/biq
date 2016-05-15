using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightLine.Core;

namespace BrightLine.Common.Core.Attributes
{
	public class NotApplicableAttribute : ValidationAttribute
	{
		public bool IsRequired { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
				return !IsRequired;

			var v = value.ToString();
			var id = 0;
			if (value is int)
				id = int.Parse(v);
			else
			{
				var lookup = value as ILookup;
				if (lookup == null)
					return false;

				id = lookup.Id;
			}

			if (EntityLookup.NotApplicable.Id == id)
				return true;
			if (id < 0)
				return false;

			return true;
		}
	}
}
