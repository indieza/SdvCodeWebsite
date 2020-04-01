function showAddCommentForm(parentId) {
    $("#AddCommentForm input[name='ParentId']").val(parentId);
    $("#AddCommentForm").show();
    $([document.documentElement, document.body]).animate({
        scrollTop: $("#AddCommentForm").offset().top
    }, 1000);
}