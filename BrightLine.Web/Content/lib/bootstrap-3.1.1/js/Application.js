$(function () {
	Application.init();
});



var Application = function () {

	var validationRules = getValidationRules();

	return { init: init, validationRules: validationRules };

	function init() {
		enableLightbox();
		enableCirque();
		enableEnhancedAccordion();
		enableSubMenu();
		enableFocusedModal();

		$('.ui-tooltip').tooltip();
		$('.ui-popover').popover();
	}

	function enableSubMenu() {
		$("li.dropdown-submenu > a").on("click", function (e) {
			e.preventDefault();
			if (e.preventBubble) e.preventBubble();
			if (e.stopPropagation) e.stopPropagation();
			$(this).parent().find(".pull-left").css("left", function (e) {
				return -1 * $(this).outerWidth();
			});
			return false;
		});
		$("li.dropdown-submenu > a").on("mouseout", function (e) {
			$(this).blur();
		});
		$("li.dropdown-submenu > a").on("mouseover", function (e) {
			$(this).parent().find(".pull-left").css("left", function (e) {
				return -1 * $(this).outerWidth();
			});
		});
	}

	function enableCirque() {
		if ($.fn.lightbox) {
			$('.ui-lightbox').lightbox();
		}
	}





	function enableLightbox() {
		if ($.fn.cirque) {
			$('.ui-cirque').cirque({});
		}
	}

	function enableBackToTop() {
		//var backToTop = $('<a>', { id: 'back-to-top', href: '#top' });
		var icon = $('<i>', { 'class': 'icon-chevron-up' });

		backToTop.appendTo('body');
		icon.appendTo(backToTop);

		backToTop.hide();

		$(window).scroll(function () {
			if ($(this).scrollTop() > 150) {
				backToTop.fadeIn();
			} else {
				backToTop.fadeOut();
			}
		});

		backToTop.click(function (e) {
			e.preventDefault();

			$('body, html').animate({
				scrollTop: 0
			}, 600);
		});
	}

	function enableEnhancedAccordion() {
		$('.accordion-toggle').on('click', function (e) {
			$(e.target).parent().parent().parent().addClass('open');
		});

		$('.accordion-toggle').on('click', function (e) {
			$(this).parents('.panel').siblings().removeClass('open');
		});
	}



	function enableFocusedModal() {
		$('[data-toggle="modal"]').each(function () {
			var modal = $(this).attr('data-target');
			$(modal).on("shown.bs.modal", function () {
				$(this).find("select,input").first().focus();
			});
		});
	}

	function getValidationRules() {
		var custom = {
			focusCleanup: false,

			wrapper: 'div',
			errorElement: 'span',

			highlight: function (element) {
				$(element).parents('.form-group').removeClass('success').addClass('error');
			},
			success: function (element) {
				$(element).parents('.form-group').removeClass('error').addClass('success');
				$(element).parents('.form-group:not(:has(.clean))').find('div:last').before('<div class="clean"></div>');
			},
			errorPlacement: function (error, element) {
				error.prependTo(element.parents('.form-group'));
			}

		};

		return custom;
	}
}();