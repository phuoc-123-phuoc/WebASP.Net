////function formatPrice(currencySymbol, currencySymbolPosition, productPrice, thousandsPointType, decimalDigits, decimalPointType) {
////    if (thousandsPointType === "POINT") {
////        thousandsPointType = ".";
////    } else {
////        thousandsPointType = ",";
////    }

////    if (decimalPointType === "POINT") {
////        decimalPointType = ".";
////    } else {
////        decimalPointType = ",";
////    }

////    var formattedPrice = "";

////    if (currencySymbolPosition === "Before price") {
////        formattedPrice += currencySymbol;
////    }

////    formattedPrice += productPrice.toFixed(decimalDigits).replace(".", decimalPointType);

////    formattedPrice = formattedPrice.replace(/\B(?=(\d{3})+(?!\d))/g, thousandsPointType);

////    if (currencySymbolPosition === "After price") {
////        formattedPrice += currencySymbol;
////    }

////    return formattedPrice;
////}

function formatPrice(currencySymbol, currencySymbolPosition, productPrice, thousandsPointType, decimalDigits, decimalPointType) {
    if (thousandsPointType === "POINT") {
        thousandsPointType = ".";
    } else {
        thousandsPointType = ",";
    }
    if (decimalPointType === "POINT") {
        decimalPointType = ".";
    } else {
        decimalPointType = ",";
    }

    var formattedPrice = "";
    if (currencySymbolPosition === "Before price") {
        formattedPrice += currencySymbol;
    }

    // Parse productPrice as a number
    var price = parseFloat(productPrice);

    // Check if the parsed price is valid
    if (!isNaN(price)) {
        formattedPrice += price.toFixed(decimalDigits);
        formattedPrice = formattedPrice.replace(/\B(?=(\d{3})+(?!\d))/g, thousandsPointType);
    } else {
        // Handle invalid productPrice
        formattedPrice = "Invalid price";
    }

    if (currencySymbolPosition === "After price") {
        formattedPrice += currencySymbol;
    }

    return formattedPrice;
}

