using BrightLine.Common.Framework;
using BrightLine.Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BrightLine.Common.ViewModels.Developer
{
	public class NightwatchTestsViewModel
	{
		public Guid TransactionId { get; set; }

		public string BuildVersion { get; set; }

		public string BuildCommitHash { get; set; }
	}
}
