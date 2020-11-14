$(document).ready(function () {
    $('#emojiButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#popupEmoji').toggleClass("show");
    });

    $('#popupEmoji').click(function (e) {
        e.stopPropagation();
    });

    $('body').click(function () {
        $('#popupEmoji').removeClass("show");
    });
});

function popUpEmoji() {
    let popup = document.getElementById("popupEmoji");
    popup.classList.add("show");
}

function changeEmojisTabs(emojiType) {
    let tabs = document.querySelectorAll(".emoji-type-tab");

    for (let tab of tabs) {
        if (tab.id == `${emojiType}-Tab`) {
            tab.style.backgroundColor = "#727272";
        } else {
            tab.style.backgroundColor = "";
        }
    }

    let tabsSections = document.querySelectorAll(".tab-section");

    for (let tabSection of tabsSections) {
        if (tabSection.id == `${emojiType}-Tab-Section`) {
            tabSection.style.display = "block";
        } else {
            tabSection.style.display = "none";
        }
    }
}

function addEmoji(emojiSpan) {
    let src = emojiSpan.children[0].src;
    document.getElementById("messageInput").innerHTML += `<img style="width: 1.4em;" src="${src}" /> `;
}