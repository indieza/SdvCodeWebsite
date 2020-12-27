"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/privateChatHub").build();

function updateScroll() {
    var element = document.getElementById("demo-chat-body");
    element.scrollTop = element.scrollHeight;
}

function userType(toUsername, fromUsername, fromUserImageUrl) {
    connection.invoke("UserType", fromUsername, toUsername, fromUserImageUrl).catch(function (err) {
        return console.error(err.toString());
    });
}
function userStopType(toUsername) {
    connection.invoke("UserStopType", toUsername).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("VisualizeUserType", function (fromUsername, fromUserImageUrl) {
    let divElement = document.getElementById("typeMessage");

    if (!divElement) {
        document.getElementById("messagesList").innerHTML += `
        <li class="mar-btm" id="typeMessage">
            <div class="media-left">
                <img src=${fromUserImageUrl} class="img-circle img-sm" alt="Profile Picture">
            </div>
            <div class="media-body pad-hor">
                <div class="speech">
                    <a asp-area="" asp-controller="Profile" asp-action="Index" asp-route-username="${fromUsername}" class="media-heading">${fromUsername}</a>
                    <p>
                        Typing
                        <span class="jumping-dots">
                            <span class="jumping-dot-1"></span>
                            <span class="jumping-dot-2"></span>
                            <span class="jumping-dot-3"></span>
                        </span>
                    </p>
                </div>
            </div>
        </li>`;
        updateScroll();
    }
});

connection.on("VisualizeUserStopType", function () {
    let divElement = document.getElementById("typeMessage");

    if (divElement) {
        document.getElementById("messagesList").removeChild(divElement);
    }
})

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

connection.on("ReceiveThemeUpdate", function (themeUrl) {
    let panel = document.getElementById("chatPanel");
    if (panel) {
        panel.style.backgroundImage = `url(${themeUrl})`;
    }

    let oldBadge = document.querySelector(".theme-image .select-theme-badge");
    if (oldBadge) {
        let child = oldBadge.parentElement.children[1];
        oldBadge.parentElement.removeChild(child);
    }

    let allImages = document.querySelectorAll(".theme-image img");
    let targetImage = [...allImages].find(x => x.src == themeUrl);

    if (targetImage) {
        targetImage.parentElement.innerHTML += `<span class="select-theme-badge">
                                                    <i class="fas fa-check"></i>
                                                </span>`;
    }
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

connection.on("UpdateImagesUploadCount", function (count) {
    document.querySelector(".select-image-badge").innerHTML = count;
});

connection.on("UpdateFilesUploadCount", function (count) {
    document.querySelector(".select-file-badge").innerHTML = count;
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
    let toUser = document.getElementById("toUser").textContent;
    let fromUser = document.getElementById("fromUser").textContent;
    let message = document.getElementById("messageInput").innerHTML;
    let group = document.getElementById("groupName").textContent;
    let images = document.getElementById("uploadImage").files;
    let files = document.getElementById("uploadFile").files;
    let data = new FormData();

    for (var i = 0; i < images.length; i++) {
        data.append('files', images[i]);
    }

    for (var i = 0; i < files.length; i++) {
        data.append('files', files[i]);
    }

    if (message && images.length == 0 && files.length == 0) {
        connection.invoke("SendMessage", fromUser, toUser, message, group).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke("ReceiveMessage", fromUser, message, group).catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById("messageInput").value = "";
    } else {
        data.append('toUsername', toUser);
        data.append('fromUsername', fromUser);
        data.append('group', group);
        data.append('message', message);

        if (images.length > 0 || files.length > 0) {
            if (images.length > 0) {
                document.getElementById("imageSpinner").style.display = "block";
                document.querySelector("#imageButton i").classList = "";
                document.querySelector("#imageButton i").classList.add("fas", "fa-cloud-upload-alt");
                document.getElementById("uploadImage").disabled = true;
            }

            if (files.length > 0) {
                document.getElementById("fileSpinner").style.display = "block";
                document.querySelector("#fileButton i").classList = "";
                document.querySelector("#fileButton i").classList.add("fas", "fa-file-upload");
                document.getElementById("uploadFile").disabled = true;
            }

            $.ajax({
                url: `/PrivateChat/With/${toUser}/Group/${group}/SendFiles`,
                processData: false,
                contentType: false,
                type: "POST",
                data: data,
                success: function (result) {
                    document.getElementById("imageSpinner").style.display = "none";
                    document.querySelector("#imageButton i").classList = "";
                    document.querySelector("#imageButton i").classList.add("far", "fa-images");
                    document.querySelector(".select-image-badge").innerHTML = "0";
                    document.getElementById("uploadImage").disabled = false;

                    document.getElementById("fileSpinner").style.display = "none";
                    document.querySelector("#fileButton i").classList = "";
                    document.querySelector("#fileButton i").classList.add("fas", "fa-paperclip");
                    document.querySelector(".select-file-badge").innerHTML = "0";
                    document.getElementById("uploadFile").disabled = false;
                },
                error: function (err) {
                    console.log(err.statusText);
                }
            });

            document.getElementById("uploadImage").value = "";
            document.getElementById("uploadFile").value = "";

            let imageBadge = document.querySelector(".select-image-badge");
            imageBadge.style.boxShadow = "";
            imageBadge.style.animation = "";
            imageBadge.textContent = "0";

            let fileBadge = document.querySelector(".select-file-badge");
            fileBadge.style.boxShadow = "";
            fileBadge.style.animation = "";
            fileBadge.textContent = "0";
        }
    }

    document.getElementById("messageInput").innerHTML = "";
    event.preventDefault();
});

function updateInputScroller() {
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
}

function zoomChatImage(imageUrl) {
    document.querySelector(".modalChatImage").style.display = "block";
    document.querySelector("#image-content").src = imageUrl;
}

function closeChatZoomedImage() {
    document.querySelector(".modalChatImage").style.display = "none";
}

function sendStickerMessage(stickerUrl) {
    let toUser = document.getElementById("toUser").textContent;
    let fromUser = document.getElementById("fromUser").textContent;
    let group = document.getElementById("groupName").textContent;

    if (toUser && fromUser && group && stickerUrl) {
        connection.invoke("SendStickerMessage", fromUser, toUser, group, stickerUrl).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke("ReceiveStickerMessage", fromUser, group, stickerUrl).catch(function (err) {
            return console.error(err.toString());
        });
    }
}