function changeActionStatus(id, username, newStatus) {
    let element = document.getElementById(id);
    let colors = {
        "Read": "green",
        "Unread": "red",
        "Pinned": "blue"
    };

    $.ajax({
        url: `/Profile/${username}/changeActionStatus`,
        type: "POST",
        data: {
            'username': username,
            'id': id,
            'newStatus': newStatus
        },
        success: function (msg) {
            element.textContent = msg;
            element.style.color = colors[msg];
        },
        error: function (msg) {
            alert(msg);
        }
    });
}