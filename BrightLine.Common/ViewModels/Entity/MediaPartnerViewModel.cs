using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Entity
{
	public class MediaPartnerViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[UniqueMediaPartnerName(ErrorMessage = "Please enter a unique Name.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Manifest Name is required.")]
		[Display(Name = "Manifest Name")]
		[MaxLength(255, ErrorMessage = "Manifest Name length must be less than 255 characters.")]
		[UniqueManifestName(ErrorMessage = "Please enter a unique Manifest Name.")]
		public string ManifestName { get; set; }

		
		public ILookup SelectedParent { get; set; }
		[Display(Name = "Parent")]
		public IEnumerable<ILookup> Parents { get; set; }

		[Display(Name = "Is Network")]
		public bool IsNetwork { get;set;}
		public string Display { get;set;}
		public bool IsDeleted { get; set; }


		[RequiredLookup(ErrorMessage = "Category must be selected.")]
		public ILookup SelectedCategory { get; set; }
		public string CategoryDisplay { get; set; }
		[Display(Name = "Category")]
		public IEnumerable<ILookup> Categories { get;set;}

		public MediaPartnerViewModel()
		{ }

		public MediaPartnerViewModel(MediaPartner mediaPartner)
		{
			this.Id = mediaPartner.Id;
			this.Name = mediaPartner.Name;
			this.ManifestName = mediaPartner.ManifestName;
			this.SelectedParent = mediaPartner.Parent as ILookup;
			this.IsNetwork = mediaPartner.IsNetwork;
			this.Display = mediaPartner.Display;
			this.IsDeleted = mediaPartner.IsDeleted;
			this.SelectedCategory = mediaPartner.Category as ILookup;
			this.CategoryDisplay = mediaPartner.Category != null ? mediaPartner.Category.Display : "";
		}
	}


	public class UniqueMediaPartnerNameAttribute : ValidationAttribute
	{
		public UniqueMediaPartnerNameAttribute()
		{
			
		}

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var vm = validationContext.ObjectInstance as MediaPartnerViewModel;
			var id = vm.Id;
			var name = value.ToString();

			var mediaPartners = IoC.Resolve<IMediaPartnerService>();

			var duplicateMediaPartnerNames = mediaPartners.Where(c => c.Name.ToLower() == name.ToLower() && c.Id != vm.Id).ToEntities();
			if (duplicateMediaPartnerNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}

	public class UniqueManifestNameAttribute : ValidationAttribute
	{
		public UniqueManifestNameAttribute()
		{
			
		}

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var vm = validationContext.ObjectInstance as MediaPartnerViewModel;
			var id = vm.Id;
			var manifestName = value.ToString();

			var mediaPartners = IoC.Resolve<IMediaPartnerService>();

			var duplicateManifestNames = mediaPartners.Where(c => c.ManifestName.ToLower() == manifestName.ToLower() && c.Id != id).ToEntities();
			if (duplicateManifestNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}
}
