$("#imageButton i").click(function () {
    $("#uploadImage").trigger('click');
});

$('#uploadImage').on('change', function () {
    let filesCount = document.getElementById("uploadImage").files.length;
    let badge = document.querySelector(".select-image-badge");
    badge.innerText = filesCount;

    if (filesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(51, 217, 178, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }
})