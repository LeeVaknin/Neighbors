
var $form = $("#addProductForm");
var $submitbutton = $("#submitbutton");
var $datePicker = $("#datePicker");

$form.on("blur", "input", () => {
	if ($form.valid()) {
		$submitbutton.removeAttr("disabled");
	} else {
		$submitbutton.attr("disabled", "disabled");
	}
});

$(document).ready(setDateNow);

function setDateNow() {
	var date = Date();
	$datePicker.val(date);
}

