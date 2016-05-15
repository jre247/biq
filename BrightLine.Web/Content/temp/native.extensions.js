// Can be used to join an array on properties of an object in the array.
Array.prototype.propertyJoin = function (properties, separators) {
	var self = this;
	self.getPropertyValue = function (item, property) {
		if (typeof (item) === typeof (Function) || typeof (property) !== typeof (""))
			return null;

		if (!property.split)
			return item;

		var ps = property.split(".");
		var value = item;
		for (var pr in ps) {
			if (typeof ("") != typeof (ps[pr]))
				continue;
			if (typeof (value[ps[pr]]) === typeof (Function))
				value = value[ps[pr]]();
			else
				value = value[ps[pr]];
		}
		return value;
	};
	var props = new Array();
	if (typeof ([]) == typeof (properties))
		props = properties;
	else if (typeof ("") == typeof (properties))
		props = properties.split(",");

	var splitters = [];
	if (typeof ([]) == typeof (separators))
		splitters = separators;
	else if (typeof ("") == typeof (separators))
		splitters = [separators];
	if (splitters.length == 0)
		splitters.push(" ");
	if (splitters.length == 1)
		splitters.push(",");

	var a = [];
	for (i in self) {
		if (!self[i])
			continue;
		var pa = [];
		for (prop in props) {
			var p = self.getPropertyValue(self[i], props[prop]);
			if (!p)
				continue;
			pa.push(p);
		}
		if (pa.join(splitters[1]))
			a.push(pa.join(splitters[1]));
	}

	if (a.length == 0)
		return "";

	return a.join(splitters[0]);
};


// Can be used to cast all elements of an array to strings.
Array.prototype.castToString = function (includeEmptyElements) {
	var self = this;
	var cast = [];
	for (i in self) {
		if (typeof (self[i]) === typeof (Function))
			continue;
		try {
			var value = self[i].toString();
			if (value || includeEmptyElements)
				cast.push(value);
		} catch (err) {
			if (window.console && window.console.log) window.console.log(err);
			break;
		}
	}
	return cast;
};


// Can be used to cast all elements of an array to ints.
Array.prototype.castToInt = function () {
	var self = this;
	var cast = [];
	for (i in self) {
		if (typeof (self[i]) === typeof (Function))
			continue;
		try {
			var value = parseInt(self[i].toString());
			if (!isNaN(value))
				cast.push(value);
		} catch (err) {
			if (window.console && window.console.log) window.console.log(err);
			break;
		}
	}
	return cast;
};


Array.prototype.sortByProperty = function (property) {
	// see if we have a numeric array
	var numeric = true;
	for (var index = 0; index < this.length; index++) {
		if (isNaN(this[index][property])) {
			numeric = false;
			break;
		}
	}
	if (numeric) {
		this.sort(function (left, right) {
			var l = parseInt(left[property]);
			var r = parseInt(right[property]);
			if (l === r)
				return 0;
			else if (l < r)
				return -1;
			else
				return 1;
		});
	} else {
		this.sort(function (left, right) {
			if (left[property] == right[property])
				return 0;
			else if (left[property] < right[property])
				return -1;
			else
				return 1;
		});
	}
	return this;
};


// http://www.tutorialspoint.com/javascript/array_filter.htm
// filters an array based on the function passed in. Only useful for older browsers.
if (!Array.prototype.filter) {
	Array.prototype.filter = function (fun /*, thisp*/) {
		var len = this.length;
		if (typeof fun != "function")
			throw new TypeError();

		var res = new Array();
		var thisp = arguments[1];
		for (var i = 0; i < len; i++) {
			if (i in this) {
				var val = this[i]; // in case function mutates this
				if (fun.call(thisp, val, i, this))
					res.push(val);
			}
		}

		return res;
	};
}

