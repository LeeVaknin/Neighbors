
$(document).ready(function () {
	onOpenLoginModal();	
	onAddProductOpenModal();
	onEnterPressLogin();
});

function login() {

	$.ajax({
		url: "/Login",
		type: "POST",
		contentType: "application/json; charset=utf-8",
		data: JSON.stringify({
			Email: $("#email").val(),
			Password: $("#password").val(),
			RememberMe: $("#remember-me").is(':checked')
		}),
		datatype: JSON,
		success: function (result) {
			if (result.isValid == true) {
				try {
					$('#modal-trigger').click();
					$('#catErrors').alert('close');
					location.reload();
				} catch (err) {

				}
			} else {
				raiseError(result.error);
			}
		},
		error: function (data) { }
	});
}

function raiseError(err) {
	$('#login_error').html('<div id="catErrors" class="text-danger pb-2"><small>' + err + '</small></div>');
};

function onOpenLoginModal() {
	$('#login-trigger').on('click', function () {
		try {
			$('#login-modal').modal();
		} catch (err) {

		}
	});
}

function onAddProductOpenModal() {
	$('#modal-trigger').on('click', function () {
		try {
			var elem = document.getElementById("login-modal");
			if (elem != null) {
				$('#login-modal').modal();
			} else {
				$('#addProductModal').modal();
			}
		} catch (err) {
		}
	});
}

function onEnterPressLogin() {
	$('#login-form').on('keypress', function (e) {
		var code = e.keyCode || e.which;
		if (code == 13) {
			$('#submit-login').click();
		}
	});
}