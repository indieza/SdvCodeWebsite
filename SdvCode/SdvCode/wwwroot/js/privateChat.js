"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/privateChatHub").build();

function updateScroll() {
    var element = document.getElementById("demo-chat-body");
    element.scrollTop = element.scrollHeight;
}

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, image, message) {
    var msg = message;//message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let dateTime = new Date()
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    var li = document.createElement("li");

    li.classList.add("mar-btm");
    li.innerHTML = `<div class="media-left">
                                        <img src=${image} class="img-circle img-sm" alt="Profile Picture">
                                    </div>
                                    <div class="media-body pad-hor">
                                        <div class="speech">
                                            <a href="/Profile/${user}" class="media-heading">${user}</a>
                                            <p>${msg}</p>
                                            <p class="speech-time">
                                                <i class="fa fa-clock-o fa-fw"></i>${formattedDate}
                                            </p>
                                        </div>
                                    </div>`;
    document.getElementById("messagesList").appendChild(li);
    updateScroll();
});

connection.on("SendMessage", function (user, image, message) {
    var msg = message;//message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let dateTime = new Date()
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    var li = document.createElement("li");

    li.classList.add("mar-btm");
    li.innerHTML = `<div class="media-right">
                                        <img src=${image} class="img-circle img-sm" alt="Profile Picture">
                                    </div>
                                    <div class="media-body pad-hor speech-right">
                                        <div class="speech">
                                            <a href="/Profile$/{user}" class="media-heading">${user}</a>
                                            <p>${msg}</p >
                                            <p class="speech-time">
                                                <i class="fa fa-clock-o fa-fw"></i> ${formattedDate}
                                            </p>
                                        </div>
                                    </div>`;
    document.getElementById("messagesList").appendChild(li);
    updateScroll();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    var toUser = document.getElementById("toUser").textContent;
    var fromUser = document.getElementById("fromUser").textContent;
    var group = document.getElementById("groupName").textContent;

    connection.invoke("AddToGroup", `${group}`, toUser, fromUser).catch(function (err) {
        return console.error(err.toString());
    });
    connection.invoke("UpdateMessageNotifications", toUser, fromUser).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var toUser = document.getElementById("toUser").textContent;
    var fromUser = document.getElementById("fromUser").textContent;
    var message = document.getElementById("messageInput").innerHTML;
    var group = document.getElementById("groupName").textContent;

    if (message) {
        connection.invoke("SendMessage", fromUser, toUser, message, group).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke("ReceiveMessage", fromUser, message, group).catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById("messageInput").value = "";
    }
    document.getElementById("messageInput").innerHTML = "";
    event.preventDefault();
});