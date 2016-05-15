ko.u = ko.utils.unwrapObservable;
ko.immediate = function (self, read, options) {
	var defaults = {
		deferEvaluation: false,
		owner: self,
		read: read || function () { },
		write: function () { },
	};

	var opts = $.extend({}, defaults, options);
	return ko.computed(opts).extend({ throttle: 100 });
};
ko.deferred = function (self, read, options) {
	var defaults = {
		deferEvaluation: true,
		owner: self,
		read: read || function () { },
		write: function () { },
	};

	var opts = $.extend({}, defaults, options);
	return ko.computed(opts).extend({ throttle: 100 });
};


ko.bindingHandlers.href = {
	init: function (element, valueAccessor, allBindings, data, context) {
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var href = ko.u(va);
		ko.applyBindingsToNode(element, { attr: { "href": href } });
	}
};

ko.bindingHandlers.src = {
	init: function (element, valueAccessor, allBindings, data, context) {
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var src = ko.u(va);
		ko.applyBindingsToNode(element, { attr: { "src": src } });
	}
};

ko.bindingHandlers.date = {
	init: function (element, valueAccessor, allBindings, data, context) {
		var dateFormat = allBindings.get("dateFormat") || "MM/DD/YY hh:mm A";
		var u = valueAccessor();
		var interceptor = ko.dependentObservable({
			read: function () {
				try {
					var o = ko.u(u);
					var date = (!o) ? null : moment(o).format(dateFormat);
					return date;
				} catch (e) {
					console.log(e);
					return null;
				}
			},
			write: function (value) {
				try {
					var date = (!value) ? null : moment(value).format(dateFormat);
					u(date);
				} catch (e) {
					console.log(e);
					u(null);
				}
			},
			disposeWhenNodeIsRemoved: element
		});
		//ko.bindingHandlers.value.init(element, interceptor, allBindings);
		if (element.tagName == "INPUT")
			ko.applyBindingsToNode(element, { value: interceptor, valueUpdate: "input" });
		else
			ko.applyBindingsToNode(element, { text: interceptor });
	},
	update: function (element, valueAccessor, allBindings, data, context) {
	}
};

ko.bindingHandlers.numeric = {
	init: function (element, valueAccessor, allBindings, data, context) {
		$(element).on("keydown", function (event) {
			// Allow: backspace, delete, tab, escape, and enter
			if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
				// Allow: Ctrl+A, Ctrl+C, Ctrl+V
                (event.keyCode == 65 && event.ctrlKey === true) || (event.keyCode == 67 && event.ctrlKey === true) || (event.keyCode == 86 && event.ctrlKey === true) ||
				// Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
				return;
			} else if (event.keyCode == 188 || event.keyCode == 190) { // only allow 1 dot
				var v = $(this).val();
				if (v.indexfOf(".") >= 0)
					e.preventDefault();
			}
			else { // Ensure that it is a number and stop the keypress
				if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
					event.preventDefault();
				}
			}
		});
		var u = valueAccessor();
		var interceptor = ko.dependentObservable({
			read: function () {
				var o = ko.u(u);
				return commafy(o);
			},
			write: function (value) {
				var v = uncommafy(value);
				if (isNaN(v))
					return;

				u(v);
			},
			disposeWhenNodeIsRemoved: element
		});
		//ko.bindingHandlers.value.init(element, interceptor, allBindings);
		if (element.tagName == "INPUT")
			ko.applyBindingsToNode(element, { value: interceptor, valueUpdate: "input" });
		else
			ko.applyBindingsToNode(element, { text: interceptor });
	},
	update: function (element, valueAccessor, allBindings, data, context) {

	}
};

ko.bindingHandlers.details = {
	init: function (element, valueAccessor, allBindings, data, context) {
		var options = valueAccessor();
		var details = allBindings.get("click") || function () {
			return function () {
				if (typeof (data.Details) == "function")
					data.Details.call(data, data, context);
			};
		};
		ko.bindingHandlers.click.init(element, details, allBindings, data, context);
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var display = ko.u(va);
		ko.applyBindingsToNode(element, { attr: { title: display + "\ndetails" } });
	}
};

ko.bindingHandlers.edit = {
	init: function (element, valueAccessor, allBindings, data, context) {
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var display = ko.u(va);
		var enable = allBindings.get("enable") || function () {
			return !ko.u(data.IsDeleted);
		};
		ko.bindingHandlers.enable.update(element, enable);
		ko.applyBindingsToNode(element, { attr: { title: "Edit\n" + display } });
	}
};

ko.bindingHandlers.archive = {
	init: function (element, valueAccessor, allBindings, data, context) {
		var options = valueAccessor();
		var archive = allBindings.get("click") || function () {
			return function () {
				if (typeof (data.Archive) == "function")
					data.Archive.call(data, data, context);
			};
		};
		ko.bindingHandlers.click.init(element, archive, allBindings, data, context);
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var display = ko.u(va);
		var visible = allBindings.get("visible") || function () {
			return !ko.u(data.IsDeleted);
		};
		ko.bindingHandlers.visible.update(element, visible);
		ko.applyBindingsToNode(element, { attr: { title: "Archive\n" + display } });
	}
};

ko.bindingHandlers.delete = {
	init: function (element, valueAccessor, allBindings, data, context) {
		var options = valueAccessor();
		var destroy = allBindings.get("click") || function () {
			return function () {
				if (typeof (data.Delete) == "function")
					data.Delete.call(data, data, context);
			};
		};
		ko.bindingHandlers.click.init(element, destroy, allBindings, data, context);
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var display = ko.u(va);
		var visible = allBindings.get("visible") || function () {
			return !ko.u(data.IsDeleted);
		};
		ko.bindingHandlers.visible.update(element, visible);
		ko.applyBindingsToNode(element, { attr: { title: "Delete\n" + display } });
	}
};

ko.bindingHandlers.restore = {
	init: function (element, valueAccessor, allBindings, data, context) {
		var options = valueAccessor();
		var restore = allBindings.get("click") || function () {
			return function () {
				if (typeof (data.Restore) == "function")
					data.Restore.call(data, data, context);
			};
		};
		ko.bindingHandlers.click.init(element, restore, allBindings, data, context);
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var display = ko.u(va);
		var visible = allBindings.get("visible") || function () {
			return ko.u(data.IsDeleted);
		};
		ko.bindingHandlers.visible.update(element, visible);
		ko.applyBindingsToNode(element, { attr: { title: "Restore\n" + display } });
	}
};

ko.bindingHandlers.title = {
	init: function (element, valueAccessor, allBindings, data, context) {
	},
	update: function (element, valueAccessor, allBindings, data, context) {
		var va = valueAccessor();
		var title = ko.u(va);
		ko.applyBindingsToNode(element, { attr: { title: title } });
		$(element).tooltip({ container: "body", placement: "bottom", html: true });
	}
};


ko.utils.isFunction = function (test) {
	var isFunction = test && typeof (test) === typeof (function () { }) && !ko.isObservable(test);
	return isFunction;
};


ko.utils.sortBy = function (array, property) {
	var a = ko.u(array);
	if (!a || !Array.isArray(a) || !property)
		return array;

	var sorted = a.sort(function (left, right) {
		var l = ko.u(left[property]);
		var r = ko.u(right[property]);
		if (l == r)
			return 0;
		if (l < r)
			return -1;

		return 1;
	});
	return sorted;
};


