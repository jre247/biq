using BrightLine.Common.Core;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Common.Utility;
using BrightLine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLine.Common.ViewModels.Blueprints
{
	public class BlueprintViewModel : ViewModelBase
	{
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(255, ErrorMessage = "Name cannot be longer than 255 characters.")]
		[UniqueBlueprintName(ErrorMessage = "Please enter a unique Name.")]
		public string Name { get; set; }

		public string ConnectedCreativeDisplay { get; set; }

		[Required(ErrorMessage = "Repository is required.")]
		[Display(Name = "Repository")]
		[MaxLength(255, ErrorMessage = "Repository length must be less than 255 characters.")]
		[UniqueManifestName(ErrorMessage = "Please enter a unique Repository.")]
		public string ManifestName { get; set; }

		public string DateUpdated { get; set; }

		[RequiredLookup(ErrorMessage = "Feature Type Group must be selected.")]
		public ILookup SelectedFeatureTypeGroup { get; set; }
		public string FeatureTypeGroupDisplay { get; set; }
		[Display(Name = "Feature Type Group")]
		public IEnumerable<ILookup> FeatureTypeGroups { get; set; }

		[RequiredLookup(ErrorMessage = "Feature Type must be selected.")]
		public ILookup SelectedFeatureType { get; set; }
		public string FeatureTypeDisplay { get; set; }
		[Display(Name = "Feature Type")]
		public IEnumerable<ILookup> FeatureTypes { get; set; }

		[RequiredLookup(ErrorMessage = "Preview Image is required.")]
		[Display(Name = "Preview Image")]
		public ILookup Preview { get; set; }
		public string PreviewDownloadUrl { get; set; }

		[Display(Name = "Connected TV Creative")]
		public ILookup ConnectedTVCreative { get; set; }
		public string ConnectedTVCreativeDownloadUrl { get; set; }

		[Display(Name = "Connected TV Support")]
		public ILookup ConnectedTVSupport { get; set; }
		public string ConnectedTVSupportDownloadUrl { get; set; }

		public string FeatureTypesDictionary { get; set; }

		public BlueprintViewModel()
		{}

		public BlueprintViewModel(Blueprint blueprint)
		{
			var fileHelper = IoC.Resolve<IFileHelper>();

			Id = blueprint.Id;
			Name = blueprint.Name;
			ManifestName = blueprint.ManifestName;
			SelectedFeatureType = blueprint.FeatureType;
			SelectedFeatureTypeGroup = blueprint.FeatureType.FeatureTypeGroup;

			FeatureTypeDisplay = blueprint.FeatureType.Name;

			if (blueprint.FeatureType.FeatureTypeGroup != null)
				FeatureTypeGroupDisplay = blueprint.FeatureType.FeatureTypeGroup.Name;
				
			ConnectedCreativeDisplay = blueprint.ConnectedTVCreative != null ? blueprint.ConnectedTVCreative.Name : null;

			DateUpdated = DateHelper.ToString(DateHelper.ToUserTimezone(blueprint.DateUpdated), null, null, "MM/dd/yy");

			Preview = EntityLookup.ToLookup(blueprint.Preview, "name");
			if (Preview != null)
				PreviewDownloadUrl = fileHelper.GetCloudFileDownloadUrl(blueprint.Preview);

			ConnectedTVCreative = EntityLookup.ToLookup(blueprint.ConnectedTVCreative, "name");
			if (ConnectedTVCreative != null)
				ConnectedTVCreativeDownloadUrl = fileHelper.GetCloudFileDownloadUrl(blueprint.ConnectedTVCreative);

			ConnectedTVSupport = EntityLookup.ToLookup(blueprint.ConnectedTVSupport, "name");
			if (ConnectedTVSupport != null)
				ConnectedTVSupportDownloadUrl = fileHelper.GetCloudFileDownloadUrl(blueprint.ConnectedTVSupport); 
		}

		
	}

	public class UniqueBlueprintNameAttribute : ValidationAttribute
	{
		public UniqueBlueprintNameAttribute()
		{

		}

		public string[] PropertyNames { get; private set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var blueprints = IoC.Resolve<IBlueprintService>();

			var vm = validationContext.ObjectInstance as BlueprintViewModel;
			var id = vm.Id;
			var name = value.ToString();

			var duplicateBlueprintNames = blueprints.Where(c => c.Name.ToLower() == name.ToLower() && c.Id != vm.Id).ToEntities();
			if (duplicateBlueprintNames.Count > 0)
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
			var blueprints = IoC.Resolve<IBlueprintService>();

			var vm = validationContext.ObjectInstance as BlueprintViewModel;
			var id = vm.Id;
			var manifestName = value.ToString();

			var duplicateManifestNames = blueprints.Where(c => c.ManifestName.ToLower() == manifestName.ToLower() && c.Id != id).ToEntities();
			if (duplicateManifestNames.Count > 0)
				return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

			return null;
		}
	}
}
