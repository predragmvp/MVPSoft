//Line Chart Call
$(function () {
	var data = {
		labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
		series: [
			[12, 9, 7, 8, 5],
			[2, 1, 3.5, 7, 3],
			[1, 3, 4, 5, 6]
		]
	};
	var options = {
		fullWidth: true,
		chartPadding: {
			right: 40
		}
	}
	new Chartist.Line('#lineChart', data, options);
});

// Hole Chart Call
$(function () {
	var data = {
		labels: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16],
		series: [
			[5, 5, 10, 8, 7, 5, 4, null, null, null, 10, 10, 7, 8, 6, 9],
			[10, 15, null, 12, null, 10, 12, 15, null, null, 12, null, 14, null, null, null],
			[null, null, null, null, 3, 4, 1, 3, 4, 6, 7, 9, 5, null, null, null],
			[{ x: 3, y: 3 }, { x: 4, y: 3 }, { x: 5, y: undefined }, { x: 6, y: 4 }, { x: 7, y: null }, { x: 8, y: 4 }, { x: 9, y: 4 }]
		]
	}
	var options = {
		fullWidth: true,
		chartPadding: {
			right: 10
		},
		low: 0,
	}
	var chart = new Chartist.Line('#holeChart', data, options);
	var addedEvents = false;
	chart.on('draw', function () {
		if (!addedEvents) {
			$('.ct-point').on('mouseover', function () {
				$('.chartist-tooltip').html('<b>Selected Value: </b>' + $(this).attr('ct:value'));
				var x = $(this).attr('x1');
				var y = $(this).attr('y1');
				$(chart.container).siblings('.chartist-tooltip').css({ "top": y - 60 + 'px', "left": x - 75 + 'px', "display": "block", })
			});

			$('.ct-point').on('mouseout', function () {
				$('.chartist-tooltip').html('<b>Selected Value:</b>');
				$(chart.container).siblings('.chartist-tooltip').css({ "display": "none", })
			});
		}
	});
});

// Bar Chart Call
$(function () {
	var data = {
		labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
		series: [
			[5, 4, 3, 7, 5, 10, 3, 4, 8, 10, 6, 8],
			[3, 2, 9, 5, 4, 6, 4, 6, 7, 8, 7, 4]
		],
	};

	var options = {
		seriesBarDistance: 10
	};

	var responsiveOptions = [
		['screen and (max-width: 640px)', {
			seriesBarDistance: 5,
			axisX: {
				labelInterpolationFnc: function (value) {
					return value[0];
				}
			}
		}]
	];
	new Chartist.Bar('#barChart', data, options, responsiveOptions);
});

// Pie Chart Call
$(function () {
	var data = {
		series: [5, 3, 4]
	};
	var sum = function (a, b) { return a + b };
	new Chartist.Pie('#pieChart', data, {
		labelInterpolationFnc: function (value) {
			return Math.round(value / data.series.reduce(sum) * 100) + '%';
		}
	});
})

// Doughnut Chart Call
$(function () {
	var chart = new Chartist.Pie('#donghnutChart', {
		series: [10, 20, 50, 20, 5, 50, 15],
		labels: [1, 2, 3, 4, 5, 6, 7],
	}, {
			donut: true,
			donutWidth: 40,
			showLabel: true
		});
	chart.on('draw', function (data) {
		if (data.type === 'slice') {
			var pathLength = data.element._node.getTotalLength();
			data.element.attr({
				'stroke-dasharray': pathLength + 'px ' + pathLength + 'px'
			});
			var animationDefinition = {
				'stroke-dashoffset': {
					id: 'anim' + data.index,
					dur: 1000,
					from: -pathLength + 'px',
					to: '0px',
					easing: Chartist.Svg.Easing.easeOutQuint,
					fill: 'freeze'
				}
			};
			// if(data.index !== 0) {
			//   animationDefinition['stroke-dashoffset'].begin = 'anim' + (data.index - 1) + '.end';
			// }
			data.element.attr({
				'stroke-dashoffset': -pathLength + 'px'
			});
			data.element.animate(animationDefinition, false);
		}
	});
});

// Gauge Chart Call
$(function () {
	new Chartist.Pie('#gaugeChart', {
		series: [20, 10, 30, 40]
	}, {
			donut: true,
			donutWidth: 40,
			donutSolid: true,
			startAngle: 270,
			total: 200,
			showLabel: true
		});
})