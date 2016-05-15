var tabs = {
	FeatureBlueprints: { Display: "Feature Blueprints", Hash: "#fb" },
	FeatureBlueprintDetails: { Display: "Feature Blueprint Details", Hash: "#fbd" },
	PromotionalBlueprints: { Display: "Promotional Ad Blueprints", Hash: "#pab" },
	UxtvProducts: { Display: "UXTV Product Line Admin", Hash: "#uxtv" },
	Placements: { Display: "Placements", Hash: "#p" },
	PlacementMappings: { Display: "Placement Mapping", Hash: "#pm" },
};
var showPreview = function (element, xOffset) {
	var img = $(element);
	var src = img.prop("src");
	var o = img.offset();
	var css = { position: "absolute", top: o.top + "px", left: o.left + xOffset + "px" };
	var imgContainer = $("<div/>").prop("id", "preview-hover").css(css).appendTo("body").fadeIn();
	var preview = $("<img alt=''/>").prop("src", src);
	imgContainer.append(preview);
};
var removePreview = function () {
	$("#preview-hover").remove();
};

var AvailableFeatureTypeGroups = [];
var AvailableFeatureTypes = [];
var AvailableFeatureCategories = [];
var AvailablePlatforms = [];

var AdministrationViewModel = function (data, featureTypeGroups, featureTypes, featureCategories, platforms) {
	AvailableFeatureTypeGroups = ko.utils.sortBy(featureTypeGroups, "Display");
	AvailableFeatureTypes = ko.utils.sortBy(featureTypes, "Display");
	AvailableFeatureCategories = ko.utils.sortBy(featureCategories, "Display");
	AvailablePlatforms = ko.utils.sortBy(platforms, "Display");

	AvailableFeatureTypeGroups.unshift({ Id: -1, Display: "Select feature group..." });
	AvailablePlatforms.unshift({ Id: -1, Display: "Select platform..." });

	var self = this;
	self.tab = ko.observable(tabs.FeatureBlueprints);
	self.saved = ko.observable();

	var ftvm = new FeatureBlueprintsTabViewModel(data.FeatureBlueprints, self);
	self.FeatureBlueprints = ftvm;

	self.uploadFile = function (element, name, vm) {
		if (!name)
			return;
		var file = vm[name];

		var css = vm[name + "Css"];
		var progressBar = vm[name + "Progress"];
		var filename = "";
		var options = {
			files: element.files,
			form: $(element).parents("form").first(),
			fileError: function (error) {
				alert(error);
				vm.Processing(false);
			},
			begin: function () {
				progressBar(0);
				vm.Processing(true);
			},
			start: function (fn) {
				filename = fn;
			},
			progress: function (loaded, total) {
				progressBar((loaded / total) * 100);
			},
			success: function (resource) {
				css("has-success");
				$.msgGrowl.success(filename + " uploaded.");
				progressBar(100);

				var rvm = ResourceViewModel.map(resource, filename);
				file(rvm);
			},
			error: function () {
				css("has-error");
			},
			complete: function () {
				vm.Processing(false);
			},
		};

		$.entitiesApi.uploadFile("resource", 0, options); // always a new file to be uploaded, so id = 0
	};
};

var FeatureBlueprintsTabViewModel = function (blueprints, administrationViewModel) {
	var self = this;
	self.AdministrationViewModel = administrationViewModel;
	var bps = [];
	ko.utils.arrayForEach(blueprints || [], function (bp) {
		var bpvm = FeatureBlueprintViewModel.map(self, bp);
		bps.push(bpvm);
	});
	self.Blueprints = ko.observableArray(bps).extend({ searchable: true });
	self.Blueprint = ko.observable();
	self.BlueprintPlatform = ko.observable();

	self.EditBlueprint = function (blueprint) {
		var fbvm = blueprint || new FeatureBlueprintViewModel(self);
		fbvm.PreviewCss("");
		fbvm.ConnectedTVCreativeCss("");
		fbvm.ConnectedTVSupportCss("");
		fbvm.PreviewProgress(0);
		fbvm.ConnectedTVCreativeProgress(0);
		fbvm.ConnectedTVSupportProgress(0);
		fbvm.Processing(false);
		self.Blueprint(fbvm);
		$("#modal-edit-feature-blueprint").modal("show");
	};
	self.FeatureBlueprintDetails = function (blueprint) {
		if (!blueprint)
			return;

		$(".tooltip").hide();
		self.Blueprint(blueprint);
		self.AdministrationViewModel.tab(tabs.FeatureBlueprintDetails);
	};
	self.ReturnToFeatureBlueprints = function () {
		$(".tooltip").hide();
		self.AdministrationViewModel.tab(tabs.FeatureBlueprints);
	};
	self.EditBlueprintPlatform = function (blueprintPlatform, blueprint) {
		var fbpvm = blueprintPlatform || new FeatureBlueprintPlatformViewModel(blueprint);
		fbpvm.CreativeCss("");
		fbpvm.CreativeProgress(0);
		fbpvm.Processing(false);
		self.BlueprintPlatform(fbpvm);
		$("#modal-edit-feature-blueprint-platform").modal("show");
	};
};

