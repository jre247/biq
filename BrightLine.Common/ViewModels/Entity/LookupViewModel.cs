using BrightLine.Common.Models;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Entity
{
	public class LookupViewModel
	{
		//json deserializer in unit tests needs to be able to deserialize with parameterless constructor
		private LookupViewModel()
		{ }

		public int id;
		public string name;

		internal static LookupViewModel Parse(ILookup lookup)
		{
			if (lookup == null)
				return null;

			var lu = new LookupViewModel()
			{
				id = lookup.Id,
				name = lookup.Name
			};

			return lu;
		}
	}
}
