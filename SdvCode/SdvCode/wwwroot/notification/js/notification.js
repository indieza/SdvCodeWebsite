"use strict"

let connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();