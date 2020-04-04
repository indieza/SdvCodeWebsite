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