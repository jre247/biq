
var EasternTimeZone = "Eastern Standard Time";
var AvailableTimeZones = [];
var AvailableRoles = [];
var EmployeeRole;
var AvailableAdvertisers = [];
var AdvertiserRoles = [];
var MediaAgencies = [];
var AvailableMediaAgencies = [];
var AgencyPartnerRole;
var ClientRole;
var AvailableMediaPartners = [];
var MediaPartners = [];
var MediaPartnerRole;
var rMapping = function (options) { return ko.entities.select(AvailableRoles, options.data); };
var uvmMapping = {
	"Roles": { create: rMapping, update: rMapping, key: ko.entities.idMapping }
};
var loadData = function (timeZones, callback) {
	AvailableTimeZones = timeZones.sortByProperty("DisplayName");
	var p = $.entitiesApi.all("role", "advertiser", "agency", "mediapartner");
	$.when(p[0], p[1], p[2], p[3])
		.done(function (rs, as, agencies, mediaPartners) {
			AvailableRoles = rs[0];

			AdvertiserRoles = AvailableRoles.filter(function (a) {
				return a.Id == 9 || a.Id == 10 || a.Id == 22;
			});
		
			AvailableAdvertisers = ko.utils.sortBy(as[0], "Display");
			AvailableAdvertisers.unshift({ Id: -1, Display: "Select advertiser..." });

			EmployeeRole = ko.utils.arrayFirst(AvailableRoles, function (ar) {
				return ar.Id == 3;
			});

			ClientRole = ko.utils.arrayFirst(AvailableRoles, function (ar) {
				return ar.Id == 9;
			});

			AgencyPartnerRole = ko.utils.arrayFirst(AvailableRoles, function (ar) {
				return ar.Id == 10; 
			});		
			AvailableMediaAgencies = ko.utils.sortBy(agencies[0], "Name");
			AvailableMediaAgencies.unshift({ Id: -1, Display: "Select Media Agency..." });

			AvailableMediaPartners = ko.utils.sortBy(mediaPartners[0], "Display");
			AvailableMediaPartners.unshift({ Id: -1, Display: "Select Media Partner..." });
			MediaPartnerRole = AvailableRoles.filter(function (a) {
				return a.Id == 22;
			});

			callback();
		});
};

var UsersViewModel = function (users) {
	var self = this;
	var us = [];
	ko.utils.arrayForEach(users || [], function (u) {
		var uvm = new UserViewModel();
		uvm = ko.mapping.fromJS(u, uvmMapping, uvm);
		uvm.clean();
		us.push(uvm);
	});;
	us = ko.utils.sortBy(us, "FirstName");
	self.Users = ko.observableArray(us);
	self.User = ko.observable();

	self.EditUser = function (user) {
		var uvm = user || new UserViewModel();
		self.User(uvm);
	};
	self.ArchiveUser = function (user) {
		var id = user.Id();
		var name = user.FullName();
		$.entitiesApi.archive("user", id, function () {
			$.msgGrowl.success(name + " archived.");
			user.IsDeleted(true);
		}).fail(function () {
			$.msgGrowl.error("Error archiving " + name + ".");
		});
	};
	self.RestoreUser = function (user) {
		var id = user.Id();
		var name = user.FullName();
		$.entitiesApi.restore("user", id, function () {
			$.msgGrowl.success(name + " restored.");
			user.IsDeleted(false);
		}).fail(function () {
			$.msgGrowl.error("Error restoring " + name + ".");
		});
	};
	self.ResendInvitation = function (user) {
		var id = user.Id();
		var name = user.FullName();
		$.entitiesApi.typed.get("users", "resendinvitation", id, function () {
			$.msgGrowl.success(name + " - invitation sent.");
		}).fail(function () {
			$.msgGrowl.error("Error sending " + name + " invitation.");
		});
	};
	self.UnlockUser = function (user) {
		var id = user.Id();
		var name = user.FullName();
		$.entitiesApi.typed.get("users", "unlock", id, function () {
			$.msgGrowl.success(name + " unlocked.");
			user.IsLocked(0);
		}).fail(function () {
			$.msgGrowl.error("Error unlocking " + name + ".");
		});
	};
	self.SaveUser = function () {
		var user = self.User();
		user.Processing(true);
		var id = ko.u(user.Id);
		var json = ko.toJSON(user);
		var name = user.FullName();
		$.entitiesApi.typed.post("users", "save", id, json, function (u) {
			$("#modal-edit-user").modal("hide");
			$.msgGrowl.success(name + " saved.");
			user = ko.mapping.fromJS(u, uvmMapping, user);
			if (id == 0) {
				user.AccountInvitations.push({ DateExpired: new Date("01/01/9999") }); // fake far in the future expiration for User.Status()
				self.Users.push(user);
			}
			user.clean();

		}).fail(function () {
			$.msgGrowl.error("Error saving " + name + ".");
		}).always(function () {
			user.Processing(false);
		});
	};

	self.Search = ko.observable();
};

