﻿$('#chooseProfileFile').bind('change', function () {
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

function checkCountryCode(field) {
    let code = field.value;
    let pattern = new RegExp('^(\\+{1}\\d{1,3}|\\+{1}\\d{1,4})$');

    if (!pattern.test(code)) {
        alert("Invalid Country Code, example +359, +1, +569 etc.");
        field.value = "";
    }
}