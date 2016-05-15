using BrightLine.Common.Framework;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Service
{
	public class FieldTypeService : CrudService<FieldType>, IFieldTypeService
	{
		public FieldTypeService(IRepository<FieldType> repo)
			: base(repo)
		{

		}

		public int GetFieldTypeId(string fieldTypeName)
		{
			int fieldType = 0;

			switch (fieldTypeName)
			{
				case FieldTypeConstants.FieldTypeNames.Bool:
					var fieldTypeBoolId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool];
					fieldType = Get(fieldTypeBoolId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Datetime:
					var fieldTypeDateId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime];
					fieldType = Get(fieldTypeDateId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Float:
					var fieldTypeFloatId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Float];
					fieldType = Get(fieldTypeFloatId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Html:
					var fieldTypeHtmlId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Html];
					fieldType = Get(fieldTypeHtmlId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Image:
					var fieldTypeImageId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Image];
					fieldType = Get(fieldTypeImageId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Integer:
					var fieldTypeIntegerId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer];
					fieldType = Get(fieldTypeIntegerId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.RefToModel:
					var fieldTypeModelRefId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToModel];
					fieldType = Get(fieldTypeModelRefId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.RefToPage:
					var fieldTypePageRefId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage];
					fieldType = Get(fieldTypePageRefId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.String:
					var fieldTypeStringId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String];
					fieldType = Get(fieldTypeStringId).Id;
					break;
				case FieldTypeConstants.FieldTypeNames.Video:
					var fieldTypeVideoId = Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Video];
					fieldType = Get(fieldTypeVideoId).Id;
					break;
			}
			return fieldType;
		}
	}
}
