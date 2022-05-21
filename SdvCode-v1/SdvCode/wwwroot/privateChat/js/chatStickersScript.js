$(document).ready(function () {
    $('#stickersButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#chatStickerSpan').toggleClass("show");
        $('#savedRepliesSpan').removeClass("show");
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

function removeStickerType(stickerTypeId) {
    $.ajax({
        type: "POST",
        url: `/PrivateChat/CollectStickers/RemoveStickerFromFavourite`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'stickerTypeId': stickerTypeId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data.isRemoved) {
                let removeIcon = document.getElementById(stickerTypeId);

                if (removeIcon) {
                    removeIcon.parentElement.removeChild(removeIcon);
                    let newIcon = document.querySelector(".stickersTypesTabs").firstElementChild;

                    if (newIcon) {
                        newIcon.firstElementChild.style.backgroundColor = "lightgrey";
                    }
                }

                let removeTab = document.getElementById(`${stickerTypeId}-Tab-Section`);

                if (removeTab) {
                    removeTab.parentElement.removeChild(removeTab);
                    let newTab = document.querySelector(".stickers-tab-container").firstElementChild;

                    if (newTab) {
                        newTab.style.display = "block";
                    }
                }
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}