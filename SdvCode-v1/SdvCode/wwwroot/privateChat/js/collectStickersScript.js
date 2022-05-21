$(".collapseIconSticker").on('click', function () {
    $(this).children('.fa-chevron-down, .fa-chevron-up').toggleClass("fa-chevron-down fa-chevron-up");
    $(this).toggleClass("activeCollapse")
});

function addStickerType(stickerTypeId) {
    $.ajax({
        type: "POST",
        url: `/PrivateChat/CollectStickers/AddStickerToFavourite`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'stickerTypeId': stickerTypeId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data.isAdded) {
                document.getElementById(`${stickerTypeId}-button-section`).innerHTML = `
                    <button class="btn btn-danger addStickerTypeButton" onclick="removeStickerType('${stickerTypeId}')">
                        <i class="fas fa-minus"></i> Remove From Favourite
                    </button>`;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}

function removeStickerType(stickerTypeId) {
    $.ajax({
        type: "POST",
        url: `/PrivateChat/CollectStickers/RemoveStickerFromFavourite`,
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        data: {
            'stickerTypeId': stickerTypeId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            if (data.isRemoved) {
                document.getElementById(`${stickerTypeId}-button-section`).innerHTML = `
                    <button class="btn btn-success addStickerTypeButton" onclick="addStickerType('${stickerTypeId}')">
                        <i class="fas fa-plus"></i> Add To Favourite
                    </button>`;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    })
}