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

    let singleEmojis = document.getElementsByClassName("singleEmoji");

    for (let singleEmoji of singleEmojis) {
        singleEmoji.addEventListener("click", function () {
            addEmoji(singleEmoji);
        });
    }

    let emojiSkinTimer;
    let isSelectedForSkin = false;
    let isEventsForPhone = false;

    let emojisWithSkins = this.documentElement.getElementsByClassName("hasEmojiSkin");
    for (let emojiWithSkin of emojisWithSkins) {
        emojiWithSkin.addEventListener("touchstart", function () {
            isEventsForPhone = true;
            emojiSkinTimer = setTimeout(function sayHi() {
                resetEmojiSkins([...emojisWithSkins].filter(x => x.parentNode.children[1].style.visibility == "visible"));
                emojiWithSkin.parentNode.children[1].style.visibility = "visible";
                isSelectedForSkin = true;
            }, 350)
        });
        emojiWithSkin.addEventListener("touchend", function () {
            clearTimeout(emojiSkinTimer);
            if (!isSelectedForSkin) {
                addEmoji(emojiWithSkin);
            } else {
                isSelectedForSkin = false;
            }
        });

        emojiWithSkin.addEventListener("mousedown", function () {
            emojiSkinTimer = setTimeout(function sayHi() {
                resetEmojiSkins([...emojisWithSkins].filter(x => x.parentNode.children[1].style.visibility == "visible"));
                emojiWithSkin.parentNode.children[1].style.visibility = "visible";
                isSelectedForSkin = true;
            }, 350)
        });
        emojiWithSkin.addEventListener("mouseup", function () {
            clearTimeout(emojiSkinTimer);
            if (!isSelectedForSkin && !isEventsForPhone) {
                addEmoji(emojiWithSkin);
            } else {
                isSelectedForSkin = false;
                isEventsForPhone = false;
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