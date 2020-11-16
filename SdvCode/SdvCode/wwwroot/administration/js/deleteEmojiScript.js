function selectedName(emojiName) {
    let value = emojiName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/DeleteEmoji/GetEmojiData`,
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
            document.getElementById("emojiSection").style.display = "block";
            $("#emojiImage").attr("src", data.url);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}

function selectedEmoji(emojiId) {
    document.getElementById("emojisOptions").value = emojiId;
    selectedName(document.getElementById("emojisOptions"));
}