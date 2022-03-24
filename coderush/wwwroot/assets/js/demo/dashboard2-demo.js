// Draw Sparkline Chart
var drawSparklines = function () {
	if ($('#sparklinedash').length > 0) {
		$('#sparklinedash').sparkline([0, 5, 6, 10, 9, 12, 4, 9], {
			type: 'bar',
			height: '20',
			barWidth: '3',
			resize: true,
			barSpacing: '4',
			barColor: '#0cca8e',
		});
	}

	if ($('#sparklinedash2').length > 0) {
		$('#sparklinedash2').sparkline([0, 5, 6, 10, 9, 12, 4, 9], {
			type: 'bar',
			height: '20',
			barWidth: '3',
			resize: true,
			barSpacing: '3',
			barColor: '#F1635F',
		});
	}

	if ($('#sparklinedash3').length > 0) {
		$('#sparklinedash3').sparkline([0, 5, 6, 10, 9, 12, 4, 9], {
			type: 'bar',
			height: '20',
			barWidth: '3',
			resize: true,
			barSpacing: '3',
			barColor: '#FEBA47',
		});
	}

	if ($('#sparklinedash4').length > 0) {
		$('#sparklinedash4').sparkline([0, 5, 6, 10, 9, 12, 4, 9], {
			type: 'bar',
			height: '20',
			barWidth: '3',
			resize: true,
			barSpacing: '3',
			barColor: '#009c8a',
		});
	}
};

// Draw Vector Map
function vectorMapInit() {
	if ($(document).find('#vmap').length > 0) {
		$(document).find('#vmap').vectorMap({
			map: 'world_mill',
			backgroundColor: '#fff',
			borderColor: '#fff',
			borderOpacity: 0.25,
			borderWidth: 0,
			color: '#e6e6e6',
			regionStyle: {
				initial: {
					fill: '#e4ecef',
				},
			},

			markerStyle: {
				initial: {
					r: 7,
					'fill': '#fff',
					'fill-opacity': 1,
					'stroke': '#000',
					'stroke-width': 2,
					'stroke-opacity': 0.4,
				},
			},

			markers: [{
				latLng: [21.00, 78.00],
				name: 'INDIA : 350',
			}, {
				latLng: [-33.00, 151.00],
				name: 'Australia : 250',
			}, {
				latLng: [36.77, -119.41],
				name: 'USA : 250',
			}, {
				latLng: [55.37, -3.41],
				name: 'UK   : 250',
			}, {
				latLng: [25.20, 55.27],
				name: 'UAE : 250',
			}],
			series: {
				regions: [{
					values: {
						'US': 298,
						'SA': 200,
						'AU': 760,
						'IN': 200,
						'GB': 120,
					},
					scale: ['#03a9f3', '#02a7f1'],
					normalizeFunction: 'polynomial',
				}],
			},
			hoverOpacity: null,
			normalizeFunction: 'linear',
			zoomOnScroll: false,
			scaleColors: ['#b6d6ff', '#005ace'],
			selectedColor: '#c9dfaf',
			selectedRegions: [],
			enableZoom: false,
			hoverColor: '#fff',
		});
	}
};

// Draw Easy Pie Charts
var easyPieChart = function () {
	if ($('.easy-pie-chart').length > 0) {
		$('.easy-pie-chart').easyPieChart({
			onStep(from, to, percent) {
				this.el.children[0].innerHTML = `${Math.round(percent)} %`;
			},
		});
	}
}

// Monthly Stats Line Chart
var lineChart = function () {
	if ($('#monthly-stats').length > 0) {
		var canvas = document.getElementById("monthly-stats");
		var ctx = canvas.getContext('2d');
		new Chart(ctx, {
			type: 'line',
			data: {
				labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
				datasets: [{
					label: 'Series A',
					backgroundColor: 'rgba(237, 231, 246, 0.5)',
					borderColor: '#238feb',
					pointBackgroundColor: 'rgba(38, 155, 255, 0.7)',
					borderWidth: 2,
					data: [60, 50, 70, 60, 50, 70, 60],
				}, {
					label: 'Series B',
					backgroundColor: 'rgba(232, 245, 233, 0.5)',
					borderColor: '#0cca8e',
					pointBackgroundColor: 'rgba(12, 202, 142, 0.7)',
					borderWidth: 2,
					data: [70, 75, 85, 70, 75, 85, 70],
				}],
			},

			options: {
				legend: {
					display: false,
				},
			},

		});
	}
}

// Method Calls
drawSparklines();
easyPieChart();
lineChart();
vectorMapInit();
