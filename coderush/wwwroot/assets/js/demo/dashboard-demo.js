// Morris Line Graph Chart
function lineGraph() {
	var graphLines = new Morris.Line({
		element: 'morris-line-example',
		resize: true,
		lineColors: ["#009c8a", "#0cca8e"],
		data: [{
			y: '2006',
			a: 50,
			b: 0
		},
		{
			y: '2007',
			a: 75,
			b: 78
		},
		{
			y: '2008',
			a: 30,
			b: 12
		},
		{
			y: '2009',
			a: 35,
			b: 50
		},
		{
			y: '2010',
			a: 70,
			b: 100
		},
		{
			y: '2011',
			a: 78,
			b: 65
		}
		],
		grid: true,
		xkey: 'y',
		pointSize: 0,
		lineWidth: 2,
		ykeys: ['a', 'b'],
		labels: ['Series A', 'Series B'],
		hideHover: "always"
	});
	return graphLines
}

//Doughnut Chart
function doughnut() {
	var doughnutChart = {
		data: {
			labels: ["Import", "Export", "Return", "Revenue"],
			datasets: [{
				label: 'Doughnut chart',
				data: [80, 100, 80, 100],
				backgroundColor: [
					'#0cca8e',
					'#F1635F',
					'#FEBA47',
					'#009c8a'
				],
				borderWidth: 0
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: false,

			legendCallback: function (chart) {
				var legendHtml = [];
				legendHtml.push('<ul class="row">');
				var item = chart.data.datasets[0];
				for (var i = 0; i < item.data.length; i++) {
					legendHtml.push('<li class="col-6">');
					legendHtml.push('<span class="chart-legend" style="border-color:' + item.backgroundColor[i] + '"></span>');
					legendHtml.push('<span class="chart-legend-label-text">' + chart.data.labels[i] + '&nbsp;' + item.data[i]);
					legendHtml.push('</li>');
				}

				legendHtml.push('</ul>');
				return legendHtml.join("");
			},
			cutoutPercentage: 50
		}
	}
	var canvas = document.getElementById("sales-status");
	var ctx = canvas.getContext('2d');

	var myChart = new Chart(ctx, {
		type: "doughnut",
		data: doughnutChart.data,
		options: doughnutChart.options
	});
	$('#sales-status-legend').html(myChart.generateLegend());
}

// Product Sales Line Chart
function lineChart() {
	var canvas = document.getElementById('product-sales');
	var ctx = canvas.getContext("2d");
	var gradientStrokeFill_1 = ctx.createLinearGradient(1, 1, 1, 400);
	gradientStrokeFill_1.addColorStop(0, "#0cca8e");
	gradientStrokeFill_1.addColorStop(1, "#009c8a");

	var gradientStrokeFill_2 = ctx.createLinearGradient(1, 1, 1, 250);
	gradientStrokeFill_2.addColorStop(0, "#0cca8e");
	gradientStrokeFill_2.addColorStop(1, "#009c8a");

	var ProductSalesCanvas = $("#product-sales").get(0).getContext("2d");
	var ProductSales = new Chart(ProductSalesCanvas, {
		type: 'line',
		data: {
			labels: ["Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct"],
			datasets: [{
				label: 'Shoes',
				data: [0, 16, 3, 5, 2, 12, 9, 3],
				borderColor: "#0cca8e",
				backgroundColor: gradientStrokeFill_1,
				fill: true,
				borderWidth: 2,
			},
			{
				label: 'Watches',
				data: [0, 23, 7, 12, 40, 17, 26, 5],
				borderColor: "#009c8a",
				backgroundColor: gradientStrokeFill_2,
				fill: true,
				borderWidth: 2,
			}
			]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			animation: {
				animateScale: true,
				animateRotate: true
			},
			legend: false,
			legendCallback: function (chart) {
				var text = [];
				text.push('<ul>');
				for (var i = 0; i < chart.data.datasets.length; i++) {
					text.push('<li><span class="chart-legend" style="border-color:' +
						chart.data.datasets[i].borderColor + '"></span>');
					text.push('<span class="chart-legend-label-text"> ' + chart.data.datasets[i].label + '</span>');
					text.push('</li>');
				}
				text.push('</ul>');
				return text.join("");
			},
			stepsize: 100,
			scales: {
				xAxes: [{
					gridLines: {
						color: 'rgba(0, 0, 0, 0)',
						display: false
					}
				}],
				yAxes: [{
					display: false,
					gridLines: {
						color: 'rgba(0, 0, 0, 0.05)',
						display: false
					}
				}]
			}
		}
	});
	$('#product-sales-legend').html(ProductSales.generateLegend());
}

// Sales Bar Chart
function barChart() {
	var barGraph = Morris.Bar({
		element: 'dashboard-sales-chart',
		data: [{
			y: '01',
			a: 79,
			b: 40
		},
		{
			y: '02',
			a: 60,
			b: 65
		},
		{
			y: '03',
			a: 80,
			b: 100
		},
		{
			y: '04',
			a: 79,
			b: 50
		}
		],
		barColors: ["#009c8a", "#0cca8e"],
		barGap: 3,
		grid: false,
		barSizeRatio: 0.40,
		xkey: 'y',
		ykeys: ['a', 'b'],
		gridTextSize: 0,
		padding: 1,
		labels: ['Series A', 'Series B'],
		resize: true
	});
	return barGraph
}

// Method Calls
var lineGraph = lineGraph();
var doughnut = doughnut();
var lineChart = lineChart();
var barGraph = barChart();

// Resize Charts
$(window).resize(function () {
	lineGraph.redraw();
});