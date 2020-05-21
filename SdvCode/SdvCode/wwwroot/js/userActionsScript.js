function markActionAsRead(id, username) {
    let element = document.getElementById(id);
    $.ajax({
        url: `/Profile/${username}/MarkActionAsRead`,
        type: "POST",
        data: {
            username: username,
            id: id
        },
        success: function (msg) {
            element.textContent = msg;
            element.style.color = "green";
        },
        error: function (msg) {
            alert(msg);
        }
    });
}

function pinAction(id, username) {
    let element = document.getElementById(id);
    $.ajax({
        url: `/Profile/${username}/PinAction`,
        type: "POST",
        data: {
            username: username,
            id: id
        },
        success: function (msg) {
            element.textContent = msg;
            element.style.color = "blue";
        },
        error: function (msg) {
            alert(msg);
        }
    });
}