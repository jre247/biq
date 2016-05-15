using BrightLine.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility
{
	public static class AdConstants
	{
		public static class Coordinates
		{
			public static class Platforms
			{
				public static class Roku
				{
					public static class Hd
					{
						public const int xMin = 0;
						public const int xMax = 1280;
						public const int yMin = 0;
						public const int yMax = 720;
					}

					public static class Sd
					{
						public const int xMin = 0;
						public const int xMax = 720;
						public const int yMin = 0;
						public const int yMax = 480;
					}
				}

				public static class NonRoku
				{
					public static class Hd
					{
						public const int xMin = 0;
						public const int xMax = 1920;
						public const int yMin = 0;
						public const int yMax = 1080;
					}
				}
			}				
		}
		
	}
}
