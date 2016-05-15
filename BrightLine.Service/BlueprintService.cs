using System.Collections.Generic;
using System.Linq;
using BrightLine.Common.Core.Attributes;
using BrightLine.Common.Framework;
using BrightLine.Common.Models;
using BrightLine.Common.Services;
using BrightLine.Core;
using BrightLine.Common.ViewModels.Blueprints;
using BrightLine.Common.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using BrightLine.Utility;
using BrightLine.Common.Framework.Exceptions;
using System.Web;
using BrightLine.Common.Utility;
using Octokit;
using BrightLine.Common.Utility.Blueprints;
using BrightLine.Common.Utility.SqlServer;
using System.Data;

namespace BrightLine.Service
{
	public class BlueprintService : CrudService<Blueprint>, IBlueprintService
	{

		public BlueprintService(IRepository<Blueprint> repo)
			: base(repo)
		{
		}

		public async Task<Blueprint> Save(BlueprintViewModel model, HttpRequestBase Request)
		{
			var blueprintImportService = IoC.Resolve<IBlueprintImportService>();
			var fileHelper = IoC.Resolve<IFileHelper>();

			var files = Request.Files;

			if (model.IsNewModel)
			{
				//validate preview image is included
				var preview = Request.Files[BlueprintConstants.PREVIEW_FILE];
				if (preview == null || !fileHelper.IsFilePresent(preview))
					throw new ModelImageNotFoundException();
			}
			else
			{
				var existingBlueprint = Get(model.Id);
				if (existingBlueprint == null)
					throw new ModelNotFoundException();
			}

			var blueprint = SaveBlueprintPropertyValues(model, files);

			if (model.IsNewModel)
			{
				var boolMessageItem = await blueprintImportService.ImportBlueprint(blueprint.Id, model.ManifestName);
				if(!boolMessageItem.Success)
					throw new BlueprintImportException();
			}

			return blueprint;
		}

		public override Blueprint Create(Blueprint blueprint)
		{
			Upsert(blueprint);
			return blueprint;
		}

		public override List<Blueprint> Create(IEnumerable<Blueprint> blueprints)
		{
			Upsert(blueprints);
			return blueprints.ToList();
		}

		public override Blueprint Update(Blueprint blueprint)
		{
			Upsert(blueprint);
			return blueprint;
		}

		public override List<Blueprint> Update(IEnumerable<Blueprint> blueprints)
		{
			Upsert(blueprints);
			return blueprints.ToList();
		}

		public override Blueprint Upsert(Blueprint blueprint)
		{
			//TODO: proper versioning - all Create, Update, Cud calls filter throught this method to ensure the version will be correct throughout
			//var current = Blueprints.Get(blueprint.Id);
			//if (IsNewVersion(blueprint, current))
			//{
			//	Version(blueprint, current);
			//	base.Create(blueprint);
			//}
			if (blueprint.IsNewEntity)
				base.Create(blueprint);
			else
				base.Update(blueprint);

			return blueprint;
		}

		public override List<Blueprint> Upsert(IEnumerable<Blueprint> blueprints)
		{
			foreach (var blueprint in blueprints)
			{
				Upsert(blueprint);
			}

			return blueprints.ToList();
		}

		public override Blueprint Cud(Blueprint blueprint, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (blueprint == null)
				return null;

			if (blueprint.IsDeleted)
				base.Delete(blueprint.Id, deleteType);
			else
				Upsert(blueprint);

			return blueprint;
		}

		public override List<Blueprint> Cud(IEnumerable<Blueprint> blueprints, DeleteTypes deleteType = DeleteTypes.Hard)
		{
			if (blueprints == null)
				return new List<Blueprint>();

			foreach (var blueprint in blueprints)
			{
				if (blueprint.IsDeleted)
					base.Delete(blueprint.Id, deleteType);
				else
					Upsert(blueprint);
			}

			return blueprints.Where(bp => !bp.IsDeleted).ToList();
		}

