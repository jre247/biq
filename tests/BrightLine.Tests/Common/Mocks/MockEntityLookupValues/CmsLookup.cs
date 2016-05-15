using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.Utility.ResourceType;
using BrightLine.Common.ViewModels;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Tests.Component.CMS.Validator_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BrightLine.Tests.Common.Mocks
{
	public partial class MockEntities
	{
		public static List<Expose> CreateExposes()
		{
			var exposes = new List<Expose>();
			exposes.Add(new Expose { Id = 1, Name = "Server" });
			exposes.Add(new Expose { Id = 2, Name = "Client" });
			exposes.Add(new Expose { Id = 3, Name = "Both" });

			return exposes;

		}

		public static List<CmsRefType> CreateCmsRefTypes()
		{
			var cmsRefTypes = new List<CmsRefType>();
			cmsRefTypes.Add(new CmsRefType { Id = 1, Name = "known" });
			cmsRefTypes.Add(new CmsRefType { Id = 2, Name = "unknown" });

			return cmsRefTypes;
		}

		public static List<StorageSource> CreateStorageSources()
		{
			var recs = new List<StorageSource>()
				{
					new StorageSource{
						Id = 1,
						Name = "Media"
						
					},
					new StorageSource{
						Id = 2,
						Name = "External"
						
					}
				};

			return recs;
		}

		public static List<FileType> CreateFileTypes()
		{
			var fileTypes = new List<FileType>();

			fileTypes.Add(new FileType { Id = 1, Name = "jpeg" });
			fileTypes.Add(new FileType { Id = 2, Name = "png" });
			fileTypes.Add(new FileType { Id = 3, Name = "mp4" });
			fileTypes.Add(new FileType { Id = 4, Name = "jpg" });
			fileTypes.Add(new FileType { Id = 5, Name = "gif" });

			return fileTypes;
		}

		public static List<FieldType> CreateFieldTypes()
		{
			var fieldTypes = new List<FieldType>();

			fieldTypes.Add(new FieldType { Id = 1, Name = FieldTypeConstants.FieldTypeNames.Image });
			fieldTypes.Add(new FieldType { Id = 2, Name = "refToModel" });
			fieldTypes.Add(new FieldType { Id = 3, Name = FieldTypeConstants.FieldTypeNames.String });
			fieldTypes.Add(new FieldType { Id = 4, Name = FieldTypeConstants.FieldTypeNames.Integer });
			fieldTypes.Add(new FieldType { Id = 5, Name = "float" });
			fieldTypes.Add(new FieldType { Id = 6, Name = FieldTypeConstants.FieldTypeNames.Bool });
			fieldTypes.Add(new FieldType { Id = 7, Name = FieldTypeConstants.FieldTypeNames.Datetime });
			fieldTypes.Add(new FieldType { Id = 8, Name = "video" });
			fieldTypes.Add(new FieldType { Id = 9, Name = "refToPage" });
			fieldTypes.Add(new FieldType { Id = 10, Name = "html" });

			return fieldTypes;
		}

		public static List<ResourceType> CreateResourceTypes()
		{
			var resourceTypes = new List<ResourceType>();

			resourceTypes.Add(new ResourceType { Id = 1, Name = ResourceTypeConstants.ResourceTypeNames.SdImage });
			resourceTypes.Add(new ResourceType { Id = 2, Name = ResourceTypeConstants.ResourceTypeNames.HdImage });
			resourceTypes.Add(new ResourceType { Id = 3, Name = ResourceTypeConstants.ResourceTypeNames.SdVideo });
			resourceTypes.Add(new ResourceType { Id = 4, Name = ResourceTypeConstants.ResourceTypeNames.HdVideo });

			return resourceTypes;
		}


		internal static FileType GetFileType(string resourceTypeName = ResourceTypeConstants.ResourceTypeNames.SdImage)
		{
			return new FileType { ResourceType = new ResourceType { Id = Lookups.ResourceTypes.HashByName[resourceTypeName] } };
		}

	}

}