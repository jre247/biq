using BrightLine.Common.Models;
using BrightLine.Common.Utility;
using BrightLine.Common.Utility.ValidationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels
{
	public class ValidationViewModel
	{
		public string name { get; set; }
		public string value { get; set; }

		public ValidationViewModel()
		{

		}

		public ValidationViewModel(Validation validation)
		{
			name = validation.ValidationType.Name;

			var extensionValidationType = Lookups.ValidationTypes.HashByName[ValidationTypeConstants.ValidationTypeNames.Extension];
			if (validation.ValidationType.Id == extensionValidationType)
				GetExtensionsForValidation(validation);
			else
				value = validation.Value;
			
		}

		private void GetExtensionsForValidation(Validation validation)
		{
			var extensionsValue = "";
			var extensions = validation.FileTypeValidations.Select(c => c.FileType.Name).ToList();
			if (extensions.Count() > 0)
				extensionsValue = String.Join(",", extensions);

			value = extensionsValue;
		}
	}

}
