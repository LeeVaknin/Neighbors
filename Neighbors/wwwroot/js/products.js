


function searchProducts() {

	$.ajax({
		url: "/Products/Search",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
			Name: $("#search-name").val(),
			Category: $("#search-category").val(),
			Location: JSON.stringify({
				City: $("#search-city").val(),
				StreetAddress: $("#search-address").val(),
			}),
		}),
		datatype: JSON,
		success: function (result) {
			console.log(result);
		},
		error: function (data) { }
	});
}


function getCities() {
	// Create a seed for that shit
	var result = [ "Afula", "Azor", "Ashdod", "Arad", "Ashkelon", "Bat-Yam", "BeerSheva", "Bazra", "Ashdod", "Holon", "Eilat", "Krayot"]
	$(result).each(function (i, city) {
		var value = $("<option></option>").val(city).html(city);
		$("#citiesList").append(value);
	});
}
