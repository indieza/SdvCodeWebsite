"use strict";

function markTableRow() {
    $("tr").hover(function () {
        let day = new Date().getDate();
        if (day % 2 == 0) {
            $(this).css("background-color", "#fff995");
        } else {
            $(this).css("background-color", "#d0ff95");
        }
    }, function () {
        $(this).css("background-color", "white");
    });
}