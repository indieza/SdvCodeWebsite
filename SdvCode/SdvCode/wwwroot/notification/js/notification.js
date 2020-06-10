"use strict"

let notificationConnection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
let savedNotificationsForHide = [];

notificationConnection.start().then(function () {
    notificationConnection.invoke("GetUserNotificationCount").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

notificationConnection.on("ReceiveNotification", function (count) {
    document.getElementById("notificationCount").innerText = count;
    if (count > 0) {
        document.querySelector("audio").play();
    }
});

notificationConnection.on("VisualizeNotification", function (notification, maxCount) {
    let div = document.getElementById("allUserNotifications");

    if (div) {
        let newNotification = createNotification(notification);
        if (div.children.length % maxCount == 0) {
            div.children.removeChild(div.lastChild);
        }

        div.insertBefore(newNotification, div.childNodes[0]);
    }
});

function createNotitfication(notification) {
    let newNotification = document.createElement("div");
    newNotification.id = notification.id;

    let allStatuses = "";

    for (var status of notification.allStatuses) {
        allStatuses += `<a onclick="updateStatus('${status}', '${notification.id}')">${status}</a>`;
    }

    newNotification.innerHTML =
        `<div class="ts-testimonial-content">
                        <img src="${notification.imageUrl}" alt="avatar">
                        <h4 class="ts-testimonial-text userNotificationsHeading">
                            <span>
                                <a class="deleteNotificationIcon" onclick="deleteNotification('${notification.id}')">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </span>
                            <span>
                                ${notification.heading}
                            </span>
                        </h4>
                        <div class="ts-testimonial-text dropdownNotification">
                            <button class="dropbtnNotification">
                                <i class="fas fa-chevron-down notificationArrow"></i>
                            </button>
                            <div class="dropdown-content-notification">
                                ${allStatuses}
                            </div>
                            <span>Status: </span>
                            <b>
                                <span id="${notification.id}orderStatus" style="color: red; text-transform: uppercase">
                                   ${notification.allStatuses[notification.status - 1]}
                                </span>
                            </b>
                        </div>
                        <p class="ts-testimonial-text">
                            ${notification.text}
                        </p>

                        <div class="ts-testimonial-author">
                            <h3 class="name userNotificationsHeading">
                                <a href="/Profile/${notification.targetUsername}">
                                    ${notification.targetFirstName} ${notification.targetLastName}
                                </a>
                                <span>
                                    ${notification.createdOn}
                                </span>
                            </h3>
                        </div>
                    </div>`;

    return newNotification;
}

function loadMoreNotifications() {
    let skip = document.getElementById("allUserNotifications").children.length;
    let div = document.getElementById("allUserNotifications");

    $.ajax({
        type: "GET",
        url: `/UserNotifications/Notification/GetMoreNotitification`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            "skip": skip
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            savedNotificationsForHide.push(data.length);
            for (var notification of data) {
                let result = createNotitfication(notification);
                div.appendChild(result);
                document.getElementById("loadLessNotifications").style.display = "";
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function hideNotifications(maxCount) {
    let div = document.getElementById("allUserNotifications");

    for (var i = 0; i < Math.min(savedNotificationsForHide[savedNotificationsForHide.length - 1], div.children.length); i++) {
        div.removeChild(div.lastChild);
    }

    savedNotificationsForHide.pop();

    if (div.children.length <= maxCount) {
        document.getElementById("loadLessNotifications").style.display = "none";
    }
}