using System;
using System.IO;
using System.Reflection;

namespace BrightLine.Tests.Common
{
	/// <summary>
	/// Loads embedded resource.
	/// </summary>
	public class ResourceLoader
	{
		private const string TestDllName = "BrightLine.Tests";

		/// <summary>
		/// e.g. Csv\Csv1.csv.
		/// Where Csv is the folder under the Content folder.
		/// </summary>
		/// <param name="filePath">\Csv\Csv1.csv</param>
		/// <returns></returns>
		public static string ReadText(string path)
		{
			string text;
			// NOTE: Folder names are separated by a ".".
			var fullPath = string.Format(TestDllName + ".Resources.{0}", path);
			var current = Assembly.GetExecutingAssembly();
			using (var resourceStream = current.GetManifestResourceStream(fullPath))
			using (var textStream = new StreamReader(resourceStream))
			{
				text = textStream.ReadToEnd();
			}

			return text;
		}

		/// <summary>
		/// e.g. Csv\Csv1.csv.
		/// Where Csv is the folder under the Content folder.
		/// </summary>
		/// <param name="path">\Csv\Csv1.csv</param>
		/// <returns></returns>
		public static byte[] ReadBytes(string path)
		{
			// NOTE: Folder names are separated by a ".".
			var fullPath = string.Format(TestDllName + ".Resources.{0}", path);
			var assembly = Assembly.GetExecutingAssembly();
			byte[] bytes;
			using (var resourceStream = assembly.GetManifestResourceStream(fullPath))
			{
				if (resourceStream == null)
					throw new FileNotFoundException(fullPath);

				var length = (int)resourceStream.Length;
				bytes = new byte[length];
				resourceStream.Read(bytes, 0, length);
			}

			return bytes;
		}

		public static Stream BuildStream(string path)
		{
			// NOTE: Folder names are separated by a ".".
			var fullPath = string.Format(TestDllName + ".Resources.{0}", path);
			var current = Assembly.GetExecutingAssembly();
			var resourceStream = current.GetManifestResourceStream(fullPath);

			return resourceStream;
		}

		public static string GetResourceFileLocation(string path)
		{
			var basePath = AppDomain.CurrentDomain.BaseDirectory;
			basePath = basePath.Replace("bin\\Local\\", "");
			basePath = basePath.Replace(@"bin\", "");

			return string.Format("{0}\\{1}", basePath, path);
		}
	}
}
