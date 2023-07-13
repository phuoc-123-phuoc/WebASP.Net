var dropDownCountry;
var dataListState;
var fieldState;

$(document).ready(function () {
	dropDownCountry = $("#country");
	//dataListState = $("#listStates"); // 1.Option Dependent Dropdown $("#listStates")
	dataListState = $("#stateList"); // 2.Option Dependent Dropdown $("#stateList")
	fieldState = $("#state");

	dropDownCountry.on("change", function () {
		loadStatesForCountry();
		fieldState.val("").focus();
	});

	$("#buttonCancel").on("click", function () {
		//console.log(moduleURL);
		window.location = moduleURL;
	});
});

function loadStatesForCountry() {
	
	selectedCountry = $("#country option:selected");
	countryId = selectedCountry.val();
	url = contextPath + "/listAllStateByCountry/" + countryId;
	//alert(url);
	$.get(url, function (responseJSON) {
		dataListState.empty();

		$.each(responseJSON, function (index, state) {
			
			$("<option>").val(state.name).text(state.name).appendTo(dataListState);
		});

	}).fail(function () {
		alert('failed to connect to the server!');
	});
}

function checkPasswordMatch(confirmPassword) {
	if (confirmPassword.value != $("#password").val()) {
		confirmPassword.setCustomValidity("Passwords do not match!");
	} else {
		confirmPassword.setCustomValidity("");
	}
}

