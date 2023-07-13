function checkEmailUnique(form) {
	url = contextPathCustomer + "/CheckDuplicateEmail";
	customerEmail = $("#email").val();
	

	params = { email: customerEmail};

	$.post(url, params, function (response) {
		if (response == "OK") {
			form.submit();
		} else if (response == "Duplicated") {
			showWarningModal("There is another customer having the email " + customerEmail);
		} else {
			showErrorModal("Unknown response from server");
		}
	}).fail(function () {
		showErrorModal("Could not connect to the server");
	});

	return false;
}