// Production steps of ECMA-262, Edition 5, 15.4.4.19
// Reference: http://es5.github.com/#x15.4.4.19
if (!Array.prototype.map) {
	Array.prototype.map = function (callback, thisArg) {

		var T, A, k;

		if (this == null) {
			throw new TypeError(" this is null or not defined");
		}

		// 1. Let O be the result of calling ToObject passing the |this| value as the argument.
		var O = Object(this);

		// 2. Let lenValue be the result of calling the Get internal method of O with the argument "length".
		// 3. Let len be ToUint32(lenValue).
		var len = O.length >>> 0;

		// 4. If IsCallable(callback) is false, throw a TypeError exception.
		// See: http://es5.github.com/#x9.11
		if (typeof callback !== "function") {
			throw new TypeError(callback + " is not a function");
		}

		// 5. If thisArg was supplied, let T be thisArg; else let T be undefined.
		if (thisArg) {
			T = thisArg;
		}

		// 6. Let A be a new array created as if by the expression new Array( len) where Array is
		// the standard built-in constructor with that name and len is the value of len.
		A = new Array(len);

		// 7. Let k be 0
		k = 0;

		// 8. Repeat, while k < len
		while (k < len) {

			var kValue, mappedValue;

			// a. Let Pk be ToString(k).
			//   This is implicit for LHS operands of the in operator
			// b. Let kPresent be the result of calling the HasProperty internal method of O with argument Pk.
			//   This step can be combined with c
			// c. If kPresent is true, then
			if (k in O) {

				var Pk = k.toString(); // This was missing per item a. of the above comment block and was not working in IE8 as a result

				// i. Let kValue be the result of calling the Get internal method of O with argument Pk.
				kValue = O[Pk];

				// ii. Let mappedValue be the result of calling the Call internal method of callback
				// with T as the this value and argument list containing kValue, k, and O.
				mappedValue = callback.call(T, kValue, k, O);

				// iii. Call the DefineOwnProperty internal method of A with arguments
				// Pk, Property Descriptor {Value: mappedValue, Writable: true, Enumerable: true, Configurable: true},
				// and false.

				// In browsers that support Object.defineProperty, use the following:
				// Object.defineProperty( A, Pk, { value: mappedValue, writable: true, enumerable: true, configurable: true });

				// For best browser support, use the following:
				A[Pk] = mappedValue;
			}
			// d. Increase k by 1.
			k++;
		}

		// 9. return A
		return A;
	};
}

// only implement if no native implementation is available
// http://stackoverflow.com/questions/767486/how-do-you-check-if-a-variable-is-an-array-in-javascript
if (typeof (Array.isArray) === 'undefined') {
	Array.isArray = function (obj) {
		return !obj ? false : Object.toString.call(obj) === '[object Array]';
	};
};

// checks if an array contains a value or any of an array of values
// the comparer shoud be a function that overrides the default obj1==obj2 comparison. Useful for comparing entity Ids, names, etc.
Array.prototype.contains = function (value, comparer) {
	// coerce into an array
	if (!Array.isArray(value))
		value = [value];
	// set up the comparision function
	if (typeof (comparer) !== "function") comparer = null;
	var compare = comparer || function (v1, v2) { return v1 == v2; };

	// compare
	for (i in this) {
		var t = this[i];
		if (typeof (t) === typeof (Function))
			continue;

		for (j in value) {
			var v = value[j];
			if (compare(t, v))
				return true;
		}
	}

	return false;
};


// http://dreaminginjavascript.wordpress.com/2008/08/22/eliminating-duplicates/
Array.prototype.distinct = function (property) {
	var i, len = this.length, out = [], obj = {};
	if (typeof (property) === typeof ("")) {
		for (i = 0; i < len; i++) {
			if (!obj[this[i][property]]) {
				obj[this[i][property]] = {};
				out.push(this[i]);
			}
		}
	} else {
		for (i = 0; i < len; i++) {
			if (!obj[this[i]]) {
				obj[this[i]] = {};
				out.push(this[i]);
			}
		}
	}
	return out;
};


// returns elements in an array that pass the comparer function (default is to return the array element).
Array.prototype.where = function (comparer) {
	var out = [];
	if (typeof (comparer) !== "function") comparer = null;
	var compare = comparer || function (item) { return item; };
	for (var i = 0; i < this.length; i++) {
		var element = this[i];
		if (compare(element))
			out.push(element);
	}

	return out;
};


