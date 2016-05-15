using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Resources
{
	public class ImageResizeOptions
	{
		public int Height { get; set; }
		public int Width { get; set; }
		public bool IgnoreAspectRatio { get; set; }
	}
}
