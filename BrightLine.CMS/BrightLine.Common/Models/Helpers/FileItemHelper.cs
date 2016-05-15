using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrightLine.Common.Utility;
using BrightLine.Common.Models;
using BrightLine.Core;
using BrightLine.Utility;


namespace BrightLine.Common.Models.Helpers
{
	public class FileItemHelper
	{

		/// <summary>
		/// Validate the file item ( no empty content )
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static BoolMessage Validate(FileItem item)
		{
			if(item.Contents == null || item.Contents.Length == 0)
				return new BoolMessage(false, "No file contents");

			return new BoolMessage(true, string.Empty);
		}


		/// <summary>
		/// Converts the name and fills the extension.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static void FillProperties(FileItem item)
		{
			var ndxLastDot = item.FullNameRaw.LastIndexOf(".");
			item.Name = item.FullNameRaw.Substring(0, ndxLastDot);
			item.Extension = item.FullNameRaw.Substring(ndxLastDot + 1);
			item.Name = TransformName(item.Name);
			item.Length = item.Length / 1024; // Kilobytes.
		}


		/// <summary>
		/// Convert the name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string TransformName(string name)
		{
			var validName = "";
			for (var ndx = 0; ndx < name.Length; ndx++)
			{
				var ch = name[ndx];
				if (Char.IsLetterOrDigit(ch) || ch == '-')
				{
					validName += ch;
				}
			}
			return validName;
		}
	}
}