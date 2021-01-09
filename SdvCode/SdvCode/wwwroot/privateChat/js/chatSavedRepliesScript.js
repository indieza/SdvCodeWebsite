$(document).ready(function () {
    $('#savedRepliesButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $('#savedRepliesSpan').toggleClass("show");
        $('#themeSpan').removeClass("show");
        $('#popupEmoji').removeClass("show");
        $('#chatStickerSpan').removeClass("show");
    });

    $('#savedRepliesSpan').click(function (e) {
        e.stopPropagation();
    });

    $('body').click(function () {
        $('#savedRepliesSpan').removeClass("show");
    });
});

function pasteQuickReply(element) {
    document.getElementById("messageInput").innerHTML += element.innerHTML;
}

function addQuickReply() {
    let quickReplyText = document.getElementById('messageInput').innerHTML;
    let group = document.getElementById("groupName").textContent;
    let toUser = document.getElementById("toUser").textContent;

    $.ajax({
        type: "POST",
        url: `/PrivateChat/With/${toUser}/Group/${group}/AddChatQuickReply`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'username': toUser,
            'group': group,
            'quickReplyText': quickReplyText
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            console.log(data);
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}