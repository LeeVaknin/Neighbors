
$(document).ready(function () {
	$.ajax({
		url: "Categories/ReturnJSONCategories",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		datatype: JSON,
		success: function (result) {
			$(result).each(function (i, category) {
				console.log(`This is ${category.id}`);
				$("#CatFromJson").append($("<option></option>").val(category.id).html(category.name));
			});
		},
		error: function (data) { }
	});
});
