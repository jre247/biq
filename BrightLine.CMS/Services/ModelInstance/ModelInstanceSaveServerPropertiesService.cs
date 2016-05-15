using BrightLine.CMS.Service;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.FieldType;
using BrightLine.Common.ViewModels.Models;
using BrightLine.Core;
using BrightLine.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.CMS.Services
{
	public class ModelInstanceSaveServerPropertiesService : IModelInstanceSaveServerPropertiesService
	{
		public BoolMessageItem SaveField(CmsField cmsField, CmsModelInstance modelInstance, List<string> fieldValues)
		{
			CmsModelInstanceField cmsModelInstanceField = null;
			IEnumerable<CmsModelInstanceField> cmsModelInstanceFields = null;

			if (modelInstance.Fields != null)
			{
				//cmsModelInstanceField Name maps to CmsField Name, and field Name must be unique among an instance
				cmsModelInstanceFields = modelInstance.Fields.Where(f => f.ModelInstance.Id == modelInstance.Id && f.Name.ToLower() == cmsField.Name.ToLower());
			}

			if (cmsModelInstanceFields != null && cmsModelInstanceFields.Count() > 1)
				return new BoolMessageItem(false, "There exists multiple Model Instance Fields for the name: " + cmsField.Name);

			if (cmsModelInstanceFields != null)
				cmsModelInstanceField = cmsModelInstanceFields.SingleOrDefault();

			//insert
			if (cmsModelInstanceField == null)
			{
				cmsModelInstanceField = new CmsModelInstanceField
				{
					Name = cmsField.Name,
					ModelInstance = modelInstance,
					FieldType_Id = cmsField.Type.Id
				};

				modelInstance.Fields.Add(cmsModelInstanceField);
			}

			foreach (var fieldValue in fieldValues)
				SaveFieldValue(cmsField, cmsModelInstanceField, fieldValue);

			return new BoolMessageItem(true, null);
		}


		private void SaveFieldValue(CmsField cmsField, CmsModelInstanceField cmsModelInstanceField, string fieldValue)
		{
			CmsModelInstanceFieldValue cmsModelInstanceFieldValue = null;

			//*note: only one cmsModelInstanceFieldValue value record corresponds to one cmsModelInstanceField record
			cmsModelInstanceFieldValue = cmsModelInstanceField.Values.SingleOrDefault(f => f.CmsModelInstanceField.Id == cmsModelInstanceField.Id);

			//insert
			if (cmsModelInstanceFieldValue == null)
			{
				cmsModelInstanceFieldValue = new CmsModelInstanceFieldValue
				{
					CmsModelInstanceField = cmsModelInstanceField
				};

				SaveFieldValueForType(cmsField, fieldValue, cmsModelInstanceFieldValue);

				cmsModelInstanceField.Values.Add(cmsModelInstanceFieldValue);
			}
			//update
			else
				SaveFieldValueForType(cmsField, fieldValue, cmsModelInstanceFieldValue);
		}

		private void SaveFieldValueForType(CmsField cmsField, string fieldValue, CmsModelInstanceFieldValue cmsModelInstanceFieldValue)
		{
			var fieldTypeId = cmsField.Type.Id;

			if (fieldTypeId == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.String])
			{
				cmsModelInstanceFieldValue.StringValue = fieldValue;
			}
			else if (fieldTypeId == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Integer])
			{
				cmsModelInstanceFieldValue.NumberValue = double.Parse(fieldValue);
			}
			else if (fieldTypeId == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Bool])
			{
				cmsModelInstanceFieldValue.BoolValue = bool.Parse(fieldValue);
			}
			else if (fieldTypeId == (int)Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.Datetime])
			{
				cmsModelInstanceFieldValue.DateValue = DateTime.Parse(fieldValue);
			}

		}

	}
}
