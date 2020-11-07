"use strict"

let userStatusConnection = new signalR.HubConnectionBuilder().withUrl("/userStatusHub").build();

userStatusConnection.start().then(function () {
    let h5 = document.getElementById("currentUsername");
    let alldots = document.querySelectorAll(".status-circle");
    if (h5) {
        let username = h5.innerText.substr(1, h5.innerText.length);
        userStatusConnection.invoke("IsUserOnline", username)
            .catch(function (err) {
                return console.error(err.toString());
            });
    }
    if (alldots) {
        for (var user of alldots) {
            let username = user.children[0].value;
            userStatusConnection.invoke("IsUserOnline", username)
                .catch(function (err) {
                    return console.error(err.toString());
                });
        }
    }
}).catch(function (err) {
    return console.error(err.toString());
});

userStatusConnection.on("UserIsOnline", function (username) {
    let profileStatus = document.getElementById(`${username}userSatus`);
    if (profileStatus) {
        profileStatus.innerText = "Online";
        document.getElementById("userStatusDot").style.backgroundColor = "green";
    }

    let allUserProfileStatus = document.getElementById(`${username}allUsersStatus`);
    if (allUserProfileStatus) {
        allUserProfileStatus.style.backgroundColor = "green";
        allUserProfileStatus.innerHTML = `
            <input type="hidden" value="${username}" />
            <i class="fas fa-check" style="color: white"></i>`;
    }
});

userStatusConnection.on("UserIsOffline", function (username) {
    let profileStatus = document.getElementById(`${username}userSatus`);
    if (profileStatus) {
        profileStatus.innerText = "Offline";
        document.getElementById("userStatusDot").style.backgroundColor = "red";
    }

    let allUserProfileStatus = document.getElementById(`${username}allUsersStatus`);
    if (allUserProfileStatus) {
        allUserProfileStatus.style.backgroundColor = "red";
        allUserProfileStatus.innerHTML = `
            <input type="hidden" value="${username}" />
            <i class="fas fa-times" style="color: white"></i>`;
    }
});