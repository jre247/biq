using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Account
{
	public class UpdatePasswordViewModel
	{
		[Required]
		 [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).+$", ErrorMessage = "Password must be at least 8 characters, contain one number, and one special character")]
		public string Password { get; set; }
	}
}
