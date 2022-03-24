// Bootstrap Tooltip Call
$(function () {
	$('[data-toggle="tooltip"]').tooltip();
	$('[data-toggle="tooltip"]').on('shown.bs.tooltip', function () {
		var attr = $(this).attr("data-state")
		if (typeof attr !== typeof undefined && attr !== false) {
			$(".tooltip").removeClass("primary danger dark info warning success secondary").addClass(attr);
		}
	});
});

// Bootstrap Popover Call
$(function () {
	$('[data-toggle="popover"]').popover();
	$('[data-toggle="popover"]').on('shown.bs.popover', function () {
		var attr = $(this).attr("data-state");
		var id = $(this).attr("aria-describedby");
		if (typeof attr !== typeof undefined && attr !== false) {
			$("#" + id).addClass(attr);
		}
	});
});