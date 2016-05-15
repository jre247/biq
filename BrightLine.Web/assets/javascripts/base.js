;
(function ($, undefined) {
  "use strict";

  var utility = {
    adjustTitle: function(prefix, suffix) {
      prefix = prefix || ''
      suffix = suffix || "Brightline";
      document.title = [prefix, suffix].join(' - ');
    },

    getQueryValue: function (name, caseInsensitive) {
      var url = location.search,
        caseInsensitive = caseInsensitive || false;

      if (caseInsensitive) {
        name = name.toLowerCase();
        url = url.toLowerCase();
      }

      name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
      var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
      results = regex.exec(url);
      return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    updateQueryString: function (key, value, url) {
      if (!url) url = window.location.href;
      var re = new RegExp("([?&])" + key + "=.*?(&|#|$)(.*)", "gi"),
			hash;

      if (re.test(url)) {
        if (typeof value !== 'undefined' && value !== null)
          return url.replace(re, '$1' + key + "=" + value + '$2$3');
        else {
          hash = url.split('#');
          url = hash[0].replace(re, '$1$3').replace(/(&|\?)$/, '');
          if (typeof hash[1] !== 'undefined' && hash[1] !== null)
            url += '#' + hash[1];
          return url;
        }
      }
      else {
        if (typeof value !== 'undefined' && value !== null) {
          var separator = url.indexOf('?') !== -1 ? '&' : '?';
          hash = url.split('#');
          url = hash[0] + separator + key + '=' + value;
          if (typeof hash[1] !== 'undefined' && hash[1] !== null)
            url += '#' + hash[1];
          return url;
        }
        else
          return url;
      }
    },
    scroll: {
      intoView: function ($el, offset, duration, skipIfInView) {
        if (!$el.length)
          return;

        skipIfInView = skipIfInView || false;
        offset = offset || 0;
        duration = duration || 0;

        var scrollTopElement = $el.offset().top,
            scrollTopWindow = $(document).scrollTop(),
            scroll = false;
        if (skipIfInView) {
          var validBoundTop = scrollTopWindow + offset,
              validBoundBottom = scrollTopWindow + $(window).height() + offset;

          //if not(within valid bounds), scroll.
          if (!((validBoundTop < scrollTopElement) && (scrollTopElement < validBoundBottom))) {
            scroll = true;
          }
        } else
          scroll = true;
        if (scroll) {
          $('html, body').animate({
            scrollTop: scrollTopElement + offset
          }, duration);
        }
      },
      autoScrollToFeedback: function () {
        var notifier = $('#notifier'),
          validationErrors = $('.validation-summary-errors');
        if (validationErrors.length) {
          utility.scroll.intoView(validationErrors, -50, 10, true)
        } else if (notifier.children().length) {
          utility.scroll.intoView(notifier, -50, 10, true);
        }
      }
    },

    textbox: {
      clear: function (elementOrSelector) {
        var el = typeof elementOrSelector === 'string' ? $(elementOrSelector).get(0) : elementOrSelector;
        if (el) {
          el.value = "";
        }
      },

      selectAll: function () { }
    },
    updateDropdownsOnChange: function ($originDD, states) {
      $originDD.change(function () {
        var targetsUpdated = false;
        for (var i = 0; i < states.length; i++) {
          var state = states[i],
            originDDValues = state.originDDValues,
            targets = state.targets;


          for (var j = 0; j < originDDValues.length; j++) {

            var originDDValue = originDDValues[j];
            if ($originDD.val().toString() == originDDValue.toString()) {
              for (var k = 0; k < targets.length; k++) {
                var target = targets[k],
                  targetDD = target.dropdown,
                  targetValue = target.value || '',
                  targetDisabled = target.disabled || false;

                targetDD.removeAttr('disabled');

                if (targetDD)
                  targetDD.val(targetValue);

                if (targetDD && targetDisabled)
                  targetDD.attr('disabled', 'disabled');
              }
              targetsUpdated = true;
            } else {
              for (var k = 0; k < targets.length; k++) {
                var target = targets[k],
                  targetDD = target.dropdown;
                targetDD.removeAttr('disabled');
              }
            }
            if (targetsUpdated) break;
          }
          if (targetsUpdated) break;

        }


      });
    },
    formErrorHighlight: function () {

      //This will red-highlight fields that has a field specific validation error.
      var fields = $('.form-group');
      for (var i = 0; i < fields.length; i++) {
        //On user input, check for the 'input-validation-error' class, and toggle the 'has-error' class on the parent '.form-group' div
        (function (i) {
          var field = fields.eq(i),
            fieldError = field.find('.bg-danger.hidden .field-validation-error');

          var toggleHasError = function ($input) {
            if (!($input).hasClass('input-validation-error'))
              field.removeClass('has-error')
            else
              field.addClass('has-error')
          }

          if (fieldError.length) {
            field.addClass('has-error');
            field.find('select, input[type="radio"]').bind('change', function (el) { setTimeout(function () { toggleHasError($(el.currentTarget), 0) }) });
            field.find('input[type="text"]').bind('keyup', function (el) { setTimeout(function () { toggleHasError($(el.currentTarget), 0) }) });
          }
        })(i);
      }
    },
    formErrorHighlightDelayed: function () {
      $('body').on('click', '.form-group .btn', function () {



        setTimeout(utility.formErrorHighlight, 0);
      });
    },
    file: {
      update: function (fileElementOrSelector, targetElementOrSelector) {
        var fel = typeof fileElementOrSelector === 'string' ? $(fileElementOrSelector).get(0) : fileElementOrSelector;
        var tel = typeof targetElementOrSelector === 'string' ? $(targetElementOrSelector).get(0) : targetElementOrSelector;

        $(fel).change(function (e) {
          e.preventDefault();
          var value = $(this).val();
          var clean = (value || "Select file...").replace("C:\\fakepath\\", ""); // for chrome, the bastard.
          tel.text(clean);
        });
      }
    },
    tables: {
      autoResize: function () {
        var tableResponsive = $('.table-responsive');
        if (tableResponsive.length === 0)
          return;
        var $window = $(window);

        var resizeTable = function () {
          var wWidth = $window.width();

          if (wWidth >= 768) {
            tableResponsive.css({ height: 'initial', 'overflow-x': 'hidden', 'overflow-y': 'hidden' });
          } else {
            var wHeight = $window.height();
            tableResponsive.css({ height: (wHeight - 210), 'overflow-x': 'scroll', 'overflow-y': 'scroll' });
          }
        }
        var autoResizeTimeout = 0;
        $window.resize(function () {
          clearTimeout(autoResizeTimeout);
          setTimeout(resizeTable, 100);
        });

        resizeTable();
      }
    },
    validationSummaryErrors: (function () {
      function VSError() {
        this.errors = [];
      }

      VSError.prototype.add = function (msgOrMsgs) {
        if (msgOrMsgs instanceof Array)
          this.errors = this.errors.concat(msgOrMsgs);
        else
          this.errors.push(msgOrMsgs);
        return this;
      };
      VSError.prototype.clear = function () {
        this.errors = [];
        $('.validation-summary').empty();
        return this;
      };
      VSError.prototype.render = function () {
        var html = '<div class="bg-danger validation-summary-errors" data-valmsg-summary="true"><ul><li>' + this.errors.join('</li><li>') + '</li></ul></div>';
        $('.validation-summary').html(html);
        return this;
      };
      VSError.prototype.set = function (msgOrMsgs) {
        return this.clear().add(msgOrMsgs).render();
      }
      return new VSError();
    })(),

    HighlightUpdates: (function () {
      HighlightUpdates.prototype.optionsDefaults = {
        key: '',
        className: 'updated'
      };

      function HighlightUpdates(optionsCustom) {
        this.optionsCustom = optionsCustom;
        this.options = $.extend(this.optionsDefaults, this.optionsCustom);
        this.listCookie = new utility.Persist({
          key: 'HighlightUpdates' //context of the page. eg: 'Campaign.Summary'
        });
      }

      HighlightUpdates.prototype.add = function (selector) {
        var selectors;
        this.list = this.listCookie.get() || {};
        selectors = this.list[this.options.key] = this.list[this.options.key] || [];
        selectors.push(selector);
        this.listCookie.set(this.list);
      };

      HighlightUpdates.prototype.highlight = function ($container) {
        $container = $container || $('body');
        var $item, removal, removals, selector, selectors, _i, _j, _len, _len1;
        this.list = this.listCookie.get() || {};
        selectors = this.list[this.options.key] || [];
        if (selectors.length === 0) {
          return { removals: 0, removed: 0 };
        }

        //An array to keep track of highlighted items in the current run
        removals = [];
        var removedCount = 0;

        //Highlight, and keep track of everything highlighted (to remove later)
        for (_i = 0, _len = selectors.length; _i < _len; _i++) {
          selector = selectors[_i];
          $item = $container.find(selector);
          if ($item.length) {
            $item.addClass(this.options.className);
            removals.push(selector);
          }
        }

        //Remove everything that was highlighted
        for (_j = 0, _len1 = removals.length; _j < _len1; _j++) {
          removal = removals[_j];
          selectors.splice(selectors.indexOf(removal), 1);
          removedCount++;
        }

        //Remove the context if it's empty
        if (this.list[this.options.key].length === 0) {
          delete this.list[this.options.key];
        }

        //save the current state
        this.listCookie.set(this.list);

        //Return number of items removed. (useful, if child items haven't loaded, and a timeout needs to be used)
        return { removals: removals.length, removed: removedCount }
      };

      return HighlightUpdates;

    })(),

    Persist: (function () {
      var cookiePrefix = '_bl.react.v1.';
      // Note: The cookie suffix should closely match the url of the page for ease of debugging.
      Persist.prototype.config = {
        expires: null,
        key: ''
      };

      function Persist(config) {
        // Prefix the cookie's name with '_bl.', if it doesn't start with it.
        if (config.key.indexOf(cookiePrefix) != 0)
          config.key = cookiePrefix + config.key;

        this.config = $.extend(true, {}, this.config, config);
      }

      Persist.prototype.set = function (objectJSON) {
        var objectString;
        objectString = JSON.stringify(objectJSON);
        return $.cookie(this.config.key, objectString, {
          expires: this.config.expires,
          path: '/'
        });
      };

      Persist.prototype.get = function () {
        var objectString;
        objectString = $.cookie(this.config.key);
        if (typeof objectString === "undefined") {
          return void 0;
        } else if (objectString === "") {
          return null;
        } else {
          return JSON.parse(objectString);
        }
      };

      return Persist;

    })(),

    cookies: {
      removeAll: function (prefix, path) {
        prefix || (prefix = '');
        path || (path = '/');

        var cookies = document.cookie ? document.cookie.split('; ') : [];

        for (var i = 0, l = cookies.length; i < l; i++) {
          var parts = cookies[i].split('=');
          var name = decodeURIComponent(parts.shift());

          // Remove the cookie if it starts with the prefix '_bl.'
          if (name.indexOf(prefix) == 0)
            $.removeCookie(name, { path: path });
        }
      }
    },

    json: {
      toCamelCasing: function (json) {
        if (!json) return;
        json = _.cloneDeep(json);

        var i, item, itemKey, itemVal, _i, _len;
        if (_.isArray(json)) {
          for (i = _i = 0, _len = json.length; _i < _len; i = ++_i) {
            item = json[i];
            json[i] = utility.json.toCamelCasing(json[i]);
          }
        } else if (_.isObject(json)) {
          for (itemKey in json) {
            itemVal = json[itemKey];
            var itemKeyFirstLower = itemKey.charAt(0).toLowerCase() + itemKey.slice(1);
            json[itemKeyFirstLower] = utility.json.toCamelCasing(itemVal);
            if(itemKey != itemKeyFirstLower)
              delete json[itemKey];
          }
        } else {
          return json;
        }
        return json;
      }

    },

    image: {
      showPreview: function (element, xOffset) {
        var img = $(element);
        var src = img.prop("src");
        var o = img.offset();
        var css = { position: "absolute", top: o.top + "px", left: o.left + xOffset + "px" };
        var imgContainer = $("<div/>").prop("id", "preview-hover").css(css).appendTo("body").fadeIn();
        var preview = $("<img alt=''/>").prop("src", src).css({ "max-height": "140px", "max-width": "260px;" });
        imgContainer.append(preview);
      },
      removePreview: function () {
        $("#preview-hover").remove();
      }
    },
    string: {
      capitalize: function (str) {
        if (typeof str == 'undefined' || str == null) {
          console.warn('String is undefined or null. Using fallback');
          return '';
        }
        return str.charAt(0).toUpperCase() + str.slice(1)
      },
      truncate: function (str, len) {
        if (!str)
          return ''

        if (str.length < len)
          return str;
        else {
          return str.slice(0, len) + '...';
        }
      }
    },
    admin: {
      clearCache: function () {
        var campaignId = location.href.split('/campaigns/')[1].split('/')[0]; // anything found between '*/campaigns/<id>/*'
        $.getJSON('/api/campaigns/' + campaignId + '/cache/clear')
        .then(function () {
          location.reload()
        });
      }
    },

    //Shorthand for returning new promises
    promise: {
      empty: function () {
        return RSVP.defer().promise;
      },

      rejected: function (reason) {
        var deferred = RSVP.defer();
        deferred.reject(reason)
        return deferred.promise;
      },

      resolved: function (resolvedObj) {
        var deferred = RSVP.defer()
        deferred.resolve(resolvedObj)
        return deferred.promise;
      },
      log: function (promise) {
        if (_.isArray(promise)) {
          RSVP.all(promise).then(function (a) {console.log(a)})
        } else {
          promise.then(function (a) {console.log(a)})
        }
      }
    },

    accountViews: {
      center: function () {
        var container = $('.account-container');
        var preContainer = $('.account-precontainer');
        if (!(container.length && preContainer.length))
          return;

        preContainer.css({
          visibility: 'visible',
          width: container.width() + 'px',
          height: container.height() + 42 * 2 + 'px' // Add twice the height of the footer
        });
      }
    },
    
    layout: {
      setActiveNav: function () {
        var currentPathname = location.pathname;
        var $navLIs = $('#nav-main-items').children('li');
        for (var i = 0, len = $navLIs.length; i < len; i++) {
          var $navLI = $navLIs.eq(i),
            $navLink = $navLI.children('a');
          var navLinkUrl = $navLink.attr('href');
          if (currentPathname.indexOf(navLinkUrl) == 0) {
            $navLI.addClass('active');
            break;
          }
        }
      }
    },

    resource: {
      getSrc: function (resource) {
        var emptyGifSrc = "/content/img/transparent.gif"
        if (resource && resource.id) {
          var resourceTypesImage = [1, 2]
          if (resourceTypesImage.indexOf(resource.resourceType) >= 0) {
            return resource.path || resource.url || emptyGifSrc;
          } else {
            return emptyGifSrc;
          }
        } else {
          return emptyGifSrc;
        }
      },
      getBackgroundImage: function (resource) {
        if (resource && resource.id) {

        }
      }
    },

    user: {
      // @param {string/array} roles - A string/list of Roles. eg: 'Admin', or ['Admin', 'AdOps']
      is: function (roles) {
        if (typeof roles === 'string')
          roles = [roles];

        roles = roles || [];

        var user = _bl.user;
        var matchingRoles = _.filter(roles, function (role) {
          return user["is" + role];
        });
        return matchingRoles.length > 0;
      },

      isnt: function (roles) {
        return !utility.user.is(roles);
      }

    },

    moment: {
      format: function (dt, format) {
        if (dt == null || dt == '')
          return ''
        var dateMoment = null

        if (moment.isMoment(dt)) {
          dateMoment = dt;
        } else if (dt instanceof Date) {
          dateMoment = moment(dt);
        } else if (typeof dt == 'string') {
          dateMoment = moment(new Date(dt));
        }

        return dateMoment.format(format)
      },

      // converts either datestring, date, or moment to moment.
      allToString: function (dateOrMoment, format) {
        // Default to analytics format
        format || (format = 'YYYYMMDD');

        return utility.moment.allToMoment(dateOrMoment, format).format(format);
      },

      // converts either datestring, date, or moment to moment.
      allToMoment: function (dateOrMoment, format) {
        // Default to analytics format
        format || (format = 'YYYYMMDD');

        if (moment.isMoment(dateOrMoment)) {
          // Already in moment. return.
          return moment(dateOrMoment);
        } else {
          var date = null;
          if (typeof dateOrMoment == 'string') {
            if (dateOrMoment.length == 8) {
              // This is probably in format: '20150230'. Convert using moment
              return moment(dateOrMoment, format);
            } else if (dateOrMoment.length > 8) {
              // This is a string that can be converted to Date. Could be of format: "2015-09-15T16:30:10"
              date = new Date(dateOrMoment);
            }
          } else if (typeof dateOrMoment == 'number') {
            // This is probably in format: 20150230. Convert using moment
            return moment(dateOrMoment, format);
          } else if (moment.isDate(dateOrMoment)) {
            // This is a Date object
            date = dateOrMoment;
          }
          return moment(date);
        }
      },

      // converts either datestring, date, or moment to date.
      allToDate: function (dateOrMoment, format) {
        return utility.moment.allToMoment(dateOrMoment, format).toDate();
      }
    },

    alterDefaultFormSubmit: function(){
      $(document).on('keypress', 'input', function(e) {
        if(e.charCode==13){
          var btn = $('.btn.save');
          if (btn.length) {
            e.preventDefault();
            btn.click();
          }
        }
      });
    },

    enableBackButtonInRazorViews: function () {
      // For some reason, clicking the browser back button in a razorview(after getting there from the SPA) changes the url, but doesn't reload the page.
      // This function watches for changes in the url, and triggers a reload.
      if (!window.isSpa) {
        // This is a razor view. Set up a handler that will trigger when url changes.
        var currentPath = location.pathname;
        page('/campaigns*', function (context) {
          if (currentPath != context.path)
            location.href = context.path;
        });
        page.start();
      }
    },

    init: function () {
      if (_bl.signout)
        utility.cookies.removeAll();

      utility.formErrorHighlight();
      utility.formErrorHighlightDelayed();

      $(function () {
        utility.scroll.autoScrollToFeedback();
        utility.tables.autoResize();
        utility.accountViews.center();
        utility.layout.setActiveNav();
        utility.alterDefaultFormSubmit();
        utility.enableBackButtonInRazorViews();
      });

    }
  };
  window.utility = utility;

  utility.init();

  (function initClearCookiesOnSignout() {
    var signoutLink = $('#nav').find('a[href="/account/signout"]');

    signoutLink.click(function () {
      utility.cookies.removeAll('_bl.');
      return true;
    })

  }());

})(jQuery);

_bl.constants = {
  selects_select: -2147483648,
  selects_na: -1
};

(function ($, undefined) {
  $.extend({
    entitiesApi: {
      destroy: function (model, id, callback) {
        var url = "/api/delete/" + model + "/" + id;
        return $.getJSON(url, callback);
      },
      archive: function (model, id, callback) {
        var url = "/api/delete/" + model + "/" + id + "/SoftCascade/";
        return $.getJSON(url, callback);
      },
      restore: function (model, id, callback) {
        var url = "/api/restore/" + model + "/" + id;
        return $.getJSON(url, callback);
      },
      get: function (model, id, callback) {
        var url = "/api/get/" + model + "/" + id;
        return $.getJSON(url, callback);
      },
      all: function (models) {
        if (!models)
          return [];

        var ms = [];
        for (var i = 0; i < arguments.length; i++) {
          var a = arguments[i];
          ms.push(a);
        }

        var promises = [];
        for (var i = 0; i < ms.length; i++) {
          var m = ms[i];
          if (typeof (m) !== typeof (""))
            continue;

          var pUrl = "/api/getall/" + m + "/";
          var p = $.getJSON(pUrl);
          promises.push(p);
        }

        return promises;
      },
      save: function (model, id, json, callback, stringify) {
        var url = "/api/save/" + model + "/" + id + "/";
        var data = typeof (json) == "string" ? json : JSON.stringify(json);
        return $.ajax({
          url: url,
          type: "POST",
          data: data,
          success: ($.isFunction(callback)) ? callback : null,
          cache: false,
          contentType: "application/json; charset=utf-8",
          enctype: "application/json; charset=utf-8",
          processData: false
        });
      },
      multipartSave: function (model, options) {
        if (!model)
          return;

        if (options && $.isFunction(options.begin))
          options.begin();
        var rv = $.entitiesApi.getFormData(options, false);
        var form = rv[0];
        var opts = rv[1];
        if (opts.errorMessage && $.isFunction(opts.fileError)) {
          opts.fileError(opts.errorMessage);
          return;
        }
        if ($.isFunction(opts.start))
          opts.start(opts.filename);

        var url = "/api/save/" + model;
        $.ajax({
          url: url,
          type: "POST",
          data: form,
          xhr: function () {
            var uxhr = $.ajaxSettings.xhr();
            if (!($.isFunction(opts.progress)))
              return uxhr;

            if (uxhr.upload) {
              uxhr.upload.addEventListener("progress", function (e) {
                if (e.lengthComputable) {
                  if (e.total > 0)
                    opts.progress(e.load, e.total);
                }
              }, false);
            }
            return uxhr;
          },
          success: ($.isFunction(opts.success)) ? opts.success : null,
          error: ($.isFunction(opts.error)) ? opts.error : null,
          complete: ($.isFunction(opts.complete)) ? opts.complete : null,
          cache: false,
          contentType: false,
          enctype: "multipart/form-data",
          processData: false
        });
      },
      uploadFile: function (model, id, options) {
        if (!model)
          return;
        if (!id || isNaN(id) || id < 0)
          id = 0;

        if (options && $.isFunction(options.begin))
          options.begin();
        var rv = $.entitiesApi.getFormData(options, true);
        var form = rv[0];
        var opts = rv[1];
        if (opts.errorMessage && $.isFunction(opts.fileError)) {
          opts.fileError(opts.errorMessage);
          return;
        }
        if ($.isFunction(opts.start))
          opts.start(opts.filename);

        var url = "/api/uploadfile/" + model + "/" + id + "/" + opts.async;
        return $.ajax({
          url: url,
          type: "POST",
          data: form,
          xhr: function () {
            var uxhr = $.ajaxSettings.xhr();
            if (!($.isFunction(opts.progress)))
              return uxhr;

            if (uxhr.upload) {
              uxhr.upload.addEventListener("progress", function (e) {
                if (e.lengthComputable) {
                  if (e.total > 0)
                    opts.progress(e.load, e.total);
                }
              }, false);
            }
            return uxhr;
          },
          success: ($.isFunction(opts.success)) ? opts.success : null,
          error: ($.isFunction(opts.error)) ? opts.error : null,
          complete: ($.isFunction(opts.complete)) ? opts.complete : null,
          cache: false,
          contentType: false,
          enctype: "multipart/form-data",
          processData: false
        });
      },
      typed: {
        get: function (controller, method, id, params, callback) {
          var url = "/api/" + controller + "/" + method + "/" + id;
          if (typeof (params) === typeof (Function))
            callback = params;
          else if (params !== null && params !== undefined)
            url += "/" + ((typeof (params) === typeof ([])) ? params : [params]).join("/");

          return $.get(url, callback);
        },
        post: function (controller, method, id, json, params, callback) {
          var url = "/api/" + controller + "/" + method + "/" + id;
          if (typeof (params) === typeof (Function))
            callback = params;
          else if (params !== null && params !== undefined)
            url += "/" + ((typeof (params) === typeof ([])) ? params : [params]).join("/");

          var data = JSON.stringify(json);
          return $.ajax({
            url: url,
            type: "POST",
            data: data,
            success: ($.isFunction(callback)) ? callback : null,
            cache: false,
            contentType: "application/json; charset=utf-8",
            enctype: "application/json; charset=utf-8",
            processData: false
          });
        },
      },
      getFormData: function (options, requireFile) {
        var defaults = {
          maxFileSize: 104857600, // 100 Mb
          form: null,
          files: null,
          fileError: function () { },
          errorMessage: null,
          begin: function () { },
          start: function () { },
          progress: function () { },
          success: function () { },
          error: function () { },
          complete: function () { },
          async: false
        };
        var opts = $.extend({}, defaults, options);
        if (!opts.form) {
          opts.errorMessage = "Form not found.";
          return [undefined, opts];
        }

        var data = new FormData(opts.form);
        if (requireFile) {
          if (!opts.files) opts.errorMessage = "Files not provided.";
          else if (opts.files.length > 1) opts.errorMessage = "Multiple files selected.";
          else if (opts.files.length === 0) opts.errorMessage = "No file selected.";
          if (opts.errorMessage)
            return [undefined, opts];
        }

        if (opts.files && opts.files.length > 0) {
          if (isNaN(opts.maxFileSize)) opts.errorMessage = "Max file size is not a number.";
          if (opts.errorMessage)
            return [undefined, opts];

          var f = opts.files[0];
          opts.filename = f.name.replace(/C:\\fakepath\\/i, '');
          if (f.size >= opts.maxFileSize) {
            opts.errorMessage = filename + " is too large. Max size is " + Math.round(opts.maxFileSize / 1048576, 0) + "Mb.";
            return [undefined, opts];
          }
          data.append("file0", f);
        }

        return [data, opts];
      },
      deleteFile: function (model, id) {
        return $.post("/api/deletefile/" + model + "/" + id)
      },
      deleteResource: function (id) {
        return $.entitiesApi.deleteFile('Resource', id)
      }
    }
  });
})(jQuery);
