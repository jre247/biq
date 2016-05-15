using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
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

namespace BrightLine.CMS.Service
{
	public class PageRefValidatorService : BaseValidatorService
	{
		private const string MODEL_INSTANCE_INCORRECT_FORMAT = "Field with type 'ref' and with id {0} has one or more model instance values that are not in the correct format";
		private const string MODEL_INSTANCE_INVALID_FOR_MODEL = "Field with type 'ref' and id {0} references a model instance with id {1} that don't exist for model with name {2}";
		private const string VALIDATION_OPERATION_INVALID = "Page validation operation failed: {0}.";
		private const string CREATIVE_REFERENCE_INVALID = "Field value has a page id that references a page that is of a different creative than the original model instance.";
		private const string PAGE_DOES_NOT_EXIST = "Page doesn't exist for field value pageId.";
		private const string MODEL_INSTANCE_PAGE_ID_INVALID = "Page Id does not exist for field.";

		public PageRefValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			var validationTypeId = Validation.ValidationType.Id;
			var boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			boolMessage = new BoolMessageItem(true, null);
			bool validationValueAsBool = true;

			if (validationTypeId == ValidationTypeRequired)
			{
				boolMessage = base.ValidateForRequired();
				return boolMessage;
			}

			//convert field value from json string to strongly typed obj
			var fieldValueScrubbed = InstanceFieldValue.Replace("\r\n", "").Replace("\t", ""); //remove tabs and new lines from value string in order to make correct json format

			if (CmsField.Type.Id != Lookups.FieldTypes.HashByName[FieldTypeConstants.FieldTypeNames.RefToPage])
				return new BoolMessageItem(false, "Field does not have Type of Page Ref.");

			boolMessage = ValidatePageReference(ref validationValueAsBool, fieldValueScrubbed);

			return boolMessage;
		}

		private BoolMessageItem ValidatePageReference(ref bool validationValueAsBool, string fieldValueScrubbed)
		{
			var boolMessage = BoolMessageItem.GetSuccessMessage();
			var validationTypeId = Validation.ValidationType.Id;
			var pagesRepo = IoC.Resolve<IRepository<Page>>();

			var refValue = JsonConvert.DeserializeObject<PageRefFieldValue>(fieldValueScrubbed);
			if (refValue == null)
				return new BoolMessageItem(false, string.Format(MODEL_INSTANCE_INCORRECT_FORMAT, InstanceField.id));

			//make sure the referenced page's id exists
			var pageExists = pagesRepo.Get(refValue.pageId);
			if (pageExists == null)
				return new BoolMessageItem(false, PAGE_DOES_NOT_EXIST);

			boolMessage = ValidateForCreative(refValue);
			if (!boolMessage.Success)
				return boolMessage;

			//if Validation Type is set to Unique then the Validation value will either be "true" or "false", signifying if the field value should be unique or not
			if (validationTypeId == ValidationTypeUnique)
			{
				boolMessage = base.ParseValidationValueToBool(ref validationValueAsBool);
				if (!boolMessage.Success)
					return boolMessage;
			}

			var isValid = base.ValidateForRefOperation();
			if (!isValid)
				return new BoolMessageItem(false, string.Format(VALIDATION_OPERATION_INVALID, Validation.ValidationType.Name));

			return boolMessage;
		}

		/// <summary>
		/// Validate that the referenced page has the same creative id as the original model instance
		/// </summary>
		/// <param name="refValue"></param>
		/// <returns></returns>
		private BoolMessageItem ValidateForCreative(PageRefFieldValue refValue)
		{
			var boolMessage = new BoolMessageItem(true, null);
			var pagesRepo = IoC.Resolve<IRepository<Page>>();

			var pages = pagesRepo.Where(p => p.Id == refValue.pageId);
			if(pages == null)
				return new BoolMessageItem(false, MODEL_INSTANCE_PAGE_ID_INVALID);
			var page = pages.SingleOrDefault();
			if(page == null)
				return new BoolMessageItem(false, MODEL_INSTANCE_PAGE_ID_INVALID);

			var originalInstanceCreative = base.ModelInstanceLookups.ModelInstance.Model.Feature.Creative.Id;
			var refInstanceCreative = page.Feature.Creative.Id;

			if (originalInstanceCreative != refInstanceCreative)
				boolMessage = new BoolMessageItem(false, CREATIVE_REFERENCE_INVALID);

			return boolMessage;
		}

		
	}
}
