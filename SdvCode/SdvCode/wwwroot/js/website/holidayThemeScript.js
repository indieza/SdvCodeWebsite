function setHolidayTheme() {
    $.ajax({
        type: "GET",
        url: `/Home/GetHolidayTheme`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data) {
                document.getElementById("holidayTheme").innerHTML = '';
                for (let icon of data) {
                    document.getElementById("holidayTheme").innerHTML += `
                        <div class="snowflake">
                            <img class="singleThemeIcon" src="${icon}"/>
                        </div>`;
                }
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}