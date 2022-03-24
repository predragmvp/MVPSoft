//Bar Chart Call
function barChart() {
	var data = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}];

	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var plot = new Plottable.Plots.Bar()
		.addDataset(new Plottable.Dataset(data))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale)
		.animated(true);
	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#barChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});

}

//Area Chart Call
function areaChart() {
	var data = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}];

	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var data = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}
	];

	var plot = new Plottable.Plots.Area()
		.addDataset(new Plottable.Dataset(data))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale);

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#areaChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});
}

//Line Chart Call
function lineChart() {
	var data = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}];

	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var plot = new Plottable.Plots.Line()
		.addDataset(new Plottable.Dataset(data))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale);

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#lineChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});
}

//Pie Chart Call
function pieChart() {
	var scale = new Plottable.Scales.Linear();
	var colorScale = new Plottable.Scales.InterpolatedColor();
	colorScale.range(["#2b416b", '#1e4692', "#1961e8", "#5279C7"]);
	var data = [2, 4, 6, 8, 10, 20];

	var plot = new Plottable.Plots.Pie()
		.addDataset(new Plottable.Dataset(data))
		.sectorValue(function (d) {
			return d;
		}, scale)
		.attr("fill", function (d) {
			return d;
		}, colorScale)
		.labelsEnabled(true)
		.renderTo("svg#pieChart");

}

//Rectangle Chart Call
function rectangleChart() {
	var xScale = new Plottable.Scales.Category();
	var yScale = new Plottable.Scales.Category();
	var xAxis = new Plottable.Axes.Category(xScale, "bottom");
	var yAxis = new Plottable.Axes.Category(yScale, "left");
	var colorScale = new Plottable.Scales.InterpolatedColor();
	colorScale.range(["#f97525", "#1ab394"]);
	var data = [{
		x: 1,
		y: 1,
		val: 4
	}, {
		x: 1,
		y: 2,
		val: 6
	}, {
		x: 1,
		y: 3,
		val: 3
	},
	{
		x: 2,
		y: 1,
		val: 3
	}, {
		x: 2,
		y: 2,
		val: 2
	}, {
		x: 2,
		y: 3,
		val: 1
	},
	{
		x: 3,
		y: 1,
		val: 1
	}, {
		x: 3,
		y: 2,
		val: 3
	}, {
		x: 3,
		y: 3,
		val: 10
	}];

	var plot = new Plottable.Plots.Rectangle()
		.addDataset(new Plottable.Dataset(data))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale)
		.attr("fill", function (d) {
			return d.val;
		}, colorScale);

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#rectangleChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});
}

//Scatter Chart Call
function scatterChart() {
	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var data = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}];

	var plot = new Plottable.Plots.Scatter()
		.addDataset(new Plottable.Dataset(data))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale);

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#scatterChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});

}

//Segment Chart Call
function segmentChart() {
	var data = [{
		x: 1,
		x2: 3,
		y: 5,
		y2: 5
	},
	{
		x: 2,
		x2: 4,
		y: 3,
		y2: 3
	},
	{
		x: 3,
		x2: 7,
		y: 1,
		y2: 1
	},
	{
		x: 4,
		x2: 6,
		y: 4,
		y2: 4
	},
	{
		x: 5,
		x2: 7,
		y: 2,
		y2: 2
	},
	{
		x: 5,
		x2: 7,
		y: 5,
		y2: 5
	}];

	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var plot = new Plottable.Plots.Segment()
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale)
		.x2(function (d) {
			return d.x2;
		})
		.y2(function (d) {
			return d.y2;
		})
		.addDataset(new Plottable.Dataset(data));

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#segmentChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});
}

//Stack Bar Chart Call
function stackChart() {
	var xScale = new Plottable.Scales.Linear();
	var yScale = new Plottable.Scales.Linear();
	var xAxis = new Plottable.Axes.Numeric(xScale, "bottom");
	var yAxis = new Plottable.Axes.Numeric(yScale, "left");
	var colorScale = new Plottable.Scales.InterpolatedColor();
	colorScale.range(["#f97525", "#1ab394"]);

	var primaryData = [{
		x: 1,
		y: 1
	}, {
		x: 2,
		y: 3
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 4
	}, {
		x: 5,
		y: 3
	}, {
		x: 6,
		y: 5
	}];

	var secondaryData = [{
		x: 1,
		y: 2
	}, {
		x: 2,
		y: 1
	}, {
		x: 3,
		y: 2
	},
	{
		x: 4,
		y: 1
	}, {
		x: 5,
		y: 2
	}, {
		x: 6,
		y: 1
	}];

	var plot = new Plottable.Plots.StackedBar()
		.addDataset(new Plottable.Dataset(primaryData).metadata(5))
		.addDataset(new Plottable.Dataset(secondaryData).metadata(3))
		.x(function (d) {
			return d.x;
		}, xScale)
		.y(function (d) {
			return d.y;
		}, yScale)
		.attr("fill", function (d, i, dataset) {
			return dataset.metadata();
		}, colorScale);

	var table = new Plottable.Components.Table([[null, null],
	[yAxis, plot],
	[null, xAxis]]);
	table.renderTo("svg#barStackChart");

	window.addEventListener("resize", function () {
		plot.redraw();
	});
}

// Methods Call
barChart();
areaChart();
lineChart();
pieChart();
rectangleChart();
scatterChart();
segmentChart();
stackChart();