function selectedName(categoryName) {
    let categoryId = categoryName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/BlogAddons/ExtractCategoryData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'categoryId': categoryId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#categoryName").val(data.name);
            tinyMCE.get("categoryDescription").setContent(data.description);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}