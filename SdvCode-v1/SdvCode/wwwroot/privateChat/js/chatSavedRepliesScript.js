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

    $("#quickRepliesSearchInputFiled").on('input', function () {
        let searchField = document.getElementById("quickRepliesSearchInputFiled");
        let replies = document.querySelectorAll('.savedReplies-badge');

        for (var reply of replies) {
            if (reply.innerText.toUpperCase().includes(searchField.value.toUpperCase())) {
                reply.parentElement.style.display = "block";
            } else {
                reply.parentElement.style.display = "none";
            }
        }
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
            let isExist = document.getElementById(`${data.id}`);
            if (!isExist) {
                let collectionReplies = document.getElementById('repliesCollection');
                let firstChild = collectionReplies.firstChild;
                let newReply = document.createElement('div');
                newReply.classList.add(['savedReplies-frame'])
                newReply.id = data.id;
                newReply.innerHTML = `
                    <div class="savedReplies-badge" onclick="pasteQuickReply(this)">
                        ${data.reply}
                    </div>
                    <span class="delete-savedRelies-icon" onclick="deleteWuickReply('${data.id}')">
                        <i class="fas fa-trash-alt"></i>
                    </span>`;

                collectionReplies.insertBefore(newReply, firstChild);
                let newCount = parseInt(document.getElementById('quickRepliesCount').innerText) + 1;
                document.getElementById('quickRepliesCount').innerText = newCount;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}

function deleteWuickReply(id) {
    let group = document.getElementById("groupName").textContent;
    let toUser = document.getElementById("toUser").textContent;

    $.ajax({
        type: "POST",
        url: `/PrivateChat/With/${toUser}/Group/${group}/RemoveChatQuickReply`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'username': toUser,
            'group': group,
            'id': id
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data.item1) {
                let oldReply = document.getElementById(data.item2);
                document.getElementById("repliesCollection").removeChild(oldReply);
                let newCount = parseInt(document.getElementById('quickRepliesCount').innerText) - 1;
                document.getElementById('quickRepliesCount').innerText = newCount;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}