"use strict"

let userStatusConnection = new signalR.HubConnectionBuilder().withUrl("/userStatusHub").build();

userStatusConnection.start().then(function () {
    let h5 = document.getElementById("currentUsername");
    if (h5) {
        let username = h5.innerText.substr(1, h5.innerText.length);
        userStatusConnection.invoke("IsUserOnline", username)
            .catch(function (err) {
                return console.error(err.toString());
            });;
    }
}).catch(function (err) {
    return console.error(err.toString());
});

userStatusConnection.on("UserIsOnline", function (username) {
    document.getElementById(`${username}userSatus`).innerText = "Online";
    document.getElementById("userStatusDot").style.backgroundColor = "green";
});

userStatusConnection.on("UserIsOffline", function (username) {
    document.getElementById(`${username}userSatus`).innerText = "Offline";
    document.getElementById("userStatusDot").style.backgroundColor = "orange";
});