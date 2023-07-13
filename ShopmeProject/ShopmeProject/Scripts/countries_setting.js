var buttonLoad;
var dropDownCountry;
var buttonAddCountry;
var buttonUpdateCountry;
var buttonDeleteCountry;
var labelCountryName;
var fieldCountryName;
var fieldCountryCode;

$(document).ready(function () {
	buttonLoad = $("#buttonLoadCountries");
	dropDownCountry = $("#dropDownCountries");
	buttonAddCountry = $("#buttonAddCountry");
	buttonUpdateCountry = $("#buttonUpdateCountry");
	buttonDeleteCountry = $("#buttonDeleteCountry");
	labelCountryName = $("#labelCountryName");
	fieldCountryName = $("#fieldCountryName");
	fieldCountryCode = $("#fieldCountryCode");

	fieldCountryName.prop("disabled", true);
	fieldCountryCode.prop("disabled", true);

	buttonLoad.click(function () {
		
		loadCountries();
	});

	dropDownCountry.on("change", function () {
		changeFormStateToSelectedCountry();
	});

	buttonAddCountry.click(function () {
		
		if (buttonAddCountry.val() == "Add") {

			checkDuplicateName().then(function (result) {
				if (result) {
					console.log("No duplicates found");
					addCountry();
					// Do something if there are no duplicates
				} else {
					console.log("Duplicates found");
					showToastMessage("Country already added !");
					// Do something if there are duplicates
				}
			}).catch(function (error) {
				console.log(error);
				showToastMessage("Unknown response from server");
				// Do something if there was an error
			});
		
			//if (checkUnique()) {
				
			//	addCountry();
			//}
		} else {
			changeFormStateToNewCountry();
		}
	});

	buttonUpdateCountry.click(function () {
		updateCountry();
	});

	buttonDeleteCountry.click(function () {
		deleteCountry();
	});
});

function deleteCountry() {
	optionValue = dropDownCountry.val();
	countryId = optionValue.split("-")[0];

	url = contextPath + "/Delete/" + countryId;

	$.ajax({
		type: 'POST',
		url: url
		
	}).done(function () {
		$("#dropDownCountries option[value='" + optionValue + "']").remove();
		changeFormStateToNewCountry();
		showToastMessage("The country has been deleted");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function updateCountry() {

	if (!validateFormCountry()) return;

	url = contextPath + "/Save";
	countryName = fieldCountryName.val();
	countryCode = fieldCountryCode.val();

	countryId = dropDownCountry.val().split("-")[0];

	jsonData = { Id: countryId, Name: countryName, Code: countryCode };

	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(jsonData),
		contentType: 'application/json'
	}).done(function (countryId) {
		$("#dropDownCountries option:selected").val(countryId + "-" + countryCode);
		$("#dropDownCountries option:selected").text(countryName);
		showToastMessage("The country has been updated");

		changeFormStateToNewCountry();
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function addCountry() {

	if (!validateFormCountry()) return;

	url = contextPath + "/Save";
	countryName = fieldCountryName.val();
	countryCode = fieldCountryCode.val();
	jsonData = { Name: countryName, Code: countryCode };
	//console.log(url);
	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(jsonData),
		contentType: 'application/json'
	}).done(function (countryId) {
		selectNewlyAddedCountry(countryId, countryCode, countryName);
		showToastMessage("The new country has been added");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});

}

function selectNewlyAddedCountry(countryId, countryCode, countryName) {
	optionValue = countryId + "-" + countryCode;
	$("<option>").val(optionValue).text(countryName).appendTo(dropDownCountry);

	$("#dropDownCountries option[value='" + optionValue + "']").prop("selected", true);

	fieldCountryCode.val("");
	fieldCountryName.val("").focus();
}

function changeFormStateToNewCountry() {
	buttonAddCountry.val("Add");
	labelCountryName.text("Country Name:");

	fieldCountryName.prop("disabled", false);
	fieldCountryCode.prop("disabled", false);

	buttonUpdateCountry.prop("disabled", false);
	buttonDeleteCountry.prop("disabled", false);

	fieldCountryCode.val("");
	fieldCountryName.val("").focus();
}

function changeFormStateToSelectedCountry() {
	buttonAddCountry.prop("value", "New");

	fieldCountryName.prop("disabled", false);
	fieldCountryCode.prop("disabled", false);

	buttonUpdateCountry.prop("disabled", false);
	buttonDeleteCountry.prop("disabled", false);

	labelCountryName.text("Selected Country:");

	selectedCountryName = $("#dropDownCountries option:selected").text();
	fieldCountryName.val(selectedCountryName);

	countryCode = dropDownCountry.val().split("-")[1];
	fieldCountryCode.val(countryCode);

}

function loadCountries() {
	url = contextPath + "/listAll";
	$.get(url, function (responseJSON) {
		dropDownCountry.empty();

		$.each(responseJSON, function (index, country) {
			optionValue = country.Id + "-" + country.Code;
			
			$("<option>").val(optionValue).text(country.Name).appendTo(dropDownCountry);
		});

	}).done(function () {
		buttonLoad.val("Refresh Country List");
		showToastMessage("All countries have been loaded");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function showToastMessage(message) {
	$("#toastMessage").text(message);
	$(".toast").toast('show');
}

function validateFormCountry() {
	formCountry = document.getElementById("formCountry");
	if (!formCountry.checkValidity()) {
		formCountry.reportValidity();
		return false;
	}

	return true;
}




function checkDuplicateName() {
	var countryName = $("#fieldCountryName").val();
	return new Promise(function (resolve, reject) {
		var jsonData = { name: countryName };
		var checkUniqueUrl = contextPath + "/CheckDuplicateName";
		$.ajax({
			type: "POST",
			url: checkUniqueUrl,
			data: JSON.stringify(jsonData),
			contentType: "application/json"
		}).done(function (response) {
			if (response === "OK") {
				resolve(true);
			} else if (response === "Duplicated") {
				resolve(false);
			} else {
				reject("Unknown response from server");
			}
		}).fail(function () {
			reject("ERROR: Could not connect to server or server encountered an error");
		});
	});
}
