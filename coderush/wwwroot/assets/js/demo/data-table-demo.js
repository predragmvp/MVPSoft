// Data Table Plugin Call
$(function () {
	$('#view_report_table').DataTable({
		dom: 'lTf<"html5buttons"B>gtip',
		pageLength: 10,
		buttons: [
			'copy', 'csv', 'excel', 'pdf', 'print'
		]
	});
});