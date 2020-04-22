function selectedName(categoryName) {
    let value = categoryName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/Shop/ExtractProductCategoryData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'categoryName': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#categoryTitle").val(data.title);
            tinyMCE.get("categoryDescription").setContent(data.description);
            $("#categoryId").val(data.id);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}