// checks if a string contains the value passed in. Essentially true/false indexOf with case insensitivity possible.
String.prototype.contains = function (value, ignoreCase) {
	var s = new String(this);
	if (ignoreCase) {
		value = (value || "").toLowerCase();
		s = s.toLowerCase();
	}
	return !value || (s.indexOf(value) > -1);
};


// converts a string the a Number object with the given precision - defaults to 0
// it will strip out any non-numeric character (-|0-9|.) before the attempted cast.
String.prototype.toNumber = function (precision) {
	var v = this;
	var regex = /[^\d\.\-]/g;
	v = v.replace(regex, "");
	if (isNaN(v))
		v = 0;

	return (new Number(v).toFixed(precision || 0)) * 1;
};


if (!String.prototype.padLeft) {
	String.prototype.padLeft = function (pad, length) {
		var padded = this;
		if (!pad || !length)
			return padded;

		while (padded.length < (length * pad.length)) {
			padded = pad + padded;
		}
		return padded;
	};
}
if (!String.prototype.padRight) {
	String.prototype.padRight = function (pad, length) {
		var padded = this;
		if (!pad || !length)
			return padded;

		while (padded.length < (length * pad.length)) {
			padded = padded + pad;
		}

		return padded;
	};
}


if (typeof String.isNullOrEmpty === 'undefined') {
	String.isNullOrEmpty = function (value, allowWhitespace) {
		if (typeof (value) === "undefined" || value === null)
			return true;

		if (typeof (value) !== "string")
			value = value.toString();

		var v = (allowWhitespace) ? value : value.trim();
		return (v.length == 0);
	};
};

// based on https://raw.githubusercontent.com/umbrae/jsonlintdotcom/master/c/js/jsl.format.js
// jsl.format - Provide json reformatting in a character-by-character approach, so that even invalid JSON may be reformatted (to the best of its ability).
String.prototype.formatJson = function (asText) {
	var json = this;
	var newJson = "";
	var repeat = function (s, count) { return new Array(count + 1).join(s); };
	var newLine = (asText) ? "\n" : "<br/>";
	var tab = (asText) ? "    " : "&nbsp;&nbsp;&nbsp;&nbsp;";
	var indentLevel = 0;
	var inString = false;
	var currentChar;

	for (var i = 0, il = json.length; i < il; i += 1) {
		currentChar = json.charAt(i);

		switch (currentChar) {
			case '{':
			case '[':
				if (!inString) {
					newJson += currentChar + newLine + repeat(tab, indentLevel + 1);
					indentLevel += 1;
				} else {
					newJson += currentChar;
				}
				break;
			case '}':
			case ']':
				if (!inString) {
					indentLevel -= 1;
					newJson += newLine + repeat(tab, indentLevel) + currentChar;
				} else {
					newJson += currentChar;
				}
				break;
			case ',':
				if (!inString) {
					newJson += "," + newLine + repeat(tab, indentLevel);
				} else {
					newJson += currentChar;
				}
				break;
			case ':':
				if (!inString) {
					newJson += ": ";
				} else {
					newJson += currentChar;
				}
				break;
			case ' ':
			case "\n":
			case "\t":
				if (inString) {
					newJson += currentChar;
				}
				break;
			case '"':
				if (i > 0 && json.charAt(i - 1) !== '\\') {
					inString = !inString;
				}
				newJson += currentChar;
				break;
			default:
				newJson += currentChar;
				break;
		}
	}

	return newJson;
};

// returns the Number as a string with thousands separators
var commafy = function (value) {
	if (!value)
		return 0;

	var parts = new String(value).split(".");
	var p1 = parts[0];
	var p2 = ((parts.length == 2) ? "." + parts[1] : "");
	var re = /(\d+)(\d{3})/;
	while (re.test(p1)) {
		p1 = p1.replace(re, "$1" + "," + "$2");
	}
	return p1 + p2;
};


// returns a number with no commas
var uncommafy = function (value) {
	if (!value)
		return 0;

	var stripped = new String(value).replace(/,/g, "");
	if (isNaN(stripped))
		return 0;

	return new Number(stripped);
};


// this is from somewhere, but the link got lost.
// clones an object properties. does not include functions.
var clone = function (src) {

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
