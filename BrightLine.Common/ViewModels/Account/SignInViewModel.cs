using System.ComponentModel.DataAnnotations;

namespace BrightLine.Common.ViewModels.Account
{
	public class SignInViewModel
	{
		[Required]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Required]
		public string Password { get; set; }
		[Display(Name = "Remember me")]
		public bool RememberMe { get; set; }
		
	}
}