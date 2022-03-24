// Line Chart Call
$(function () {
	c3.generate({
		bindto: '#lineChart',
		data: {
			columns: [
				['Increase', 30, 200, 100, 400, 150, 250],
				['Decrease', 50, 20, 10, 40, 15, 25]
			],
			colors: {
				Increase: '#0bbc84',
				Decrease: '#de5b57'
			}
		},
	});
});

// Spline Chart Call
$(function () {
	c3.generate({
		bindto: '#splineChart',
		data: {
			columns: [
				['Increase', 30, 200, 100, 400, 150, 250],
				['Decrease', 50, 20, 10, 40, 15, 25]
			],
			colors: {
				Increase: '#0bbc84',
				Decrease: '#de5b57'
			},
			type: "spline",
		},
	});
});

// Area Chart Call
$(function () {
	c3.generate({
		bindto: '#areaChart',
		data: {
			columns: [
				['Increase', 30, 200, 100, 400, 150, 250],
				['Decrease', 50, 20, 10, 40, 15, 25]
			],
			colors: {
				Increase: '#1ab394',
				Decrease: '#f97525'
			},
			types: {
				Increase: 'area-spline',
				Decrease: 'area-spline'
			}
		},
	});
});

// Step Chart Call
$(function () {
	c3.generate({
		bindto: '#stepChart',
		data: {
			columns: [
				['Increase', 30, 200, 100, 400, 150, 250],
				['Decrease', 50, 20, 10, 40, 15, 25]
			],
			types: {
				Increase: 'step',
				Decrease: 'area-step'
			},
		},
		color: {
			pattern: ['rgba(88,216,163,1)', 'rgba(4,189,254,0.6)', 'rgba(237,28,36,0.6)']
		},
	});
});

// Bar Chart Call
$(function () {
	c3.generate({
		bindto: '#barChart',
		data: {
			columns: [
				['Increase', 100, 150, 200, 250, 300, 350],
				['Decrease', 75, 150, 225, 300, 375, 450]
			],
			colors: {
				Increase: '#269bff',
				Decrease: '#0bbc84'
			},
			type: 'bar',
		},
	});
});

// Stack Chart Call
$(function () {
	c3.generate({
		bindto: '#barStackChart',
		data: {
			columns: [
				['Increase', 100, 150, 200, 250, 300, 350],
				['Decrease', 75, 150, 225, 300, 375, 450]
			],
			colors: {
				Increase: '#F1635F',
				Decrease: '#0bbc84'
			},
			type: 'bar',
			groups: [
				['Increase', 'Decrease']
			]

		}
	});
});

//Pie Chart Call
$(function () {
	c3.generate({
		bindto: '#pieChart',
		data: {
			columns: [
				['Increase', 400],
				['Decrease', 150]
			],
			colors: {
				Increase: '#F1635F',
				Decrease: '#269bff'
			},
			type: 'pie',
		}
	});
});

//Donut Chart Call
$(function () {
	c3.generate({
		bindto: '#donutChart',
		data: {
			columns: [
				['Increase', 400],
				['Decrease', 150]
			],
			colors: {
				Increase: '#269bff',
				Decrease: '#EDB867'
			},
			type: 'donut',
		}
	});
});


//Gauge Chart Call
$(function () {
	c3.generate({
		bindto: '#gaugeChart',
		data: {
			columns: [
				['Increase', 400]
			],
			colors: {
				Increase: '#0bbc84'
			},
			type: 'gauge',
		}
	});
});

//Scatter Chart Call
$(function () {
	var chart = c3.generate({
		bindto: '#scatterChart',
		data: {
			xs: {
				setosa: 'setosa_x',
				versicolor: 'versicolor_x',
			},
			// iris data from R
			columns: [
				["setosa_x", 3.5, 3.0, 3.2, 3.1, 3.6, 3.9, 3.4, 3.4, 2.9, 3.1, 3.7, 3.4, 3.0, 3.0, 4.0, 4.4, 3.9, 3.5, 3.8, 3.8, 3.4, 3.7, 3.6, 3.3, 3.4, 3.0, 3.4, 3.5, 3.4, 3.2, 3.1, 3.4, 4.1, 4.2, 3.1, 3.2, 3.5, 3.6, 3.0, 3.4, 3.5, 2.3, 3.2, 3.5, 3.8, 3.0, 3.8, 3.2, 3.7, 3.3],
				["versicolor_x", 3.2, 3.2, 3.1, 2.3, 2.8, 2.8, 3.3, 2.4, 2.9, 2.7, 2.0, 3.0, 2.2, 2.9, 2.9, 3.1, 3.0, 2.7, 2.2, 2.5, 3.2, 2.8, 2.5, 2.8, 2.9, 3.0, 2.8, 3.0, 2.9, 2.6, 2.4, 2.4, 2.7, 2.7, 3.0, 3.4, 3.1, 2.3, 3.0, 2.5, 2.6, 3.0, 2.6, 2.3, 2.7, 3.0, 2.9, 2.9, 2.5, 2.8],
				["setosa", 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.3, 0.2, 0.2, 0.1, 0.2, 0.2, 0.1, 0.1, 0.2, 0.4, 0.4, 0.3, 0.3, 0.3, 0.2, 0.4, 0.2, 0.5, 0.2, 0.2, 0.4, 0.2, 0.2, 0.2, 0.2, 0.4, 0.1, 0.2, 0.2, 0.2, 0.2, 0.1, 0.2, 0.2, 0.3, 0.3, 0.2, 0.6, 0.4, 0.3, 0.2, 0.2, 0.2, 0.2],
				["versicolor", 1.4, 1.5, 1.5, 1.3, 1.5, 1.3, 1.6, 1.0, 1.3, 1.4, 1.0, 1.5, 1.0, 1.4, 1.3, 1.4, 1.5, 1.0, 1.5, 1.1, 1.8, 1.3, 1.5, 1.2, 1.3, 1.4, 1.4, 1.7, 1.5, 1.0, 1.1, 1.0, 1.2, 1.6, 1.5, 1.6, 1.5, 1.3, 1.3, 1.3, 1.2, 1.4, 1.2, 1.0, 1.3, 1.2, 1.3, 1.3, 1.1, 1.3],
			],
			type: 'scatter',
			colors: {
				setosa: '#1ab394',
				versicolor: '#f97525'
			},
		},
	});
});