ko.utils.filterTable = function (instance, term, properties) {
	if (!instance)
		return false;
	if (!term)
		return true;

	var t = ko.u(term);
	if (!t)
		return true;

	t = t.toString();
	var ps = [];
	if (properties && Array.isArray(properties))
		ps = properties;
	else if (typeof (properties) == typeof (""))
		ps = properties.split(",");
	else if (typeof (properties) !== "undefined" && properties !== null)
		ps = [properties];

	for (var i = 0; i < ps.length; i++) {
		ps[i] = ps[i].trim();
	}
	var found = false;
	if (ps.length == 0) {
		for (var i in instance) {
			var search = ko.utils.unwrapNestedObservable(instance, i);
			if (typeof (search) === "undefined" || search === null)
				continue;

			if (search.toString().contains(t, true)) {
				found = true;
				break;
			}
		}
	} else {
		for (var i in instance) {
			for (var j = 0; j < ps.length; j++) {
				var p = ps[j];
				var search = ko.utils.unwrapNestedObservable(instance, p);
				if (typeof (search) === "undefined" || search === null)
					continue;

				if (search.toString().contains(t, true)) {
					found = true;
					break;
				}
			}
			if (found)
				break;
		}
	}

	return found;
};


ko.utils.propertyJoin = function (array, property, separator) {
	var self = ko.u(array);
	var pa = [];
	for (i in self) {
		if (!self[i])
			continue;

		//if (!ko.isObservable(self[i]))
		//	continue;

		var o = ko.u(self[i]);
		if (!Array.isArray(o))
			o = [o];

		for (io in o) {
			var oio = ko.u(o[io]);
			var p = ko.u(oio[property]);
			if (p)
				pa.push(p);
		}
	}

	return pa.join(separator);
};


ko.utils.clone = function (src) {

	function mixin(dest, source, copyFunc) {
		var name, s, i, empty = {};
		for (name in source) {
			// the (!(name in empty) || empty[name] !== s) condition avoids copying properties in "source"
			// inherited from Object.prototype.	 For example, if dest has a custom toString() method,
			// don't overwrite it with the toString() method that source inherited from Object.prototype
			s = source[name];
			if (!(name in dest) || (dest[name] !== s && (!(name in empty) || empty[name] !== s))) {
				dest[name] = copyFunc ? copyFunc(s) : s;
			}
		}
		return dest;
	}

	if (!src || typeof src != "object" || Object.prototype.toString.call(src) === "[object Function]") {
		// null, undefined, any non-object, or function
		return src; // anything
	}
	if (src.nodeType && "cloneNode" in src) {
		// DOM Node
		return src.cloneNode(true); // Node
	}
	if (src instanceof Date) {
		// Date
		return new Date(src.getTime()); // Date
	}
	if (src instanceof RegExp) {
		// RegExp
		return new RegExp(src); // RegExp
	}
	var r, i, l;
	if (src instanceof Array) {
		// array
		r = [];
		for (i = 0, l = src.length; i < l; ++i) {
			if (i in src) {
				r.push(clone(src[i]));
			}
		}
		// we don't clone functions for performance reasons
		//		}else if(d.isFunction(src)){
		//			// function
		//			r = function(){ return src.apply(this, arguments); };
	} else {
		// generic objects
		r = src.constructor ? new src.constructor() : {};
	}
	return mixin(r, src, clone);
};


ko.utils.findNestedObservable = function (self, path) {
	var value;
	if (!path)
		return value; // undefined at present

	var paths = path.split(".");
	if (paths.length == 1)
		value = self[path];
	else if (path.length > 1) {
		var o = self;
		for (var i = 0; i < paths.length && (!(o === null || typeof (o) === "undefined")) ; i++) {
			var p = paths[i];
			value = o[p];
			o = ko.u(value);
		}
	}

	return value;
};


ko.utils.unwrapNestedObservable = function (self, path) {
	var value = ko.utils.findNestedObservable(self, path);
	return ko.u(value);
};


ko.utils.select = function (available, item, comparer) {
	var c = comparer || function (l, r) { return l == r; };
	var av = ko.u(available);
	if (!Array.isArray(av))
		return c(item, av);

	var first = ko.utils.arrayFirst(av, function (a) {
		return c(item, a);
	});
	return first;
};


ko.utils.selectMany = function (available, items, comparer, property) {
	var av = ko.u(available);
	var its = ko.u(items);
	if (!Array.isArray(av))
		av = [av];
	if (!Array.isArray(its))
		its = [its];

	if (typeof (comparer) !== "function") comparer = null;
	var compare = comparer || function (l, r) { return l == r; };
	var es = [];
	for (var i = 0; i < av.length; i++) {
		var a = av[i];
		for (var j = 0; j < its.length; j++) {
			var it = its[j];
			if (compare(a, it)) {
				if (typeof (property) === typeof ("") && a[property])
					a = a[property];

				es.push(a);
			}
		}
	}

	return es;
};


//based on http://stackoverflow.com/questions/10622707/detecting-change-to-knockout-view-model
ko.extenders.trackChange = function (target, options) {
	if (options) {
		// if tracker is a function, use it for comparison. if it is not, just compare old and new values.
		var c = ko.utils.isFunction(options.tracker) ? options.tracker : function (ov, nv) { return (nv != ov) || !(nv >= ov && nv <= ov); };
		var r = ko.utils.isFunction(options.reverter) ? options.reverter : function (ov) { return ov; };
		target.isDirty = ko.observable(false);
		var cv = ko.u(target);
		target.cleanValue = cv;
		if (Array.isArray(cv)) target.cleanArrayValue = clone(cv);
		target.subscribe(function (newValue) {
			var ov = (Array.isArray(cv) ? target.cleanArrayValue : target.cleanValue);
			var dirty = c(ov, newValue);
			target.isDirty(dirty);
		});

		target.clean = function () {
			var cv = ko.u(target);
			target.cleanValue = cv;
			if (Array.isArray(cv)) target.cleanArrayValue = clone(cv);
			target.isDirty(false);
		};
		target.reset = function (newValue) {
			var rv = newValue || (Array.isArray(cv) ? target.cleanArrayValue : target.cleanValue) || "";
			target(r(rv));
			target.isDirty(false);
		};
	}
	return target;
};


ko.extenders.hasError = function (target, predicate) {
	if (predicate && ko.utils.isFunction(predicate)) {
		var iv = target();
		var init = predicate(iv);
		target.hasError = ko.observable(init);
		target.subscribe(function (newValue) {
			var he = predicate(newValue);
			target.hasError(he);
		});
		target.checkError = function () {
			var c = target();
			var ce = predicate(c);
			target.hasError(ce);
			return target.hasError();
		};
	}
	return target;
};


ko.extenders.onChange = function (target, callback) {
	if (callback && ko.utils.isFunction(callback)) {
		target.lastValue = target();
		target.subscribe(function (newValue) {
			if (callback(newValue) === false)
				target(target.lastValue);
			else
				target.lastValue = target();
		});
	}
	return target;
};


ko.extenders.logChange = function (target, property) {
	target.lastValue = target();
	target.subscribe(function (newValue) {
		if (console && console.log && ko.utils.isFunction(console.log)) {
			console.log(property + ": " + JSON.stringify(target.lastValue));
			console.log(" ----> " + JSON.stringify(newValue));
		}
		target.lastValue = target();
	});
	return target;
};


ko.extenders.navigate = function (target, tabs) {
	var keys = Object.keys(tabs) || [];
	var first = target() || tabs[keys[0]];
	target.subscribe(function (tab) {
		window.location.hash = tab ? tab.Hash : first;
		console.log("subscribe: " + window.location.hash);
	});

	$(window).on("hashchange", function (e) {
		var t = ko.utils.arrayFirst(keys, function (k) {
			return (tabs[k].Hash == window.location.hash);
		});
		console.log("hashchange: " + window.location.hash);
		target((t && tabs[t]) ? tabs[t] : first);
	});
};


ko.extenders.searchable = function (target, options) {
	target.Search = ko.observable();
	target.Search.clear = function () {
		target.Search("");
	};

	return target;
};


ko.isDirty = ko.computed({
	deferEvaluation: false,
	write: function () { },
	read: function () {
		if (self === window) return false;
		for (key in self) {
			if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && ko.utils.isFunction(self[key].isDirty)) {
				return self[key].isDirty();
			}
		}
		return false;
	}
});


