﻿var buttonLoad4States;
var dropDownCountry4States;
var dropDownStates;
var buttonAddState;
var buttonUpdateState;
var buttonDeleteState;
var labelStateName;
var fieldStateName;

$(document).ready(function () {
	buttonLoad4States = $("#buttonLoadCountriesForStates");
	dropDownCountry4States = $("#dropDownCountriesForStates");
	dropDownStates = $("#dropDownStates");
	buttonAddState = $("#buttonAddState");
	buttonUpdateState = $("#buttonUpdateState");
	buttonDeleteState = $("#buttonDeleteState");
	labelStateName = $("#labelStateName");
	fieldStateName = $("#fieldStateName");

	fieldStateName.prop("disabled", true);

	buttonLoad4States.click(function () {
		loadCountries4States();
	});

	dropDownCountry4States.on("change", function () {
		loadStates4Country();
	});

	dropDownStates.on("change", function () {
		changeFormStateToSelectedState();
	});

	buttonAddState.click(function () {
		if (buttonAddState.val() == "Add") {

			checkDuplicateNameState().then(function (result) {
				if (result) {
					console.log("No duplicates found");
					addState();
					// Do something if there are no duplicates
				} else {
					console.log("Duplicates found");
					showToastMessage("State already added !");
					// Do something if there are duplicates
				}
			}).catch(function (error) {
				console.log(error);
				showToastMessage("Unknown response from server");
				// Do something if there was an error
			});

			//if (checkUnique()) {
			//	addState();
			//}
		} else {
			changeFormStateToNew();
		}
	});

	buttonUpdateState.click(function () {
		updateState();
	});

	buttonDeleteState.click(function () {
		deleteState();
	});
});

function deleteState() {
	stateId = dropDownStates.val();

	url = contextPath + "/DeleteState/" + stateId;

	$.ajax({
		type: 'POST',
		url: url
	}).done(function () {
		$("#dropDownStates option[value='" + stateId + "']").remove();
		changeFormStateToNew();
		showToastMessage("The state has been deleted");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function updateState() {

	if (!validateFormState()) return;

	url = contextPath + "/SaveState";
	stateId = dropDownStates.val();
	stateName = fieldStateName.val();

	selectedCountry = $("#dropDownCountriesForStates option:selected");
	countryId = selectedCountry.val();
	countryName = selectedCountry.text();

	jsonData = { Id: stateId, Name: stateName, CountryId: countryId };

	$.ajax({
		type: 'POST',
		url: url,
		data: JSON.stringify(jsonData),
		contentType: 'application/json'
	}).done(function (stateId) {
		$("#dropDownStates option:selected").text(stateName);
		showToastMessage("The state has been updated");
		changeFormStateToNew();
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function addState() {

	if (!validateFormState()) return;

	url = contextPath + "/SaveState";
	stateName = fieldStateName.val();

	selectedCountry = $("#dropDownCountriesForStates option:selected");
	countryId = selectedCountry.val();
	countryName = selectedCountry.text();

	jsonData = {Name: stateName, CountryId: countryId };

	$.ajax({
		type: 'POST',
		url: url,
		
		data: JSON.stringify(jsonData),
		contentType: 'application/json'
	}).done(function (stateId) {
		selectNewlyAddedState(stateId, stateName);
		showToastMessage("The new state has been added");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});

}

function selectNewlyAddedState(stateId, stateName) {
	$("<option>").val(stateId).text(stateName).appendTo(dropDownStates);

	$("#dropDownStates option[value='" + stateId + "']").prop("selected", true);

	fieldStateName.val("").focus();
}

function changeFormStateToNew() {
	buttonAddState.val("Add");
	labelStateName.text("State/Province Name:");

	buttonUpdateState.prop("disabled", true);
	buttonDeleteState.prop("disabled", true);

	fieldStateName.prop("disabled", false);

	fieldStateName.val("").focus();
}

function changeFormStateToSelectedState() {
	buttonAddState.prop("value", "New");
	buttonUpdateState.prop("disabled", false);
	buttonDeleteState.prop("disabled", false);

	fieldStateName.prop("disabled", false);

	labelStateName.text("Selected State/Province:");

	selectedStateName = $("#dropDownStates option:selected").text();
	fieldStateName.val(selectedStateName);

}

function loadStates4Country() {
	selectedCountry = $("#dropDownCountriesForStates option:selected");
	countryId = selectedCountry.val();
	url = contextPath + "/listAllStateByCountry/" + countryId;

	$.get(url, function (responseJSON) {
		dropDownStates.empty();

		$.each(responseJSON, function (index, state) {
			$("<option>").val(state.id).text(state.name).appendTo(dropDownStates);
		});

	}).done(function () {
		changeFormStateToNew();
		showToastMessage("All states have been loaded for country " + selectedCountry.text());
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function loadCountries4States() {
	url = contextPath + "/listAll";
	$.get(url, function (responseJSON) {
		dropDownCountry4States.empty();

		$.each(responseJSON, function (index, country) {
			$("<option>").val(country.Id).text(country.Name).appendTo(dropDownCountry4States);
		});

	}).done(function () {
		buttonLoad4States.val("Refresh Country List");
		showToastMessage("All countries have been loaded");
	}).fail(function () {
		showToastMessage("ERROR: Could not connect to server or server encountered an error");
	});
}

function validateFormState() {
	formState = document.getElementById("formState");
	if (!formState.checkValidity()) {
		formState.reportValidity();
		return false;
	}

	return true;
}



function checkDuplicateNameState() {
	var stateName = $("#fieldStateName").val();
	return new Promise(function (resolve, reject) {
		var jsonData = { name: stateName };
		var checkUniqueUrl = contextPath + "/CheckDuplicateNameState";
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