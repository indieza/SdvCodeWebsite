"use strict"

let notificationConnection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

notificationConnection.start().then(function () {
    notificationConnection.invoke("GetUserNotificationCount").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

notificationConnection.on("ReceiveNotification", function (count) {
    document.getElementById("notificationCount").innerText = count;
    if (count > 0) {
        document.querySelector("audio").play();
    }
});

notificationConnection.on("VisualizeNotification", function (result) {
    let notifications = document.getElementById("allUserNotifications");
    let div = document.createElement("div");
    let item = JSON.parse(result);
    div.id = item.Id;
    div.classList.add(["col-md-6", "col-sm-6"]);
    div.innerHTML = `<div class="ts-testimonial-content">
                        <img src="${item.ApplicationUser.ImageUrl}" alt="avatar">
                        <h4 class="ts-testimonial-text userNotificationsHeading">
                            <span>
                                <a class="deleteNotificationIcon" onclick="deleteNotification('${item.Id}')">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </span>
                            <span>
                                ${item.NotificationHeading}
                            </span>
                        </h4>
                        <div class="ts-testimonial-text dropdownNotification">
                            <button class="dropbtnNotification">
                                <i class="fas fa-chevron-down notificationArrow"></i>
                            </button>
                            <div class="dropdown-content-notification">
                                @foreach (var status in Enum.GetValues(typeof(NotificationStatus)))
                                {
                                    <a onclick="updateStatus('@status.ToString()', '@item.Id')">@status.ToString()</a>
                                }
                            </div>
                            <span>Status: </span>
                            <b>
                                <span id="${item.Id}orderStatus" style="color: @colors[item.Status.ToString()]; text-transform: uppercase">
                                    @item.Status.ToString()
                                </span>
                            </b>
                        </div>
                        <p class="ts-testimonial-text">
                            ${item.Text}
                        </p>

                        <div class="ts-testimonial-author">
                            <h3 class="name userNotificationsHeading">
                                <a asp-area="" asp-controller="Profile" asp-action="Index" asp-route-username="@item.ApplicationUser.UserName">
                                    ${item.ApplicationUser.FirstName} ${item.ApplicationUser.LastName}
                                </a>
                                <span>
                                    @item.CreatedOn.ToLocalTime().ToString("dd-MMMM-yyyy HH:mm tt")
                                </span>
                            </h3>
                        </div>
                    </div>`;
    notifications.insertBefore(div, notifications.childNodes[0]);
});