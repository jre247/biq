using System;

namespace BrightLine.Common.Framework.Exceptions
{
	public class ImageSizeException : Exception
	{
		public ImageSizeException(int maxSize, string message)
			: base(message)
		{
			MaxSize = maxSize;
		}

		public int MaxSize { get; set; }
	}
}
