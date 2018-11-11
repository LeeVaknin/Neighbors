

$(document).ready(updateDatepickers);


function tryRemoveLoader() {
	try {
		$("#offers-loader").remove();
	} catch (err) { }
}

function getCities() {
    // Create a seed for that shit
    var result = ["Afula", "Azor", "Ashdod", "Arad", "Ashkelon", "Bat-Yam", "BeerSheva", "Bazra", "Ashdod", "Holon", "Eilat", "Krayot"];
    $(result).each(function (i, city) {
        var value = $("<option></option>").val(city).html(city);
		$("#search-city").append(value);
    });
}

function searchProducts() {

    $.ajax({
        url: "/Products/Search",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            name: $("#search-name").val(),
			categoryId: ( parseInt($("#catList_2").children("option:selected").val(), 10) || 0 ),
			minPrice: parseInt($("#min-price").val(), 10),
			maxPrice: parseInt($("#max-price").val(),10),
            location: {
				city: $("#search-city").children("option:selected").val(),
                streetAddress: $("#search-address").val()
            }
        }),
        datatype: JSON,
        success: function (result) {
            try {
                $("#search-menu-trigger").click();
            } catch(err) {
                console.log("couldn't close menu.");
            }
            $("#productBody").html(result);
        }
    });
}

function getProducts() {

    $.ajax({
        url: "/Products/GetJsonProducts",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $("#productBody").html(result);
            $("#productBody").fadeIn("slow");
        }
    });
}

function updateDatepickers() {
    try {
        document.getElementById("availableFrom").valueAsDate = new Date();
        document.getElementById("availableUntil").valueAsDate = new Date();
    } catch (err) { }
}

function getOffers() {

	$.ajax({
		url: "/Products/Offers",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		success: function (result) {
			tryRemoveLoader();
			$("#offersBody").html(result);
			$("#offersBody").fadeIn("slow");
		}
	});
}
