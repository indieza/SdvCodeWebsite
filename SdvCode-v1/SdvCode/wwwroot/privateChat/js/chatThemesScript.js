$(document).ready(function () {
    $('#themeButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#themeSpan').toggleClass("show");
        $('#savedRepliesSpan').removeClass("show");
        $('#popupEmoji').removeClass("show");
        $('#chatStickerSpan').removeClass("show");
    });

    $('#themeSpan').click(function (e) {
        e.stopPropagation();
    });

    $('body').click(function () {
        $('#themeSpan').removeClass("show");
    });
});

function changeTheme(imageTag, themeId, themeName, themeUrl) {
    let panel = document.getElementById("chatPanel");
    if (panel) {
        panel.style.backgroundImage = `url(${themeUrl})`;
    }

    let oldBadge = document.querySelector(".theme-image .select-theme-badge");

    if (oldBadge) {
        let child = oldBadge.parentElement.children[1];
        oldBadge.parentElement.removeChild(child);
    }
    imageTag.parentElement.innerHTML += `<span class="select-theme-badge">
                                          <i class="fas fa-check"></i>
                                      </span>`;

    addGroupThemeToDatabase(themeId);
}

function addGroupThemeToDatabase(themeId) {
    let group = document.getElementById("groupName").textContent;
    let toUser = document.getElementById("toUser").textContent;

    $.ajax({
        type: "POST",
        url: `/PrivateChat/With/${toUser}/Group/${group}/ChangeChatTheme`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'username': toUser,
            'group': group,
            'themeId': themeId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}