$(document).ready(function () {
    $('#themeButton').click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        let span = document.getElementById("themeSpan");
        if (span) {
            if (span.style.visibility == "visible") {
                span.style.visibility = "";
            } else {
                span.style.visibility = "visible";
            }
        }
    });

    $('body').click(function () {
        let span = document.getElementById("themeSpan");
        if (span) {
            span.style.visibility = "";
        }
    });
});

function changeTheme(image) {
    let src = image.src;
    let panel = document.getElementById("chatPanel");
    if (panel) {
        panel.style.backgroundImage = `url(${src})`;
    }

    let oldBadge = document.querySelector(".theme-image .select-theme-badge");

    if (oldBadge) {
        let child = oldBadge.parentElement.children[1];
        oldBadge.parentElement.removeChild(child);
    }
    image.parentElement.innerHTML += `<span class="select-theme-badge">
                                          <i class="fas fa-check"></i>
                                      </span>`;
}