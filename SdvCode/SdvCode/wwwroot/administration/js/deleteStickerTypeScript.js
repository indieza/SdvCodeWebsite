function selectedName(stickerTypeName) {
    let value = stickerTypeName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/DeleteChatStickerType/GetChatStickerTypeData`,
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
            document.getElementById("stickersSection").style.display = "block";
            let div = document.getElementById("stickersDiv");
            div.innerHTML = "";

            for (var url of data.urls) {
                div.innerHTML += `<span>
                    <img class="singleStickerForDelete" src='${url}' />
                </span>`;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}