		public List<Blueprint> GetLatestVersions(bool includeDeleted = false)
		{
			var blueprints = GetAll(includeDeleted);
			var latest = (from bp in blueprints
						  group bp by bp.GroupId into bpgs
						  select new
						  {
							  Group = bpgs,
							  MajorVersion = bpgs.Max(b => b.MajorVersion),
							  MinorVersion = bpgs.Max(b => b.MinorVersion),
							  Patch = bpgs.Max(b => b.Patch),
						  }
							  into gbps
							  from bps in gbps.Group
							  let last = gbps.Group.FirstOrDefault(b => b.MajorVersion == gbps.MajorVersion && b.MinorVersion == gbps.MinorVersion && b.Patch == gbps.Patch)
							  select last);

			return latest.Distinct().ToList();
		}

		public BlueprintViewModel GetViewModel()
		{
			var vm = new BlueprintViewModel();
			GetLookupsForBlueprint(vm);

			return vm;
		}

		public BlueprintViewModel GetViewModel(Blueprint blueprint)
		{
			if (blueprint == null)
				return null;

			var vm = new BlueprintViewModel(blueprint);
			GetLookupsForBlueprint(vm);

			return vm;
		}
		
		public void GetLookupsForBlueprint(BlueprintViewModel vm)
		{
			GetFeatureTypeGroups(vm);

			GetFeatureTypes(vm);

			BuildFeatureTypesDictionary(vm);
		}

		#region Private Methods

		private void GetFeatureTypeGroups(BlueprintViewModel vm)
		{
			var featureTypeGroups = IoC.Resolve<IRepository<FeatureTypeGroup>>();

			vm.FeatureTypeGroups = featureTypeGroups.GetAll().
						 OrderBy(p => p.Name).
						 ToLookups().
						 InsertLookups(0, EntityLookup.Select);
		}

		private void GetFeatureTypes(BlueprintViewModel vm)
		{
			var featureTypesRepo = IoC.Resolve<IRepository<FeatureType>>();

			//only fill Feature Types list if editing blueprint or if the user has already selected a feature type
			if (!vm.IsNewModel || vm.SelectedFeatureType != null)
			{
				//get all Feature Types for the selected Feature Type Group
				if (vm.SelectedFeatureTypeGroup != null)
				{
					vm.FeatureTypes = featureTypesRepo.Where(f => f.FeatureTypeGroup.Id == vm.SelectedFeatureTypeGroup.Id).ToList().
						OrderBy(p => p.Name).
						ToLookups().
						InsertLookups(0, EntityLookup.Select);
				}
			}
			//create empty list of Feature Types
			else
			{
				IQueryable<FeatureType> featureTypes = null;
				vm.FeatureTypes = featureTypes
					.InsertLookups(0, EntityLookup.Select);
			}
		}

		/// <summary>
		/// Build a dictionary where the key is a feature type group id, and the value is a list of feature types for 
		/// that specific feature type group 
		/// </summary>
		/// <param name="vm"></param>
		private void BuildFeatureTypesDictionary(BlueprintViewModel vm)
		{
			var featureTypesRepo = IoC.Resolve<IRepository<FeatureType>>();

			var featureTypesDictionary = new Dictionary<string, List<EntityLookup>>();
			var featureTypesGroups = featureTypesRepo.Where(f => f.FeatureTypeGroup != null).GroupBy(f => f.FeatureTypeGroup.Id).ToList();
			foreach (var featureTypeGroup in featureTypesGroups)
			{
				var featureTypeGroupId = featureTypeGroup.Key;
				var featureTypesForGroup = featureTypeGroup.OrderBy(c => c.Name).Select(f => new EntityLookup { Id = f.Id, Name = f.Name }).ToList();

				featureTypesDictionary.Add(featureTypeGroupId.ToString(), featureTypesForGroup);

			}
			vm.FeatureTypesDictionary = JsonConvert.SerializeObject(featureTypesDictionary);
		}

