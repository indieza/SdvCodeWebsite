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
        resetEmojiSkins(allSkins);
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
    resetEmojiSkins(allSkins);
}

function addEmojiSkin(emojiSkin) {
    let src = emojiSkin.src;
    document.getElementById("messageInput").innerHTML += `<img style="width: 1.4em;" src="${src}" /> `;
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
    let allSkins = document.getElementsByClassName("hasEmojiSkin");
    resetEmojiSkins(allSkins);
}

$(function () {
    $(".hasEmojiSkin").on("contextmenu", function (e) {
        return false;
    });
    var start;
    var end;
    var delta;
    var isPhone = false;

    let emojisWithSkins = this.documentElement.getElementsByClassName("hasEmojiSkin");
    for (let emojiWithSkin of emojisWithSkins) {
        emojiWithSkin.addEventListener("touchstart", function () {
            start = new Date();
            isPhone = true;
        });
        emojiWithSkin.addEventListener("touchend", function () {
            end = new Date();
            delta = (end - start) / 1000.0;
            if (delta >= 0.5) {
                resetEmojiSkins(emojisWithSkins);
                emojiWithSkin.parentNode.children[1].style.visibility = "visible";
            } else {
                addEmoji(emojiWithSkin);
            }
        });

        emojiWithSkin.addEventListener("mousedown", function () {
            start = new Date();
        });
        emojiWithSkin.addEventListener("mouseup", function () {
            end = new Date();
            delta = (end - start) / 1000.0;
            if (delta >= 0.5) {
                resetEmojiSkins(emojisWithSkins);
                emojiWithSkin.parentNode.children[1].style.visibility = "visible";
            } else {
                if (!isPhone) {
                    addEmoji(emojiWithSkin);
                } else {
                    isPhone = false;
                }
            }
        });
    }
});

function resetEmojiSkins(emojisWithSkins) {
    for (var skin of emojisWithSkins) {
        if (skin.parentNode.children[1].style.visibility == "visible") {
            skin.parentNode.children[1].style.visibility = "";
        }
    }
}