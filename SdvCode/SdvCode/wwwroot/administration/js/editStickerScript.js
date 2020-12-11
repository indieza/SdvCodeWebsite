function selectedName(stickerName) {
    let value = stickerName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/EditChatSticker/GetStickerData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'stickerId': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#stickerName").val(data.name);
            $("#stickerImage").attr("src", data.url);
            $("#stickerType").val(data.stickerTypeId);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}