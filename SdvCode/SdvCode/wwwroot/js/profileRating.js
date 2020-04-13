function selectRate(rate) {
    let username = document.getElementsByTagName("h5")[0].innerText.substring(1).toLocaleLowerCase();
    $.ajax({
        url: `/RateUser`,
        type: "POST",
        data: {
            username: username,
            rate: rate
        },
        success: function (msg) {
            document.getElementById("profileRating").innerText = msg;
        },
        error: function (msg) {
            alert(msg);
        }
    });
}