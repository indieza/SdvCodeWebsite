$(document).ready(function () {
    $("#demo-chat-body").scroll(function () {
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
                            console.log(document.getElementById("messagesSkipCount").value);

                            for (var message of data) {
                                let oldMessageList = document.getElementById("messagesList").innerHTML;
                                let newMessage = `<h1>El</h1>`;
                                document.getElementById("messagesList").innerHTML = newMessage + oldMessageList;
                            }
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