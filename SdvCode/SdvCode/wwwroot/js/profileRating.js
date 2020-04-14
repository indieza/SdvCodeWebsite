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
            markRateStars(msg.split("/")[0], false);
            var latestScore = document.getElementById("latestScore");
            latestScore.innerHTML = rate;
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
        if (latestScore) {
            document.getElementById(`rating-${latestScore.innerText}`).checked = true;
        }
    }
}