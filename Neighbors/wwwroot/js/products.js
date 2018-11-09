

$(document).ready(updateDatepickers)

function getCities() {
	// Create a seed for that shit
	var result = [ "Afula", "Azor", "Ashdod", "Arad", "Ashkelon", "Bat-Yam", "BeerSheva", "Bazra", "Ashdod", "Holon", "Eilat", "Krayot"]
	$(result).each(function (i, city) {
		var value = $("<option></option>").val(city).html(city);
		$("#citiesList").append(value);
	});
}

function searchProducts() {

	$.ajax({
		url: "/Products/Search",
		type: "POST",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
			Name: $("#search-name").val(),
			Category: $("#search-category").val(),
			Location: {
				City: $("#search-city").val(),
				StreetAddress: $("#search-address").val(),
			}
		}),
		datatype: JSON,
		success: function (result) {
			try {
				$('#search-menu-trigger').click();
			}
			catch { console.log("couldn't close menu.") }
			$('#productBody').html(result);
		},
		error: function (data) { }
	});
}

function getProducts() {

	$.ajax({
		url: "/Products/GetJsonProducts",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		success: function (result) {
			$('#productBody').html(result);
			$('#productBody').fadeIn('slow');
		},
		error: function (data) { }
	});
}

function updateDatepickers() {
	document.getElementById("availableFrom").valueAsDate = new Date();
	document.getElementById("availableUntil").valueAsDate = new Date();
}