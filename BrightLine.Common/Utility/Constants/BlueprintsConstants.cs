using BrightLine.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.Utility.Blueprints
{
	public static class BlueprintConstants
	{
		public const string BLUEPRINT_SAVED = "Blueprint saved";
		public const string PREVIEW_FILE = "PreviewFile";
		public const string CONNECTED_TV_FILE = "ConnectedTVCreativeFile";
		public const string CONNECTED_TV_SUPPORT_FILE = "ConnectedTVSupportFile";
		public const string PAGE_TYPE_PAGE = "page";
		
	

		public static class Errors
		{
			public const string PREVIEW_IMAGE_REQUIRED = "Preview Image is required.";
			public const string BLUEPRINT_DOES_NOT_EXIST = "Blueprint does not exist.";
			public const string REPOSITORY_DOES_NOT_EXIST = "Repository does not exist.";
			public const string UNEXPECTED_ERROR = "There was an unexpected error that occured during the Blueprint import process.";
		}

		public static class SqlProcedures
		{
			public const string CascadeDeleteBlueprint =  "[dbo].[CascadeDeleteBlueprint]";

			public static class Params
			{
				public const string BlueprintId = "@BlueprintId";
			}
		}
	}
}
