function enablePhone() {
    let code = document.getElementById("countryCodeDrop");

    if (code.value) {
        document.getElementById("profilePhoneNumber").disabled = false;
    }
}

$('#chooseProfileFile').bind('change', function () {
    var filename = $("#chooseProfileFile").val();
    if (/^\s*$/.test(filename)) {
        $("#chooseProfileImage").removeClass('active');
        $("#noFile").text("No file chosen...");
    }
    else {
        $("#chooseProfileImage").addClass('active');
        $("#noProfileFile").text(filename.replace("C:\\fakepath\\", ""));
    }
});

$('#chooseCoverFile').bind('change', function () {
    var filename = $("#chooseCoverFile").val();
    if (/^\s*$/.test(filename)) {
        $("#chooseCoverImage").removeClass('active');
        $("#noCoverFile").text("No file chosen...");
    }
    else {
        $("#chooseCoverImage").addClass('active');
        $("#noCoverFile").text(filename.replace("C:\\fakepath\\", ""));
    }
});