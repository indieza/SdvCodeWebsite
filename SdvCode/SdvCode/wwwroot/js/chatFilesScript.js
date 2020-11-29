$("#fileButton i").click(function () {
    $("#uploadFile").trigger('click');
});

$('#uploadFile').on('change', function () {
    const fileExtensions = ["TXT", "TEXT", "DOCX", "DOC", "PDF", "PPT", "XLS", "XLSX"];
    let files = document.getElementById("uploadFile").files;
    const dt = new DataTransfer();

    for (var file of files) {
        let fileExtension = file.name.split('.').pop();
        if (fileExtensions.includes(fileExtension.toUpperCase())) {
            dt.items.add(file);
        }
    }

    document.getElementById("uploadFile").files = dt.files;
    let filesCount = document.getElementById("uploadFile").files.length;
    let badge = document.querySelector(".select-file-badge");
    badge.innerText = filesCount;

    if (filesCount > 0) {
        badge.style.boxShadow = "0 0 0 0 rgba(2, 191, 255, 1)";
        badge.style.animation = "pulse-green 2s infinite";
    } else {
        badge.style.boxShadow = "";
        badge.style.animation = "";
    }
})