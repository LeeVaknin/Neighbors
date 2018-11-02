
$(document).ready(getCategories);

$('#newCategoryName').on('focusout', function () {
	addCategory();
});	

function getCategories() {
	$.ajax({
		url: "Categories/ReturnJSONCategories",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		datatype: JSON,
		success: function (result) {
			$(result).each(function (i, category) {
				var value = $("<option></option>").val(category.id).html(category.name);
				$("#CatFromJson").append(value);
			});
		},
		error: function (data) { }
	});
}

function addCategory() {

	$.ajax({
		url: "Categories",
		type: "POST",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
				Name: $('#newCategoryName').val() 
		}),
		datatype: JSON,
		success: function (result) {
			if (result.isValid == true) {
				var value = $("<option></option>").val(result.model.id).html(result.model.name);
				$("#CatFromJson").append(value);
				updateAnimation(value);
			} else {
				raiseError(result.error);
			}
		},
		error: function (data) { }
	});
}

function updateAnimation(valueToSelect) {
	var message = "The category was added successfuly.";
	$('#catErrors').alert('close');
	$('#addCategory').collapse('hide');
	$('#success_placeholder').html('<div class="alert alert-dismissible alert-success"> <button type="button" class="close" data-dismiss="alert">&times;</button><small>' + message + '</small></div>');
	var element = document.getElementById("CatFromJson");
	element.selectedIndex = element.length - 1;
};

function raiseError(err) {
	$('#alert_placeholder').html('<div id="catErrors" class="alert alert-danger"><small>' + err + '</small></div>');
};