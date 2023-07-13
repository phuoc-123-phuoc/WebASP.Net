
//$(document).ready(function () {
//    $('form').submit(function (event) {
//        var passwordField = $('input[name="user.Password"]');
//        if (passwordField.val().trim() === '') {
//            alert("Pass word is require")
            
//            passwordField.focus();
//            event.preventDefault();
//        }
//    });
//});

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

function checkEmailUnique(form) {
    
    url = CheckDuplicateEmailUrl;
    userEmail = $("#user_Email").val();
    userId = $("#user_Id").val();
    params = { id: userId, email: userEmail };
    $.post(url, params, function (response) {
        if (response == "OK") {
            form.submit();
        } else if (response == "Duplicated") {
            showModalDialog("Warning", "There is another user having the email " + userEmail);

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
