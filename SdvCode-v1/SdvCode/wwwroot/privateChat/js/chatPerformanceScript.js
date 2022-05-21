$(document).ready(function () {
    $("#messageInput").on('input', function () {
        $('#messageInput').css('padding-left', '30px');
        if (document.getElementById('messageInput').innerHTML) {
            $('#messageInput').css('padding-left', '30px');
            document.getElementById("quickRepliesAddBadge").style.visibility = "visible";
        } else {
            $('#messageInput').css('padding-left', '10px');
            document.getElementById("quickRepliesAddBadge").style.visibility = "hidden";
        }
    });

    $("#demo-chat-body").scroll(function () {
        let scrollHeight = document.getElementById("demo-chat-body").scrollHeight;
        let scrollDistanceToTop = document.getElementById("demo-chat-body").scrollTop;
        let chatBoyHeight = document.getElementById("demo-chat-body").offsetHeight;
        let distanceToBottom = scrollHeight - scrollDistanceToTop - chatBoyHeight;

        if (distanceToBottom > 400) {
            document.getElementById("scrollBottomButton").style.visibility = "visible";
        } else if (distanceToBottom >= 0 && distanceToBottom <= 100) {
            document.getElementById("scrollBottomButton").style.visibility = "hidden";
        }
        if ($("#demo-chat-body").scrollTop() == 0) {
            let messagesSkipCount = document.getElementById("messagesSkipCount").value;
            let username = document.getElementById("toUser").textContent;
            let group = document.getElementById("groupName").textContent;

            if (messagesSkipCount && username && group) {
                $.ajax({
                    type: "GET",
                    url: `/PrivateChat/With/${username}/Group/${group}/LoadMoreMessages/${messagesSkipCount}`,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: {
                        'username': username,
                        'group': group,
                        'messagesSkipCount': messagesSkipCount
                    },
                    headers: {
                        RequestVerificationToken:
                            $('input:hidden[name="__RequestVerificationToken"]').val()
                    },
                    success: function (data) {
                        if (data.length > 0) {
                            let oldCount = parseInt(document.getElementById("messagesSkipCount").value)
                            document.getElementById("messagesSkipCount").value = oldCount + data.length;
                            let oldScrollHeight = document.getElementById("demo-chat-body").scrollHeight;

                            for (var message of data) {
                                let newMessage = document.createElement("li");
                                newMessage.classList.add(["mar-btm"]);
                                newMessage.id = message.id;
                                if (message.fromUsername == message.currentUsername) {
                                    newMessage.innerHTML += `
                                            <div class="media-right">
                                                <img src=${message.fromImageUrl} class="img-circle img-sm" alt="Profile Picture">
                                            </div>
                                            <div class="media-body pad-hor speech-right">
                                                <div class="speech">
                                                    <a href="/Profile/${message.fromUsername}" class="media-heading">${message.fromUsername}</a>
                                                    <p>${message.content}</p>
                                                    <p class="speech-time">
                                                        <i class="fa fa-clock-o fa-fw"></i> ${message.sendedOn}
                                                    </p>
                                                </div>
                                            </div>`;
                                } else {
                                    newMessage.innerHTML += `
                                            <div class="media-left">
                                                <img src=${message.fromImageUrl} class="img-circle img-sm" alt="Profile Picture">
                                            </div>
                                            <div class="media-body pad-hor">
                                                <div class="speech">
                                                    <a href="/Profile/${message.fromUsername}" class="media-heading">${message.fromUsername}</a>
                                                    <p>${message.content}</p>
                                                    <p class="speech-time">
                                                        <i class="fa fa-clock-o fa-fw"></i> ${message.sendedOn}
                                                    </p>
                                                </div>
                                            </div>`
                                }

                                let firstMessage = document.getElementById("messagesList").firstChild;
                                document.getElementById("messagesList").insertBefore(newMessage, firstMessage);
                            }

                            let scroll = document.getElementById("demo-chat-body");
                            let newScrollTop = scroll.scrollHeight - oldScrollHeight;
                            scroll.scrollTop = newScrollTop;
                        }
                    },
                    error: function (msg) {
                        console.error(msg);
                    }
                });
            }
        }
    });
});

function scrollChatToBottom() {
    $("#demo-chat-body").animate({
        scrollTop: document.getElementById("demo-chat-body").scrollHeight
    }, 800);
}