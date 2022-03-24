// Visit Line Chart
function visitLineChart() {
	new Chartist.Line('#ct-visits', {
		labels: ['2008', '2009', '2010', '2011', '2012', '2013', '2014', '2015'],
		series: [
			[5, 2, 7, 4, 5, 3, 5, 4],
			[2, 5, 2, 6, 2, 5, 2, 4]
		]
	},
		{
			top: 0,
			low: 1,
			showPoint: true,
			fullWidth: true,
			plugins: [
				Chartist.plugins.tooltip()
			],
			axisY: {
				labelInterpolationFnc: function (value) {
					return (value / 1) + 'k';
				}
			},
			showArea: false
		});
}

// Sales Bar Chart
function salesBarChart() {
	new Chartist.Bar('#ct-daily-sales', {
		labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
		series: [
			[5, 4, 3, 7, 5, 2, 3]

		]
	},
		{
			axisX: {
				showLabel: false,
				showGrid: false,
				// On the x-axis start means top and end means bottom
				position: 'start'
			},

			chartPadding: {
				top: -20,
				left: 45,
			},
			axisY: {
				showLabel: false,
				showGrid: false,
				// On the y-axis start means left and end means right
				position: 'end'
			},
			height: 335,
			plugins: [
				Chartist.plugins.tooltip()
			]
		});
}

// Weather Line Chart
function weatherLineChart() {
	var chart = new Chartist.Line('#ct-weather', {
		labels: ['1', '2', '3', '4', '5', '6'],
		series: [
			[1, 0, 5, 3, 2, 2.5]

		]
	},
		{
			showArea: true,
			showPoint: false,

			chartPadding: {
				left: -20
			},
			axisX: {
				showLabel: false,
				showGrid: false
			},
			axisY: {
				showLabel: false,
				showGrid: true
			},
			fullWidth: true,

		});
}

// Morris Area Chart
function morrisAreaChart() {
	Morris.Area({
		element: 'morris-area-chart2',
		data: [{
			period: '2010',
			SiteA: 50,
			SiteB: 0,
		}, {
			period: '2011',
			SiteA: 160,
			SiteB: 100,
		}, {
			period: '2012',
			SiteA: 110,
			SiteB: 60,
		}, {
			period: '2013',
			SiteA: 60,
			SiteB: 200,
		}, {
			period: '2014',
			SiteA: 130,
			SiteB: 150,
		}, {
			period: '2015',
			SiteA: 200,
			SiteB: 90,
		}
			, {
			period: '2016',
			SiteA: 100,
			SiteB: 150,
		}],
		xkey: 'period',
		ykeys: ['SiteA', 'SiteB'],
		labels: ['Site A', 'Site B'],
		pointSize: 0,
		fillOpacity: 0.1,
		pointStrokeColors: ['#79e580', '#2cabe3'],
		behaveLikeLine: true,
		gridLineColor: '#ffffff',
		lineWidth: 2,
		smooth: true,
		hideHover: 'auto',
		lineColors: ['#79e580', '#2cabe3'],
		resize: true
	});
}

// Generate Calender
function calender() {
	var elem = document.getElementById('calender');
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

// Method Calls
visitLineChart();
salesBarChart();
weatherLineChart();
morrisAreaChart();
calender();