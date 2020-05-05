document.addEventListener('DOMContentLoaded', function () {
    let rating = document.getElementById("profileRating").innerHTML.split("/")[0];
    markRateStars(rating, true);
}, false);

function selectRate(rate) {
    let username = document.getElementsByTagName("h5")[0].innerText.substring(1);
    $.ajax({
        url: `/RateUser`,
        type: "POST",
        data: {
            username: username,
            rate: rate
        },
        success: function (msg) {
            document.getElementById("profileRating").innerText = msg;
            var latestScore = document.getElementById("latestScore");

            if (latestScore.innerText == "0") {
                latestScore.innerText = rate;
                markRateStars(msg.split("/")[0], true);
            } else {
                latestScore.innerText = rate;
                markRateStars(msg.split("/")[0], false);
            }
            markRateStars(msg.split("/")[0], true);
        },
        error: function (msg) {
            alert(msg);
        }
    });
}

function markRateStars(score, isFirst) {
    let starsDiv = document.getElementById("starScoreRating");
    let latestScore = document.getElementById("latestScore");
    if (!isFirst) {
        if (latestScore) {
            document.getElementById(`rating-${latestScore.innerText}`).checked = false;
        }
    }
    let div = document.createElement("div");
    starsDiv.innerHTML = "";

    for (var i = 0; i < Math.round(score); i++) {
        div.innerHTML += `<span class="ratingStar">★ </span>`;
    }

    for (var i = Math.round(score); i < 5; i++) {
        div.innerHTML += `<span>★ </span>`;
    }
    starsDiv.appendChild(div);
    if (isFirst) {
        if (latestScore.innerText != "0" && score != 0) {
            document.getElementById(`rating-${latestScore.innerText}`).checked = true;
        }
    }
}