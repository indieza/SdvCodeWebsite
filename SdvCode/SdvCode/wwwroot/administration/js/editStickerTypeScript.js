function selectedName(stickerTypeName) {
    let value = stickerTypeName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/EditChatStickerType/GetStickerTypeData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'stickerTypeId': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#stickerTypeName").val(data.name);
            $("#stickerTypeImage").attr("src", data.url);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}