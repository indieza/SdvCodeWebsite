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