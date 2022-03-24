// Stock Chart Call
$(function () {
	new Dygraph(document.getElementById("stockChart"), "../../assets/data/stock-data.txt",
		{
			colors: ['rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)'],
			customBars: true,
			logscale: true
		}
	);
});

//Per Series Call
$(function () {
	new Dygraph(document.getElementById("perSeries"), function () {
		var zp = function (x) { if (x < 10) return "0" + x; else return x; };
		var r = "date,parabola,line,another line,sine wave\n";
		for (var i = 1; i <= 31; i++) {
			r += "200610" + zp(i);
			r += "," + 10 * (i * (31 - i));
			r += "," + 10 * (8 * i);
			r += "," + 10 * (250 - 8 * i);
			r += "," + 10 * (125 + 125 * Math.sin(0.3 * i));
			r += "\n";
		}
		return r;
	},
		{
			strokeWidth: 2,
			colors: ['#1ab394', '#f97525'],
			series: {
				'parabola': {
					strokeWidth: 0.0,
					drawPoints: true,
					pointSize: 4,
					highlightCircleSize: 6
				},
				'line': {
					strokeWidth: 1.0,
					drawPoints: true,
					pointSize: 1.5
				},
				'sine wave': {
					strokeWidth: 3,
					highlightCircleSize: 10
				}
			}
		}
	);
});

//Independent Series Call
$(function () {
	new Dygraph(document.getElementById('independentSeries'), [
			[1, null, 3],
			[2, 2, null],
			[3, null, 7],
			[4, 5, null],
			[5, null, 5],
			[6, 3, null]
		],
		{
			colors: ['#1ab394', '#f97525'],
			labels: ['x', 'A', 'B'],
			connectSeparatedPoints: true,
			drawPoints: true
		}
	);
});

// Plooter Series Call
function fn(x) {
	return [0.1 * x, 0.1 * x + Math.sin(x), 0.1 * x + Math.cos(x)];
}

function plot() {
	var graph = document.getElementById("plotter");
	var width = parseInt(graph.style.width, 10);
	var x1 = -10;
	var x2 = 10;
	var xs = 1.0 * (x2 - x1) / width;
	var data = [];
	for (var i = 0; i < width; i++) {
		var x = x1 + i * xs;
		var y = fn(x);
		var row = [x];
		if (y.length > 0) {
			for (var j = 0; j < y.length; j++) {
				row.push(y[j]);
			}
		} else {
			row.push(y);
		}
		data.push(row);
	}

	new Dygraph(graph, data)
}

//Plotter Method Call
plot();
