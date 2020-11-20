function openImage(modalElement, tag) {
    let modal = document.getElementById(`${modalElement}-Modal`);
    let captionText = document.getElementById(`${modalElement}-Caption`);
    modal.style.display = "block";
    captionText.innerHTML = `${tag.innerText}`;
}

function closeImage(element) {
    let modal = document.getElementById(`${element}-Modal`);
    modal.style.display = "none";
}