function emojiTypeSelect(emoji) {
    let emojiType = emoji.value;

    $.ajax({
        type: "GET",
        url: `/Administration/EditEmojiPosition/GetEmojisPosition`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'emojiType': emojiType
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            let div = document.getElementById("allEmojis");
            div.innerHTML = "";

            if (data.length > 0) {
                div.innerHTML += `
                    <div class="row">
                        <div class="col-xl-3 col-md-6 mb-4">
                            <button class="btn btn-info btn-icon-split" onclick="resetPositionsData()">
                                <span class="icon text-white-50">
                                    <i class="fas fa-sync-alt"></i>
                                </span>
                                <span class="text">Edit Positions</span>
                            </button>
                        </div>
                    </div>
                    <hr />`;
            }

            for (let currentEmoji of data) {
                div.innerHTML += `
                    <div class="row emojiElements">
                        <div class="col-xl-1 col-md-1 mb-4">
                            <input class="form-control" type="text" placeholder="Code" value="${currentEmoji.code}" readonly />
                        </div>
                        <div class="col-xl-9 col-md-9 mb-4">
                            <input class="form-control" type="text" placeholder="Name" value="${currentEmoji.name}" readonly />
                        </div>
                        <div class="col-xl-2 col-md-2 mb-4">
                            <input id="${currentEmoji.id}position" class="form-control" type="number" min="1" placeholder="0"  value="${currentEmoji.position}" onchange="positionChange(this)"/>
                        </div>
                        <input type="hidden" value="${currentEmoji.id}" />
                    </div>
                    <hr />`;
            }

            if (data.length > 0) {
                div.innerHTML += `
                    <div class="row">
                        <div class="col-xl-3 col-md-6 mb-4">
                            <button class="btn btn-success btn-icon-split" onclick="submitData()">
                                <span class="icon text-white-50">
                                    <i class="fas fa-edit"></i>
                                </span>
                                <span class="text">Edit Positions</span>
                            </button>
                        </div>
                    </div>`;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}

function positionChange(currentInput) {
    let allNubersInputFields = document.querySelectorAll('input[type=number]');

    for (let currentInputNumber of allNubersInputFields) {
        if (currentInput != currentInputNumber && currentInput.value == currentInputNumber.value) {
            currentInputNumber.value = currentInput.defaultValue;
            currentInputNumber.defaultValue = currentInputNumber.value;
        }
    }
    currentInput.defaultValue = currentInput.value;
}

function submitData() {
    let allEmojis = [];
    let data = document.querySelectorAll(".emojiElements");

    for (let currentPart of data) {
        allEmojis.push({
            "Id": currentPart.children[3].value,
            "Name": currentPart.children[1].children[0].value,
            "Position": document.getElementById(`${currentPart.children[3].value}position`).value
        });
    }

    $.ajax({
        type: "POST",
        url: `/Administration/EditEmojiPosition/EditEmojisPosition`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'json': JSON.stringify(allEmojis)
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            window.location.href = data;
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}

function resetPositionsData() {
    let allNumbers = document.querySelectorAll('input[type="number"]');

    for (var count = 0; count < allNumbers.length; count++) {
        allNumbers[count].value = count + 1;
    }
}