var UserViewModel = function () {
	var self = this;
	var emailRegex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	self.Id = ko.observable(0);
	self.Email = ko.observable().extend({ trackChange: true });
	self.Internal = ko.observable().extend({ trackChange: true });
	self.FirstName = ko.observable().extend({ trackChange: true });
	self.LastName = ko.observable().extend({ trackChange: true });
	self.LastLoginDate = ko.observable();
	self.LastActivityDate = ko.observable();
	self.IsLocked = ko.observable().extend({ trackChange: true });
	self.IsActive = ko.observable(true).extend({ trackChange: true });
	self.AccountInvitations = ko.observableArray();
	self.TimeZoneId = ko.observable("Eastern Standard Time").extend({ trackChange: true });
	self.Processing = ko.observable(false);

	self.Roles = ko.observableArray().extend({ trackChange: true });
	self.Advertiser = ko.observable().extend({ trackChange: true });
	self.MediaAgency = ko.observable().extend({ trackChange: true });
	self.MediaPartner = ko.observable().extend({ trackChange: true });
	self.EmailAvailable = ko.observable(true);

	self.IsNewEntity = ko.computed(function () { return (ko.u(this.Id) == 0); }, this);
	self.FullName = ko.computed(function () { return ko.u(this.FirstName) + " " + ko.u(this.LastName); }, this);
	self.LastLoginDateSort = ko.deferred(self, function () {
		var d = ko.u(this.LastLoginDate);
		if (!d)
			return "";

		var m = moment(d);
		return m.format("YYDDDDHHMM");
	});
	self.LastActivityDateSort = ko.deferred(self, function () {
		var d = ko.u(this.LastActivityDate);
		if (!d)
			return "";

		var m = moment(d);
		return m.format("YYDDDDHHMM");
	});
	self.Status = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			if (ko.u(this.Id) == 0)
				return "New User - Active";

			var accountInvitations = (ko.utils.arrayFilter(ko.u(this.AccountInvitations) || [], function (ai) {
				try { // catch any date issues from moment
					var de = moment(ko.u(ai.DateExpired));
					return !ko.u(ai.DateActivated) && moment().isBefore(de);
				} catch (e) {
					return false;
				}
			}));

			var status = (accountInvitations.length > 0) ? "Invited" : (ko.u(this.IsActive) ? "Active" : "Inactive");
			return status;
		}
	});
	self.AllowInvite = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			var s = ko.u(this.Status);
			return s == 'Invited';
		}
	});
	self.EmailCss = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			var e = this.Email();
			var a = self.EmailAvailable();
			if (!this.IsNewEntity())
				return "has-success";
			if (!a)
				return "has-invalid";
			if (emailRegex.test(e))
				return "has-success";

			return "has-error";
		}
	});
	self.FirstNameCss = ko.observableCss.text(self, "FirstName");
	self.LastNameCss = ko.observableCss.text(self, "LastName");
	self.CheckEmailAvailable = function () {
		var email = self.Email();
		$.entitiesApi.typed.get("users", "emailavailable", email, function (a) {
			self.EmailAvailable(a);
		}).fail(function () {
			$.msgGrowl.error("Error checking email availability.");
		});
	};
	self.CheckRole = function (role) {
		if (!role)
			return true;
		
		var adRole = ko.utils.arrayFirst(AdvertiserRoles, function (ar) {
			return ar.Id == role.Id;
		});
		if (adRole) {
			self.Roles.removeAll();
			self.Roles.push(adRole);
		}
		else {
			self.Roles.removeAll(AdvertiserRoles);
			self.Roles.push(EmployeeRole);
		}

		return true;
	};
	self.IsAdvertiserRole = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			var isAdvertiser = this.Roles().contains(ClientRole);
			if (isAdvertiser) {
				return true;
			}

			self.Advertiser(null);
			return false;
		}
	});

	self.IsMediaAgencyRole = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			var isMediaAgency = this.Roles().contains(AgencyPartnerRole);
			if (isMediaAgency) {

				return true;
			}

			self.MediaAgency(null);
			return false;
		}
	});

	self.IsMediaPartnerRole = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			var isMediaPartner = this.Roles().contains(MediaPartnerRole);
			if (isMediaPartner) {
				return true;
			}
	
			self.MediaPartner(null);
			return false;
		}
	});

	self.HasErrors = ko.computed({
		deferEvaluation: true,
		owner: this,
		read: function () {
			
			var email = self.Email();
			var available = self.EmailAvailable();
			var first = self.FirstName();
			var last = self.LastName();
			var roles = self.Roles();
			var advertiser = self.Advertiser();
			var mediaAgency = self.MediaAgency();
			var mediaPartner = self.MediaPartner();

			if (!email || email.length == 0 || !emailRegex.test(email) || !available)
				return true;
			if (!first || first.length == 0)
				return true;
			if (!last || last.length == 0)
				return true;
			if (!roles || roles.length == 0)
				return true;

			var isAgencyPartnerRoleSelected = _.some(roles, function (item) {
				return item.Id === 9;
			});
			if (isAgencyPartnerRoleSelected && advertiser == -1)
				return true;

			var isMediaAgencyRoleSelected = _.some(roles, function (item) {
				return item.Id === 10;
			});
			if (isMediaAgencyRoleSelected && mediaAgency === -1)
				return true;

			var isMediaPartnerRoleSelected = _.some(roles, function (item) {
				return item.Id === 22;
			});
			if (isMediaPartnerRoleSelected && mediaPartner === -1)
				return true;

			return false;
		}
	});

	self.clean = function () {
		self.Email.clean();
		self.FirstName.clean();
		self.LastName.clean();
		self.IsLocked.clean();
		self.IsActive.clean();
		self.TimeZoneId.clean();
		self.Roles.clean();
		self.Advertiser.clean();
		self.MediaAgency.clean();
		self.MediaPartner.clean();
		self.Internal.clean();
	};

	self.reset = function () {
		self.Email.reset();
		self.FirstName.reset();
		self.LastName.reset();
		self.IsLocked.reset();
		self.IsActive.reset();
		self.TimeZoneId.reset();
		self.Roles.reset();
		self.Advertiser.reset();
		self.MediaAgency.reset();
		self.MediaPartner.reset();
		self.Internal.reset();
		$("#modal-edit-user").modal("hide");
	};
};