ko.hasError = function () {
	var he = ko.computed({
		deferEvaluation: false,
		owner: this,
		write: function () { },
		read: function () {
			if (self === window) return false;
			for (key in self) {
				if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && ko.utils.isFunction(self[key].hasError)) {
					return self[key].hasError();
				}
			}
			return false;
		}
	});
	he.text = function (nv) { return String.isNullOrEmpty(nv); };
	he.date = function (nv) { return String.isNullOrEmpty(nv) || !moment(nv).isValid(); };
	he.numeric = function (nv) { return String.isNullOrEmpty(nv) || isNaN(nv); };
	he.select = function (nv) { return !nv; };
	he.multiSelect = function (nv) { return !nv; };
	he.radio = function (nv) { return !nv; };
	he.file = function (nv) { return String.isNullOrEmpty(nv); };
	he.optional = {
		text: function (nv) { return false; },
		date: function (nv) { return !String.isNullOrEmpty(nv) && !moment(nv).isValid(); },
		numeric: function (nv) { return !String.isNullOrEmpty(nv) && isNaN(nv); },
		select: function (nv) { return false; },
		multiSelect: function (nv) { return false; },
		radio: function (nv) { return false; },
		file: function (nv) { return true; }
	};
	return he;
};


ko.entities = {
	empty: { Id: -2, Display: "", Name: "" },
	id: function (self) {
		if (!self) return 0;
		var s = ko.u(self);
		if (!s) return 0;

		var id = ko.u(s.Id);
		if (!id && typeof (id) !== typeof (number)) return 0;

		return id;
	},
	isNew: function (self) {
		var is = ko.immediate(self, function () {
			var s = ko.u(self);
			if (!s) return;

			var id = ko.u(s.Id);
			return (id == 0);
		});
		return is;
	},
	toEntity: function (e, allowId) {
		if (allowId && !isNaN(e))
			return { Id: e };

		if (!e)
			return;

		e = ko.mapping.toJS(e);
		if (e && typeof (e.Id) === typeof (0))
			return e;

		return;
	},
	jsonId: function (self, allowNull) {
		var id = ko.entities.id(self);
		if (id == 0 && allowNull)
			return null;
		return { Id: id };
	},
	idMapping: function (data) { return ko.u(data.id); },
	dateMapping: function (options) {
		var d = ko.u(options.data);
		var m;
		if (d) {
			try {
				m = moment(d).format("MM/DD/YY hh:mm A");
			} catch (e) { }
		}
		return ko.observable(m);
	},
	distinct: function (entities, property) {
		var es = [];
		if (!Array.isArray(entities))
			entities = [entities];

		var p = (property || "Id").toString();
		var contains = function (v1, v2) {
			var l = ko.entities.toEntity(v1);
			var r = ko.entities.toEntity(v2);
			return (l && r) && (l[p] == r[p]);
		};
		ko.utils.arrayForEach(entities, function (e) {
			if (!es.contains(e, contains))
				es.push(e);
		});
		return es;
	},
	select: function (available, entity, comparer, property) {
		var e = ko.entities.toEntity(entity, true);
		if (!e) return;

		if (!Array.isArray(available))
			available = [available];

		var compare = comparer || function (l, r) {
			var el = ko.entities.toEntity(l);
			var er = ko.entities.toEntity(r);
			return ((el && er) && (el.Id == er.Id));
		};

		var first;
		for (var i = 0; i < available.length; i++) {
			var ae = available[i];
			if (compare(e, ae)) {
				first = ae;
				break;
			}
		}

		if (first && typeof (property) === typeof ("") && first[property])
			first = first[property];

		return first;
	},
	selectMany: function (available, entities, comparer, property) {
		if (!Array.isArray(available))
			available = [available];
		if (!Array.isArray(entities))
			entities = [entities];

		var compare = comparer || function (l, r) {
			var el = ko.entities.toEntity(l);
			var er = ko.entities.toEntity(r);
			return ((el && er) && (el.Id == er.Id));
		};

		var es = [];
		for (var i = 0; i < available.length; i++) {
			var a = available[i];
			for (var j = 0; j < entities.length; j++) {
				var e = entities[j];
				if (compare(a, e)) {
					if (typeof (property) === typeof ("") && a[property])
						a = a[property];

					es.push(a);
				}
			}
		}

		return es;
	},
	contains: function (entities, entity) {
		if (!entities) return false;
		if (!entity) return false;
		if (!Array.isArray(entities))
			entities = [entities];

		var eId = ko.entities.id(entity);
		for (var i = 0; i < entities.length; i++) {
			var ceId = ko.entities.id(entities[i]);
			if (ceId === eId)
				return true;
		}

		return false;
	},
};


ko.observableCss = {
	text: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = (ko.u(o) || "").toString().trim();
				var css;
				if (optional)
					css = (v.length == 0) ? "has-warning" : "has-success";
				else
					css = (v.length == 0) ? "has-error" : "has-success";

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o.written)
					o.written = ko.observable();

				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100 });
	},
	numeric: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = (ko.u(o) || "").toString().trim();
				var s = uncommafy(v);
				var css;
				if (isNaN(s))
					css = "has-error";
				else {
					if (optional)
						css = (s.length == 0) ? "has-warning" : "has-success";
					else
						css = (s == 0 || s.length == 0) ? "has-error" : "has-success";
				}

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o.written)
					o.written = ko.observable();

				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100 });
	},
	date: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = (ko.u(o) || "").toString().trim();
				var css;
				var valid = moment(v).isValid();
				if (valid)
					css = "has-success";
				else {
					if (optional)
						css = (v.length > 0) ? "has-error" : "has-warning";
					else
						css = "has-error";
				}

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o.written)
					o.written = ko.observable();

				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100, notify: "always" });
	},
	select: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = ko.u(o);
				var css;
				if (optional)
					css = (ko.entities.id(v, true) <= 0) ? "has-warning" : "has-success";
				else
					css = (ko.entities.id(v, true) <= 0) ? "has-error" : "has-success";

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o.written)
					o.written = ko.observable();
				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100 });
	},
	radio: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = ko.u(o);
				var css;
				if (optional)
					css = (ko.entities.id(v, true) <= 0) ? "has-warning" : "has-success";
				else
					css = (ko.entities.id(v, true) <= 0) ? "has-error" : "has-success";

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100 });
	},
	multiSelect: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = ko.u(o);
				var css;
				if (optional)
					css = (!v || v.length == 0) ? "has-warning" : "has-success";
				else
					css = (!v || v.length == 0) ? "has-error" : "has-success";

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o.written)
					o.written = ko.observable();
				o.written(nv);
			}
		};

		return ko.computed(options).extend({ throttle: 100 });
	},
	file: function (self, path, optional) {
		var no = ko.utils.findNestedObservable(self, path);
		if (no)
			no.written = ko.observable();
		var options = {
			deferEvaluation: true,
			owner: self,
			read: function () {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o || !ko.isObservable(o))
					return "";

				if (!o.written)
					o.written = ko.observable();
				var w = o.written();
				o.written(null);
				if (w)
					return w;

				var v = (ko.u(o) || "").toString().trim();
				var css;
				if (optional)
					css = /*(v.length == 0) ? "has-warning" :*/ "has-success";
				else
					css = (v.length == 0) ? "has-error" : "has-success";

				return css;
			},
			write: function (nv) {
				var o = ko.utils.findNestedObservable(self, path);
				if (!o)
					return;
				if (!o.written)
					o.written = ko.observable();
				o.written(nv);
			}
		};

		return ko.computed(options).extend({ notify: "always" });
	},
};


