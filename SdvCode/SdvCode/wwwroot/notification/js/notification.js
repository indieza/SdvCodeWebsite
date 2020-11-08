"use strict"

let notificationConnection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

notificationConnection.start().then(function () {
    if (!sessionStorage.getItem("isFirstNotificaitonSound")) {
        sessionStorage.setItem("isFirstNotificaitonSound", true);
    } else {
        sessionStorage.setItem("isFirstNotificaitonSound", false);
    }
    let isFirstNotificaitonSound = sessionStorage.getItem("isFirstNotificaitonSound") == "true" ? true : false;
    notificationConnection.invoke("GetUserNotificationCount", isFirstNotificaitonSound).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

notificationConnection.on("ReceiveNotification", function (count, isFirstNotificaitonSound) {
    document.getElementById("notificationCount").innerText = count;
    let title = document.querySelector("head title");
    let bracketIndex = title.innerText.indexOf(")");
    let newTitle = "";

    if (count > 0) {
        if (isFirstNotificaitonSound) {
            document.querySelector("audio").load();
            document.querySelector("audio").play();
        }

        document.getElementById("notificationCount").classList.add("notificationCircle", "notificationPulse");
        newTitle = `(${count}) ${title.innerText.substring(bracketIndex + 1, title.innerText.length)}`;
    } else {
        document.getElementById("notificationCount").classList.remove("notificationCircle", "notificationPulse");
        newTitle = `${title.innerText.substring(bracketIndex + 1, title.innerText.length)}`;
    }

    title.innerText = newTitle;
});

notificationConnection.on("VisualizeNotification", function (notification) {
    let div = document.getElementById("allUserNotifications");

    if (div) {
        let newNotification = createNotification(notification);
        if (div.children.length % 5 == 0 && div.children.length > 0) {
            let lastNotification = div.lastElementChild;
            div.removeChild(lastNotification);
            document.getElementById("loadMoreNotifications").disabled = false;
        }
        if (div.children.length == 0) {
            div.appendChild(newNotification);
        } else {
            div.insertBefore(newNotification, div.childNodes[0]);
        }
    }
});

function createNotification(notification) {
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