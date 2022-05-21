function selectedName(stickerName) {
    let value = stickerName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/DeleteChatSticker/GetChatStickerData`,
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
            document.getElementById("stickerSection").style.display = "block";
            $("#stickerImage").attr("src", data.url);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function selectedSticker(stickerId) {
    document.getElementById("stickersOptions").value = stickerId;
    selectedName(document.getElementById("stickersOptions"));
}