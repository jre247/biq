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
	public class PlacementViewModel
	{
		public int Id { get; set; }

		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[Required(ErrorMessage = "Name is required.")]
		[UniquePlacementName(ErrorMessage = "Please enter a unique Name.")]
		public string Name { get; set; }

		[Range(1, int.MaxValue, ErrorMessage="Height must be between 1 and 2147483647")]
		public int? Height { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Width must be between 1 and 2147483647")]
		public int? Width { get; set; }

		[Display(Name = "Location Details")]
		[MaxLength(2000, ErrorMessage = "Location Details must be less than 2000 characters.")]
		public string LocationDetails { get; set; }

		[RequiredLookup(ErrorMessage = "MediaPartner must be selected.")]
		public ILookup SelectedMediaPartner { get; set; }
		[Display(Name = "Media Partner")]
		public IEnumerable<ILookup> MediaPartners { get; set; }
		public string MediaPartnerDisplay { get; set; }

		[RequiredLookup(ErrorMessage = "Add Type Group must be selected.")]
		public ILookup SelectedAdTypeGroup { get; set; }
		[Display(Name = "Ad Type Group")]
		public IEnumerable<ILookup> AdTypeGroups { get; set; }
		public string AdTypeGroupDisplay { get; set; }

		[RequiredLookup(ErrorMessage = "Category must be selected.")]
		public ILookup SelectedCategory { get; set; }
		[Display(Name = "Category")]
		public IEnumerable<ILookup> Categories { get; set; }
		public string CategoryDisplay { get; set; }

		public ILookup SelectedApp { get; set; }
		[Display(Name = "App")]
		public IEnumerable<ILookup> Apps { get; set; }

		public string Display { get; set; }
		public bool IsDeleted { get; set; }

		public PlacementViewModel()
		{ }

		public PlacementViewModel(Placement placement)
		{
			this.Id = placement.Id;
			this.Name = placement.Name;
			this.SelectedCategory = placement.Category as ILookup;
			this.CategoryDisplay = placement.Category != null ? placement.Category.Display : "";
			this.MediaPartnerDisplay = placement.MediaPartner != null ? placement.MediaPartner.Display : "";
			this.Width = placement.Width;
			this.Height = placement.Height;
			this.LocationDetails = placement.LocationDetails;
			this.SelectedMediaPartner = placement.MediaPartner as ILookup;
			this.SelectedAdTypeGroup = placement.AdTypeGroup as ILookup;
			this.SelectedApp = placement.App as ILookup;
		}
	}

	public class UniquePlacementName : ValidationAttribute
	{
		public UniquePlacementName()
		{ }

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var vm = validationContext.ObjectInstance as PlacementViewModel;
			var id = vm.Id;
			var name = value.ToString();

			var placements = IoC.Resolve<IPlacementService>();

			var duplicateNames = placements.Where(c => c.Name.ToLower() == name.ToLower() && c.Id != vm.Id).ToEntities();
			if (duplicateNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}

	public class MaxValueAttribute : ValidationAttribute
	{
		private readonly int _maxValue;

		public MaxValueAttribute(int maxValue)
		{
			_maxValue = maxValue;
		}

		public override bool IsValid(object value)
		{
			return (int)value <= _maxValue;
		}
	}
}
