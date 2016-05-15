

(function () {
	$(document).ready(function (e) {
		$(".radio-content").click(function (e) {
			var radio = $("input[type='radio']", this);
			var checked = radio.prop("checked");
			if (checked) // if already checked, nothing to do
				return;

			var name = $(this).parents(".radio-select").attr("data-name");
			$(".radio-select[data-name='" + name + "'] .radio-content").removeAttr("data-selected");
			radio.prop("checked", true).parents(".radio-content").attr("data-selected", "true");
			radio.change();
		});
		$(".radio-content input[type='radio']").each(function (e) {
			var container = $(this).parents(".radio-select");
			var name = container.attr("data-name");
			var checked = $(this).prop("checked");
			if (checked)
				container.attr("title", "Initial selection.\n\n" + (container.attr("title") || ""));
			$(this).prop("name", name).attr("data-selected-initial", checked);
		});
		$(".checkbox-content").click(function (e) {
			e.preventDefault();
			var cb = $("input[type='checkbox']", this);
			var checked = cb.prop("checked");
			cb.prop("checked", !checked).parents(".checkbox-content").attr("data-selected", !checked);
		});
		$(".check-all").on('click', function (e) {
			var checked = $(this).prop("checked");
			var targets = $(this).attr("data-targets");
			$(targets).not('disabled').prop("checked", checked).change();
		});
		$("input[type='checkbox']").on('change', function (e) {
			$(this).classes(function (c) {
				var allChecked = true;
				$("." + c).each(function () { allChecked = allChecked && $(this).prop("checked"); });
				$("[data-targets='." + c + "']").prop("checked", allChecked);
			});
		});
		$("[data-targets]").on('click', function (e) {
			var targets = $(this).attr("data-targets");
			var cb = $("input[type='checkbox']" + targets, this);
			var checked = cb.prop("checked");
			cb.prop("checked", !checked).change().parents(".checkbox-content").attr("data-selected", !checked);
		});
	});
})();

