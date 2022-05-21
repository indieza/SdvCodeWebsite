let savedNotificationsForHide = [];

function updateStatus(newStatus, id) {
    let colors = {
        "Read": "green",
        "Unread": "red",
        "Pinned": "blue"
    };

    $.ajax({
        type: "POST",
        url: `/UserNotifications/Notification/EditStatus`,
        data: {
            'newStatus': newStatus,
            'id': id
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data) {
                document.getElementById(`${id}orderStatus`).innerText = newStatus;
                document.getElementById(`${id}orderStatus`).style.color = colors[newStatus];
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function deleteNotification(id) {
    $.ajax({
        type: "POST",
        url: `/UserNotifications/Notification/DeleteNotification`,
        data: {
            'id': id
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data) {
                let notification = document.getElementById(id);
                document.getElementById(`allUserNotifications`).removeChild(notification);
                loadMoreNotifications(1, true);
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

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

function loadMoreNotifications(take, isForDeleted) {
    let skip = document.getElementById("allUserNotifications").children.length;
    let div = document.getElementById("allUserNotifications");

    $.ajax({
        type: "GET",
        url: `/UserNotifications/Notification/GetMoreNotitification`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            "skip": skip,
            "take": take
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            for (var notification of data.newNotifications) {
                let result = createNotification(notification);
                div.appendChild(result);
            }
            if (!data.hasMore) {
                document.getElementById("loadMoreNotifications").disabled = true;
            }
            if (isForDeleted) {
                document.getElementById("loadLessNotifications").style.display = "none";
            } else {
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

    for (var i = 0; i < Math.min(maxCount, div.children.length % maxCount + div.children.length / maxCount); i++) {
        div.removeChild(div.lastChild);
    }

    if (div.children.length <= maxCount) {
        document.getElementById("loadLessNotifications").style.display = "none";
    }

    document.getElementById("loadMoreNotifications").disabled = false;
}