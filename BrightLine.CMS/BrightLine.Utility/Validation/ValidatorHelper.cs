using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Utility.Validation
{
	public class ValidatorHelper
	{
		/// <summary>
		/// Collects all errors from all the validators.
		/// </summary>
		/// <param name="validators"></param>
		/// <returns></returns>
		public static BoolMessageItem<List<string>> Validate(List<ValidatorBase> validators, bool stopOnFirstValidatorFailure)
		{
			var allErrors = new List<string>();
			
			foreach (var validator in validators)
			{
				validator.Validate();

				if (validator.HasErrors())
				{ 
					allErrors.AddRange(validator.GetErrors());
					if(stopOnFirstValidatorFailure)
						break;
				}
			}
			return new BoolMessageItem<List<string>>(allErrors.Count == 0, string.Empty, allErrors);
		}


		/// <summary>
		/// Collects all errors from all the validators.
		/// </summary>
		/// <param name="validators"></param>
		/// <returns></returns>
		public static List<string> CollectErrors(List<ValidatorBase> validators)
		{
			var allErrors = new List<string>();
			foreach (var validator in validators)
			{
				if(validator.HasErrors())
					allErrors.AddRange(validator.GetErrors());
			}
			return allErrors;
		}


		/// <summary>
		/// Build a single erorr message from all the validators.
		/// </summary>
		/// <param name="validators"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string BuildSingleErrorMessage(List<ValidatorBase> validators, string separator)
		{
			var allErrors = CollectErrors(validators);
			var message = "";
			foreach (var msg in allErrors)
			{
				message += msg + separator;
			}
			return message;
		}		
	}
}
