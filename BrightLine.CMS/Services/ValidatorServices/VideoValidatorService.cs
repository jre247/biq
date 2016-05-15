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
	public class VideoValidatorService : BaseValidatorService
	{
		public VideoValidatorService(ValidatorServiceParams serviceParams)
			: base(serviceParams)
		{ }


		public override BoolMessageItem Validate(string instanceFieldValue)
		{
			var resources = IoC.Resolve<IResourceService>();

			base.InstanceFieldValue = instanceFieldValue;
			var boolMessage = ValidateIfFieldValueNotRequired();
			if (boolMessage != null)
				return boolMessage;

			boolMessage = new BoolMessageItem(true, null);
			int resourceId = 0;
			bool isResourceInputValid = true;

			//right now it's assumed that the model instance field's value will be an integer that represents the resource id. 
			if (!string.IsNullOrEmpty(InstanceFieldValue))
				isResourceInputValid = int.TryParse(InstanceFieldValue, out resourceId);

			if(!isResourceInputValid)
				return new BoolMessageItem(false, "Field value needs to be a number which represents a ResourceId.");

			var resource = resources.Where(r => r.Id == resourceId).SingleOrDefault();
			if(resource == null)
				return new BoolMessageItem(false, "There is no Resource that matches this Resource Id.");
			if (resource.Extension == null)
				return new BoolMessageItem(false, "Resource does not have an extension.");

			//validate that the resource's extension exists
			if (!Lookups.VideoResourceTypes.HashByName.ContainsKey(resource.Extension.ResourceType.Name))
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
		///		1) Max Video Size (in bytes)
		///		2) Max Video Duration (in seconds)
		///		3) Min Height
		///		6) Required
		/// </summary>
		/// <param name="validation"></param>
		/// <param name="field"></param>
		/// <param name="fieldValue"></param>
		/// <param name="resource"></param>
		/// <returns></returns>
		private bool ValidateForOperation(Resource resource)
		{
			var settings = IoC.Resolve<ISettingsService>();

			int videoSize = 0, videoDuration = 0, size = 0, duration = 0;
			var iqVideoMaxSize = int.Parse(settings.IqMaxVideoSize);
			var iqVideoMaxDuration = int.Parse(settings.IqMaxVideoDuration);
			var validationTypeId = Validation.ValidationType.Id;
			var isValid = true;

			if (validationTypeId == ValidationTypeMaxVideoSize)
			{
				videoSize = resource.Size.Value; //bytes
				size = int.Parse(Validation.Value.ToString());
				if (videoSize > size ||
				  videoSize > iqVideoMaxSize)
					isValid = false;
			}
			else if (validationTypeId == ValidationVideoDuration)
			{
				videoDuration = resource.Duration.Value; //seconds
				duration = int.Parse(Validation.Value.ToString());
				if (videoDuration > duration ||
				  videoDuration > iqVideoMaxDuration)
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
