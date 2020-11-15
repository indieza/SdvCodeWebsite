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
        let allSkins = document.getElementsByClassName("hasEmojiSkin");
        for (var skin of allSkins) {
            if (skin.parentNode.children[1].style.visibility == "visible") {
                skin.parentNode.children[1].style.visibility = "";
                skin.setAttribute('onclick', 'addEmoji();'); // for FF
                skin.onclick = function () { addEmoji(event.currentTarget); }; // for IE
            }
        }
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
    let src = emojiSpan.src;
    document.getElementById("messageInput").innerHTML += `<img style="width: 1.4em;" src="${src}" /> `;
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
    let allSkins = document.getElementsByClassName("hasEmojiSkin");
    for (var skin of allSkins) {
        if (skin.parentNode.children[1].style.visibility == "visible") {
            skin.parentNode.children[1].style.visibility = "";
            skin.setAttribute('onclick', 'addEmoji();'); // for FF
            skin.onclick = function () { addEmoji(event.currentTarget); }; // for IE
        }
    }
}

function addEmojiSkin(emojiSkin) {
    let src = emojiSkin.src;
    document.getElementById("messageInput").innerHTML += `<img style="width: 1.4em;" src="${src}" /> `;
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
    let allSkins = document.getElementsByClassName("hasEmojiSkin");
    for (var skin of allSkins) {
        if (skin.parentNode.children[1].style.visibility == "visible") {
            skin.parentNode.children[1].style.visibility = "";
            skin.setAttribute('onclick', 'addEmoji();'); // for FF
            skin.onclick = function () { addEmoji(event.currentTarget); }; // for IE
        }
    }
}

$(function () {
    $(".hasEmojiSkin").bind("taphold", tapholdHandler);
    $(".hasEmojiSkin").on("contextmenu", function (e) {
        return false;
    });

    function tapholdHandler(event) {
        let allSkins = document.getElementsByClassName("hasEmojiSkin");
        for (var skin of allSkins) {
            if (skin.parentNode.children[1].style.visibility == "visible") {
                skin.parentNode.children[1].style.visibility = "";
                skin.setAttribute('onclick', 'addEmoji();'); // for FF
                skin.onclick = function () { addEmoji(event.currentTarget); }; // for IE
            }
        }
        event.currentTarget.removeAttribute("onclick");
        event.currentTarget.parentNode.children[1].style.visibility = "visible";
    }
});