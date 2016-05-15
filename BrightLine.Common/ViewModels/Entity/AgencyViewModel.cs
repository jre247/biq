using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Entity
{
	public class AgencyViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[UniqueAgencyName(ErrorMessage = "Please enter a unique Name.")]
		public string Name { get; set; }

		public ILookup SelectedParent { get; set; }
		[Display(Name = "Parent")]
		public IEnumerable<ILookup> Parents { get; set; }

		public string Display { get; set; }

		public AgencyViewModel()
		{ }

		public AgencyViewModel(Agency agency)
		{
			this.Id = agency.Id;
			this.Display = agency.Display;
			this.SelectedParent = agency.Parent as ILookup;
			this.Name = agency.Name;
		}
	}


	public class UniqueAgencyNameAttribute : ValidationAttribute
	{
		public UniqueAgencyNameAttribute()
		{
			
		}

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var vm = validationContext.ObjectInstance as AgencyViewModel;
			var id = vm.Id;
			var name = value.ToString();
			var agencies = IoC.Resolve<IRepository<Feature>>();

			var duplicateAgencyNames = agencies.Where(c => c.Name.ToLower() == name.ToLower() && c.Id != vm.Id).ToEntities();
			if (duplicateAgencyNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}
}
