using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrightLine.Common.ViewModels.Account
{
	public class UserViewModel
	{
		[Required]
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }

		[DataType(DataType.Text)]
		[Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required.")]
		[RegularExpression(@"^[ a-zA-Z0-9\-\""\']{2,}$", ErrorMessage = "First Name must be more than 2 letters.")]
		[Display(Name = "First Name")]
		[StringLength(255, ErrorMessage = "First Name cannot be longer than 255 characters.")]
		public string FirstName { get; set; }

		[DataType(DataType.Text)]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required.")]
		[RegularExpression(@"^[ a-zA-Z0-9\-\""\']{2,}$", ErrorMessage = "Last Name must be more than 2 letters.")]
		[Display(Name = "Last Name")]
		[StringLength(255, ErrorMessage = "Last Name cannot be longer than 255 characters.")]
		public string LastName { get; set; }

		[DataType(DataType.Text)]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).+$",
			ErrorMessage = "Password must be at least 8 characters, contain one number, and one special character")]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Passwords do not match")]
		[Display(Name = "Confirm Password")]
		public string PasswordConfirm { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string TimeZoneId { get; set; }

		public bool Success { get; set; }
	}
}