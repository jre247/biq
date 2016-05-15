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
	public class AppViewModel
	{
		public int Id { get; set; }

		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[Required(ErrorMessage = "Name is required.")]
		[UniqueAppName(ErrorMessage = "Please enter a unique Name.")]
		public string Name { get; set; }

		[RequiredLookup(ErrorMessage = "Media Partner must be selected.")]
		public ILookup SelectedMediaPartner { get; set; }
		[Display(Name = "Media Partner")]
		public IEnumerable<ILookup> MediaPartners { get; set; }
		public string MediaPartnerDisplay { get; set; }

		public ILookup SelectedAdvertiser { get; set; }
		[Display(Name = "Advertiser")]
		public IEnumerable<ILookup> Advertisers { get; set; }
		public string AdvertiserDisplay { get; set; }

		public string CategoryDisplay { get; set; }

		public string Display { get; set; }
		public bool IsDeleted { get; set; }

		public AppViewModel()
		{ }

		public AppViewModel(App app)
		{
			this.Id = app.Id;
			this.Name = app.Name;
			this.CategoryDisplay = app.Category != null ? app.Category.Display : "";
			this.MediaPartnerDisplay = app.MediaPartner != null ? app.MediaPartner.Display : "";
			this.SelectedMediaPartner = app.MediaPartner as ILookup;
			this.SelectedAdvertiser = app.Advertiser as ILookup;
		}
	}

	public class UniqueAppName : ValidationAttribute
	{
		public UniqueAppName()
		{ }

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var apps = IoC.Resolve<IAppService>();

			var vm = validationContext.ObjectInstance as AppViewModel;
			var id = vm.Id;
			var name = value.ToString();

			var duplicateNames = apps.Where(c => c.Name.ToLower() == name.ToLower() && c.Id != vm.Id).ToEntities();
			if (duplicateNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}
}
