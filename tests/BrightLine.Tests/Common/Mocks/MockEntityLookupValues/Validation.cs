using BrightLine.CMS.Service;
using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Authentication;
using BrightLine.Common.Utility.ValidationType;
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
		public static List<ValidationType> CreateValidationTypes()
		{
			var validationTypes = new List<ValidationType>();
			validationTypes.Add(new ValidationType { Id = 1, Name = ValidationTypeConstants.ValidationTypeNames.Required, SystemType = "bool" });
			validationTypes.Add(new ValidationType { Id = 2, Name = ValidationTypeConstants.ValidationTypeNames.Unique, SystemType = "string" });
			validationTypes.Add(new ValidationType { Id = 3, Name = ValidationTypeConstants.ValidationTypeNames.Width, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 4, Name = ValidationTypeConstants.ValidationTypeNames.MaxWidth, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 5, Name = ValidationTypeConstants.ValidationTypeNames.MinWidth, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 6, Name = ValidationTypeConstants.ValidationTypeNames.Height, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 7, Name = ValidationTypeConstants.ValidationTypeNames.MaxHeight, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 8, Name = ValidationTypeConstants.ValidationTypeNames.MinHeight, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 9, Name = ValidationTypeConstants.ValidationTypeNames.Length, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 10, Name = ValidationTypeConstants.ValidationTypeNames.MaxLength, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 11, Name = ValidationTypeConstants.ValidationTypeNames.MinLength, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 12, Name = ValidationTypeConstants.ValidationTypeNames.MinDatetime, SystemType = "datetime" });
			validationTypes.Add(new ValidationType { Id = 13, Name = ValidationTypeConstants.ValidationTypeNames.MaxDatetime, SystemType = "datetime" });
			validationTypes.Add(new ValidationType { Id = 14, Name = ValidationTypeConstants.ValidationTypeNames.MaxImageSize, SystemType = "image" });
			validationTypes.Add(new ValidationType { Id = 15, Name = ValidationTypeConstants.ValidationTypeNames.MaxVideoSize, SystemType = "video" });
			validationTypes.Add(new ValidationType { Id = 16, Name = ValidationTypeConstants.ValidationTypeNames.MaxVideoDuration, SystemType = "video" });
			validationTypes.Add(new ValidationType { Id = 17, Name = ValidationTypeConstants.ValidationTypeNames.Extension, SystemType = "image/video" });
			validationTypes.Add(new ValidationType { Id = 18, Name = ValidationTypeConstants.ValidationTypeNames.MinFloat, SystemType = "float" });
			validationTypes.Add(new ValidationType { Id = 19, Name = ValidationTypeConstants.ValidationTypeNames.MaxFloat, SystemType = "float" });

			return validationTypes;

		}
		
		public static List<FileTypeValidation> CreateFileTypeValidations()
		{
			var recs = new List<FileTypeValidation>()
				{
					CreateFileTypeValidation(1, 1, 1),
					CreateFileTypeValidation(2, 2, 1),
					CreateFileTypeValidation(3, 3, 2),
					CreateFileTypeValidation(4, 4, 2),
					
				};

			return recs;
		}

		public static List<Validation> CreateValidations()
		{
			var recs = new List<Validation>()
				{
					new Validation{
						Id = 1,
						CmsField = new CmsField{Id = 12300}
					}
					
				};

			return recs;

		}
		
		public static Validation GetValidationInstanceForRequired(string value)
		{
			return new Validation
			{
				Id = 1,
				Value = value,
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Required],
					SystemType = ValidationTypeSystemTypeConstants.BOOL,
					Name = ValidationTypeNameConstants.REQUIRED
				}
			};
		}

		public static Validation GetValidationInstanceForExtension()
		{
			var validationTypeExtensionId = (int)Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Extension];

			return new Validation
			{
				Id = 1,
				ValidationType = new ValidationType
				{
					Id = validationTypeExtensionId,
					SystemType = "image/video",
					Name = Lookups.ValidationTypes.HashById[validationTypeExtensionId]
				}
			};
		}

		public static FileTypeValidation CreateFileTypeValidation(int id = 1, int fileTypeId = 3, int validationId = 1)
		{
			return new FileTypeValidation{Id = id, FileType = new FileType{Id = fileTypeId}, Validation = new Validation{Id = validationId, CmsField = new CmsField{Id = 1000}}};
		}

		public static Validation GetValidationInstanceForMaxLength(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxLength], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MAX_LENGTH);
		}

		public static Validation GetValidationInstanceForMinLength(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinLength], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MIN_LENGTH);
		}

		public static Validation GetValidationInstanceForLength(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Length], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.LENGTH);
		}

		public static Validation GetValidationInstanceForMinWidth(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinWidth], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MIN_WIDTH);
		}

		public static Validation GetValidationInstanceForMaxDuration(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxVideoDuration], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MAX_VIDEO_DURATION);
		}

		public static Validation GetValidationInstanceForMaxSize(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxVideoSize], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MAX_VIDEO_SIZE);
		}

		public static Validation GetValidationInstanceForMaxWidth(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxWidth], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MAX_WIDTH);
		}

		public static Validation GetValidationInstanceForMinHeight(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MinHeight], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MIN_HEIGHT);
		}

		public static Validation GetValidationInstanceForMaxHeight(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxHeight], ValidationTypeSystemTypeConstants.FLOAT, ValidationTypeNameConstants.MAX_HEIGHT);
		}

		public static Validation GetValidationInstanceForMaxImageSize(string value)
		{
			return MockEntities.GetValidationForType(1, value, Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.MaxImageSize], ValidationTypeSystemTypeConstants.IMAGE, ValidationTypeNameConstants.MAX_SIZE);
		}

		public static Validation GetValidationForType(int id, string value, int vallidationTypeId, string systemName, string validationTypeName)
		{
			return new Validation
			{
				Id = id,
				Value = value,
				ValidationType = new ValidationType
				{
					Id = vallidationTypeId,
					SystemType = systemName,
					Name = validationTypeName
				}
			};
		}

		public static Validation GetValidationInstanceForUnique(string value)
		{
			return new Validation
			{
				Id = 1,
				Value = value,
				ValidationType = new ValidationType
				{
					Id = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Unique],
					SystemType = ValidationTypeSystemTypeConstants.BOOL,
					Name = ValidationTypeNameConstants.UNIQUE
				}
			};
		}
	}

}