dropdownBrands = $("#brand");
dropdownCategories = $("#category");

$(document).ready(function () {

	$("#product_ShortDescription").richText();
	$("#product_FullDescription").richText();

	dropdownBrands.change(function () {
		dropdownCategories.empty();
		getCategories();
	});

	getCategoriesForNewForm();

});

function getCategoriesForNewForm() {
	catIdField = $("#categoryId");
	editMode = false;

	if (catIdField.val() !=0) {
		editMode = true;
	}

	if (!editMode) getCategories();
}

function getCategories() {
	brandId = dropdownBrands.val();
	url = brandModuleURL + "/" + brandId;

	$.get(url, function (responseJson) {
		$.each(responseJson, function (index, category) {
			$("<option>").val(category.Id).text(category.Name).appendTo(dropdownCategories);
		});
	});
}


function checkUnique(form) {

	url = CheckDuplicateNamelUrl;
	productName = $("#product_Name").val();
	productId = $("#product_Id").val();
	params = { id: productId, name: productName };
	$.post(url, params, function (response) {
		if (response == "OK") {
			form.submit();

		} else if (response == "Duplicated") {
			showModalDialog("Warning", "There is another product having the name " + productName);

		} else {
			showModalDialog("Error", "Unknown response from server");
		}

	}).fail(function () {
		showModalDialog("Error", "Could not connect to the server");
	});
	return false;
}
function showModalDialog(title, message) {
	$("#modalTitle").text(title);
	$("#modalBody").text(message);
	$("#modalDialog").modal();
}
