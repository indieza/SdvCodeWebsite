function selectedName(productName) {
    let value = productName.value;

    $.ajax({
        type: "GET",
        url: `/Administration/Shop/ExtractProductData`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            'productName': value
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            document.getElementById("contentForm").style.display = "block";
            $("#productName").val(data.name);
            $("#productCategory").val(data.productCategory);
            $("#productPrice").val(data.price);
            tinyMCE.get("productDescription").setContent(data.description);
            $("#productId").val(data.id);
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}