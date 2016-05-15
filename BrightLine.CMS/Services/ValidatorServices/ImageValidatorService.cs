using BrightLine.CMS.Services;
using BrightLine.Common.Framework;
using BrightLine.Common.Framework.Exceptions;
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
	public class ImageValidatorService : BaseValidatorService
	{
		public ImageValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }

		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			base.InstanceFieldValue = instanceFieldValue;
			var boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			boolMessage = new BoolMessageItem(true, null);
			int resourceId = 0;
			bool isResourceInputValid = true;

			//right now it's assumed that the instance's field value will be an integer that represents the resource id, so check if the instance's field value can be parsed to an int
			if (!string.IsNullOrEmpty(InstanceFieldValue))
				isResourceInputValid = int.TryParse(InstanceFieldValue, out resourceId);

			if(!isResourceInputValid)
				return new BoolMessageItem(false, "Field value needs to be a number which represents a ResourceId.");

			var resources = IoC.Resolve<IResourceService>();

			var resource = resources.Where(r => r.Id == resourceId).SingleOrDefault();
			if(resource == null)
				return new BoolMessageItem(false, "There is no Resource that matches this Resource Id.");
			if (resource.Extension == null)
				return new BoolMessageItem(false, "Resource does not have an extension.");

			//check if the model instance field's file extension is valid
			if (!Lookups.ImageResourceTypes.HashByName.ContainsKey(resource.Extension.ResourceType.Name))
			{ 
				boolMessage = new BoolMessageItem(false, "Resource to be registered has an invalid extension.");
				return boolMessage;
			}

			var isValid = ValidateForOperation(resource);
			if (!isValid)
				boolMessage = new BoolMessageItem(false, "validation failed for field of type 'ref'");

			return boolMessage;
		}

		/// <summary>
		/// Will validate for one of the following validation operations:
		///		1) Min Width
		///		2) Max Width
		///		3) Min Height
		///		4) MaxHeight
		///		5) Max Image Size (in bytes)
		///		6) Required
		/// </summary>
		/// <param name="validation"></param>
		/// <param name="field"></param>
		/// <param name="fieldValue"></param>
		/// <returns></returns>
		private bool ValidateForOperation(Resource resource)
		{
			int imageWidth = 0, imageHeight = 0, max = 0, min = 0;
			var isValid = true;
			var validationTypeId = Validation.ValidationType.Id;

			if (validationTypeId == ValidationTypeMinWidth)
			{
				imageWidth = resource.Width.Value;
				min = int.Parse(Validation.Value.ToString());
				if (imageWidth < min)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxWidth)
			{
				imageWidth = resource.Width.Value;
				max = int.Parse(Validation.Value.ToString());
				if (imageWidth > max)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMinHeight)
			{
				imageHeight = resource.Width.Value;
				min = int.Parse(Validation.Value.ToString());
				if (imageHeight < min)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxHeight)
			{
				imageHeight = resource.Width.Value;
				max = int.Parse(Validation.Value.ToString());
				if (imageHeight > max)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeMaxImageSize)
			{
				imageHeight = resource.Size.Value; //bytes
				max = int.Parse(Validation.Value.ToString());
				if (imageHeight > max)
					isValid = false;
			}
			else if (validationTypeId == ValidationTypeRequired)
			{
				var isRequired = bool.Parse(Validation.Value.ToString());
				if (isRequired && string.IsNullOrEmpty(InstanceFieldValue))
					isValid = false;
			}

			return isValid;
		}
	}
}
