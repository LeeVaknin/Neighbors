
// ----- Pie Chart -----

var colorScheme = [
	'rgba(255, 99, 132, 0.2)',
	'rgba(54, 162, 235, 0.2)',
	'rgba(255, 206, 86, 0.2)',
	'rgba(75, 192, 192, 0.2)',
	'rgba(153, 102, 255, 0.2)',
	'rgba(255, 159, 64, 0.2)'
];

var borderScheme = [
	'rgba(255,99,132,1)',
	'rgba(54, 162, 235, 1)',
	'rgba(255, 206, 86, 1)',
	'rgba(75, 192, 192, 1)',
	'rgba(153, 102, 255, 1)',
	'rgba(255, 159, 64, 1)'
];


function getProductsBy(groupType) {
	$.ajax({
		url: "/Products/GroupBy" + groupType,
		type: "GET",
		contentType: "application/json; charset=utf-8",
		datatype: JSON,
		success: function (result) {
			var finalResult = { 'labels': [], 'count': [] };
			$(result).each(function (i, group) {
				finalResult.labels.push(group.name);
				finalResult.count.push(group.count);
			});
			if ("Category" == groupType) {
				loadPie(finalResult);
			} else if ("City" == groupType) {
				loadChart(finalResult);
			}
		},
		error: function (data) { return null; }
	});
}

// ----- Diagram Chart -----


function loadChart(result) {
	var chartTitle = 'Available Products by Cities';
	var ctx = document.getElementById("products-by-city-chart");
	var barData = {
		responsive: true,
		labels: result.labels,
		datasets: [{
			label: '# of products per city',
			data: result.count,
			backgroundColor: colorScheme,
			borderColor: borderScheme,
			borderWidth: 1
		}]
	};

	var myChart = new Chart(ctx, {
		type: 'bar',
		data: barData,
		options: {
			scales: {
				yAxes: [{
					ticks: {
						beginAtZero: true
					}
				}]
			},
			title: {
				display: true,
				text: chartTitle
			}
		}

	}
	);
}

function loadPie(result) {
	var pieTitle = "Available Products by Categories";
	var ctx = document.getElementById("products-by-cat-pie");

	var pieData = {
		responsive: true,
		labels: result.labels,
		datasets: [{
			data: result.count,
			backgroundColor: colorScheme,
			borderColor: borderScheme,
			borderWidth: 1,

		}]
	};

	var myPie = new Chart(ctx, {
		data: pieData,
		type: 'pie',
		options: {
			title: {
				display: true,
				text: pieTitle
			}
		}
	});
}