ko.associatedSelect = function (self, dependency, available, showSelect) {
	var params = {
		deferEvaluation: true,
		owner: self,
		write: function () { },
		read: function () {
			var d = this[dependency];
			var dvId = ko.entities.id(d);
			if (!dvId)
				return [];

			var ds = (ko.utils.arrayFilter(available, function (ad) {
				if (!ad[dependency])
					return false;
				var adId = ko.u(ad[dependency].Id);
				return (adId == dvId);
			}) || []);

			ds = ko.utils.sortBy(ds, "Display");
			ds = ko.utils.sortBy(ds, "Name");
			if (showSelect) {
				var display = "Select" + (typeof (showSelect) === typeof ("") ? " " + showSelect : "") + "...";
				ds.unshift({ Id: -1, Display: display, Name: display });
			}
			return ds;
		}
	};

	return ko.computed(params);
};


ko.dependentSelect = function (self, dependency, available, property, showSelect) {
	var params = {
		deferEvaluation: true,
		owner: self,
		write: function () { },
		read: function () {
			var d = this[dependency];
			if (!ko.isObservable(d))
				return [];

			var dv = ko.u(d);
			if (!dv || !dv[property] || dv[property].length == 0)
				return [];

			var ds = (ko.utils.arrayFilter(available, function (ad) {
				var adId = ko.u(ad.Id);
				var sd = ko.utils.arrayFirst(dv[property], function (a) {
					var aId = ko.u(a.Id);
					return (aId == adId);
				});
				return (sd != null);
			}) || []);

			ds = ko.utils.sortBy(ds, "Display");
			ds = ko.utils.sortBy(ds, "Name");
			if (showSelect) {
				var display = "Select" + (typeof (showSelect) === typeof ("") ? " " + showSelect : "") + "...";
				ds.unshift({ Id: -1, Display: display, Name: display });
			}
			return ds || [];
		}
	};

	return ko.computed(params);
};


