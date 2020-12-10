$(document).ready(function () {
    $('#stickersButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#chatStickerSpan').toggleClass("show");
        $('#popupEmoji').removeClass("show");
        $('#themeSpan').removeClass("show");
    });

    $('#chatStickerSpan').click(function (e) {
        e.stopPropagation();
    });

    $('body').click(function () {
        $('#chatStickerSpan').removeClass("show");
    });
});

function changeStickersTabs(tabSectionId) {
    let tabs = document.querySelectorAll(".singleStickerTypeForTab");

    for (let tab of tabs) {
        if (tab.id == `${tabSectionId}-Tab`) {
            tab.style.backgroundColor = "lightgrey";
        } else {
            tab.style.backgroundColor = "";
        }
    }

    let tabsSections = document.querySelectorAll(".sticker-tab-section");

    for (let tabSection of tabsSections) {
        if (tabSection.id == `${tabSectionId}-Tab-Section`) {
            tabSection.style.display = "block";
        } else {
            tabSection.style.display = "none";
        }
    }
}