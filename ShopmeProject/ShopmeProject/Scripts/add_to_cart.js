$(document).ready(function () {
	$("#buttonAdd2Cart").on("click", function (evt) {
		addToCart();
	});
});

function addToCart() {
	quantity = $("#quantity" + productId).val();
	url = contextPath + "?productId=" + productId + "&quantity=" + quantity;
	$.ajax({
		type: "POST",
		url: url
	}).done(function (response) {
		showModalDialog("Shopping Cart", response);
	}).fail(function () {
		showErrorModal("Error while adding product to shopping cart.");
	});
}