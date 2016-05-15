using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Helpers
{
	public static class BlueprintImportHelper
	{
		/// <summary>
		/// Convert a string to have its first letter capitalized and the rest of the string lowercase
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string ConvertStringToFirstLetterUppercase(string val)
		{
			var firstLetter = val[0];
			var valFormatted = string.Format("{0}{1}", firstLetter.ToString().ToUpper(), val.Substring(1, val.Length - 1));

			return valFormatted;
		}
	}
}
