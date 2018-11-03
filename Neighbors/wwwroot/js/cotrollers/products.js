
var $form = $("#addProductForm");
var $submitbutton = $("#submitbutton");

$form.on("blur", "input", () => {
	if ($form.valid()) {
		$submitbutton.removeattr("disabled");
	} else {
		$submitbutton.attr("disabled", "disabled");
	}
});

//$(document).ready(setDateNow);

//function setDateNow() {
//	$('.date-pick').datepicker().datepicker("setDate", new Date());
//}

