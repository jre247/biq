(function ($) {

	$.msgGrowl = {
		timers: [],
		defaults: {
			type: "info",
			title: "",
			text: "",
			html: "",
			maxShown: 5,
			//lifetime: 3000,
			sticky: true,
			position: "top-center",
			closeTrigger: true,
			clearContent: false,
			onOpen: function () {
			},
			onClose: function () {
			  $('#notifier').empty();
			},
			onChange: function () {
			}
		},
		remove: (typeof window.msgGrowlDisabled === 'undefined' || !window.msgGrowlDisabled),
		skip: (typeof window.msgGrowlDisabled !== 'undefined' && window.msgGrowlDisabled),
		options: {},
		show: function (config) {
			if ($.msgGrowl.skip)
				return;

			this.options = $.extend({}, this.defaults, config);
			var opts = this.options;
			var container = $(".msgGrowl-container." + opts.position);
			if (!container.length) {
				var row = $("<div>", { "class": "row msgGrowl-row" });
				var col = $("<div>", { "class": "col-md-8 col-md-push-2 col-lg-8 col-lg-push-2 col-sm-10 col-sm-push-1 col-xs-12" });
				container = $("<div>", { "class": "alert msgGrowl-container " + opts.position });
				container.appendTo(col);
				col.appendTo(row);
				var notifier = $('#notifier');
				if (notifier.length)
				  row.appendTo(notifier);
        else
				  row.appendTo("body");
			}
			clearTimeout(this.timers[opts.type]);
			container.stop().fadeIn("fast");

			var text = (!opts.text) ? null : $("<p>").text(opts.text);
			var html = (!opts.html) ? null : $("<p>").html(opts.html);

			var containerId = "__msgGrowl_" + opts.type;
			var contentId = "__msgGrowl_content_" + opts.type;
			var msgGrowl = $("#" + containerId);
			msgGrowl.stop().fadeIn(1);
			var content = $("#" + contentId);
			if (msgGrowl.length > 0) {
				if (opts.lifetime > 0 && !opts.sticky) {
					clearTimeout(this.timers[opts.type]);
					this.timers[opts.type] = setTimeout(function () {
						if (typeof opts.onClose === "function") {
							opts.onClose();
						}
						msgGrowl.fadeOut(3000, function () { $(this).remove(); });
					}, opts.lifetime);
				}

				if (opts.clearContent === true) {
					content.find("p").text("");
					content.find("p").html("");
				} else {
					if (content.find("hr").length > opts.maxShown) {
						content.find("p:first").remove();
						content.find("hr:first").remove();
					}
					var hr = $("<hr style='margin:5px;'/>");
					hr.appendTo(content);
				}
				if (text) text.appendTo(content);
				if (html) html.appendTo(content);
				if (typeof opts.onChange === "function")
					opts.onChange(container);
				return;
			}

			msgGrowl = $("<div>", { "class": "msgGrowl alert alert-" + opts.type, id: containerId });
			content = $("<div>", { "class": "msgGrowl-content", id: contentId });
			var title = (!opts.title) ? null : $("<h4>").text(opts.title);
			var close = (!opts.closeTrigger) ? null : $("<div>", { "text": "X", "class": "msgGrowl-close", "click": function (e) { e.preventDefault(); $(this).parent().fadeOut("medium", function () { $(this).remove(); if (typeof opts.onClose === "function") { opts.onClose(); } }); } });

			container.addClass(opts.position);
			var position = (opts.position.split("-")[0] == "top") ? msgGrowl.prependTo(container) : msgGrowl.appendTo(container);

			if (content) content.appendTo(msgGrowl);
			if (title) title.prependTo(content);
			if (text) text.appendTo(content);
			if (html) html.appendTo(content);
			if (close) close.appendTo(msgGrowl);
			if (opts.lifetime > 0 && !opts.sticky) {
				clearTimeout(this.timers[opts.type]);
				this.timers[opts.type] = setTimeout(function () {
					if (typeof opts.onClose === "function") {
						opts.onClose();
					}
					msgGrowl.fadeOut("slow", function () { $(this).remove(); });
				}, opts.lifetime);
			}

			if (typeof opts.onOpen === "function")
				opts.onOpen(container);
		},
		hide: function () {
			var s = this;
			$(".msgGrowl").hide(0, function (e) {
				if (typeof s.options.onClose === "function")
					s.options.onClose();
			});
		},
		fadeOut: function (duration) {
			var s = this;
			$(".msgGrowl").fadeOut(duration || 250, function (e) {
				if (typeof s.options.onClose === "function")
					s.options.onClose();
			});
		},
		success: function (msg, options) {
			var so = $.extend({}, options || {}, { html: msg, type: "success" });
			this.show(so);
			var self = this;
			$('body').one('click', '.btn', function () {
			  self.hide();
			});
		},
		error: function (msg, options) {
			var oo = function (c) { };
			var os = $.extend({}, options, { html: msg, type: "danger", lifetime: 5000, onOpen: oo });
			this.show(os);
		},
		info: function (msg, options) {
			var io = $.extend({}, this.defaults, options, { html: msg, type: "info" });
			this.show(io);
		},
		warning: function (msg, options) {
			var wo = $.extend({}, this.defaults, options, { html: msg, type: "warning" });
			this.show(wo);
		},
		redirect: {
			cookieMessage: function () {
				$.each(["Success", "Info", "Error", "Warning"], function (i, type) {
					var name = "Flash.Redirect." + type;
					var cookie = $.cookie(name);
					if (cookie) {
						var trimmed = cookie.trim();
						if (trimmed.toLowerCase() !== "null") {
							var t = type.toLowerCase();
							$.msgGrowl[t](trimmed);
							if ($.msgGrowl.remove)
								$.cookie(name, null, { path: "/" });
						}
					}
				});
			}
		},
		cookieMessage: function () {
			$.each(["Success", "Info", "Error", "Warning"], function (i, type) {
				var name = "Flash." + type;
				var cookie = $.cookie(name);
				if (cookie) {
					var trimmed = cookie.trim();
					if (trimmed.toLowerCase() !== "null") {
						var t = type.toLowerCase();
						$.msgGrowl[t](trimmed);
						if ($.msgGrowl.remove)
							$.cookie(name, null, { path: "/" });
					}
				}
			});
		},
		debugMessage: function () {
			$.each(["Flash.Debug", "Flash.Redirect.Debug"], function (i, name) {
				var cookie = $.cookie(name);
				if (cookie) {
					var trimmed = cookie.trim();
					if (trimmed.toLowerCase() !== "null") {
						$.msgGrowl.info(trimmed);
					}
					if ($.msgGrowl.remove)
						$.cookie(name, null, { path: "/" });
				}
			});
		},
		conditional: function (bool, successMessage, failureMessage) {
			if (bool) this.success(successMessage);
			else this.failure(failureMessage);
		},
		conditionals: function (options) {
			var successes = [];
			var errors = [];
			$.each(options, function (i, option) {
				var bool = option.bool;
				var success = option.successMessage;
				var error = option.errorMessage;
				if (bool)
					successes.push(success);
				else
					errors.push(error);
			});
			if (successes.length > 0) {
				var s = successes.join("<br/>");
				this.success(s);
			}
			if (errors.length > 0) {
				var e = errors.join("<br/>");
				this.error(e);
			}
		},
	};

  //Close msgGrowl if user clicks anywhere in the container
  $('body').on('click', '.msgGrowl-row', function (e) {
    var el = e.target,
      elTag = el.tagName;

    if (elTag !== 'A') {
	    $.msgGrowl.hide();
    }
  });

  // Show any pending messages( from the server - message is saved in a cookie ).
  $(document).ready(function () {
    $.msgGrowl.cookieMessage();
    $.msgGrowl.redirect.cookieMessage();
    //setInterval($.msgGrowl.cookieMessage, 1000);
    if (_bl.debuggingEnabled) {
      $.msgGrowl.debugMessage; //setInterval($.msgGrowl.debugMessage, 1000);
    }
  });
})(jQuery);
