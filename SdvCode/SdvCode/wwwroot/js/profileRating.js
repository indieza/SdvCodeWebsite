let username = document.getElementsByTagName("h5")[0].innerText.substring(1);
let profileRating = document.getElementById("profileRating");
let starsDiv = document.getElementById("starScoreRating");
let latestScore = document.getElementById("latestScore");

document.addEventListener('DOMContentLoaded', function () {
    let rating = profileRating.innerHTML.split("/")[0];
    if (latestScore) {
        markRateStars(Number(rating), parseInt(latestScore.innerText));
    }
}, false);

function selectRate(rate) {
    $.ajax({
        url: `/RateUser`,
        type: "POST",
        data: {
            username: username,
            rate: rate
        },
        success: function (msg) {
            profileRating.innerText = msg;
            latestScore.innerText = rate;
            markRateStars(Number(msg.split("/")[0]), parseInt(latestScore.innerText));
        },
        error: function (msg) {
            alert(msg);
        }
    });
}

function markRateStars(score, latestScore) {
    if (latestScore != 0) {
        document.getElementById(`rating-${latestScore}`).checked = true;
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
}