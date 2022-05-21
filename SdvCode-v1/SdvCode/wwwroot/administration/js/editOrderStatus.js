document.getElementById("editStatusSelect").addEventListener("change", function (e) {
    e.preventDefault();

    document.getElementById("editStatusButton").disabled = false;
    document.getElementById("cancelEditStatusButton").disabled = true;
})

function setOrderIdAndStatus(id) {
    document.getElementById("targetOrderId").value = id;
    $.ajax({
        type: "GET",
        url: `/Administration/Shop/GetOrderStatus`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'orderId': id
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("editStatusSelect").value = data;
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function changeOrderStatus() {
    let status = parseInt(document.getElementById("editStatusSelect").value);
    let id = document.getElementById("targetOrderId").value;
    $.ajax({
        type: "POST",
        url: `/Administration/Shop/EditOrderStatus`,
        contentType: 'application/x-www-form-urlencoded',
        dataType: "json",
        data: {
            id: id,
            status: status
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("editStatusButton").disabled = true;
            document.getElementById("cancelEditStatusButton").disabled = false;
            document.getElementById("messageStatus").innerHTML =
                `<p>Successfully changed order status: <b style="color: green">${data}</b></p>`
            console.log(data);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}