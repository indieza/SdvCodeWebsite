$("#imageButton i").click(function () {
    $("#uploadImage").trigger('click');
});

$('#uploadImage').on('change', function () {
    const imageExtensions = ["JPG", "PNG", "JPEG"];
    let files = document.getElementById("uploadImage").files;
    const dt = new DataTransfer();

    for (var file of files) {
        let fileExtension = file.name.split('.').pop();
        if (imageExtensions.includes(fileExtension.toUpperCase())) {
            dt.items.add(file);
        }
    }

    document.getElementById("uploadImage").files = dt.files;
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