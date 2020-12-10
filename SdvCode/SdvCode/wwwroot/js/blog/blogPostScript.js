function showAddCommentForm(parentId) {
    $("#AddCommentForm input[name='ParentId']").val(parentId);
    $("#AddCommentForm").show();
    $([document.documentElement, document.body]).animate({
        scrollTop: $("#AddCommentForm").offset().top
    }, 1000);
}

$(".btn-comment-circle").on('click', function () {
    $(this).children('.fa-caret-down, .fa-caret-up').toggleClass("fa-caret-down fa-caret-up");
    $(this).toggleClass("btn-success btn-danger")
});

function checkFileExtension(inputField) {
    for (var file of inputField.files) {
        let fileExtension = file.name.split('.').pop();
        let imageExtensions = ["JPG", "PNG", "JPEG"];

        if (!imageExtensions.includes(fileExtension.toUpperCase())) {
            inputField.value = "";
            alert(`Please select only image files - ${imageExtensions.join(", ")}`)
            break;
        }
    }
}