// Generate Chart
function chart(id, type, data, options) {
	var canvas = document.getElementById(id);
	var ctx = canvas.getContext('2d');
	new Chart(ctx, {
		type: type,
		data: data,
		options: options
	});
}

//Bar Chart
$(function () {
	var barChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
			datasets: [{
				label: 'bar chart',
				data: [50, 100, 150, 200, 250, 300],
				backgroundColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)',
					'rgba(255, 159, 64, 1)'
				],
				borderColor: [
					'rgba(255,99,132,0.2)',
					'rgba(54, 162, 235, 0.2)',
					'rgba(255, 206, 86, 0.2)',
					'rgba(75, 192, 192, 0.2)',
					'rgba(153, 102, 255, 0.2)',
					'rgba(255, 159, 64, 0.2)'
				],
				borderWidth: 1,
				fill: false
			}, {
				label: 'multibar chart',
				data: [50, 100, 150, 200, 250, 300],
				backgroundColor: [
					'rgba(255,99,132,0.5)',
					'rgba(54, 162, 235, 0.5)',
					'rgba(255, 206, 86, 0.5)',
					'rgba(75, 192, 192, 0.5)',
					'rgba(255, 159, 64, 0.5)',
					'rgba(153, 102, 255, 0.5)',
				],
				borderColor: [
					'rgba(255,99,132,0.2)',
					'rgba(54, 162, 235, 0.2)',
					'rgba(255, 206, 86, 0.2)',
					'rgba(75, 192, 192, 0.2)',
					'rgba(153, 102, 255, 0.2)',
					'rgba(255, 159, 64, 0.2)'
				],
				borderWidth: 1,
				fill: false
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			}
		}
	}
	chart("barChart", "bar", barChart.data, barChart.options);
});

//Line Chart
$(function () {
	var lineChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
			datasets: [{
				label: 'line chart',
				data: [12, 19, 3, 5, 2, 3],
				backgroundColor: [
					'rgba(255, 99, 132, 1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)',
					'rgba(255, 159, 64, 1)'
				],
				borderColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)',
					'rgba(255, 159, 64, 1)'
				],
				borderWidth: 1,
				fill: false
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			}
		}
	}
	chart("lineChart", "line", lineChart.data, lineChart.options);
});

//Multi Chart
$(function () {
	var multiChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
			datasets: [{
				label: 'bar chart',
				data: [50, 100, 150, 200, 250, 300],
				backgroundColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)',
					'rgba(255, 159, 64, 1)'
				],
				borderColor: [
					'rgba(255,99,132,0.2)',
					'rgba(54, 162, 235, 0.2)',
					'rgba(255, 206, 86, 0.2)',
					'rgba(75, 192, 192, 0.2)',
					'rgba(153, 102, 255, 0.2)',
					'rgba(255, 159, 64, 0.2)'
				],
				borderWidth: 1
			}, {
				label: 'line chart',
				data: [250, 250, 250, 300, 350, 400],
				type: "line",
				backgroundColor: [
					'rgba(255,99,132,0.5)',
					'rgba(54, 162, 235, 0.5)',
					'rgba(255, 206, 86, 0.5)',
					'rgba(75, 192, 192, 0.5)',
					'rgba(255, 159, 64, 0.5)',
					'rgba(153, 102, 255, 0.5)',
				],
				borderColor: [
					'rgba(255,99,132,0.2)',
					'rgba(54, 162, 235, 0.2)',
					'rgba(255, 206, 86, 0.2)',
					'rgba(75, 192, 192, 0.2)',
					'rgba(153, 102, 255, 0.2)',
					'rgba(255, 159, 64, 0.2)'
				],
				borderWidth: 1,
				fill: false
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			}
		}
	}
	chart("multiChart", "bar", multiChart.data, multiChart.options);
});

//Pie Chart
$(function () {
	var pieChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green", "Purple"],
			datasets: [{
				label: 'Pie chart',
				data: [500, 250, 300, 550, 600],
				backgroundColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)',
					'rgba(255, 159, 64, 1)'
				],
				borderWidth: 0
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			}
		}
	}
	chart("pieChart", "pie", pieChart.data, pieChart.options);
});

//Doughnut Chart
$(function () {
	var doughnutChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green", "Purple"],
			datasets: [{
				label: 'Doughnut chart',
				data: [500, 250, 300, 550, 600],
				backgroundColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
					'rgba(153, 102, 255, 1)'
				],
				borderWidth: 0
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			},
			cutoutPercentage: 50
		}
	}
	chart("doughnutChart", "doughnut", doughnutChart.data, doughnutChart.options);
});

//Polar Chart
$(function () {
	var polarChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green"],
			datasets: [{
				label: 'Polar chart',
				data: [200, 250, 300, 350],
				backgroundColor: [
					'rgba(255,99,132,1)',
					'rgba(54, 162, 235, 1)',
					'rgba(255, 206, 86, 1)',
					'rgba(75, 192, 192, 1)',
				],
				borderWidth: 0
			}]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			},
		}
	}
	chart("polarChart", "polarArea", polarChart.data, polarChart.options);
});

//Radar Chart
$(function () {
	var radarChart = {
		data: {
			labels: ["Red", "Blue", "Yellow", "Green"],
			datasets: [{
				label: 'Radar chart',
				data: [100, 150, 200, 250],
				backgroundColor: "rgba(220,220,220,0.2)",
				borderColor: "rgba(220,220,220,1)",
				borderWidth: 0
			},
			{
				label: 'Radar chart',
				data: [50, 75, 90, 95],
				backgroundColor: "rgba(26,179,148,0.2)",
				borderColor: "rgba(26,179,148,1)",
				borderWidth: 0
			}
			]
		},
		options: {
			responsive: true,
			maintainAspectRatio: false,
			legend: {
				position: "top"
			},
		}
	}
	chart("radarChart", "radar", radarChart.data, radarChart.options);
});