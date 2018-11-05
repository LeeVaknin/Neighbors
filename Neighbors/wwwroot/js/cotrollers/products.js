
//var $form = $("#addProductForm");
//var $submitbutton = $("#submitbutton");

//$("#addProductForm").on("blur", "input", () => {
//	if ($("#addProductForm").valid()) {
//		$("#submitbutton").removeattr("disabled");
//	} else {
//		$("#submitbutton").attr("disabled", "disabled");
//	}
//});

//$('.glyphicon-calendar').on('click', function () {
//	$('#ARDT').focus();
//});

$(document).ready(getCities);

function getCities() {
	// Create a seed for that shit
	var result = [ "Afula", "Azor", "Ashdod", "Arad", "Ashkelon", "Bat-Yam", "BeerSheva", "Bazra", "Ashdod", "Holon", "Eilat", "Krayot"]
	$(result).each(function (i, city) {
		var value = $("<option></option>").val(city).html(city);
		$("#citiesList").append(value);
	});
}
