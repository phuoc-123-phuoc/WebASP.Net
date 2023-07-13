$(document).ready(function () {
    $('#buttonCancel').click(function () {
        window.location.href = indexUrl;
    });
    $("#fileImage")
        .change(
            function () {
                fileSize = this.files[0].size;

                if (fileSize > 1048576) {
                    this
                        .setCustomValidity("You must choose an image less than 1MB!");
                    this.reportValidity();
                } else {
                    this.setCustomValidity("");
                    showImageThumbnail(this);
                }
            });
});

function showImageThumbnail(fileInput) {
    var file = fileInput.files[0];
    var reader = new FileReader();
    reader.onload = function (e) {
        $("#thumbnail").attr("src", e.target.result);
    };

    reader.readAsDataURL(file);
}

function checkNameUnique(form) {

    url = CheckDuplicateNamelUrl;
    categoryName = $("#category_Name").val();
    categoryId = $("#category_Id").val();

    urlAlias = CheckDuplicateAliaslUrl;
    categoryAlias = $("#category_Alias").val();
    params = { id: categoryId, name: categoryName };
    $.post(url, params, function (response) {
        if (response == "OK") {
            checkAliasUnique(urlAlias, categoryAlias, categoryId, form);
               
           
        } else if (response == "Duplicated") {
            showModalDialog("Warning", "There is another category having the name " + categoryName);

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
function checkAliasUnique(url, categoryAlias, categoryId,form) {

    params = { id: categoryId, Alias: categoryAlias };
    $.post(url, params, function (response) {
        if (response == "OK") {
            form.submit();
        } else if (response == "Duplicated") {
            showModalDialog("Warning", "There is another category having the alias " + categoryAlias);

        } else {
            showModalDialog("Error", "Unknown response from server");
        }

    }).fail(function () {
        showModalDialog("Error", "Could not connect to the server");
    });
    return false;
}