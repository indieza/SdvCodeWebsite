"use strict"

let userStatusConnection = new signalR.HubConnectionBuilder().withUrl("/userStatusHub").build();

userStatusConnection.start().then(function () {
    let h5 = document.getElementById("currentUsername");
    if (h5) {
        let username = h5.innerText.substr(1, h5.innerText.length);
        userStatusConnection.invoke("IsUserOnline", username)
            .catch(function (err) {
                return console.error(err.toString());
            });
    }

    let alldots = document.querySelectorAll(".status-circle");
    if (alldots.length > 0) {
        for (var user of alldots) {
            let username = user.children[0].value;
            userStatusConnection.invoke("IsUserOnline", username)
                .catch(function (err) {
                    return console.error(err.toString());
                });
        }
    }

    let postDot = document.querySelector(`.status-circle-blog-post`);
    if (postDot) {
        let username = postDot.children[0].value;
        userStatusConnection.invoke("IsUserOnline", username)
            .catch(function (err) {
                return console.error(err.toString());
            });
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
            <span class="tooltipUserStatus">User Online!</span>
            <i class="fas fa-check" style="color: white"></i>`;
    }

    let postDot = document.getElementById(`${username}usersBlogPostStatus`);
    if (postDot) {
        postDot.style.backgroundColor = "green";
        postDot.innerHTML = `
            <input type="hidden" value="${username}" />
            <span class="tooltipUserStatus">User Online!</span>
            <i class="fas fa-check" style="color: white; margin-top: 4px; font-size: x-large;"></i>`;
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
            <span class="tooltipUserStatus">User Offline!</span>
            <i class="fas fa-times" style="color: white"></i>`;
    }

    let postDot = document.getElementById(`${username}usersBlogPostStatus`);
    if (postDot) {
        postDot.style.backgroundColor = "red";
        postDot.innerHTML = `
            <input type="hidden" value="${username}" />
            <span class="tooltipUserStatus">User Offline!</span>
            <i class="fas fa-times" style="color: white; margin-top: 4px; font-size: x-large;"></i>`;
    }
});