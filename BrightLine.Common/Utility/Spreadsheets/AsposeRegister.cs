using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Spreadsheets
{
	public static class AsposeRegistration
	{
		public static void RegisterLicense()
		{
			var license = new Aspose.Cells.License();

			// Pass only the name of the license file embedded in the assembly
			license.SetLicense("Aspose.Cells.lic");
		}
	}
}