/// Knockout Mapping plugin v2.4.1
/// (c) 2013 Steven Sanderson, Roy Jacobs - http://knockoutjs.com/
/// License: MIT (http://www.opensource.org/licenses/mit-license.php)
(function (factory) {
	// Module systems magic dance.

	if (typeof require === "function" && typeof exports === "object" && typeof module === "object") {
		// CommonJS or Node: hard-coded dependency on "knockout"
		factory(require("knockout"), exports);
	} else if (typeof define === "function" && define["amd"]) {
		// AMD anonymous module with hard-coded dependency on "knockout"
		define(["knockout", "exports"], factory);
	} else {
		// <script> tag: use the global `ko` object, attaching a `mapping` property
		factory(ko, ko.mapping = {});
	}
}(function (ko, exports) {
	var DEBUG = true;
	var mappingProperty = "__ko_mapping__";
	var realKoDependentObservable = ko.dependentObservable;
	var mappingNesting = 0;
	var dependentObservables;
	var visitedObjects;
	var recognizedRootProperties = ["create", "update", "key", "arrayChanged"];
	var emptyReturn = {};

	var _defaultOptions = {
		include: ["_destroy"],
		ignore: [],
		copy: [],
		observe: []
	};
	var defaultOptions = _defaultOptions;

	function unionArrays() {
		var args = arguments,
		l = args.length,
		obj = {},
		res = [],
		i, j, k;

		while (l--) {
			k = args[l];
			i = k.length;

			while (i--) {
				j = k[i];
				if (!obj[j]) {
					obj[j] = 1;
					res.push(j);
				}
			}
		}

		return res;
	}

	function extendObject(destination, source) {
		var destType;

		for (var key in source) {
			if (source.hasOwnProperty(key) && source[key]) {
				destType = exports.getType(destination[key]);
				if (key && destination[key] && destType !== "array" && destType !== "string") {
					extendObject(destination[key], source[key]);
				} else {
					var bothArrays = exports.getType(destination[key]) === "array" && exports.getType(source[key]) === "array";
					if (bothArrays) {
						destination[key] = unionArrays(destination[key], source[key]);
					} else {
						destination[key] = source[key];
					}
				}
			}
		}
	}

	function merge(obj1, obj2) {
		var merged = {};
		extendObject(merged, obj1);
		extendObject(merged, obj2);

		return merged;
	}

	exports.isMapped = function (viewModel) {
		var unwrapped = ko.utils.unwrapObservable(viewModel);
		return unwrapped && unwrapped[mappingProperty];
	}

	exports.fromJS = function (jsObject /*, inputOptions, target*/) {
		if (arguments.length == 0) throw new Error("When calling ko.fromJS, pass the object you want to convert.");

		try {
			if (!mappingNesting++) {
				dependentObservables = [];
				visitedObjects = new objectLookup();
			}

			var options;
			var target;

			if (arguments.length == 2) {
				if (arguments[1][mappingProperty]) {
					target = arguments[1];
				} else {
					options = arguments[1];
				}
			}
			if (arguments.length == 3) {
				options = arguments[1];
				target = arguments[2];
			}

			if (target) {
				options = merge(options, target[mappingProperty]);
			}
			options = fillOptions(options);

			var result = updateViewModel(target, jsObject, options);
			if (target) {
				result = target;
			}

			// Evaluate any dependent observables that were proxied.
			// Do this after the model's observables have been created
			if (!--mappingNesting) {
				while (dependentObservables.length) {
					var DO = dependentObservables.pop();
					if (DO) {
						DO();

						// Move this magic property to the underlying dependent observable
						DO.__DO["throttleEvaluation"] = DO["throttleEvaluation"];
					}
				}
			}

			// Save any new mapping options in the view model, so that updateFromJS can use them later.
			result[mappingProperty] = merge(result[mappingProperty], options);

			return result;
		} catch (e) {
			mappingNesting = 0;
			throw e;
		}
	};

	exports.fromJSON = function (jsonString /*, options, target*/) {
		var parsed = ko.utils.parseJson(jsonString);
		arguments[0] = parsed;
		return exports.fromJS.apply(this, arguments);
	};

	exports.updateFromJS = function (viewModel) {
		throw new Error("ko.mapping.updateFromJS, use ko.mapping.fromJS instead. Please note that the order of parameters is different!");
	};

	exports.updateFromJSON = function (viewModel) {
		throw new Error("ko.mapping.updateFromJSON, use ko.mapping.fromJSON instead. Please note that the order of parameters is different!");
	};

	exports.toJS = function (rootObject, options) {
		if (!defaultOptions) exports.resetDefaultOptions();

		if (arguments.length == 0) throw new Error("When calling ko.mapping.toJS, pass the object you want to convert.");
		if (exports.getType(defaultOptions.ignore) !== "array") throw new Error("ko.mapping.defaultOptions().ignore should be an array.");
		if (exports.getType(defaultOptions.include) !== "array") throw new Error("ko.mapping.defaultOptions().include should be an array.");
		if (exports.getType(defaultOptions.copy) !== "array") throw new Error("ko.mapping.defaultOptions().copy should be an array.");

		// Merge in the options used in fromJS
		options = fillOptions(options, rootObject[mappingProperty]);

		// We just unwrap everything at every level in the object graph
		return exports.visitModel(rootObject, function (x) {
			return ko.utils.unwrapObservable(x)
		}, options);
	};

	exports.toJSON = function (rootObject, options) {
		var plainJavaScriptObject = exports.toJS(rootObject, options);
		return ko.utils.stringifyJson(plainJavaScriptObject);
	};

	exports.defaultOptions = function () {
		if (arguments.length > 0) {
			defaultOptions = arguments[0];
		} else {
			return defaultOptions;
		}
	};

	exports.resetDefaultOptions = function () {
		defaultOptions = {
			include: _defaultOptions.include.slice(0),
			ignore: _defaultOptions.ignore.slice(0),
			copy: _defaultOptions.copy.slice(0),
			observe: _defaultOptions.observe.slice(0)
		};
	};

	exports.getType = function (x) {
		if ((x) && (typeof (x) === "object")) {
			if (x.constructor === Date) return "date";
			if (x.constructor === Array) return "array";
		}
		return typeof x;
	}

	function fillOptions(rawOptions, otherOptions) {
		var options = merge({}, rawOptions);

		// Move recognized root-level properties into a root namespace
		for (var i = recognizedRootProperties.length - 1; i >= 0; i--) {
			var property = recognizedRootProperties[i];

			// Carry on, unless this property is present
			if (!options[property]) continue;

			// Move the property into the root namespace
			if (!(options[""] instanceof Object)) options[""] = {};
			options[""][property] = options[property];
			delete options[property];
		}

		if (otherOptions) {
			options.ignore = mergeArrays(otherOptions.ignore, options.ignore);
			options.include = mergeArrays(otherOptions.include, options.include);
			options.copy = mergeArrays(otherOptions.copy, options.copy);
			options.observe = mergeArrays(otherOptions.observe, options.observe);
		}
		options.ignore = mergeArrays(options.ignore, defaultOptions.ignore);
		options.include = mergeArrays(options.include, defaultOptions.include);
		options.copy = mergeArrays(options.copy, defaultOptions.copy);
		options.observe = mergeArrays(options.observe, defaultOptions.observe);

		options.mappedProperties = options.mappedProperties || {};
		options.copiedProperties = options.copiedProperties || {};
		return options;
	}

	function mergeArrays(a, b) {
		if (exports.getType(a) !== "array") {
			if (exports.getType(a) === "undefined") a = [];
			else a = [a];
		}
		if (exports.getType(b) !== "array") {
			if (exports.getType(b) === "undefined") b = [];
			else b = [b];
		}

		return ko.utils.arrayGetDistinctValues(a.concat(b));
	}

	// When using a 'create' callback, we proxy the dependent observable so that it doesn't immediately evaluate on creation.
	// The reason is that the dependent observables in the user-specified callback may contain references to properties that have not been mapped yet.
	function withProxyDependentObservable(dependentObservables, callback) {
		var localDO = ko.dependentObservable;
		ko.dependentObservable = function (read, owner, options) {
			options = options || {};

			if (read && typeof read == "object") { // mirrors condition in knockout implementation of DO's
				options = read;
			}

			var realDeferEvaluation = options.deferEvaluation;

			var isRemoved = false;

			// We wrap the original dependent observable so that we can remove it from the 'dependentObservables' list we need to evaluate after mapping has
			// completed if the user already evaluated the DO themselves in the meantime.
			var wrap = function (DO) {
				// Temporarily revert ko.dependentObservable, since it is used in ko.isWriteableObservable
				var tmp = ko.dependentObservable;
				ko.dependentObservable = realKoDependentObservable;
				var isWriteable = ko.isWriteableObservable(DO);
				ko.dependentObservable = tmp;

				var wrapped = realKoDependentObservable({
					read: function () {
						if (!isRemoved) {
							ko.utils.arrayRemoveItem(dependentObservables, DO);
							isRemoved = true;
						}
						return DO.apply(DO, arguments);
					},
					write: isWriteable && function (val) {
						return DO(val);
					},
					deferEvaluation: true
				});
				if (DEBUG) wrapped._wrapper = true;
				wrapped.__DO = DO;
				return wrapped;
			};

			options.deferEvaluation = true; // will either set for just options, or both read/options.
			var realDependentObservable = new realKoDependentObservable(read, owner, options);

			if (!realDeferEvaluation) {
				realDependentObservable = wrap(realDependentObservable);
				dependentObservables.push(realDependentObservable);
			}

			return realDependentObservable;
		}
		ko.dependentObservable.fn = realKoDependentObservable.fn;
		ko.computed = ko.dependentObservable;
		var result = callback();
		ko.dependentObservable = localDO;
		ko.computed = ko.dependentObservable;
		return result;
	}

	function updateViewModel(mappedRootObject, rootObject, options, parentName, parent, parentPropertyName, mappedParent) {
		var isArray = exports.getType(ko.utils.unwrapObservable(rootObject)) === "array";

		parentPropertyName = parentPropertyName || "";

		// If this object was already mapped previously, take the options from there and merge them with our existing ones.
		if (exports.isMapped(mappedRootObject)) {
			var previousMapping = ko.utils.unwrapObservable(mappedRootObject)[mappingProperty];
			options = merge(previousMapping, options);
		}

		var callbackParams = {
			data: rootObject,
			parent: mappedParent || parent
		};

		var hasCreateCallback = function () {
			return options[parentName] && options[parentName].create instanceof Function;
		};

		var createCallback = function (data) {
			return withProxyDependentObservable(dependentObservables, function () {

				if (ko.utils.unwrapObservable(parent) instanceof Array) {
					return options[parentName].create({
						data: data || callbackParams.data,
						parent: callbackParams.parent,
						skip: emptyReturn
					});
				} else {
					return options[parentName].create({
						data: data || callbackParams.data,
						parent: callbackParams.parent
					});
				}
			});
		};

		var hasUpdateCallback = function () {
			return options[parentName] && options[parentName].update instanceof Function;
		};

		var updateCallback = function (obj, data) {
			var params = {
				data: data || callbackParams.data,
				parent: callbackParams.parent,
				target: ko.utils.unwrapObservable(obj)
			};

			if (ko.isWriteableObservable(obj)) {
				params.observable = obj;
			}

			return options[parentName].update(params);
		}

		var alreadyMapped = visitedObjects.get(rootObject);
		if (alreadyMapped) {
			return alreadyMapped;
		}

		parentName = parentName || "";

		if (!isArray) {
			// For atomic types, do a direct update on the observable
			if (!canHaveProperties(rootObject)) {
				switch (exports.getType(rootObject)) {
					case "function":
						if (hasUpdateCallback()) {
							if (ko.isWriteableObservable(rootObject)) {
								rootObject(updateCallback(rootObject));
								mappedRootObject = rootObject;
							} else {
								mappedRootObject = updateCallback(rootObject);
							}
						} else {
							mappedRootObject = rootObject;
						}
						break;
					default:
						if (ko.isWriteableObservable(mappedRootObject)) {
							if (hasUpdateCallback()) {
								var valueToWrite = updateCallback(mappedRootObject);
								mappedRootObject(valueToWrite);
								return valueToWrite;
							} else {
								var valueToWrite = ko.utils.unwrapObservable(rootObject);
								mappedRootObject(valueToWrite);
								return valueToWrite;
							}
						} else {
							var hasCreateOrUpdateCallback = hasCreateCallback() || hasUpdateCallback();

							if (hasCreateCallback()) {
								mappedRootObject = createCallback();
							} else {
								mappedRootObject = ko.observable(ko.utils.unwrapObservable(rootObject));
							}

							if (hasUpdateCallback()) {
								mappedRootObject(updateCallback(mappedRootObject));
							}

							if (hasCreateOrUpdateCallback) return mappedRootObject;
						}
				}

			} else {
				mappedRootObject = ko.utils.unwrapObservable(mappedRootObject);
				if (!mappedRootObject) {
					if (hasCreateCallback()) {
						var result = createCallback();

						if (hasUpdateCallback()) {
							result = updateCallback(result);
						}

						return result;
					} else {
						if (hasUpdateCallback()) {
							return updateCallback(result);
						}

						mappedRootObject = {};
					}
				}

				if (hasUpdateCallback()) {
					mappedRootObject = updateCallback(mappedRootObject);
				}

				visitedObjects.save(rootObject, mappedRootObject);
				if (hasUpdateCallback()) return mappedRootObject;

				// For non-atomic types, visit all properties and update recursively
				visitPropertiesOrArrayEntries(rootObject, function (indexer) {
					var fullPropertyName = parentPropertyName.length ? parentPropertyName + "." + indexer : indexer;

					if (ko.utils.arrayIndexOf(options.ignore, fullPropertyName) != -1) {
						return;
					}

					if (ko.utils.arrayIndexOf(options.copy, fullPropertyName) != -1) {
						mappedRootObject[indexer] = rootObject[indexer];
						return;
					}

					if (typeof rootObject[indexer] != "object" && typeof rootObject[indexer] != "array" && options.observe.length > 0 && ko.utils.arrayIndexOf(options.observe, fullPropertyName) == -1) {
						mappedRootObject[indexer] = rootObject[indexer];
						options.copiedProperties[fullPropertyName] = true;
						return;
					}

					// In case we are adding an already mapped property, fill it with the previously mapped property value to prevent recursion.
					// If this is a property that was generated by fromJS, we should use the options specified there
					var prevMappedProperty = visitedObjects.get(rootObject[indexer]);
					var retval = updateViewModel(mappedRootObject[indexer], rootObject[indexer], options, indexer, mappedRootObject, fullPropertyName, mappedRootObject);
					var value = prevMappedProperty || retval;

					if (options.observe.length > 0 && ko.utils.arrayIndexOf(options.observe, fullPropertyName) == -1) {
						mappedRootObject[indexer] = ko.utils.unwrapObservable(value);
						options.copiedProperties[fullPropertyName] = true;
						return;
					}

					if (ko.isWriteableObservable(mappedRootObject[indexer])) {
						value = ko.utils.unwrapObservable(value);
						if (mappedRootObject[indexer]() !== value) {
							mappedRootObject[indexer](value);
						}
					} else {
						value = mappedRootObject[indexer] === undefined ? value : ko.utils.unwrapObservable(value);
						mappedRootObject[indexer] = value;
					}

					options.mappedProperties[fullPropertyName] = true;
				});
			}
		} else { //mappedRootObject is an array
			var changes = [];

			var hasKeyCallback = false;
			var keyCallback = function (x) {
				return x;
			}
			if (options[parentName] && options[parentName].key) {
				keyCallback = options[parentName].key;
				hasKeyCallback = true;
			}

			if (!ko.isObservable(mappedRootObject)) {
				// When creating the new observable array, also add a bunch of utility functions that take the 'key' of the array items into account.
				mappedRootObject = ko.observableArray([]);

				mappedRootObject.mappedRemove = function (valueOrPredicate) {
					var predicate = typeof valueOrPredicate == "function" ? valueOrPredicate : function (value) {
						return value === keyCallback(valueOrPredicate);
					};
					return mappedRootObject.remove(function (item) {
						return predicate(keyCallback(item));
					});
				}

				mappedRootObject.mappedRemoveAll = function (arrayOfValues) {
					var arrayOfKeys = filterArrayByKey(arrayOfValues, keyCallback);
					return mappedRootObject.remove(function (item) {
						return ko.utils.arrayIndexOf(arrayOfKeys, keyCallback(item)) != -1;
					});
				}

				mappedRootObject.mappedDestroy = function (valueOrPredicate) {
					var predicate = typeof valueOrPredicate == "function" ? valueOrPredicate : function (value) {
						return value === keyCallback(valueOrPredicate);
					};
					return mappedRootObject.destroy(function (item) {
						return predicate(keyCallback(item));
					});
				}

				mappedRootObject.mappedDestroyAll = function (arrayOfValues) {
					var arrayOfKeys = filterArrayByKey(arrayOfValues, keyCallback);
					return mappedRootObject.destroy(function (item) {
						return ko.utils.arrayIndexOf(arrayOfKeys, keyCallback(item)) != -1;
					});
				}

				mappedRootObject.mappedIndexOf = function (item) {
					var keys = filterArrayByKey(mappedRootObject(), keyCallback);
					var key = keyCallback(item);
					return ko.utils.arrayIndexOf(keys, key);
				}

				mappedRootObject.mappedGet = function (item) {
					return mappedRootObject()[mappedRootObject.mappedIndexOf(item)];
				}

				mappedRootObject.mappedCreate = function (value) {
					if (mappedRootObject.mappedIndexOf(value) !== -1) {
						throw new Error("There already is an object with the key that you specified.");
					}

					var item = hasCreateCallback() ? createCallback(value) : value;
					if (hasUpdateCallback()) {
						var newValue = updateCallback(item, value);
						if (ko.isWriteableObservable(item)) {
							item(newValue);
						} else {
							item = newValue;
						}
					}
					mappedRootObject.push(item);
					return item;
				}
			}

			var currentArrayKeys = filterArrayByKey(ko.utils.unwrapObservable(mappedRootObject), keyCallback).sort();
			var newArrayKeys = filterArrayByKey(rootObject, keyCallback);
			if (hasKeyCallback) newArrayKeys.sort();
			var editScript = ko.utils.compareArrays(currentArrayKeys, newArrayKeys);

			var ignoreIndexOf = {};

			var i, j;

			var unwrappedRootObject = ko.utils.unwrapObservable(rootObject);
			var itemsByKey = {};
			var optimizedKeys = true;
			for (i = 0, j = unwrappedRootObject.length; i < j; i++) {
				var key = keyCallback(unwrappedRootObject[i]);
				if (key === undefined || key instanceof Object) {
					optimizedKeys = false;
					break;
				}
				itemsByKey[key] = unwrappedRootObject[i];
			}

			var newContents = [];
			var passedOver = 0;
			for (i = 0, j = editScript.length; i < j; i++) {
				var key = editScript[i];
				var mappedItem;
				var fullPropertyName = parentPropertyName + "[" + i + "]";
				switch (key.status) {
					case "added":
						var item = optimizedKeys ? itemsByKey[key.value] : getItemByKey(ko.utils.unwrapObservable(rootObject), key.value, keyCallback);
						mappedItem = updateViewModel(undefined, item, options, parentName, mappedRootObject, fullPropertyName, parent);
						if (!hasCreateCallback()) {
							mappedItem = ko.utils.unwrapObservable(mappedItem);
						}

						var index = ignorableIndexOf(ko.utils.unwrapObservable(rootObject), item, ignoreIndexOf);

						if (mappedItem === emptyReturn) {
							passedOver++;
						} else {
							newContents[index - passedOver] = mappedItem;
						}

						ignoreIndexOf[index] = true;
						break;
					case "retained":
						var item = optimizedKeys ? itemsByKey[key.value] : getItemByKey(ko.utils.unwrapObservable(rootObject), key.value, keyCallback);
						mappedItem = getItemByKey(mappedRootObject, key.value, keyCallback);
						updateViewModel(mappedItem, item, options, parentName, mappedRootObject, fullPropertyName, parent);

						var index = ignorableIndexOf(ko.utils.unwrapObservable(rootObject), item, ignoreIndexOf);
						newContents[index] = mappedItem;
						ignoreIndexOf[index] = true;
						break;
					case "deleted":
						mappedItem = getItemByKey(mappedRootObject, key.value, keyCallback);
						break;
				}

				changes.push({
					event: key.status,
					item: mappedItem
				});
			}

			mappedRootObject(newContents);

			if (options[parentName] && options[parentName].arrayChanged) {
				ko.utils.arrayForEach(changes, function (change) {
					options[parentName].arrayChanged(change.event, change.item);
				});
			}
		}

		return mappedRootObject;
	}

	function ignorableIndexOf(array, item, ignoreIndices) {
		for (var i = 0, j = array.length; i < j; i++) {
			if (ignoreIndices[i] === true) continue;
			if (array[i] === item) return i;
		}
		return null;
	}

	function mapKey(item, callback) {
		var mappedItem;
		if (callback) mappedItem = callback(item);
		if (exports.getType(mappedItem) === "undefined") mappedItem = item;

		return ko.utils.unwrapObservable(mappedItem);
	}

	function getItemByKey(array, key, callback) {
		array = ko.utils.unwrapObservable(array);
		for (var i = 0, j = array.length; i < j; i++) {
			var item = array[i];
			if (mapKey(item, callback) === key) return item;
		}

		throw new Error("When calling ko.update*, the key '" + key + "' was not found!");
	}

	function filterArrayByKey(array, callback) {
		return ko.utils.arrayMap(ko.utils.unwrapObservable(array), function (item) {
			if (callback) {
				return mapKey(item, callback);
			} else {
				return item;
			}
		});
	}

	function visitPropertiesOrArrayEntries(rootObject, visitorCallback) {
		if (exports.getType(rootObject) === "array") {
			for (var i = 0; i < rootObject.length; i++)
				visitorCallback(i);
		} else {
			for (var propertyName in rootObject)
				visitorCallback(propertyName);
		}
	};

	function canHaveProperties(object) {
		var type = exports.getType(object);
		return ((type === "object") || (type === "array")) && (object !== null);
	}

	// Based on the parentName, this creates a fully classified name of a property

	function getPropertyName(parentName, parent, indexer) {
		var propertyName = parentName || "";
		if (exports.getType(parent) === "array") {
			if (parentName) {
				propertyName += "[" + indexer + "]";
			}
		} else {
			if (parentName) {
				propertyName += ".";
			}
			propertyName += indexer;
		}
		return propertyName;
	}

	exports.visitModel = function (rootObject, callback, options) {
		options = options || {};
		options.visitedObjects = options.visitedObjects || new objectLookup();

		var mappedRootObject;
		var unwrappedRootObject = ko.utils.unwrapObservable(rootObject);

		if (!canHaveProperties(unwrappedRootObject)) {
			return callback(rootObject, options.parentName);
		} else {
			options = fillOptions(options, unwrappedRootObject[mappingProperty]);

			// Only do a callback, but ignore the results
			callback(rootObject, options.parentName);
			mappedRootObject = exports.getType(unwrappedRootObject) === "array" ? [] : {};
		}

		options.visitedObjects.save(rootObject, mappedRootObject);

		var parentName = options.parentName;
		visitPropertiesOrArrayEntries(unwrappedRootObject, function (indexer) {
			if (options.ignore && ko.utils.arrayIndexOf(options.ignore, indexer) != -1) return;

			var propertyValue = unwrappedRootObject[indexer];
			options.parentName = getPropertyName(parentName, unwrappedRootObject, indexer);

			// If we don't want to explicitly copy the unmapped property...
			if (ko.utils.arrayIndexOf(options.copy, indexer) === -1) {
				// ...find out if it's a property we want to explicitly include
				if (ko.utils.arrayIndexOf(options.include, indexer) === -1) {
					// The mapped properties object contains all the properties that were part of the original object.
					// If a property does not exist, and it is not because it is part of an array (e.g. "myProp[3]"), then it should not be unmapped.
					if (unwrappedRootObject[mappingProperty]
				        && unwrappedRootObject[mappingProperty].mappedProperties && !unwrappedRootObject[mappingProperty].mappedProperties[indexer]
				        && unwrappedRootObject[mappingProperty].copiedProperties && !unwrappedRootObject[mappingProperty].copiedProperties[indexer]
				        && !(exports.getType(unwrappedRootObject) === "array")) {
						return;
					}
				}
			}

			var outputProperty;
			switch (exports.getType(ko.utils.unwrapObservable(propertyValue))) {
				case "object":
				case "array":
				case "undefined":
					var previouslyMappedValue = options.visitedObjects.get(propertyValue);
					mappedRootObject[indexer] = (exports.getType(previouslyMappedValue) !== "undefined") ? previouslyMappedValue : exports.visitModel(propertyValue, callback, options);
					break;
				default:
					mappedRootObject[indexer] = callback(propertyValue, options.parentName);
			}
		});

		return mappedRootObject;
	}

	function simpleObjectLookup() {
		var keys = [];
		var values = [];
		this.save = function (key, value) {
			var existingIndex = ko.utils.arrayIndexOf(keys, key);
			if (existingIndex >= 0) values[existingIndex] = value;
			else {
				keys.push(key);
				values.push(value);
			}
		};
		this.get = function (key) {
			var existingIndex = ko.utils.arrayIndexOf(keys, key);
			var value = (existingIndex >= 0) ? values[existingIndex] : undefined;
			return value;
		};
	};

	function objectLookup() {
		var buckets = {};

		var findBucket = function (key) {
			var bucketKey;
			try {
				bucketKey = key;//JSON.stringify(key);
			}
			catch (e) {
				bucketKey = "$$$";
			}

			var bucket = buckets[bucketKey];
			if (bucket === undefined) {
				bucket = new simpleObjectLookup();
				buckets[bucketKey] = bucket;
			}
			return bucket;
		};

		this.save = function (key, value) {
			findBucket(key).save(key, value);
		};
		this.get = function (key) {
			return findBucket(key).get(key);
		};
	};
}));