var FeatureBlueprintViewModel = function (parent) {
	var self = this;
	self.Id = ko.observable(0);
	self.Parent = parent;
	self.ManifestName = ko.observable().extend({ trackChange: true });
	self.GroupId = ko.observable();
	self.MajorVersion = ko.observable();
	self.MinorVersion = ko.observable();
	self.Patch = ko.observable();
	self.Name = ko.observable().extend({ trackChange: true });
	self.DateUpdated = ko.observable();
	self.IsDeleted = ko.observable().extend({ trackChange: true });
	self.BlueprintPlatforms = ko.observableArray().extend({ searchable: true });
	self.Processing = ko.observable(false);

	self.Preview = ko.observable().extend({ trackChange: true });
	self.ConnectedTVSupport = ko.observable().extend({ trackChange: true });
	self.ConnectedTVCreative = ko.observable().extend({ trackChange: true });
	self.PreviewProgress = ko.observable(0);
	self.ConnectedTVSupportProgress = ko.observable(0);
	self.ConnectedTVCreativeProgress = ko.observable(0);

	self.FeatureTypeGroup = ko.observable().extend({
		hasError: ko.hasError().select,
		trackChange: true,
		onChange: function () {
			self.FeatureType(ko.entities.empty);
			self.FeatureCategories([]);
		}
	});
	self.FeatureType = ko.observable().extend({ hasError: ko.hasError().select, trackChange: true, onChange: function () { self.FeatureCategories([]); } });
	self.AssociatedFeatureTypes = ko.associatedSelect(self, "FeatureTypeGroup", AvailableFeatureTypes, true);
	self.FeatureCategories = ko.observableArray().extend({
		hasError: ko.hasError().optional.multiSelect,
		trackChange: {
			tracker: function (ov, nv) {
				var o = ko.u(ov);
				var n = ko.u(nv);
				if (!Array.isArray(o) || !Array.isArray(n) || (!o && n) || (o && !n))
					return true;

				return o.length !== n.length; // ok to just check length, there shouldn't ever be a swap
			}
		}
	});
	self.AssociatedFeatureCategories = ko.dependentSelect(self, "FeatureType", AvailableFeatureCategories, "Categories");

	self.IsNewEntity = ko.entities.isNew(self);
	self.BlueprintPrefix = ko.deferred(self, function () {
		var sftg = this.FeatureTypeGroup();
		var sftgd = (!sftg || sftg.Id <= 0) ? "" : ko.u(sftg.Name);
		var sft = this.FeatureType();
		var sftd = (!sft || sft.Id <= 0) ? "" : "_" + sft.Name + " - ";
		return (sftgd + sftd);
	});
	self.Display = ko.deferred(self, function () {
		var sftg = ko.u(this.FeatureTypeGroup);
		var sftgd = (!sftg || sftg.Id <= 0) ? "" : ko.u(sftg.Display);
		var sft = ko.u(this.FeatureType);
		var sftd = (!sft || sft.Id <= 0) ? "" : "_" + ko.u(sft.Display);
		var n = ko.u(this.Name) || "";
		var d = (sftgd + sftd + " - " + n);
		if (d.length == 3)
			d = "Add new feature blueprint";

		return d;
	});
	self.HasErrors = ko.deferred(self, function () {
		var sftg = this.FeatureTypeGroup();
		var sft = this.FeatureType();
		//var a = this.ManifestName();
		var n = this.Name();

		return (!sftg || !sft || /*TODO: check app id for validity*/!n || sftg.Id <= 0 || sft.Id <= 0);
	});

	self.FeatureTypeGroupCss = ko.observableCss.select(self, "FeatureTypeGroup");
	self.FeatureTypeCss = ko.observableCss.select(self, "FeatureType");
	self.FeatureCategoriesCss = ko.observableCss.multiSelect(self, "FeatureCategories", true);
	self.ManifestNameCss = ko.observableCss.text(self, "ManifestName", true);
	self.BlueprintNameCss = ko.observableCss.text(self, "Name");
	self.PreviewCss = ko.observableCss.file(self, "Preview.Src", true);
	self.ConnectedTVCreativeCss = ko.observableCss.file(self, "ConnectedTVCreative.Src", true);
	self.ConnectedTVSupportCss = ko.observableCss.file(self, "ConnectedTVSupport.Src", true);

	self.Archive = function (b) {
		var id = ko.u(b.Id);
		var display = ko.u(b.Display);
		$.entitiesApi.archive("blueprint", id, function () {
			b.IsDeleted(true);
			b.IsDeleted.clean();
			$.msgGrowl.success(display + " archived.");
		}).fail(function () {
			$.msgGrowl.error("Error archiving " + display + ".");
		});
	};
	self.Restore = function (b) {
		var id = ko.u(b.Id);
		var display = ko.u(b.Display);
		$.entitiesApi.restore("blueprint", id, function () {
			b.IsDeleted(false);
			b.IsDeleted.clean();
			$.msgGrowl.success(display + " restored.");
		}).fail(function () {
			$.msgGrowl.error("Error restoring " + display + ".");
		});
	};
	self.Save = function () {
		self.Processing(true);
		var id = ko.u(self.Id);
		var isNew = ko.u(self.IsNewEntity);
		var display = ko.u(self.Display);

		var json = self.stringify();
		$.entitiesApi.save("blueprint", id, json, function (sbpId) {
			$("#modal-edit-feature-blueprint").modal("hide");
			$.msgGrowl.success(display + " saved.");
			if (isNew) {
				self.Id(sbpId);
				self.Parent.Blueprints.unshift(self);
			}

			self.clean();
		}).fail(function () {
			$.msgGrowl.error("Error saving " + display + ".");
		}).always(function () {
			self.Processing(false);
		});
	};

	self.reset = function () {
		self.ManifestName.reset();
		self.Name.reset();
		self.IsDeleted.reset();
		self.FeatureTypeGroup.reset();
		self.FeatureType.reset();
		self.FeatureCategories.reset();
		self.Preview.reset();
		self.ConnectedTVCreative.reset();
		self.ConnectedTVSupport.reset();
		$("#modal-edit-feature-blueprint").modal("hide");
	};
	self.clean = function () {
		self.ManifestName.clean();
		self.Name.clean();
		self.IsDeleted.clean();
		self.FeatureTypeGroup.clean();
		self.FeatureType.clean();
		self.FeatureCategories.clean();
		self.Preview.clean();
		self.ConnectedTVCreative.clean();
		self.ConnectedTVSupport.clean();
	};
	self.stringify = function () {
		var fbj = ko.entities.jsonId(this);
		fbj.GroupId = ko.u(this.GroupId);
		fbj.MajorVersion = ko.u(this.MajorVersion);
		fbj.MinorVersion = ko.u(this.MinorVersion);
		fbj.Patch = ko.u(this.Patch);
		fbj.ManifestName = ko.u(this.ManifestName);
		fbj.Name = ko.u(this.Name);
		fbj.Display = ko.u(this.Name);
		fbj.ShortDisplay = ko.u(this.Name);
		fbj.FeatureType = ko.entities.jsonId(this.FeatureType);
		fbj.FeatureCategories = [];
		ko.utils.arrayForEach(ko.u(self.FeatureCategories) || [],
			function (sfc) {
				var fc = ko.entities.jsonId(sfc);
				fbj.FeatureCategories.push(fc);
			});

		var pId = ko.entities.jsonId(this.Preview, true);
		if (pId)
			fbj.Preview = pId;
		var ctvcId = ko.entities.jsonId(this.ConnectedTVCreative, true);
		if (ctvcId)
			fbj.ConnectedTVCreative = ctvcId;
		var ctvsId = ko.entities.jsonId(this.ConnectedTVSupport, true);
		if (ctvsId)
			fbj.ConnectedTVSupport = ctvsId;

		var json = JSON.stringify(fbj);
		return json;
	};
};
FeatureBlueprintViewModel.map = function (parent, blueprint) {
	var ftMapping = function (options) {
		var ft = ko.entities.select(AvailableFeatureTypes, options.data);
		if (ko.entities.id(ft) <= 0)
			return ft;

		var ftg = ko.entities.select(AvailableFeatureTypeGroups, ft.FeatureTypeGroup);
		options.parent.FeatureTypeGroup(ftg);
		return ft;
	};
	var fcMapping = function (options) { return ko.entities.select(AvailableFeatureCategories, options.data); };
	var srcImageMapping = function (options) { return resourceMapping(options.data); };
	var srcSupportMapping = function (options) { return resourceMapping(options.data); };
	var srcCreativeMapping = function (options) { return resourceMapping(options.data); };
	var resourceMapping = function (data) {
		var rvm = new ResourceViewModel();
		if (!data)
			return null;
		var d = { Id: ko.u(data.Id) };
		rvm = ko.mapping.fromJS(d, {}, rvm);
		return rvm;
	};

	var blueprintsMapping =
	{
		"FeatureType": { create: ftMapping, update: ftMapping, key: ko.entities.idMapping },
		"FeatureCategories": { create: fcMapping, update: fcMapping, key: ko.entities.idMapping },
		"DateUpdated": { create: ko.entities.dateMapping, update: ko.entities.dateMapping },
		"Preview": { create: srcImageMapping, update: srcImageMapping, key: ko.entities.idMapping },
		"ConnectedTVSupport": { create: srcSupportMapping, update: srcSupportMapping, key: ko.entities.idMapping },
		"ConnectedTVCreative": { create: srcCreativeMapping, update: srcCreativeMapping, key: ko.entities.idMapping },
		"ignore": ["BlueprintPlatforms"],
	};
	var bpvm = new FeatureBlueprintViewModel(parent);
	bpvm = ko.mapping.fromJS(blueprint, blueprintsMapping, bpvm);
	ko.utils.arrayForEach(blueprint.BlueprintPlatforms || [], function (bpp) {
		var fbpvm = FeatureBlueprintPlatformViewModel.map(bpvm, bpp);
		bpvm.BlueprintPlatforms.push(fbpvm);
	});

	bpvm.clean();
	return bpvm;
};

