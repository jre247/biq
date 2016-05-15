using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Account
{
	public class RetrieveViewModel
	{
		[Required]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		public bool Completed { get; set; }
	}
}
