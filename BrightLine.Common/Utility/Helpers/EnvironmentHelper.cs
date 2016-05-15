using BrightLine.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public class EnvironmentHelper : IEnvironmentHelper
	{

		public bool IsLocal
		{
			get
			{
				return Env.IsLocal;
			}
			
		}
	}
}
