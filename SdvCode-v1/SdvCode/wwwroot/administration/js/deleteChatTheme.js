function selectedName(themeName) {
    let value = themeName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/DeleteChatTheme/ExtractThemeData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'themeId': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#themeName").val(data.name);
            $("#themeImage").attr("src", data.imageUrl);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}