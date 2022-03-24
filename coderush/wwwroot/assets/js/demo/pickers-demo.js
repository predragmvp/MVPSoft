var isRtl = $('html').attr('dir') === 'rtl';

// DatePicker Plugin Call
$(function () {
	$('#datepicker-base').datepicker({
		orientation: isRtl ? 'auto right' : 'auto left'
	});

	$('#datepicker-features').datepicker({
		calendarWeeks: true,
		todayBtn: 'linked',
		daysOfWeekDisabled: '1',
		clearBtn: true,
		todayHighlight: true,
		multidate: true,
		daysOfWeekHighlighted: '1,2',
		orientation: isRtl ? 'auto left' : 'auto right',

		beforeShowMonth: function (date) {
			if (date.getMonth() === 8) {
				return false;
			}
		},

		beforeShowYear: function (date) {
			if (date.getFullYear() === 2014) {
				return false;
			}
		}
	});

	$('#datepicker-range').datepicker({
		orientation: isRtl ? 'auto right' : 'auto left'
	});

	$('#datepicker-inline').datepicker();
});

// DateRangePicker Plugin Call
$(function () {
	$('#daterange-1').daterangepicker({
		applyButtonClasses: "btn-success",
		cancelButtonClasses: "btn-secondary",
		opens: (isRtl ? 'left' : 'right')
	});

	$('#daterange-2').daterangepicker({
		timePicker: true,
		timePickerIncrement: 30,
		locale: {
			format: 'MM/DD/YYYY h:mm A'
		},
		opens: (isRtl ? 'left' : 'right')
	});

	$('#daterange-3').daterangepicker({
		singleDatePicker: true,
		showDropdowns: true,
		opens: (isRtl ? 'left' : 'right')
	}, function (start, end, label) {
		var years = moment().diff(start, 'years');

		alert("You are " + years + " years old.");
	});

	var start = moment().subtract(29, 'days');
	var end = moment();

	function cb(start, end) {
		$('#daterange-4').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
	}

	$('#daterange-4').daterangepicker({
		startDate: start,
		endDate: end,
		ranges: {
			'Today': [moment(), moment()],
			'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
			'Last 7 Days': [moment().subtract(6, 'days'), moment()],
			'Last 30 Days': [moment().subtract(29, 'days'), moment()],
			'This Month': [moment().startOf('month'), moment().endOf('month')],
			'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
		},
		opens: (isRtl ? 'left' : 'right')
	}, cb);

	cb(start, end);
});

// Mini Colors Plugin Call
$(function () {
	$('#minicolors-hue').minicolors({
		control: 'hue',
		position: 'bottom ' + (isRtl ? 'right' : 'left'),
	});

	$('#minicolors-saturation').minicolors({
		control: 'saturation',
		position: 'bottom ' + (isRtl ? 'left' : 'right'),
	});

	$('#minicolors-wheel').minicolors({
		control: 'wheel',
		position: 'top ' + (isRtl ? 'left' : 'right'),
	});

	$('#minicolors-opacity').minicolors({
		control: 'wheel',
		opacity: true,
		position: 'bottom ' + (isRtl ? 'right' : 'left'),
	});

	$('#minicolors-brightness').minicolors({
		control: 'brightness',
		position: 'top ' + (isRtl ? 'right' : 'left'),
	});

	$('#minicolors-hidden').minicolors({
		position: 'top ' + (isRtl ? 'right' : 'left'),
	});
});

// Calenders Plugin Call
$(function () {
	$('.calendar3').pignoseCalendar({ disabledWeekdays: [0, 6] });

	var elem = document.getElementById('calendar');

	function calendar() {
		$(elem).fullCalendar({
			header: {
				left: 'prev,next today',
				center: 'title',
				right: 'month,agendaWeek,agendaDay,listWeek'
			},
			defaultDate: '2018-09-08',
			navLinks: true, // can click day/week names to navigate views
			editable: true,
			eventLimit: true, // allow "more" link when too many events
			events: [
				{
					title: 'All Day Event',
					start: '2018-09-03',
				},
				{
					title: 'Long Event',
					start: '2018-09-07',
					end: '2018-09-10'
				},
				{
					id: 999,
					title: 'Repeating Event',
					start: '2018-09-09T16:00:00'
				},
				{
					id: 999,
					title: 'Repeating Event',
					start: '2018-09-16T16:00:00'
				},
				{
					title: 'Conference',
					start: '2018-09-11',
					end: '2018-09-13'
				},
				{
					title: 'Meeting',
					start: '2018-09-12T10:30:00',
					end: '2018-09-12T12:30:00'
				},
				{
					title: 'Lunch',
					start: '2018-09-12T12:00:00'
				},
				{
					title: 'Meeting',
					start: '2018-03-12T14:30:00'
				},
				{
					title: 'Happy Hour',
					start: '2018-09-12T17:30:00'
				},
				{
					title: 'Dinner',
					start: '2018-09-12T20:00:00'
				},
				{
					title: 'Birthday Party',
					start: '2018-09-13T07:00:00'
				},
				{
					title: 'Click for Google',
					url: 'http://google.com/',
					start: '2018-09-28'
				}
			]
		});
	}

	calendar();
});