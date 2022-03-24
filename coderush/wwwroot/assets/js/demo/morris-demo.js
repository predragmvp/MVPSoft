//Line Chart Call
$(function () {
	new Morris.Line({
		element: 'lineChart',
		data: [
			{ year: '2008', value: 20 },
			{ year: '2009', value: 10 },
			{ year: '2010', value: 5 },
			{ year: '2011', value: 5 },
			{ year: '2012', value: 20 }
		],
		xkey: 'year',
		ykeys: ['value'],
		resize: true,
		labels: ['Value'],
		lineColors: ['#4bc0c0'],
		pointSize: 5
	});
});


//Donut Chart Call
$(function () {
	Morris.Donut({
		element: 'pieChart',
		data: [
			{ value: 100, label: 'Order' },
			{ value: 110, label: 'In store' },
			{ value: 90, label: 'Dispatch' }
		],
		resize: true,
		colors: ['#f97525', '#4bc0c0', '#f3a06e'],
		formatter: function (x) { return x + "%" }
	}).on('click', function (i, row) {
		console.log(i, row);
	});
});


//Area Chart Call
$(function () {
	Morris.Area({
		element: 'areaChart',
		lineColors: ['#76C1FA', '#F36368', '#63CF72', '#FABA66'],
		data: [
			{ x: '2012', y: 3, z: 7 },
			{ x: '2013', y: 3, z: 4 },
			{ x: '2014', y: null, z: 1 },
			{ x: '2015', y: 2, z: 5 },
			{ x: '2016', y: 8, z: 2 },
			{ x: '2017', y: 4, z: 4 }
		],
		// lineColors: ["#f97525", "#ff6000"],
		pointFillColors: ["rgba(255,96,0,0.6)", "rgba(249,117,37,0.6)"],
		xkey: 'x',
		ykeys: ['y', 'z'],
		labels: ['Y', 'Z'],
		hideHover: true,
		resize: true
	}).on('click', function (i, row) {
		console.log(i, row);
	});
});

//Line Chart Call
$(function () {
	Morris.Line({
		element: 'multilineChart',
		data: [{ y: '2011', a: 100, b: 90 },
		{ y: '2012', a: 75, b: 65 },
		{ y: '2013', a: 50, b: 40 },
		{ y: '2014', a: 75, b: 65 },
		{ y: '2015', a: 50, b: 40 },
		{ y: '2016', a: 75, b: 65 },
		{ y: '2017', a: 100, b: 90 }
		],
		xkey: 'y',
		ykeys: ['a', 'b'],
		labels: ['Series A', 'Series B'],
		hideHover: 'auto',
		resize: true,
		lineColors: ['#4bc0c0', '#35a0a0'],
	});
});


//Bar Chart Call
$(function () {
	Morris.Bar({
		element: 'barChart',
		barColors: ['#F1635F', '#0bbc84', '#1d76c2', '#FABA66'],
		data: [
			{ x: '2014', y: 300 },
			{ x: '2015', y: 100 },
			{ x: '2016', y: 400 },
			{ x: '2017', y: 200 }
		],
		xkey: 'x',
		ykeys: ['y'],
		labels: ['Y'],
		hideHover: true,
		resize: true
	}).on('click', function (i, row) {
		console.log(i, row);
	});
});

//Bar Chart Call
$(function () {
	Morris.Bar({
		element: 'barStackChart',
		data: [
			{ x: '2014', y: 50, z: 75, a: 100 },
			{ x: '2015', y: 100, z: null, a: 200 },
			{ x: '2016', y: 75, z: 150, a: 225 },
			{ x: '2017', y: 150, z: 300, a: 450 }
		],
		barColors: ['#F1635F', '#0bbc84', '#1d76c2', '#FABA66'],
		xkey: 'x',
		ykeys: ['y', 'z', 'a'],
		labels: ['Y', 'Z', 'A'],
		stacked: true,
		hideHover: true,
		resize: true
	});
});