		private Blueprint SaveBlueprintPropertyValues(BlueprintViewModel model, HttpFileCollectionBase files)
		{
			Blueprint blueprint = null;
			var featureTypesRepo = IoC.Resolve<IRepository<FeatureType>>();

			if (model.IsNewModel)
				blueprint = new Blueprint();
			else
				blueprint = Get(model.Id);

			blueprint.ManifestName = model.ManifestName;
			blueprint.Name = model.Name;

			//For now, hardcode 1.0.0 for version
			blueprint.MajorVersion = 1;
			blueprint.MinorVersion = 0;
			blueprint.Patch = 0;

			if (model.SelectedFeatureType != null)
				blueprint.FeatureType = featureTypesRepo.Get(model.SelectedFeatureType.Id);

			UploadFiles(blueprint, files);

			blueprint = Upsert(blueprint);
			return blueprint;
		}


		private void UploadFiles(Blueprint blueprint, HttpFileCollectionBase files)
		{
			var fileHelper = IoC.Resolve<IFileHelper>();

			if (files != null)
			{
				var preview = files[BlueprintConstants.PREVIEW_FILE];
				if (fileHelper.IsFilePresent(preview))
					blueprint.Preview = fileHelper.CreateFile(preview);

				var connectedTVCreative = files[BlueprintConstants.CONNECTED_TV_FILE];
				if (fileHelper.IsFilePresent(connectedTVCreative))
					blueprint.ConnectedTVCreative = fileHelper.CreateFile(connectedTVCreative);

				var connectedTVSupport = files[BlueprintConstants.CONNECTED_TV_SUPPORT_FILE];
				if (fileHelper.IsFilePresent(connectedTVSupport))
					blueprint.ConnectedTVSupport = fileHelper.CreateFile(connectedTVSupport);
			}
		}


		private bool IsNewVersion(Blueprint blueprint, Blueprint current)
		{
			//TODO: determine real version newness
			return true;
			if (blueprint.IsNewEntity || current == null)
				return true;

			// get the current db version
			var isNewVersion = (//!blueprint.BlueprintType.Equals(current.BlueprintType) ||
				//!blueprint.FeatureTypeGroup.Equals(current.FeatureTypeGroup) ||
								!blueprint.FeatureType.Equals(current.FeatureType) ||
								!blueprint.ManifestName.Equals(current.ManifestName) ||
								!blueprint.GroupId.Equals(current.GroupId));
			var isNewPreview = (blueprint.Preview == null && blueprint.Preview != null) && !blueprint.Preview.Equals(current.Preview);
			var isNewCreative = (blueprint.ConnectedTVCreative == null && blueprint.ConnectedTVCreative != null) && !blueprint.ConnectedTVCreative.Equals(current.ConnectedTVCreative);
			var isNewSupport = (blueprint.ConnectedTVSupport == null && blueprint.ConnectedTVSupport != null) && !blueprint.ConnectedTVSupport.Equals(current.ConnectedTVSupport);

			return isNewVersion && isNewPreview && isNewCreative && isNewSupport;
		}


		private void Version(Blueprint blueprint, Blueprint current)
		{
			var major = 1;
			var minor = 1;
			var patch = 0;
			// get the current db version
			if (current != null)
			{
				major = current.MajorVersion;
				minor = current.MinorVersion;
				patch = current.Patch;

				// if explicitly new version, reset minor and/or patch
				if (blueprint.MajorVersion != major)
					minor = 1;
				if (blueprint.MinorVersion != minor)
					patch = 0;
			}

			blueprint.MajorVersion = major;
			blueprint.MinorVersion = minor;
			blueprint.Patch = (patch + 1); // add 1 to bump patch no matter the major/minor version (why patch=0 above)
		}

		#endregion
	}
}