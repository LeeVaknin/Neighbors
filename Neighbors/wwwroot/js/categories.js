
// veriables

//$(document).ready(getCategories);

// update triggers

// Controller methods execution

function getCategories(selector) {
	$.ajax({
		url: "/CategoriesShort",
		type: "GET",
		contentType: "application/json; charset=utf-8",
		datatype: JSON,
		success: function (result) {
			$(result).each(function (i, category) {
				var value = $("<option></option>").val(category.id).html(category.name);
				$(selector).append(value);
			});
		},
		error: function (data) { }
	});
}

function addCategory(selector) {

	$.ajax({
		url: "/Categories",
		type: "POST",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
			Name: $("#newCategoryName").val() 
		}),
		datatype: JSON,
		success: function (result) {
			if (result.isValid == true) {
				var value = $("<option></option>").val(result.model.id).html(result.model.name);
				$(selector).append(value);
				updateAnimation(value);
			} else {
				raiseError(result.error);
			}
		},
		error: function (data) { }
	});
}

// visual behaviors

function updateAnimation(valueToSelect) {
	var message = "The category was added successfully.";
	closeModal();
	raiseSuccess(message);
	updateSelection();
	$("#catList").fadeOut(300).fadeIn(300);
};

function updateSelection() {
	var element = document.getElementById("catList");
	element.selectedIndex = element.length - 1;
}

function raiseSuccess(message) {
	$('#success_placeholder').html('<div class="alert alert-dismissible alert-success"> <button type="button" class="close" data-dismiss="alert">&times;</button><small>' + message + '</small></div>');
};

function raiseError(err) {
	$('#alert_placeholder').html('<div id="catErrors" class="alert alert-danger"><small>' + err + '</small></div>');
};

function closeModal() {
	try {
		$('#catErrors').alert('close');
		$('#addCategory').collapse('hide');
	}
	catch (err) {
	}
}