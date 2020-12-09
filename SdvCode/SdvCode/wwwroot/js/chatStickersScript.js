$(document).ready(function () {
    $('#stickersButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        let span = document.getElementById("chatStickerSpan");
        if (span) {
            if (span.style.visibility == "visible") {
                span.style.visibility = "";
            } else {
                span.style.visibility = "visible";
            }
        }
    });

    $('body').click(function () {
        let span = document.getElementById("chatStickerSpan");
        if (span) {
            span.style.visibility = "";
        }
    });
});