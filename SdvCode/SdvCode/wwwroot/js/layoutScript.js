function getLatestBlogPosts() {
    var latestPostsDiv = document.getElementById("latesBlogPosts");
    $.ajax({
        type: "GET",
        url: `/Home/GetLatestBlogPosts`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            latestPostsDiv.innerHTML = "";
            for (var post of data) {
                latestPostsDiv.innerHTML += `
                                <a class="thumb-holder" data-rel="prettyPhoto" href="/Blog/Post/${post.id}">
                                    <img src="${post.imageUrl}" alt="${post.title}">
                                </a>`;
            }
        },
        error: function (msg) {
            console.error(msg);
        }
    });
}