var FeatureBlueprintPlatformViewModel = function (blueprint) {
	var self = this;
	self.Id = ko.observable(0);
	self.Blueprint = blueprint;
	self.Platform = ko.observable().extend({ trackChange: true });
	self.Status = ko.observable().extend({ trackChange: true });
	self.DateUpdated = ko.observable();
	self.IsDeleted = ko.observable().extend({ trackChange: true });
	self.Creative = ko.observable().extend({ trackChange: true });
	//.extend({
	//	trackChange: {
	//		tracker: function (ov, nv) {
	//			var o = ko.u(ov);
	//			var n = ko.u(nv);
	//			if ((!o && n) || (o && !n))
	//				return true;

	//			if (!o && !n)
	//				return false;

	//			var of = ko.u(o.Filename);
	//			var nf = ko.u(n.Filename);
	//			return of !== nf;
	//		}
	//	}
	//});
	self.CreativeProgress = ko.observable(0);
	self.Processing = ko.observable(false);

	self.Platforms = AvailablePlatforms;
	self.IsNewEntity = ko.entities.isNew(self);
	self.Display = ko.deferred(self, function () {
		var p = ko.u(self.Platform);
		if (!p)
			return "New...";

		var display = ko.u(p.Display);
		return display;
	});
	self.HasErrors = ko.deferred(function () {
		var p = ko.u(this.Platform);
		return (!p || ko.u(p.Id) <= 0);
	});

	self.StatusCss = ko.observableCss.radio(self, "Status");
	self.PlatformCss = ko.observableCss.radio(self, "Platform");
	self.CreativeCss = ko.observableCss.file(self, "Creative", true);

	self.Save = function () {
		self.Processing(true);
		var id = ko.u(self.Id);
		var p = ko.u(self.Platform);
		var display = ko.u(p.Display);
		var isNew = ko.u(self.IsNewEntity);

		var json = self.stringify();
		$.entitiesApi.save("blueprintplatform", id, json, function (bppId) {
			if (isNew) {
				self.Id(bppId);
				self.Blueprint.BlueprintPlatforms.unshift(self);
			}

			self.clean();
			$("#modal-edit-feature-blueprint-platform").modal("hide");
			$.msgGrowl.success(display + " saved.");
		}).fail(function () {
			$.msgGrowl.error("Error saving " + display + ".");
		}).always(function () {
			self.Processing(false);
		});
	};
	self.Archive = function () {
		var id = ko.u(self.Id);
		var display = ko.u(self.Display);
		$.entitiesApi.archive("blueprintplatform", id, function () {
			self.IsDeleted(true);
			self.IsDeleted.clean();
			$.msgGrowl.success(display + " archived.");
		}).fail(function () {
			$.msgGrowl.error("Error archiving " + display + ".");
		});
	};
	self.Restore = function () {
		var id = ko.u(self.Id);
		var p = ko.u(self.Platform);
		var display = ko.u(p.Display);
		$.entitiesApi.restore("blueprintplatform", id, function () {
			self.IsDeleted(false);
			self.IsDeleted.clean();
			$.msgGrowl.success(display + " restored.");
		}).fail(function () {
			$.msgGrowl.error("Error restoring " + display + ".");
		});
	};

	self.reset = function () {
		self.Platform.reset();
		self.Status.reset();
		self.Creative.reset();
		self.IsDeleted.reset();
		$("#modal-edit-feature-blueprint-platform").modal("hide");
	};
	self.clean = function () {
		self.Platform.clean();
		self.Status.clean();
		self.Creative.clean();
		self.IsDeleted.clean();
	};
	self.stringify = function () {
		var fbpj = ko.entities.jsonId(this);
		fbpj.Blueprint = ko.entities.jsonId(this.Blueprint);
		fbpj.Platform = ko.entities.jsonId(this.Platform);
		fbpj.Status = ko.u(self.Status);
		if (this.Creative) fbpj.Creative = ko.entities.jsonId(this.Creative);

		var json = JSON.stringify(fbpj);
		return json;
	};
};
FeatureBlueprintPlatformViewModel.map = function (blueprint, blueprintPlatform) {
	var pMapping = function (options) { return ko.entities.select(AvailablePlatforms, options.data); };
	var bpMapping = function (options) {
		var id = ko.u((options.data.BlueprintType)) ? options.data.Id : options.parent.Id;
		var name = (ko.u(options.data.BlueprintType)) ? options.data.Name : options.parent.Name;
		return { Id: ko.u(id), Name: ko.u(name) };
	};
	var srcCreativeMapping = function (options) {
		var rvm = new ResourceViewModel();
		var d = { Id: (!options.data) ? 0 : ko.u(options.data.Id) };
		rvm = ko.mapping.fromJS(d, {}, rvm);
		return rvm;
	};
	var blueprintPlatformMapping = {
		"Platform": { create: pMapping, update: pMapping, key: ko.entities.idMapping },
		"Blueprint": { create: bpMapping, update: bpMapping, key: ko.entities.idMapping },
		"Creative": { create: srcCreativeMapping, update: srcCreativeMapping, key: ko.entities.idMapping },
		"DateUpdated": { create: ko.entities.dateMapping, update: ko.entities.dateMapping },
	};
	var fbpvm = new FeatureBlueprintPlatformViewModel(blueprint);
	fbpvm = ko.mapping.fromJS(blueprintPlatform, blueprintPlatformMapping, fbpvm);

	fbpvm.clean();
	return fbpvm;
};

var ResourceViewModel = function () {
	var self = this;
	self.Id = ko.observable(0);
	self.Url = ko.observable();
	self.Filename = ko.observable("Choose file");

	self.IsNewEntity = ko.entities.isNew(self);
	self.Src = ko.immediate(self, function () {
		var id = this.Id();
		if (id == 0)
			return "";

		var u = this.Url();
		var url = $.entitiesApi.downloadUrl("resource", id);
		self.Url(url);
	});
};
ResourceViewModel.map = function (resource, filename) {
	var rvm = new ResourceViewModel();
	rvm.Id(resource.Id);
	var url = $.entitiesApi.downloadUrl("resource", resource.Id);
	rvm.Url(url);
	rvm.Filename(filename);

	return rvm;
};