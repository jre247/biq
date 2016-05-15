using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Models.Lookups;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.Enums;
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
	public class ExtensionValidatorService : BaseValidatorService
	{
		private const string FieldType = "Image Extension";
		private readonly BoolMessageItem ValidationParseError = new BoolMessageItem(false, string.Format("{0} validation failed: field value is not valid {1}", FieldType, FieldType));
		private readonly BoolMessageItem ValidationError = new BoolMessageItem(false, FieldType + " validation failed");

		public ExtensionValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			BoolMessageItem boolMessage = null;
			int resourceId = 0;
			bool isResourceInputValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			if (validationTypeId == ValidationTypeRequired)
			{
				boolMessage = base.ValidateForRequired();
				return boolMessage;
			}

			//right now it's assumed that the model instance field's value will be an integer that represents a resource id
			if (!string.IsNullOrEmpty(InstanceFieldValue))
				isResourceInputValid = int.TryParse(InstanceFieldValue, out resourceId);

			if (!isResourceInputValid)
				return new BoolMessageItem(false, "Field value needs to be a number which represents a ResourceId.");

			boolMessage = ValidateFieldExtension(resourceId);

			return boolMessage;
		}

		private bool ValidateForOperation(bool validationValue)
		{
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeUnique)
				isValid = base.ValidateUniqueFieldValue();

			return isValid;
		}

		/// <summary>
		/// Validate that the resource's extension is allowed
		///		TODO: it might be a good idea to check this resource extension validation at the time that the resource is registered, instead of here
		/// </summary>
		/// <param name="resourceId"></param>
		/// <returns></returns>
		private BoolMessageItem ValidateFieldExtension(int resourceId)
		{
			var resourcesRepo = IoC.Resolve<IResourceService>();
			var fileTypeValidations = IoC.Resolve<IFileTypeValidationService>();

			var resource = resourcesRepo.Get(resourceId);
			if (resource == null)
				return new BoolMessageItem(false, "Resource does not exist for Id: " + resourceId);

			var extensions = fileTypeValidations.Where(f => f.Validation.Id == Validation.Id).ToList();

			var fieldExtensionId = resource.Extension.Id;

			var isFieldExtensionAllowed = extensions.Exists(e => e.FileType.Id == fieldExtensionId);
			if (!isFieldExtensionAllowed)
				return new BoolMessageItem(false, string.Format("Resource extension is not valid for extension: {0}.", resource.Extension.Name));

			return new BoolMessageItem(true, null);
		}
	}
}
