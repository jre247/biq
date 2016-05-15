
var _dataTable;
$(document).ready(function () {
	var table = $('#listing'),
		listingFilter = $('#listing-filter');

	_dataTable = table.dataTable({
		'sScrollY': ($(window).height() - 200) + 'px',
		'sScrollX': ($(window).width() - 100) + 'px',
		'bPaginate': false,
		'bScrollCollapse': false,
		'bStateSave': true,
		'aaSorting': [[1, 'asc']],
		'aoColumnDefs': [{ bSortable: false, bSearchable: false, aTargets: [-1]}],
		'fnPreDrawCallback': function (e) {
			$('#listing-widget').fadeIn(1000);
			if (!listingFilter.is(":focus")) // only select when not in focus
				listingFilter.select();
		}
	}
	);

	$(window).bind('resize', function () {
		_dataTable.fnSettings().oScroll.sY = ($(window).height() - 200) + 'px';
		_dataTable.fnAdjustColumnSizing();
	});

	// Filter datatable from custom input
	listingFilter.keyup(function () {
		_dataTable.fnFilter($(this).val());
	});

	// If a search filter was saved, put the value in the filter box so the user understands what has been filtered
	var filterVal = _dataTable.fnSettings().oPreviousSearch.sSearch;
	if (filterVal != null && filterVal.length > 0) {
		listingFilter.val(filterVal);
	}

	$(".glyphicon-edit").on("click", function (e) {
		var href = $(this).attr("data-href");
		window.location.href = href;
	});
	_dataTable.fnAdjustColumnSizing();
});
