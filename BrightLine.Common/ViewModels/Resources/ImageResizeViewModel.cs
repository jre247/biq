using BrightLine.Common.Framework;
using BrightLine.Common.Services;
using BrightLine.Common.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Resources
{
	public class ImageResizeViewModel
	{
		#region Members

		public byte[] Contents { get; set; }
		public string Filename { get; set; }

		#endregion

		#region Init

		public ImageResizeViewModel(byte[] contents, string fileRawPath, int width, int height, bool ignoreAspectRatio = true)
		{
			var resourceHelper = IoC.Resolve<IResourceHelper>();

			var resizeOptions = new ImageResizeOptions
			{
				IgnoreAspectRatio = ignoreAspectRatio,
				Width = width,
				Height = height
			};

			var fileSplit = fileRawPath.Split('.');
			var extension = fileSplit[fileSplit.Count() - 1];
			var baseFilePath = fileSplit.Take(fileSplit.Length - 1).ToArray()[0];

			// Resize image
			Contents = resourceHelper.ResizeImage(contents, resizeOptions);

			// Format filename
			Filename = string.Format("{0}_{1}.{2}", baseFilePath, width, extension);
		}

		#endregion
	}
}
