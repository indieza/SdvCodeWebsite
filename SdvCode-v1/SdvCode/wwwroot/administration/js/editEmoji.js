﻿function selectedName(emojiName) {
    let value = emojiName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/EditEmoji/GetEmojiData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'emojiId': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#emojiName").val(data.name);
            $("#emojiType").val(data.emojiType);
            $("#emojiImage").attr("src", data.url);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}