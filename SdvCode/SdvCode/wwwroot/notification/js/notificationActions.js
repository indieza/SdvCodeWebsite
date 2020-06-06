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
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}