/*! knockout-bootstrap version: 0.3.0
*  2014-07-15
*  Author: Bill Pullen
*  Website: http://billpull.github.com/knockout-bootstrap
*  MIT License http://www.opensource.org/licenses/mit-license.php
*/

//UUID
function s4() {
	"use strict";
	return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
}

function guid() {
	"use strict";
	return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

// Outer HTML
(function ($) {
	"use strict";
	$.fn.outerHtml = function () {
		if (this.length === 0) {
			return false;
		}
		var elem = this[0], name = elem.tagName.toLowerCase();
		if (elem.outerHTML) {
			return elem.outerHTML;
		}
		var attrs = $.map(elem.attributes, function (i) { return i.name + '="' + i.value + '"'; });
		return "<" + name + (attrs.length > 0 ? " " + attrs.join(" ") : "") + ">" + elem.innerHTML + "</" + name + ">";
	};
})(jQuery);

function setupKoBootstrap(koObject) {
	"use strict";
	// Bind twitter typeahead
	koObject.bindingHandlers.typeahead = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var $element = $(element);
			var allBindings = allBindingsAccessor();
			var typeaheadOpts = { source: koObject.utils.unwrapObservable(valueAccessor()) };

			if (allBindings.typeaheadOptions) {
				$.each(allBindings.typeaheadOptions, function (optionName, optionValue) {
					typeaheadOpts[optionName] = koObject.utils.unwrapObservable(optionValue);
				});
			}

			$element.attr("autocomplete", "off").typeahead(typeaheadOpts);
		}
	};

	// Bind Twitter Progress
	koObject.bindingHandlers.progress = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var $element = $(element);

			var bar = $('<div/>', {
				'class': 'bar',
				'data-bind': 'style: { width:' + valueAccessor() + ' }'
			});

			$element.attr('id', guid())
                .addClass('progress progress-info')
                .append(bar);

			koObject.applyBindingsToDescendants(viewModel, $element[0]);
		}
	};

	// Bind Twitter Alert
	koObject.bindingHandlers.alert = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var $element = $(element);
			var alertInfo = koObject.utils.unwrapObservable(valueAccessor());

			var dismissBtn = $('<button/>', {
				'type': 'button',
				'class': 'close',
				'data-dismiss': 'alert'
			}).html('&times;');

			var alertMessage = $('<p/>').html(alertInfo.message);

			$element.addClass('alert alert-' + alertInfo.priority)
                .append(dismissBtn)
                .append(alertMessage);
		}
	};

	// Bind Twitter Tooltip
	koObject.bindingHandlers.tooltip = {
		update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var $element, options, tooltip;
			options = koObject.utils.unwrapObservable(valueAccessor());
			$element = $(element);

			// If the title is an observable, make it auto-updating.
			if (koObject.isObservable(options.title)) {
				var isToolTipVisible = false;

				$element.on('show.bs.tooltip', function () {
					isToolTipVisible = true;
				});
				$element.on('hide.bs.tooltip', function () {
					isToolTipVisible = false;
				});

				// "true" is the bootstrap default.
				var origAnimation = options.animation || true;
				options.title.subscribe(function () {
					if (isToolTipVisible) {
						$element.data('bs.tooltip').options.animation = false; // temporarily disable animation to avoid flickering of the tooltip
						$element.tooltip('fixTitle') // call this method to update the title
                            .tooltip('show');
						$element.data('bs.tooltip').options.animation = origAnimation;
					}
				});
			}

			tooltip = $element.data('bs.tooltip');
			if (tooltip) {
				$.extend(tooltip.options, options);
			} else {
				$element.tooltip(options);
			}
		}
	};

	// Bind Twitter Popover
	// Bind Twitter Popover - had to add because the standard one is not working correctly
	koObject.bindingHandlers.popover = {
		init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
			var $element = $(element);

			// read popover options
			var popoverBindingValues = koObject.utils.unwrapObservable(valueAccessor());

			// build up all the options
			var tmpOptions = {};

			// set popover title - will use default title attr if none given
			if (popoverBindingValues.title) {
				tmpOptions.title = popoverBindingValues.title;
			} else {
				//if title is empty, then fix template
				tmpOptions.template = '<div class="popover" role="tooltip"><div class="arrow"></div><div class="popover-content"></div></div>';
			}

			// set popover placement
			if (popoverBindingValues.placement) {
				tmpOptions.placement = popoverBindingValues.placement;
			}

			// set popover container
			if (popoverBindingValues.container) {
				tmpOptions.container = popoverBindingValues.container;
			}

			// set popover delay
			if (popoverBindingValues.delay) {
				tmpOptions.delay = popoverBindingValues.delay;
			}

			// create unique identifier to bind to
			var uuid = guid();
			var domId = "ko-bs-popover-" + uuid;

			// if template defined, assume a template is being used
			var tmplDom;
			var tmplHtml = "";
			if (popoverBindingValues.template) {
				// set popover template id
				var tmplId = popoverBindingValues.template;

				// set data for template
				var data = popoverBindingValues.data;

				// get template html
				if (!data) {
					tmplHtml = $('#' + tmplId).html();
				} else {
					tmplHtml = function () {
						var container = $('<div data-bind="template: { name: template, if: data, data: data }"></div>');

						koObject.applyBindings({
							template: tmplId,
							data: data
						}, container[0]);
						return container;
					};
				}

				// create DOM object to use for popover content
				tmplDom = $('<div/>', {
					"class": "ko-popover",
					"id": domId
				}).html(tmplHtml);

			} else {
				// Must be using static content for body of popover
				if (popoverBindingValues.dataContent) {
					tmplHtml = popoverBindingValues.dataContent;
				}

				// create DOM object to use for popover content
				tmplDom = $('<div/>', {
					"class": "ko-popover",
					"id": domId
				}).html(tmplHtml);
			}

			// create correct binding context
			var childBindingContext = bindingContext.createChildContext(viewModel);

			// set internal content
			tmpOptions.content = $(tmplDom[0]).outerHtml();

			// Need to copy this, otherwise all the popups end up with the value of the last item
			var popoverOptions = $.extend({}, koObject.bindingHandlers.popover.options, tmpOptions);

			// see if the close button should be added to the title
			if (popoverOptions.addCloseButtonToTitle) {
				var closeHtml = popoverOptions.closeButtonHtml;
				if (closeHtml === undefined) {
					closeHtml = ' &times ';
				}
				if (popoverOptions.title === undefined) {
					popoverOptions.title = ' ';
				}

				var titleHtml = popoverOptions.title;
				var buttonHtml = '  <button type="button" class="close" data-dismiss="popover">' + closeHtml + '</button>';
				popoverOptions.title = titleHtml + buttonHtml;
			}

			// Build up the list of eventTypes if it is defined
			var eventType = "";
			if (popoverBindingValues.trigger) {
				var triggers = popoverBindingValues.trigger.split(' ');

				for (var i = 0; i < triggers.length; i++) {
					var trigger = triggers[i];

					if (trigger !== 'manual') {
						if (i > 0) {
							eventType += ' ';
						}

						if (trigger === 'click') {
							eventType += 'click';
						} else if (trigger === 'hover') {
							eventType += 'mouseenter mouseleave';
						} else if (trigger === 'focus') {
							eventType += 'focus blur';
						}
					}
				}
			} else {
				eventType = 'click';
			}

			var lastEventType = "";
			// bind popover to element click
			$element.on(eventType, function (e) {
				e.stopPropagation();

				var popoverAction = 'toggle';
				var popoverTriggerEl = $(this);

				// Check state before we toggle it so the animation gives us the correct state
				var popoverPrevStateVisible = $('#' + domId).is(':visible');

				// if set eventType to "click focus", then both events were fired in chrome,
				// in safari only click was fired, and focus/blur will be missed for a lot of tags.
				if (lastEventType === 'focus' && e.type === 'click' && popoverPrevStateVisible) {
					lastEventType = e.type;
					return;
				}
				lastEventType = e.type;


				// show/toggle popover
				popoverTriggerEl.popover(popoverOptions).popover(popoverAction);

				// hide other popovers - other than the one we are manipulating
				var popoverInnerEl = $('#' + domId);
				var $oldPopovers = $('.ko-popover').not(popoverInnerEl).parents('.popover');
				$oldPopovers.each(function () {
					// popover is attached to the previous element or its parent if a container was specified
					var $this = $(this);

					var popoverFound = false;
					var $parent = $this.parent();
					var parentData = $parent.data('bs.popover');
					if (parentData) {
						popoverFound = true;
						$parent.popover('destroy');
					}

					if (!popoverFound) {
						var $prev = $(this).prev();
						var prevData = $prev.data('bs.popover');
						if (prevData) {
							popoverFound = true;
							$prev.popover('destroy');
						}
					}
				});
				// popover
				//$('.ko-popover').not(popoverInnerEl).parents('.popover').remove();

				// if the popover was visible, it should now be hidden, so bind the view model to our dom ID
				if (!popoverPrevStateVisible) {

					koObject.applyBindingsToDescendants(childBindingContext, popoverInnerEl[0]);

					/* Since bootstrap calculates popover position before template is filled,
                     * a smaller popover height is used and it appears moved down relative to the trigger element.
                     * So we have to fix the position after the bind
                     */

					var triggerElementPosition = $(element).offset().top;
					var triggerElementLeft = $(element).offset().left;
					var triggerElementHeight = $(element).outerHeight();
					var triggerElementWidth = $(element).outerWidth();

					var popover = $(popoverInnerEl).parents('.popover');
					var popoverHeight = popover.outerHeight();
					var popoverWidth = popover.outerWidth();
					var arrowSize = 10;

					switch (popoverOptions.offset && popoverOptions.placement) {
						case 'left':
							popover.offset({ top: triggerElementPosition - popoverHeight / 2 + triggerElementHeight / 2, left: triggerElementLeft - arrowSize - popoverWidth });
							break;
						case 'right':
							popover.offset({ top: triggerElementPosition - popoverHeight / 2 + triggerElementHeight / 2 });
							break;
						case 'top':
							popover.offset({ top: triggerElementPosition - popoverHeight - arrowSize, left: triggerElementLeft - popoverWidth / 2 + triggerElementWidth / 2 });
							break;
						case 'bottom':
							popover.offset({ top: triggerElementPosition + triggerElementHeight + arrowSize, left: triggerElementLeft - popoverWidth / 2 + triggerElementWidth / 2 });
					}

					// bind close button to remove popover
					var popoverParent;
					if (popoverOptions.container) {
						popoverParent = $(popoverOptions.container);
					} else {
						popoverParent = popoverTriggerEl.parent();
					}
					popoverParent.on('click', 'button[data-dismiss="popover"]', function () {
						popoverTriggerEl.popover('hide');
					});
				}


				// Also tell KO *not* to bind the descendants itself, otherwise they will be bound twice
				return { controlsDescendantBindings: true };
			});
		},
		options: {
			placement: "right",
			offset: false,
			html: true,
			addCloseButtonToTitle: false,
			trigger: "manual"
		}
	};
}

(function (factory) {
	"use strict";
	// Support multiple loading scenarios
	if (typeof define === 'function' && define.amd) {
		// AMD anonymous module

		define(["require", "exports", "knockout"], function (require, exports, knockout) {
			factory(knockout);
		});
	} else {
		// No module loader (plain <script> tag) - put directly in global namespace
		factory(window.ko);
	}
}(setupKoBootstrap));
