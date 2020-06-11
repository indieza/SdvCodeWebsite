"use strict"

let userStatusConnection = new signalR.HubConnectionBuilder().withUrl("/userStatusHub").build();

userStatusConnection.start().then(function () {
    console.log("connected");
}).catch(function (err) {
    return console.error(err.toString());
});