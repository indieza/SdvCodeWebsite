$(".collapseIconSticker").on('click', function () {
    $(this).children('.fa-chevron-down, .fa-chevron-up').toggleClass("fa-chevron-down fa-chevron-up");
    $(this).toggleClass("activeCollapse")
});